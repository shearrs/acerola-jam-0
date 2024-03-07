using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action Animation", menuName = "Turn/Visual/Animation")]
public class TurnActionAnimation : TurnActionVisual
{
    private const string NULL_ANIMATION = "null";
    [SerializeField] private string animationName = NULL_ANIMATION;

    public override void PerformVisual(Turn turn, Action onComplete)
    {
        turn.User.Animator.PlayAndNotify(Level.Instance, animationName, onComplete);
    }
}
