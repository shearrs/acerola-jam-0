using System;
using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    #region Animator
    public static void PlayAndNotify(this Animator animator, MonoBehaviour coroutineRunner, string animationName, Action onComplete)
    {
        coroutineRunner.StartCoroutine(IEWaitForAnimation(animator, animationName, onComplete));
    }

    public static IEnumerator IEWaitForAnimation(Animator animator, string animationName, Action onComplete)
    {
        float length;
        float time = 0;

        animator.Play(animationName);

        while (!animator.IsPlaying(animationName))
            yield return null;

        length = animator.GetCurrentAnimatorStateInfo(0).length;

        while ((animator.IsPlaying(animationName) && time < length) || animator.IsInTransition(0))
        {
            time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            yield return null;
        }

        onComplete?.Invoke();
    }

    public static bool IsPlaying(this Animator animator, string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
    #endregion

    #region Quaternions
    // attribution:
    // from adamgolden on the unity forums: https://forum.unity.com/threads/quaternion-smoothdamp.793533/#post-5284317
    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        if (Time.deltaTime == 0)
            return current;

        if (smoothTime == 0)
            return target;

        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
          Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
          Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }
    #endregion
}
