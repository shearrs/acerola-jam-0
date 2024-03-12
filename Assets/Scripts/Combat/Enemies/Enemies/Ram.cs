using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Enemy
{
    [SerializeField] private Transform pentagram;
    [SerializeField] private AudioSource audioSource;

    public Transform Pentagram => pentagram;

    public override bool CorruptHealth => true;

    // 0 is attack
    // 1 is wait
    // 2 is demon act
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (turnCounter <= 2 || turnCounter > 4)
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

        return turn;
    }

    public void PentagramSound()
    {
        audioSource.pitch = Random.Range(1, 1.25f);
        audioSource.Play();
    }
}
