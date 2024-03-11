using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class HealthTick : MonoBehaviour
{
    [SerializeField] private float spawnDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;
    [SerializeField] private Color deadColor;
    [SerializeField] private Transform corruptOverlay;
    private SpriteRenderer spriteRenderer;

    private readonly Tween growTween = new();
    private readonly Tween corruptTween1 = new();
    private readonly Tween corruptTween2 = new();
    private readonly Tween shakeTween = new();

    private bool alive;
    private bool corruptHeart = false;
    public bool CorruptHeart 
    {
        get => corruptHeart;
        set
        {
            if (value)
                EnableCorruptHeart();

            corruptHeart = value;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Spawn()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = TweenManager.TWEEN_ZERO;
        transform.DoTweenScaleNonAlloc(scale, spawnDuration, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);

        alive = true;
    }

    public void Heal()
    {
        if (!alive)
            spriteRenderer.color = Color.white;
        else
            EnableCorruptHeart();

        alive = true;
        transform.DoTweenScaleNonAlloc(Vector3.one * 1.25f, 0.15f, growTween).SetOnComplete(() => transform.DoTweenScaleNonAlloc(Vector3.one, 0.15f, growTween));
    }

    public void Damage()
    {
        if (CorruptHeart)
        {
            DisableCorruptHeart();
        }
        else
        {
            spriteRenderer.color = deadColor;
            alive = false;
        }

        if (!shakeTween.IsPlaying)
            transform.Shake(shakeAmount, shakeDuration, shakeTween);
    }

    private void EnableCorruptHeart()
    {
        corruptOverlay.gameObject.SetActive(true);
        corruptOverlay.localScale = TweenManager.TWEEN_ZERO;
        corruptOverlay.DoTweenScaleNonAlloc(Vector3.one * 1.3f, 0.15f, corruptTween1).SetOnComplete(() => corruptOverlay.DoTweenScaleNonAlloc(Vector3.one * 1.2f, 0.15f, corruptTween2));
    }

    private void DisableCorruptHeart()
    {
        CorruptHeart = false;
        corruptOverlay.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.15f, corruptTween1).SetOnComplete(() => corruptOverlay.gameObject.SetActive(false));
    }
}
