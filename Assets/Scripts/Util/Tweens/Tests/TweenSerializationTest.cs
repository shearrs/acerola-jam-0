using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class TweenSerializationTest : MonoBehaviour
{
    public enum TweenType { POSITION, ROTATION, SCALE, CUSTOM };

    [Header("Control")]
    [SerializeField] private bool run = false;
    [SerializeField] private bool reset = false;

    [Header("Test Type")]
    [SerializeField] private TweenType tweenType;

    [Header("Test Customization")]
    [SerializeField] private Vector3 positionOffset = new(5, 0, 2);
    [SerializeField] private Vector3 rotationOffset = new(90, 360, 45);
    [SerializeField] private Vector3 scaleOffset = new(2, 2, 2);

    [Header("Tween Attributes")]
    [SerializeField] private Tween tween;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

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
        tween.Stop();

        ResetTransform();

        if (tweenType == TweenType.CUSTOM) CustomFunction();
        else NonAllocFunctions();

        tween.Start();
    }

    private void NonAllocFunctions()
    {
        switch (tweenType)
        {
            case TweenType.POSITION:
                transform.CreateTweenPositionNonAlloc(transform.TransformPoint(positionOffset), tween.Duration, tween);
                break;
            case TweenType.ROTATION:
                transform.CreateTweenRotationNonAlloc(Quaternion.Euler(rotationOffset), tween.Duration, tween);
                break;
            case TweenType.SCALE:
                transform.CreateTweenScaleNonAlloc(scaleOffset, tween.Duration, tween);
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

        TweenManager.CreateTweenCustomNonAlloc(update, tween.Duration, tween);
    }

    private void ResetTransform()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
    }
}