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
        int count = player.SinCount;

        Debug.Log("count: " + count);

        CameraManager.Instance.Shake(0.05f, 6.5f);

        for(int i = 0; i < count; i++)
        {
            Sin sin = player.Sins[0];
            player.RemoveSin(sin);

            Debug.Log("removed sin: " + sin.GetSinType());

            yield return wait;
        }

        yield return wait;

        onComplete();
    }
}