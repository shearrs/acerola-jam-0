using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wait Visual", menuName = "Turn/Visual/Wait")]
public class WaitVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Level.Instance.StartCoroutine(IEDelayOnComplete(onComplete));
    }

    private IEnumerator IEDelayOnComplete(Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);

        onComplete?.Invoke();
    }
}
