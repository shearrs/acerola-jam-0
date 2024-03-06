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
        Level.Instance.StartCoroutine(IEWaitForAnimation(turn, onComplete));
    }

    private IEnumerator IEWaitForAnimation(Turn turn, Action onComplete)
    {
        Animator animator = turn.User.Animator;
        float length;
        float time = 0;

        animator.Play(animationName);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            yield return null;

        length = animator.GetCurrentAnimatorStateInfo(0).length;

        while (time < length || animator.IsInTransition(0))
        {
            time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            yield return null;
        }

        onComplete?.Invoke();
    }
}
