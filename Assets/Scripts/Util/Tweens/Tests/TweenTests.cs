using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class TweenTests : MonoBehaviour
{
    public enum TweenType { POSITION, ROTATION, SCALE, CUSTOM };
    public enum AllocType { ALLOC, NONALLOC };

    [Header("Control")]
    [SerializeField] private bool run = false;
    [SerializeField] private bool reset = false;

    [Header("Test Type")]
    [SerializeField] private TweenType tweenType;
    [SerializeField] private AllocType allocType;

    [Header("Test Customization")]
    [SerializeField] private Vector3 positionOffset = new(5, 0, 2);
    [SerializeField] private Vector3 rotationOffset = new(90, 360, 45);
    [SerializeField] private Vector3 scaleOffset = new(2, 2, 2);

    [Header("Tween Attributes")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool looping;
    [SerializeField] private Tween.LoopType loopType = Tween.LoopType.REPEAT;
    [SerializeField] private EasingFunctions.EasingFunction easingFunction;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private Tween tween;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (reset)
        {
            ResetTransform();
            reset = false;
        }

        if (run)
        {
            RunTest();
            run = false;
        }
    }

    private void RunTest()
    {
        if (tween == null)
            tween = new();
        else
            tween.Stop();

        ResetTransform();

        if (tweenType == TweenType.CUSTOM) CustomFunction();
        else if (allocType == AllocType.ALLOC) tween = AllocFunctions();
        else NonAllocFunctions();

        int loops = (looping) ? -1 : 0;
        tween.SetLooping(loopType, loops);
        tween.SetEasingFunction(easingFunction);

        tween.Start();
    }

    private Tween AllocFunctions()
    {
        return tweenType switch
        {
            TweenType.POSITION => transform.CreateTweenPosition(transform.TransformPoint(positionOffset), duration),
            TweenType.ROTATION => transform.CreateTweenRotation(Quaternion.Euler(rotationOffset), duration),
            TweenType.SCALE => transform.CreateTweenScale(scaleOffset, duration),
            _ => null,
        };
    }

    private void NonAllocFunctions()
    {
        switch (tweenType)
        {
            case TweenType.POSITION:
                transform.CreateTweenPositionNonAlloc(transform.TransformPoint(positionOffset), duration, tween);
                break;
            case TweenType.ROTATION:
                transform.CreateTweenRotationNonAlloc(Quaternion.Euler(rotationOffset), duration, tween);
                break;
            case TweenType.SCALE:
                transform.CreateTweenScaleNonAlloc(scaleOffset, duration, tween);
                break;
        }
    }

    private void CustomFunction()
    {
        void update(float percentage)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(originalPosition, positionOffset, percentage), 
                Quaternion.Lerp(originalRotation, Quaternion.Euler(rotationOffset), percentage)
                );
            transform.localScale = Vector3.Lerp(originalScale, scaleOffset, percentage);
        }

        if (allocType == AllocType.ALLOC)
            tween = TweenManager.CreateTweenCustom(update, 1.5f);
        else
            TweenManager.CreateTweenCustomNonAlloc(update, 1.5f, tween);
    }

    private void ResetTransform()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
    }
}