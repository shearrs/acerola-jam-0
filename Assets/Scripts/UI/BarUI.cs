using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using System;

public class BarUI : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    private readonly Tween tween = new();

    public void Enable(Action onComplete = null)
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
        transform.DoTweenPositionNonAlloc(endPosition, 1f, tween);

        if (onComplete != null)
            tween.SetOnComplete(onComplete);
    }

    public void Disable(Action onComplete = null)
    {
        transform.DoTweenPositionNonAlloc(startPosition, 1f, tween).SetOnComplete(() => gameObject.SetActive(false));

        if (onComplete != null)
            tween.SetOnComplete(onComplete);
    }
}
