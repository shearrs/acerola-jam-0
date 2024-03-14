using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using UnityEngine.UI;

public class HelpMenu : Singleton<HelpMenu>
{
    [SerializeField] private RectTransform background;
    [SerializeField] private Image backDrop;
    [SerializeField] private List<RectTransform> pages;
    [SerializeField] private RectTransform nextArrow;
    [SerializeField] private RectTransform prevArrow;
    private AudioManager audioManager;
    private Color backgroundColor;
    private Color transparentBackgroundColor;
    private readonly Tween transparencyTween = new();
    private readonly Tween tween = new();
    private bool isEnabled = false;
    private RectTransform currentPage;
    private int currentPageIndex = 0;

    private void Start()
    {
        backgroundColor = backDrop.color;
        transparentBackgroundColor = backgroundColor;
        transparentBackgroundColor.a = 0;
        audioManager = AudioManager.Instance;
    }

    public void ToggleHelpMenu()
    {
        if (isEnabled)
        {
            transform.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.5f, tween).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK).SetOnComplete(OnTweenOut);

            TweenManager.DoTweenCustomNonAlloc(UnTransparencyUpdate, 0.5f, transparencyTween).SetOnComplete(() => backDrop.gameObject.SetActive(false));
            currentPageIndex = -1;
        }
        else
        {
            OpenPage(0);
            background.gameObject.SetActive(true);
            transform.localScale = TweenManager.TWEEN_ZERO;
            transform.DoTweenScaleNonAlloc(Vector3.one, 0.5f, tween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);

            // backdrop
            backDrop.gameObject.SetActive(true);
            backDrop.color = transparentBackgroundColor;

            TweenManager.DoTweenCustomNonAlloc(TransparencyUpdate, 0.5f, transparencyTween);
        }

        isEnabled = !isEnabled;
    }

    private void OnTweenOut()
    {
        currentPage.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelpMenu();
        }
        else if (!tween.IsPlaying && Input.GetKeyDown(KeyCode.E))
        {
            if (currentPageIndex != pages.Count - 1)
                OpenPage(currentPageIndex + 1);
        }
        else if (!tween.IsPlaying && Input.GetKeyDown(KeyCode.Q))
        {
            if (currentPageIndex != 0)
                OpenPage(currentPageIndex - 1);
        }
    }

    public void OpenPage(int pageIndex)
    {
        if (pageIndex > currentPageIndex)
            audioManager.UISource.pitch = 1f;
        else
            audioManager.UISource.pitch = 0.75f;

        audioManager.ButtonSound(2, false);

        nextArrow.gameObject.SetActive(pageIndex < pages.Count - 1);
        prevArrow.gameObject.SetActive(pageIndex > 0);

        currentPageIndex = pageIndex;

        if (currentPage != null)
            currentPage.gameObject.SetActive(false);

        currentPage = pages[pageIndex];
        currentPage.gameObject.SetActive(true);
    }

    private void TransparencyUpdate(float percentage)
    {
        backDrop.color = Color.Lerp(transparentBackgroundColor, backgroundColor, percentage);
    }

    private void UnTransparencyUpdate(float percentage)
    {
        backDrop.color = Color.Lerp(backgroundColor, transparentBackgroundColor, percentage);
    }
}