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
    [SerializeField] private MainMenuButton mainMenuButton;
    private Color backgroundColor;
    private Color transparentBackgroundColor;

    private readonly Tween transparencyTween = new();
    private readonly Tween textTween = new();
    private readonly Tween buttonTween = new();

    public void Enable()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayAmbience(audioManager.AmbientOoo);
        audioManager.PlaySong(null, 0.5f);

        background.gameObject.SetActive(true);
        backgroundColor = background.color;
        transparentBackgroundColor = backgroundColor;
        transparentBackgroundColor.a = 0;
        background.color = transparentBackgroundColor;

        TweenManager.DoTweenCustomNonAlloc(TransparencyUpdate, 1f, transparencyTween).SetOnComplete(TweenElements);
    }

    private void TweenElements()
    {
        text.gameObject.SetActive(true);
        text.localScale = TweenManager.TWEEN_ZERO;
        text.DoTweenScaleNonAlloc(Vector3.one, 0.35f, textTween);

        mainMenuButton.gameObject.SetActive(true);
        mainMenuButton.Disable();

        RectTransform buttonRect = mainMenuButton.GetComponent<RectTransform>();
        buttonRect.localScale = TweenManager.TWEEN_ZERO;
        buttonRect.DoTweenScaleNonAlloc(Vector3.one, 0.35f, buttonTween).SetOnComplete(() => mainMenuButton.Enable());
    }

    private void TransparencyUpdate(float percentage)
    {
        background.color = Color.Lerp(transparentBackgroundColor, backgroundColor, percentage);
    }
}
