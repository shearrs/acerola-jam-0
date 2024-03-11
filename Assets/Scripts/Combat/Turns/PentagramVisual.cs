using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pentagram Visual", menuName = "Turn/Visual/Pentagram")]
public class PentagramVisual : TurnActionVisual
{
    [SerializeField] private AudioClip pentagramSound2;

    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Ram ram = (Ram)turn.User;
        Transform pentagram = ram.Pentagram;
        ram.Healthbar.Disable();

        // float up in air
        ram.Animator.PlayAndNotify(Level.Instance, "DemonAct", () => ram.StartCoroutine(IEWait(0.5f, ram, onComplete))); // play float animation

        Level.Instance.StartStorm(1.5f); // make sky stormy

        ram.PentagramSound();
        pentagram.gameObject.SetActive(true);
        pentagram.localScale = Vector3.zero;
        pentagram.DoTweenScale(Vector3.one, 1.5f); // scale tween
    }

    private IEnumerator IEWait(float time, Ram ram, Action onComplete)
    {
        float elapsedTime = 0;

        AudioManager.Instance.PlaySound(pentagramSound2, 0.4f);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        ram.Pentagram.DoTweenScale(Vector3.zero, 1f).SetOnComplete(() => Finish(ram, onComplete));
    }

    private void Finish(Ram ram, Action onComplete)
    {
        ram.Pentagram.gameObject.SetActive(false);
        ram.Healthbar.Enable();

        ram.Animator.SetTrigger("stopFloating"); // trigger the animator to go back to idle
        ram.StartCoroutine(IEWaitForAnimator(ram.Animator, onComplete));
    }

    private IEnumerator IEWaitForAnimator(Animator animator, Action onComplete)
    {
        while (animator.IsPlaying("DemonFloat"))
            yield return null;

        onComplete?.Invoke();
    }
}