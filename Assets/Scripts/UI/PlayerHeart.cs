using CustomUI;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeart : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color livingColor;
    [SerializeField] private Color deadColor;
    [SerializeField] private float spawnDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;
    private RectTransform rect;
    private readonly Tween tween1 = new();
    private readonly Tween tween2 = new();

    public RectTransform RectTransform
    {
        get
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            return rect;
        }
    }

    private void OnEnable()
    {
        Vector3 scale = rect.localScale;
        rect.localScale = TweenManager.TWEEN_ZERO;
        rect.DoTweenScaleNonAlloc(scale, spawnDuration, tween1).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Heal()
    {
        Vector3 scale = rect.localScale;
        image.color = livingColor;
        rect.DoTweenScaleNonAlloc(scale * 1.25f, 0.15f, tween1).SetOnComplete(() => rect.DoTweenScaleNonAlloc(scale, 0.15f, tween2));
    }

    public void Damage()
    {
        image.color = deadColor;
        UIManager.Instance.Shake(rect);
    }
}
