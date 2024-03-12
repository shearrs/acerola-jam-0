using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Brimstone Visual", menuName = "Turn/Visual/Brimstone")]
public class BrimstoneVisual : TurnActionVisual
{
    private readonly int brimstoneID = Animator.StringToHash("Brimstone");

    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Imp imp = (Imp)turn.User;

        imp.StartCoroutine(IEBrimstone(imp, onComplete));
    }

    private IEnumerator IEBrimstone(Imp imp, Action onComplete)
    {
        Animator animator = imp.Animator;
        animator.Play(brimstoneID);

        while (!animator.IsPlaying("Brimstone"))
            yield return null;

        while (!animator.IsInTransition(0))
            yield return null;

        float speed = animator.speed;
        animator.speed = 0;
        imp.BrimstoneParticles();

        yield return new WaitForSeconds(0.25f);

        animator.speed = speed;

        while (animator.IsInTransition(0))
            yield return null;

        onComplete();
    }
}
