using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using TMPro;

public class DefenseTick : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float spawnDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;

    private readonly Tween growTween = new();
    private readonly Tween shakeTween = new();
    private int defense = 0;
    private bool isEnabled = false;

    public void Enable()
    {
        sprite.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DoTweenScaleNonAlloc(Vector3.one, spawnDuration, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
        isEnabled = true;
    }

    public void Disable()
    {
        void onComplete()
        {
            sprite.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }

        transform.DoTweenScaleNonAlloc(Vector3.zero, shakeDuration, growTween).SetOnComplete(onComplete);
        isEnabled = false;
    }

    public void UpdateDefense(int defense)
    {
        Debug.Log("update defense: " + defense);
        Debug.Log("enabled: " + isEnabled);

        if (this.defense > defense)
            transform.Shake(shakeAmount, shakeDuration, shakeTween);

        this.defense = defense;
        text.text = "x" + defense;

        if (isEnabled && defense == 0)
            Disable();
        else if (!isEnabled && defense > 0)
            Enable();
    }
}
