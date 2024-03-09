using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class HealthTick : MonoBehaviour
{
    [SerializeField] private float spawnDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;
    [SerializeField] private Color deadColor;
    private SpriteRenderer spriteRenderer;

    private readonly Tween growTween = new();
    private readonly Tween shakeTween = new();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Spawn()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DoTweenScaleNonAlloc(scale, spawnDuration, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Heal()
    {
        spriteRenderer.color = Color.white;
        transform.DoTweenScaleNonAlloc(Vector3.one * 1.25f, 0.15f, growTween).SetOnComplete(() => transform.DoTweenScaleNonAlloc(Vector3.one, 0.15f, growTween));
    }

    public void Damage()
    {
        spriteRenderer.color = deadColor;
        transform.Shake(shakeAmount, shakeDuration, shakeTween);
    }
}
