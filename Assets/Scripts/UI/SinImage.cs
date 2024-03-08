using CustomUI;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float enableTime;
    [SerializeField] private float disableTime;
    [SerializeField] private Color holyColor;
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Tooltip toolTip;

    private Color symbolInitialColor;
    private Color highlightInitialColor;
    private Tween tween = new();
    private Tween tween2 = new();
    private RectTransform rect;

    private void Awake()
    {
        symbolInitialColor = symbolImage.color;
        highlightInitialColor = highlightImage.color;
        rect = GetComponent<RectTransform>();
    }

    public void Enable()
    {
        gameObject.SetActive(true);

        symbolImage.color = symbolInitialColor;
        highlightImage.color = highlightInitialColor;

        rect.localScale = Vector3.zero;
        rect.DoTweenScaleNonAlloc(Vector3.one, enableTime, tween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Disable()
    {
        UIManager.Instance.Shake(rect, disableTime);
        void scaleTween()
        {
            rect.DoTweenScaleNonAlloc(Vector3.zero, 0.15f, tween2).SetOnComplete(() => gameObject.SetActive(false));
        }

        TweenManager.DoTweenCustomNonAlloc(ColorUpdate, disableTime, tween).SetOnComplete(scaleTween);
    }

    private void ColorUpdate(float percentage)
    {
        symbolImage.color = Color.Lerp(symbolInitialColor, holyColor, percentage);
        highlightImage.color = Color.Lerp(highlightInitialColor, holyColor, percentage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.Disable();
    }
}
