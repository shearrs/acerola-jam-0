using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using CustomUI;

public enum CombatDrop { NONE, LOT, HEALTH, LOT_AND_HEALTH };

public class CombatDropUI : Singleton<CombatDropUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform lotDisplay;
    [SerializeField] private RectTransform healthDisplay;
    [SerializeField] private RectTransform lotAndHealthDisplay;
    private RectTransform currentScreen;

    public bool LotSelected { get; set; } = false;
    public bool HealthSelected { get; set; } = false;

    private readonly Tween tween = new();

    public void Enable(CombatDrop drop)
    {
        currentScreen = GetScreenForDrop(drop);

        void onComplete()
        {
            currentScreen.gameObject.SetActive(true);
            StartCoroutine(IEWaitForConfirmation(drop));
        }

        container.gameObject.SetActive(true);
        container.localScale = TweenManager.TWEEN_ZERO;
        container.DoTweenScaleNonAlloc(Vector3.one, 0.65f, tween).SetOnComplete(onComplete).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    private void Disable()
    {
        UIManager.Instance.ToggleBar(false, null, true);

        currentScreen.gameObject.SetActive(false);

        LotSelected = false;
        HealthSelected = false;

        void onComplete()
        {
            container.gameObject.SetActive(false);
            Level.Instance.EndEncounter();
        }

        container.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.65f, tween).SetOnComplete(onComplete); // on complete continue game
    }

    private IEnumerator IEWaitForConfirmation(CombatDrop drop)
    {
        if (drop == CombatDrop.LOT)
        {
            while (LotSelected == false)
                yield return null;
        }
        else if (drop == CombatDrop.HEALTH)
        {
            while (HealthSelected == false)
                yield return null;
        }
        else if (drop == CombatDrop.LOT_AND_HEALTH)
        {
            while (LotSelected == false || HealthSelected == false)
                yield return null;
        }

        Disable();
    }

    private RectTransform GetScreenForDrop(CombatDrop drop)
    {
        switch (drop)
        {
            case CombatDrop.NONE:
                return null;
            case CombatDrop.LOT:
                return lotDisplay;
            case CombatDrop.HEALTH:
                return healthDisplay;
            case CombatDrop.LOT_AND_HEALTH:
                return lotAndHealthDisplay;
            default:
                return null;
        }
    }
}
