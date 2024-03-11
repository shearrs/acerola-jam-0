using CustomUI;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SinType type;
    [SerializeField] private float enableTime;
    [SerializeField] private float disableTime;
    [SerializeField] private Color holyColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color activeHighlightColor;
    [SerializeField] private Image symbolImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Tooltip toolTip;

    private Color symbolInitialColor;
    private Color highlightInitialColor;
    private readonly Tween tween = new();
    private readonly Tween tween2 = new();
    private RectTransform rect;
    private bool selected = false;

    private void Awake()
    {
        symbolInitialColor = symbolImage.color;
        highlightInitialColor = highlightImage.color;
        rect = GetComponent<RectTransform>();
    }

    public void OnActivation()
    {
        UIManager.Instance.Shake(rect, .75f);
        float duration = 0.3f;
        rect.DoTweenScaleNonAlloc(Vector3.one * 1.5f, 0.3f, tween).SetOnComplete(() => Invoke(nameof(UnTweenActivation), 0.5f));

        void colorUpdate(float percentage)
        {
            highlightImage.color = Color.Lerp(highlightInitialColor, activeHighlightColor, percentage);
        }
        TweenManager.DoTweenCustomNonAlloc(colorUpdate, duration, tween2);
    }

    private void UnTweenActivation()
    {
        float duration = 0.25f;
        rect.DoTweenScaleNonAlloc(Vector3.one, duration, tween);

        void colorUpdate(float percentage)
        {
            highlightImage.color = Color.Lerp(activeHighlightColor, highlightInitialColor, percentage);
        }
        TweenManager.DoTweenCustomNonAlloc(colorUpdate, duration, tween2);
    }

    public void Enable()
    {
        gameObject.SetActive(true);

        symbolImage.color = symbolInitialColor;
        highlightImage.color = highlightInitialColor;

        rect.localScale = TweenManager.TWEEN_ZERO;
        rect.DoTweenScaleNonAlloc(Vector3.one, enableTime, tween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);

        AudioManager.Instance.SinSound();
    }

    public void Disable()
    {
        UIManager.Instance.Shake(rect, disableTime);
        void scaleTween()
        {
            rect.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.15f, tween2).SetOnComplete(() => gameObject.SetActive(false));

            Player player = Level.Instance.Player;

            if (player.PurifyingSin && player.SelectedSin.GetSinType() == type)
                PetitionManager.Instance.PurifyMenu.Disable();
        }

        TweenManager.DoTweenCustomNonAlloc(ColorUpdate, disableTime, tween).SetOnComplete(scaleTween);
        AudioManager.Instance.PurifySound();
    }

    private void Update()
    {
        Player player = Level.Instance.Player;

        if (player.PurifyingSin && selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                toolTip.Disable();
                player.SelectedSin = player.GetSin(type);
                PetitionManager.Instance.Disable();
            }
        }
    }

    private void ColorUpdate(float percentage)
    {
        symbolImage.color = Color.Lerp(symbolInitialColor, holyColor, percentage);
        highlightImage.color = Color.Lerp(highlightInitialColor, holyColor, percentage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected)
            toolTip.Enable();

        if (tween.IsPlaying)
            return;

        AudioManager.Instance.HighlightSound(2);
        highlightImage.color = highlightColor;
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.Disable();
        highlightImage.color = highlightInitialColor;
        selected = false;
    }
}
