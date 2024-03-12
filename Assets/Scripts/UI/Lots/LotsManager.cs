using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LotsManager
{
    private const int MAX_ROLLS = 3;
    private const float PITCH_INCREMENT = 0.05f;

    [Header("References")]
    [SerializeField] private Lot lotPrefab;
    [SerializeField] private RectTransform lotsParent;
    [SerializeField] private AudioClip selectSound;

    [Header("Setup")]
    [SerializeField] private List<Spline> splines;
    private readonly List<Spline> currentSplines = new();
    [SerializeField] private List<Lot> lots = new();
    private readonly List<Lot> reserveLots = new();

    private LotsUI lotsUI;
    private Coroutine selectCoroutine;
    private Player player;
    private LotsBox lotsBox;
    private bool enabled = false;

    private Lot hoveredLot;
    public Lot HoveredLot
    {
        get => hoveredLot;
        set
        {
            if (hoveredLot != null)
                hoveredLot.Highlight(false);

            hoveredLot = value;

            if (hoveredLot != null)
                hoveredLot.Highlight(true);
        }
    }
    public int Roll { get; private set; }
    public RectTransform LotsParent => lotsParent;

    public void Initialize()
    {
        player = Level.Instance.Player;
        lotsUI = UIManager.Instance.LotsUI;
        lotsBox = CombatManager.Instance.LotsBox;
    }

    public void EnterPhase()
    {
        enabled = true;

        Roll = 0;

        if (player.HasSin(SinType.SLOTH))
        {
            player.Defense -= player.DefenseToRemove;
            player.DefenseToRemove = player.Defense;
        }
        else
            player.Defense = 0;

        lotsUI.Enable();

        CreateLots(player.LotCapacity);
        lotsUI.ToggleMinimize(false);

        if (player.HasSin(SinType.GLUTTONY) && lotsBox.LotsCount > 0)
        {
            int random = Random.Range(0, lotsBox.LotsCount);
            SinUI.Instance.ActivateUI(SinType.GLUTTONY);

            lotsBox.ReleaseLot(lotsBox.Lots[random], true);
            lotsBox.SortLots();
        }
    }

    public void ExitPhase()
    {
        if (!enabled) 
            return;

        CombatManager.Instance.StopCoroutine(selectCoroutine);
        ResetSplines();
        lotsUI.Disable();
        enabled = false;
    }

    public void ThrowLots()
    {
        if (selectCoroutine != null)
            CombatManager.Instance.StopCoroutine(selectCoroutine);

        lotsUI.MinimizeButton.Disable(false);
        lotsUI.SetLotsButtonActive(false);

        ResetSplines();

        lots[0].gameObject.SetActive(true);
        lots[0].Throw(GetSpline(), true);
        for (int i = 1; i < lots.Count; i++)
        {
            lots[i].gameObject.SetActive(true);

            lots[i].Throw(GetSpline());
        }

        Roll++;
    }

    public void ConfirmLots()
    {
        lotsBox.KeepFinalLots(lots);
        lots.Clear();
        lotsUI.Disable();

        CombatManager.Instance.EnterPhase(CombatPhase.ACTION);
    }

    public void SelectLots()
    {
        if (player.HasSin(SinType.PRIDE))
            SinUI.Instance.ActivateUI(SinType.PRIDE);

        lotsUI.SelectLots();
        SetLotTypes();
        lotsUI.MinimizeButton.Enable(false);

        if (Roll < MAX_ROLLS)
        {
            selectCoroutine = CombatManager.Instance.StartCoroutine(IESelectLots());
        }
        else
        {
            lotsUI.UpdateLotsButton(0);
        }
    }

    public void RetireLot(Lot lot)
    {
        lot.gameObject.SetActive(false);
        reserveLots.Add(lot);
    }

    private void SetLotTypes()
    {
        for (int i = 0; i < lots.Count; i++)
        {
            lots[i].RandomizeType();
        }
    }

    private IEnumerator IESelectLots()
    {
        int lotsKept = 0;

        while (true)
        {
            if (HoveredLot != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!HoveredLot.IsKept)
                    {
                        AudioManager.Instance.UISource.pitch = 1 + lotsKept * PITCH_INCREMENT;
                        AudioManager.Instance.PlaySound(selectSound, 0.65f);

                        lotsBox.KeepLot(HoveredLot);

                        if (Roll == MAX_ROLLS)
                            HoveredLot.IsLocked = true;

                        lots.Remove(HoveredLot);
                        lotsKept++;
                    }
                    else
                    {
                        AudioManager.Instance.UISource.pitch = 0.9f + lotsKept * PITCH_INCREMENT;
                        AudioManager.Instance.PlaySound(selectSound, 0.65f);

                        lotsBox.ReleaseLot(HoveredLot);
                        HoveredLot.transform.SetLocalPositionAndRotation
                            (
                            HoveredLot.OriginalPosition, 
                            HoveredLot.OriginalRotation
                            );
                        lots.Add(HoveredLot);
                        lotsKept--;
                    }

                    lotsUI.UpdateLotsButton(lots.Count);
                }
            }

            yield return null;
        }
    }

    public void SetLotsActive(bool active)
    {
        foreach (Lot lot in lots)
            lot.gameObject.SetActive(active);
    }

    private void CreateLots(int amount)
    {
        int reserve = reserveLots.Count;

        for (int j = 0; j < reserve; j++)
        {
            if (amount == 0)
                return;

            Lot lot = reserveLots[0];
            lot.gameObject.SetActive(false);
            lots.Add(lot);

            reserveLots.RemoveAt(0);
            amount--;
        }

        for (int i = 0; i < amount; i++)
        {
            Lot lot = Object.Instantiate(lotPrefab, lotsParent);
            lot.gameObject.SetActive(false);

            lots.Add(lot);
        }
    }

    #region Splines
    private Spline GetSpline()
    {
        int index = Random.Range(0, splines.Count);

        Spline spline = splines[index];

        splines.RemoveAt(index);
        currentSplines.Add(spline);

        return spline;
    }

    private void ResetSplines()
    {
        int count = currentSplines.Count;

        for (int i = 0; i < count; i++)
        {
            splines.Add(currentSplines[0]);
            currentSplines.RemoveAt(0);
        }
    }
    #endregion
}