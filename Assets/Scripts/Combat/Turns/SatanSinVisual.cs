using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Satan Sin Visual", menuName = "Turn/Visual/Satan Sin")]
public class SatanSinVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Satan satan = (Satan)turn.User;
        Player player = Level.Instance.Player;
        LotsBox lotsBox = CombatManager.Instance.LotsBox;

        satan.SinAction();

        // give player a random sin
        // wait for some amount of time
        Sin sin = Sin.GetSinForType(lotsBox.GetRandomSin());
        player.AddSin(sin);

        player.StartCoroutine(IEWait(onComplete));
    }

    private IEnumerator IEWait(Action onComplete)
    {
        yield return new WaitForSeconds(0.8f);

        onComplete?.Invoke();
    }
}
