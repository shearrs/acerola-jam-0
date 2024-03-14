using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using CustomUI;

public enum CombatDrop { NONE, LOT, HEALTH, LOT_AND_HEALTH };

public class CombatDropUI : Singleton<CombatDropUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private CombatDropDisplay lotDisplay;
    [SerializeField] private CombatDropDisplay healthDisplay;
    [SerializeField] private CombatDropDisplay lotAndHealthDisplay;
    private CombatDropDisplay currentDisplay;
    private Player player;

    public bool LotSelected { get; set; } = false;
    public bool HealthSelected { get; set; } = false;

    private readonly Tween tween = new();

    public void Enable(CombatDrop drop)
    {
        if (player == null)
            player = Level.Instance.Player;

        currentDisplay = GetDisplayForDrop(drop);

        void onComplete()
        {
            currentDisplay.Enable();
            StartCoroutine(IEWaitForConfirmation(drop));
        }

        container.gameObject.SetActive(true);
        container.localScale = TweenManager.TWEEN_ZERO;
        container.DoTweenScaleNonAlloc(Vector3.one, 0.45f, tween).SetOnComplete(onComplete).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Disable()
    {
        LotSelected = false;
        HealthSelected = false;

        void onComplete()
        {
            UIManager.Instance.ToggleBar(false, null, true);
            container.gameObject.SetActive(false);
            Level.Instance.EndEncounter();
        }

        container.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.45f, tween).SetOnComplete(onComplete); // on complete continue game
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
            while (HealthSelected == false || UIManager.Instance.PlayerHealthbar.IsHealing)
                yield return null;
        }
        else if (drop == CombatDrop.LOT_AND_HEALTH)
        {
            while (LotSelected == false || HealthSelected == false || UIManager.Instance.PlayerHealthbar.IsHealing)
                yield return null;
        }

        currentDisplay.Disable();
    }

    private CombatDropDisplay GetDisplayForDrop(CombatDrop drop)
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
