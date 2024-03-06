using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using System.Security.Cryptography;

public class CameraManager : Singleton<CameraManager>
{
    private Camera cam;
    private Vector3 originalCamPosition;
    private readonly Tween shakeTween = new();

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main;
    }

    public void Shake(float amount = 0.1f, float duration = 0.25f)
    {
        originalCamPosition = Camera.main.transform.localPosition;

        TweenManager.DoTweenCustomNonAlloc(
                                            (percentage) => ShakeUpdate(percentage, amount), duration, shakeTween
                                          )
                                          .SetOnComplete(ResetCamera);
    }

    private void ShakeUpdate(float percentage, float amount)
    {
        float magnitude = amount * (1 - percentage);
        cam.transform.localPosition = originalCamPosition + Random.insideUnitSphere * magnitude;
    }

    private void ResetCamera()
    {
        cam.transform.localPosition = originalCamPosition;
    }
}
