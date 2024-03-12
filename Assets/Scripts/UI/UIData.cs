using System;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

[CreateAssetMenu(fileName = "New UI Data", menuName = "UI/UI Data")]
public class UIData : ScriptableObject
{
    [Header("Opening")]
    [SerializeField] private Vector3 initialOpenScale;
    [SerializeField] private float openDuration;
    [SerializeField] private EasingFunctions.EasingFunction openEasingFunction;

    [Header("Closing")]
    [SerializeField] private float closeDuration;
    [SerializeField] private EasingFunctions.EasingFunction closeEasingFunction;

    [Header("Movement")]
    [SerializeField] private float moveDuration;
    [SerializeField] private EasingFunctions.EasingFunction moveEasingFunction;

    [Header("Shake")]
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDuration;

    [Header("Disabling")]
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;

    public Color DisabledImageColor => disabledImageColor;
    public Color DisabledTextColor => disabledTextColor;

    public Vector3 InitialOpenScale => initialOpenScale;
    public float OpenDuration => openDuration;
    public EasingFunctions.EasingFunction OpenEasingFunction => openEasingFunction;

    public float CloseDuration => closeDuration;
    public EasingFunctions.EasingFunction CloseEasingFunction => closeEasingFunction;

    public float MoveDuration => moveDuration;
    public EasingFunctions.EasingFunction MoveEasingFunction => moveEasingFunction;

    public float ShakeAmount => shakeAmount;
    public float ShakeDuration => shakeDuration;
}