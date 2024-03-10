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
    [SerializeField] private Color livingColor;
    [SerializeField] private Color corruptColor;
    private SpriteRenderer spriteRenderer;

    private readonly Tween growTween = new();
    private readonly Tween shakeTween = new();

    private bool alive;
    private bool corruptHeart = false;
    public bool CorruptHeart 
    {
        get => corruptHeart;
        set
        {
            if (value)
                spriteRenderer.color = corruptColor;

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
        transform.localScale = Vector3.zero;
        transform.DoTweenScaleNonAlloc(scale, spawnDuration, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);

        alive = true;
    }

    public void Heal()
    {
        if (!alive)
            spriteRenderer.color = livingColor;
        else
            spriteRenderer.color = corruptColor;

        alive = true;
        transform.DoTweenScaleNonAlloc(Vector3.one * 1.25f, 0.15f, growTween).SetOnComplete(() => transform.DoTweenScaleNonAlloc(Vector3.one, 0.15f, growTween));
    }

    public void Damage()
    {
        if (CorruptHeart)
        {
            spriteRenderer.color = livingColor;
            CorruptHeart = false;
        }
        else
        {
            spriteRenderer.color = deadColor;
            alive = false;
        }

        if (!shakeTween.IsPlaying)
            transform.Shake(shakeAmount, shakeDuration, shakeTween);
    }
}
