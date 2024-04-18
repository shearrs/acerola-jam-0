using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private RectTransform text;
    [SerializeField] private RetryButton retryButton;
    [SerializeField] private MainMenuButton mainMenuButton;
    private Color backgroundColor;
    private Color transparentBackgroundColor;

    private readonly Tween transparencyTween = new();
    private readonly Tween textTween = new();
    private readonly Tween buttonTween = new();
    private readonly Tween buttonTween2 = new();

    private void Start()
    {
        backgroundColor = background.color;
        transparentBackgroundColor = backgroundColor;
        transparentBackgroundColor.a = 0;
    }

    public void Enable()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayAmbience(audioManager.AmbientOoo);
        audioManager.PlaySong(null, 0.5f);

        background.gameObject.SetActive(true);
        background.color = transparentBackgroundColor;

        TweenManager.DoTweenCustomNonAlloc(TransparencyUpdate, 1f, transparencyTween).SetOnComplete(TweenElements);
    }

    public void Disable()
    {
        UnTweenElements();
        TweenManager.DoTweenCustomNonAlloc(UnTransparencyUpdate, 0.5f, transparencyTween).SetOnComplete(() => background.gameObject.SetActive(false));
    }

    private void TweenElements()
    {
        text.gameObject.SetActive(true);
        text.localScale = TweenManager.TWEEN_ZERO;
        text.DoTweenScaleNonAlloc(Vector3.one, 0.35f, textTween);

        mainMenuButton.gameObject.SetActive(true);
        mainMenuButton.Disable();

        retryButton.gameObject.SetActive(true);
        retryButton.Disable();

        RectTransform mButtonRect = mainMenuButton.GetComponent<RectTransform>();
        mButtonRect.localScale = TweenManager.TWEEN_ZERO;
        mButtonRect.DoTweenScaleNonAlloc(Vector3.one, 0.35f, buttonTween).SetOnComplete(() => mainMenuButton.Enable());

        RectTransform rButtonRect = retryButton.GetComponent<RectTransform>();
        rButtonRect.localScale = TweenManager.TWEEN_ZERO;
        rButtonRect.DoTweenScaleNonAlloc(Vector3.one, 0.35f, buttonTween2).SetOnComplete(() => retryButton.Enable());
    }

    private void TransparencyUpdate(float percentage)
    {
        background.color = Color.Lerp(transparentBackgroundColor, backgroundColor, percentage);
    }

    private void UnTweenElements()
    {
        text.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.35f, textTween);
        mainMenuButton.Disable();
        retryButton.Disable();

        RectTransform mButtonRect = mainMenuButton.GetComponent<RectTransform>();
        RectTransform rButtonRect = retryButton.GetComponent<RectTransform>();

        mButtonRect.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.35f, buttonTween).SetOnComplete(() => mainMenuButton.gameObject.SetActive(false));
        rButtonRect.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.35f, buttonTween2).SetOnComplete(() => retryButton.gameObject.SetActive(false));
    }

    private void UnTransparencyUpdate(float percentage)
    {
        background.color = Color.Lerp(backgroundColor, transparentBackgroundColor, percentage);
    }
}
