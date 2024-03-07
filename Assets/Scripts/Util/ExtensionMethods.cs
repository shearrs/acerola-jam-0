using System;
using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    public static void PlayAndNotify(this Animator animator, MonoBehaviour coroutineRunner, string animationName, Action onComplete)
    {
        coroutineRunner.StartCoroutine(IEWaitForAnimation(animator, animationName, onComplete));
    }

    public static IEnumerator IEWaitForAnimation(Animator animator, string animationName, Action onComplete)
    {
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
