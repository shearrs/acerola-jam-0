using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class PetitionSelection : MonoBehaviour
{
    [SerializeField] private PurifyButton healButton;
    [SerializeField] private PurifyButton purifyButton;
    [SerializeField] private Vector3 startPosition;
    private Vector3 openPosition;
    private Tween tween = new();
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        openPosition = rect.anchoredPosition3D;
    }

    private void Start()
    {

    }

    public void Enable()
    {
        void onComplete()
        {
            healButton.Enable();

            if (Level.Instance.Player.SinCount > 0 && PetitionManager.Instance.LotsBox.GetAmountOfType(LotType.HOLY) >= 3)
                purifyButton.Enable();
        }

        gameObject.SetActive(true);
        healButton.gameObject.SetActive(true);
        purifyButton.gameObject.SetActive(true);
        healButton.Disable();
        purifyButton.Disable();
        rect.anchoredPosition3D = startPosition;
        rect.DoTweenPositionNonAlloc(openPosition, 0.2f, tween).SetOnComplete(onComplete);
    }

    public void Disable()
    {
        healButton.Disable();
        purifyButton.Disable();
        rect.DoTweenPositionNonAlloc(startPosition, 0.2f, tween).SetOnComplete(() => gameObject.SetActive(false));
    }
}
