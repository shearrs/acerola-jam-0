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
        UIManager.Instance.Shake(rect, .5f);
        rect.DoTweenScaleNonAlloc(Vector3.one * 1.5f, 0.3f, tween).SetOnComplete(() => Invoke(nameof(UnTweenActivation), 0.25f));
    }

    private void UnTweenActivation()
    {
        rect.DoTweenScaleNonAlloc(Vector3.one, 0.15f, tween);
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

            Player player = Level.Instance.Player;

            if (player.PurifyingSin && player.SelectedSin.GetSinType() == type)
                PetitionManager.Instance.PurifyMenu.Disable();
        }

        TweenManager.DoTweenCustomNonAlloc(ColorUpdate, disableTime, tween).SetOnComplete(scaleTween);
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
