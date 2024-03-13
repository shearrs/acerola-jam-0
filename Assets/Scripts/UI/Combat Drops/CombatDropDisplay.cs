using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class CombatDropDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform text;
    [SerializeField] private CombatDropSelectable[] selectables;

    [Header("Tweens")]
    [SerializeField] private float textTweenTime;
    private readonly Tween textTween = new();

    public void Enable()
    {
        gameObject.SetActive(true);

        text.localScale = TweenManager.TWEEN_ZERO;
        text.gameObject.SetActive(true);
        text.DoTweenScaleNonAlloc(Vector3.one, textTweenTime, textTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK).SetOnComplete(EnableSelectables);
    }

    private void EnableSelectables()
    {
        foreach(CombatDropSelectable selectable in selectables)
            selectable.Enable();
    }

    public void Disable()
    {
        void onComplete()
        {
            text.gameObject.SetActive(false);

            CombatDropUI.Instance.Disable();

            gameObject.SetActive(false);
        }

        text.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.3f, textTween).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK).SetOnComplete(onComplete);
    }
}