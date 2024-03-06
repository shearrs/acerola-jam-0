using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class HealthTick : MonoBehaviour
{
    [SerializeField] private float spawnDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;

    private readonly Tween growTween = new();
    private readonly Tween shakeTween = new();

    public void Spawn()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DoTweenScaleNonAlloc(scale, spawnDuration, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Damage()
    {
        transform.DoTweenScaleNonAlloc(Vector3.zero, shakeDuration, growTween);
        transform.Shake(shakeAmount, shakeDuration, shakeTween).SetOnComplete(() => gameObject.SetActive(false));
    }
}
