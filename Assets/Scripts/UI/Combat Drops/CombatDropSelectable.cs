using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatDropSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private bool lot;
    [SerializeField] private RectTransform highlight;

    [Header("Tweens")]
    [SerializeField] private float tweenTime;
    private bool selected = false;
    private readonly Tween tween = new();

    public void Enable()
    {
        gameObject.SetActive(true);
        transform.localScale = TweenManager.TWEEN_ZERO;
        transform.DoTweenScaleNonAlloc(Vector3.one, tweenTime, tween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Disable()
    {
        selected = false;
        transform.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.2f, tween).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK).SetOnComplete(() => gameObject.SetActive(false));
    }

    private void Update()
    {
        if (!tween.IsPlaying && selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Player player = Level.Instance.Player;

                if (lot)
                {
                    player.LotCapacity++;
                    AudioManager.Instance.LotSound();
                    CombatDropUI.Instance.LotSelected = true;
                }
                else
                {
                    player.Heal(player.MaxHealth);
                    AudioManager.Instance.HealthSound();
                    CombatDropUI.Instance.HealthSelected = true;
                }

                Disable();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (lot)
            AudioManager.Instance.HighlightSound();
        else
            AudioManager.Instance.HighlightSound(2);

        highlight.gameObject.SetActive(true);
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.gameObject.SetActive(false);
        selected = false;
    }
}
