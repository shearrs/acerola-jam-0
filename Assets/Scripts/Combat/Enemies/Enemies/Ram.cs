using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Enemy
{
    // 0 is attack
    // 1 is wait
    // 2 is demon act
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (turnCounter <= 2 || turnCounter > 5)
        {
            turn.Action = actions[0];
            turn.Target = player;
        }
        else if (turnCounter == 4)
        {
            turn.Action = actions[1];
            turn.Target = this;
        }    
        else if (turnCounter == 5)
        {
            turn.Action = actions[2];
            turn.Target = player;
        }

        return turn;
    }
}
