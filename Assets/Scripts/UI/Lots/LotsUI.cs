using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

[System.Serializable]
public class LotsUI
{
    [Header("References")]
    [SerializeField] private RectTransform lotsUI;
    [SerializeField] private RectTransform lotsContainer;
    [SerializeField] private LotsButton lotsButton;
    [SerializeField] private LotsBox lotsBox;
    [SerializeField] private MinimizeButton minimizeButton;

    [Header("Minimizing")]
    [SerializeField] private float minimizeHeight;
    private Vector3 maximizePosition;
    private CombatManager combatManager;

    [Header("Tween")]
    [SerializeField] private Tween scaleTween;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private Tween minimizeTween;

    public MinimizeButton MinimizeButton => minimizeButton;

    public void Enable()
    {
        if (combatManager == null)
            combatManager = CombatManager.Instance;

        lotsContainer.gameObject.SetActive(true);
        lotsContainer.localScale = Vector3.zero;
        lotsContainer.DoTweenScaleNonAlloc(targetScale, scaleTween.Duration, scaleTween).SetOnComplete(() => OnTweenComplete(true));
        UpdateLotsButton(Level.Instance.Player.LotCapacity);

        if (Level.Instance.Player.Defense == 0)
        {
            DefenseDisplay defenseDisplay = combatManager.DefenseDisplay;
            defenseDisplay.IsEnabled = false;
            defenseDisplay.UpdateDefense(0);
        }

        maximizePosition = lotsUI.anchoredPosition3D;
    }

    public void Disable()
    {
        lotsButton.gameObject.SetActive(false);
        lotsContainer.DoTweenScaleNonAlloc(Vector3.zero, scaleTween.Duration, scaleTween).SetOnComplete(() => OnTweenComplete(false));
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
        lotsContainer.gameObject.SetActive(open);

        if (open)
            lotsButton.gameObject.SetActive(open);

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

            combatManager.SetLotsActive(false);
        }
        else
        {
            void enableLots()
            {
                if (combatManager.Roll == 0)
                    return;

                combatManager.SetLotsActive(true);
            }

            lotsUI.DoTweenPositionNonAlloc(maximizePosition, minimizeTween.Duration, minimizeTween).SetOnComplete(enableLots);
        }
    }
}