using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Satan Attack Visual", menuName = "Turn/Visual/Satan Attack")]
public class SatanAttackVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Satan satan = (Satan)turn.User;

        satan.AttackAction();

        // remove all the player's sins
        satan.StartCoroutine(IERemoveSins(onComplete));
    }

    private IEnumerator IERemoveSins(Action onComplete)
    {
        Player player = Level.Instance.Player;
        WaitForSeconds wait = new(0.7f);

        foreach (Sin sin in player.Sins)
        {
            player.RemoveSin(sin);

            yield return wait;
        }

        onComplete();
    }
}