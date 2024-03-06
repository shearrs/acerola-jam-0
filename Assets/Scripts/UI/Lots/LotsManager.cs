using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotsManager : Singleton<LotsManager>
{
    private const int MAX_ROLLS = 3;

    [Header("References")]
    [SerializeField] private Lot lotPrefab;
    [SerializeField] private LotsBox lotsBox;
    [SerializeField] private RectTransform lotsParent;

    [Header("Setup")]
    [SerializeField] private List<Spline> splines;
    private readonly List<Spline> currentSplines = new();
    private readonly List<Lot> lots = new();

    private LotsUI lotsUI;
    private Coroutine selectCoroutine;
    private Player player;

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

    private void Start()
    {
        player = Level.Instance.Player;
        lotsUI = UIManager.Instance.LotsUI;
    }

    public void EnterLotsPhase()
    {
        lotsUI.Enable();
        Roll = 0;

        CreateLots(player.LotCapacity);
    }

    public void ThrowLots()
    {
        if (selectCoroutine != null)
            StopCoroutine(selectCoroutine);

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
        for (int i = 0; i < lots.Count; i++)
        {
            lotsBox.KeepLot(lots[0]);
            lots.RemoveAt(0);
        }

        lotsUI.Disable();

        // go into turn selection phase
    }

    public void SelectLots()
    {
        lotsUI.SelectLots();

        SetLotTypes();

        if (Roll < MAX_ROLLS)
            selectCoroutine = StartCoroutine(IESelectLots());
        else
            lotsUI.UpdateLotsButton(0);
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
        while (true)
        {
            if (HoveredLot != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!HoveredLot.IsKept)
                    {
                        lotsBox.KeepLot(HoveredLot);
                        lots.Remove(HoveredLot);
                    }
                    else
                    {
                        lotsBox.ReleaseLot(HoveredLot);
                        HoveredLot.transform.localPosition = HoveredLot.OriginalPosition;
                        HoveredLot.transform.rotation = HoveredLot.OriginalRotation;

                        lots.Add(HoveredLot);
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
        for (int i = 0; i < amount; i++)
        {
            Lot lot = Instantiate(lotPrefab, lotsParent);
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