using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using CustomUI;

public class PetitionSelection : MonoBehaviour
{
    [SerializeField] private PurifyButton healButton;
    [SerializeField] private PurifyButton purifyButton;
    [SerializeField] private PetitionCancelButton cancelButton;
    [SerializeField] private Vector3 startPosition;
    private Vector3 openPosition;
    private readonly Tween tween = new();
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        openPosition = rect.anchoredPosition3D;
    }

    public void Enable()
    {
        void onComplete()
        {
            healButton.Enable();
            cancelButton.Enable();

            if (Level.Instance.Player.SinCount > 0 && PetitionManager.Instance.LotsBox.GetAmountOfType(LotType.HOLY) >= 3)
                purifyButton.Enable();
        }

        UIManager.Instance.ActionUI.SetActions(false);
        gameObject.SetActive(true);
        cancelButton.Disable();
        healButton.Disable();
        purifyButton.Disable();
        rect.anchoredPosition3D = startPosition;
        rect.DoTweenPositionNonAlloc(openPosition, 0.2f, tween).SetOnComplete(onComplete);
    }

    public void Disable()
    {
        UIManager.Instance.ActionUI.SetActions(true);
        healButton.Disable();
        purifyButton.Disable();
        cancelButton.Disable();
        rect.DoTweenPositionNonAlloc(startPosition, 0.2f, tween).SetOnComplete(() => gameObject.SetActive(false));
    }
}
