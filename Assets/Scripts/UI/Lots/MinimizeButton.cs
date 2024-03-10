using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using UnityEngine.UI;

public class MinimizeButton : MonoBehaviour
{
    [SerializeField] private RectTransform upArrow;
    [SerializeField] private RectTransform downArrow;
    [SerializeField] private Tween movementTween;
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 closedPosition;
    private readonly Tween rotationTween = new();
    private Button button;
    private RectTransform rect;
    private bool isMinimized;
    private LotsUI lotsUI;

    private void Start()
    {
        lotsUI = UIManager.Instance.LotsUI;
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        isMinimized = false;
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(true);
        rect.anchoredPosition3D = closedPosition;
        rect.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void Enable(bool tween = true)
    {
        if (tween)
        {
            rect.anchoredPosition3D = closedPosition;
            rect.localRotation = Quaternion.Euler(90, 0, 0);
            rect.DoTweenPositionNonAlloc(openPosition, movementTween.Duration, movementTween).SetOnComplete(() => button.enabled = true);
            rect.DoTweenRotationNonAlloc(Quaternion.Euler(0, 0, 0), movementTween.Duration, rotationTween);
        }
        else
            button.enabled = true;
    }

    public void Disable(bool tween = true)
    {
        button.enabled = false;

        if (tween)
        {
            rect.DoTweenPositionNonAlloc(closedPosition, movementTween.Duration, movementTween);
            rect.DoTweenRotationNonAlloc(Quaternion.Euler(90, 0, 0), movementTween.Duration, rotationTween);
        }
    }

    public void ToggleMinimize()
    {
        isMinimized = !isMinimized;

        if (isMinimized)
        {
            downArrow.gameObject.SetActive(false);
            upArrow.gameObject.SetActive(true);
        }
        else
        {
            downArrow.gameObject.SetActive(true);
            upArrow.gameObject.SetActive(false);
        }

        lotsUI.ToggleMinimize(isMinimized);
    }
}
