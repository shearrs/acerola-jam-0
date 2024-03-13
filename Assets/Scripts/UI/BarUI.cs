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
    private RectTransform rect;

    public void Enable(Action onComplete = null)
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        rect.anchoredPosition = startPosition;
        gameObject.SetActive(true);
        rect.DoTweenPositionNonAlloc(endPosition, 1f, tween);

        if (onComplete != null)
            tween.SetOnComplete(onComplete);
    }

    public void Disable(Action onComplete = null)
    {
        rect.DoTweenPositionNonAlloc(startPosition, 1f, tween).SetOnComplete(() => gameObject.SetActive(false));

        if (onComplete != null)
            tween.SetOnComplete(onComplete);
    }
}
