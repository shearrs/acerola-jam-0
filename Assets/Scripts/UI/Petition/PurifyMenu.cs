using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class PurifyMenu : MonoBehaviour
{
    private readonly Tween tween = new();
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        rect.localScale = TweenManager.TWEEN_ZERO;
        rect.DoTweenScaleNonAlloc(Vector3.one, 0.2f, tween);
    }

    public void Disable() 
    {
        void onComplete()
        {
            gameObject.SetActive(false);
        }

        rect.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.2f, tween).SetOnComplete(onComplete);

        Player player = Level.Instance.Player;
        player.PurifyingSin = false;
        player.SelectedSin = null;
    }
}
