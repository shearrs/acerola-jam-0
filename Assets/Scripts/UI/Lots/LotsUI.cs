using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

[System.Serializable]
public class LotsUI
{
    private const int MAX_ROLLS = 3;

    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject combatContainer;
    [SerializeField] private RectTransform lotsUI;
    [SerializeField] private RectTransform lotsParent;
    [SerializeField] private RectTransform lotsContainer;
    [SerializeField] private LotsButton lotsButton;
    [SerializeField] private LotsBox lotsBox;
    [SerializeField] private Lot lotPrefab;
    [SerializeField] private MinimizeButton minimizeButton;
    [SerializeField] private List<Spline> splines;
    private readonly List<Spline> currentSplines = new();
    private readonly List<Lot> lots = new();
    private Camera cam;
    private Player player;

    [Header("Lot Colors")]
    [SerializeField] private Color holyColor;
    [SerializeField] private Color damageColor;
    [SerializeField] private Color protectionColor;
    [SerializeField] private Color sinColor;

    // lot loop
    private int roll;
    private Coroutine selectCoroutine;

    [Header("Minimizing")]
    [SerializeField] private float minimizeHeight;
    private Vector3 maximizePosition;

    [Header("Tween")]
    [SerializeField] private Tween scaleTween;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private Tween minimizeTween;

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

    public void Enable()
    {
        if (cam == null)
        {
            cam = Camera.main;
            player = Level.Instance.Player;
        }

        void onComplete()
        {
            combatContainer.SetActive(false);
            lotsButton.gameObject.SetActive(true);
            lotsBox.gameObject.SetActive(true);
            minimizeButton.Enable();
        }

        lotsContainer.gameObject.SetActive(true);
        lotsContainer.localScale = Vector3.zero;
        lotsContainer.DoTweenScaleNonAlloc(targetScale, scaleTween.Duration, scaleTween).SetOnComplete(onComplete);
        roll = 0;

        maximizePosition = lotsUI.anchoredPosition3D;

        CreateLots(player.LotCapacity);
    }

    public void Disable()
    {
        void onComplete()
        {
            combatContainer.SetActive(true);
            lotsContainer.gameObject.SetActive(false);
            lotsButton.gameObject.SetActive(false);
            minimizeButton.Disable();
        }

        lotsContainer.DoTweenScaleNonAlloc(Vector3.zero, scaleTween.Duration, scaleTween).SetOnComplete(onComplete);
    }

    public void ToggleMinimize(bool enable)
    {
        if (enable)
        {
            lotsUI.DoTweenPositionNonAlloc(new(maximizePosition.x, minimizeHeight, maximizePosition.z), minimizeTween.Duration, minimizeTween);

            foreach (Lot lot in lots)
                lot.gameObject.SetActive(false);
        }
        else
        {
            void onComplete()
            {
                if (roll == 0)
                    return;

                foreach (Lot lot in lots)
                    lot.gameObject.SetActive(true);
            }

            lotsUI.DoTweenPositionNonAlloc(maximizePosition, minimizeTween.Duration, minimizeTween).SetOnComplete(onComplete);
        }
    }

    #region Lots Gameplay
    public void ThrowLots()
    {
        if (selectCoroutine != null)
            UIManager.Instance.StopCoroutine(selectCoroutine);

        lotsButton.Disable();

        ResetSplines();

        lots[0].gameObject.SetActive(true);
        lots[0].Throw(GetSpline(), true);
        for (int i = 1; i < lots.Count; i++)
        {
            lots[i].gameObject.SetActive(true);
            lots[i].Throw(GetSpline());
        }

        roll++;
    }

    public void ConfirmLots()
    {
        Disable();
    }

    public void SelectLots()
    {
        lotsButton.Enable();
        SetLotColors();

        if (roll < MAX_ROLLS)
            selectCoroutine = UIManager.Instance.StartCoroutine(IESelectLots());
        else
            lotsButton.UpdateState(0);
    }

    private void SetLotColors()
    {
        for (int i = 0; i < lots.Count; i++)
        {
            int colorIndex = Random.Range(0, 9);

            lots[i].SetColor(GetColorForIndex(colorIndex));
        }
    }

    private Color GetColorForIndex(int index)
    {
        if (index == 0)
            return sinColor;
        else if (index == 7)
            return holyColor;
        else if (index < 5)
            return damageColor;
        else if (index <= 8)
            return protectionColor;
        else
            return damageColor;
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
                        lots.Add(HoveredLot);
                    }

                    lotsButton.UpdateState(lots.Count);
                }
            }

            yield return null;
        }
    }

    private void CreateLots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Lot lot = Object.Instantiate(lotPrefab, lotsParent);
            lot.gameObject.SetActive(false);

            lots.Add(lot);
        }
    }
    #endregion

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