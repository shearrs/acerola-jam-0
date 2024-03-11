using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private readonly Tween tween = new();
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Enable()
    {
        gameObject.SetActive(true);

        rect.localScale = TweenManager.TWEEN_ZERO;
        rect.DoTweenScaleNonAlloc(Vector3.one, 0.125f, tween);
    }

    public void Disable()
    {
        rect.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.1f, tween).SetOnComplete(() => gameObject.SetActive(false));
    }
}
