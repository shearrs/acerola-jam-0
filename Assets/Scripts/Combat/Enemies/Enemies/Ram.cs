using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Enemy
{
    [SerializeField] private Transform pentagram;
    [SerializeField] private AudioSource audioSource;
    private int attackCounter = 0;

    public Transform Pentagram => pentagram;

    public override bool CorruptHealth => false;

    // 0 is attack
    // 1 is wait
    // 2 is demon act
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (turnCounter <= 4)
        {
            if (turnCounter <= 2)
            {
                turn.Action = actions[0];
                turn.Target = player;
            }
            else if (turnCounter == 3)
            {
                turn.Action = actions[1];
                turn.Target = this;
            }
            else if (turnCounter == 4)
            {
                turn.Action = actions[2];
                turn.Target = player;
            }
        }
        else
        {
            if (attackCounter < 2)
            {
                turn.Action = actions[0];
                turn.Target = player;
                attackCounter++;
            }
            else
            {
                turn.Action = actions[1];
                turn.Target = this;
                attackCounter = 0;
            }    
        }

        return turn;
    }

    public void PentagramSound()
    {
        audioSource.pitch = Random.Range(1, 1.25f);
        audioSource.Play();
    }
}
