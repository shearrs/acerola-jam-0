using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    public override bool CorruptHealth => false;

    // 0 is attack
    // 1 is defend
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);
        int lowRange;

        if (Battle.Enemies.Count > 1)
        {
            lowRange = -2;
        }
        else
        {
            lowRange = 0;
        }

        int random = Random.Range(lowRange, 3);

        if (random <= 2)
        {
            turn.Action = actions[0];
            turn.Target = player;
        }
        else
        {
            turn.Action = actions[1];
            turn.Target = this;
        }

        return turn;
    }
}
