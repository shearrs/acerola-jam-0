using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private MainMenuButton mainMenuButton;
    private Color backgroundColor;
    private Color backgroundColorTransparent;
    private readonly Tween transparencyTween = new();

    public void Enable()
    {
        backgroundColor = background.color;
        backgroundColorTransparent = background.color;
        backgroundColorTransparent.a = 0;

        gameObject.SetActive(true);
        background.color = backgroundColorTransparent;

        TweenManager.CreateTweenCustomNonAlloc(TransparencyUpdate, 2.5f, transparencyTween).SetOnComplete(() => mainMenuButton.Enable());
    }

    private void TransparencyUpdate(float percentage)
    {
        background.color = Color.Lerp(backgroundColorTransparent, backgroundColor, percentage);
    }
}