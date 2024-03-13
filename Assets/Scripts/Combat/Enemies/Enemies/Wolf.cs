using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    private bool blockedLastTurn = false;

    public override bool CorruptHealth => false;

    // 0 is attack
    // 1 is defend
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);
        int lowRange;

        if (Battle.Enemies.Count > 1)
        {
            lowRange = 0;
        }
        else
        {
            lowRange = -2;
        }

        int random = Random.Range(lowRange, 3);

        if (random == 2 && !blockedLastTurn)
        {
            turn.Action = actions[1];
            turn.Target = this;
            blockedLastTurn = true;
        }
        else
        {
            turn.Action = actions[0];
            turn.Target = player;
            blockedLastTurn = false;
        }

        return turn;
    }
}
