using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Enemy
{
    [SerializeField] private Transform pentagram;
    [SerializeField] private AudioSource audioSource;
    private int waitCounter = 0;

    public Transform Pentagram => pentagram;

    public override bool CorruptHealth => false;

    // 0 is attack
    // 1 is demon act
    // 2 is wait
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (turnCounter <= 2)
        {
            if (turnCounter <= 1)
            {
                turn.Action = actions[0];
                turn.Target = player;
            }
            else if (turnCounter == 2)
            {
                turn.Action = actions[1];
                turn.Target = this;
            }
        }
        else
        {
            if (waitCounter == 0)
            {
                turn.Action = actions[2];
                turn.Target = this;
                waitCounter++;
            }
            else
            {
                turn.Action = actions[0];
                turn.Target = player;
                waitCounter = 0;
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
