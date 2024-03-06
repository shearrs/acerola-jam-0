using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

[System.Serializable]
public class LotsUI
{
    private const int MAX_ROLLS = 3;

    [SerializeField] private bool drawGizmos;

    [Header("References")]
    [SerializeField] private RectTransform lotsContainer;
    [SerializeField] private Lot lotPrefab;
    [SerializeField] private List<Spline> splines;
    private readonly List<Spline> currentSplines = new();
    private readonly List<Lot> lots = new();
    private Camera cam;

    // lot loop
    private int roll;
    private bool lotConfirm;
    private List<Lot> keptLots;

    [Header("Tween")]
    [SerializeField] private Tween scaleTween;
    [SerializeField] private Vector3 targetScale;

    public Lot HoveredLot { get; set; }

    public void Enable()
    {
        if (cam == null)
            cam = Camera.main;

        lotsContainer.gameObject.SetActive(true);
        lotsContainer.localScale = Vector3.zero;
        lotsContainer.DoTweenScaleNonAlloc(targetScale, scaleTween.Duration, scaleTween).SetOnComplete(() => ThrowLots(3));
        roll = 0;
    }

    public void Disable()
    {
        lotsContainer.DoTweenScaleNonAlloc(Vector3.zero, scaleTween.Duration, scaleTween).SetOnComplete(() => lotsContainer.gameObject.SetActive(false));
    }

    public void ThrowLots(int amount)
    {
        if (amount > lots.Count)
        {
            int count = lots.Count;

            for (int i = 0; i < amount - count; i++)
            {
                Lot lot = Object.Instantiate(lotPrefab, lotsContainer);
                lot.gameObject.SetActive(false);

                lots.Add(lot);
            }
        }

        ResetSplines();

        lots[0].gameObject.SetActive(true);
        lots[0].Throw(GetSpline(), true);
        for (int i = 1; i < amount; i++)
        {
            lots[i].gameObject.SetActive(true);
            lots[i].Throw(GetSpline());
        }

        roll++;
    }

    public void ConfirmLots() => lotConfirm = true;

    public void SelectLots()
    {
        UIManager.Instance.StartCoroutine(IESelectLots());
    }

    private IEnumerator IESelectLots()
    {
        while (!lotConfirm)
        {
            if (HoveredLot != null)
            {

            }

            yield return null;
        }

        lotConfirm = false;
        // raycast to see if hovering a lot
        // if hovering a lot, make selectedLot = that lot
        // if lmb, add that lot to keptLots
        // keep doing all this until lotConfirm is true
    }

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
        for (int i = 0; i < currentSplines.Count; i++)
        {
            splines.Add(currentSplines[0]);
            currentSplines.RemoveAt(0);
        }
    }

    public void DrawGizmos()
    {
        if (!drawGizmos)
            return;
    }
}