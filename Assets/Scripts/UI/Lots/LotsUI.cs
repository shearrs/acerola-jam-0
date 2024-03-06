using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

[System.Serializable]
public class LotsUI
{
    [Header("References")]
    [SerializeField] private GameObject combatContainer;
    [SerializeField] private RectTransform lotsUI;
    [SerializeField] private RectTransform lotsContainer;
    [SerializeField] private LotsButton lotsButton;
    [SerializeField] private LotsBox lotsBox;
    [SerializeField] private MinimizeButton minimizeButton;
    private LotsManager lotsManager;

    [Header("Minimizing")]
    [SerializeField] private float minimizeHeight;
    private Vector3 maximizePosition;

    [Header("Tween")]
    [SerializeField] private Tween scaleTween;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private Tween minimizeTween;

    public void Enable()
    {
        if (lotsManager == null)
        {
            lotsManager = LotsManager.Instance;
        }

        lotsContainer.gameObject.SetActive(true);
        lotsContainer.localScale = Vector3.zero;
        lotsContainer.DoTweenScaleNonAlloc(targetScale, scaleTween.Duration, scaleTween).SetOnComplete(() => OnTweenComplete(true));

        maximizePosition = lotsUI.anchoredPosition3D;
    }

    public void Disable()
    {
        lotsContainer.DoTweenScaleNonAlloc(Vector3.zero, scaleTween.Duration, scaleTween).SetOnComplete(() => OnTweenComplete(true));
    }

    public void SelectLots()
    {
        lotsButton.Enable();
    }

    public void SetLotsButtonActive(bool active)
    {
        if (active)
            lotsButton.Enable();
        else
            lotsButton.Disable();
    }

    public void UpdateLotsButton(int lots)
    {
        lotsButton.UpdateState(lots);
    }

    private void OnTweenComplete(bool open)
    {
        combatContainer.SetActive(!open);
        lotsContainer.gameObject.SetActive(open);
        lotsButton.gameObject.SetActive(open);
        lotsBox.gameObject.SetActive(open);

        if (open)
            minimizeButton.Enable();
        else
            minimizeButton.Disable();
    }

    public void ToggleMinimize(bool enable)
    {
        if (enable)
        {
            lotsUI.DoTweenPositionNonAlloc(new(maximizePosition.x, minimizeHeight, maximizePosition.z), minimizeTween.Duration, minimizeTween);

            lotsManager.SetLotsActive(false);
        }
        else
        {
            void enableLots()
            {
                if (lotsManager.Roll == 0)
                    return;

                lotsManager.SetLotsActive(true);
            }

            lotsUI.DoTweenPositionNonAlloc(maximizePosition, minimizeTween.Duration, minimizeTween).SetOnComplete(enableLots);
        }
    }
}