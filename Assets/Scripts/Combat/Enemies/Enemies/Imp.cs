using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] private ParticleSystem brimstoneParticles;
    [SerializeField] private AudioSource audioSource;
    int brimstoneCounter = 0;

    // 0 is attack
    // 1 is defend
    // 2 is wait
    // 3 is brimstone
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (Health > MaxHealth / 2)
        {
            int even = turnCounter % 2;

            if (even == 0) // attack
            {
                turn.Action = actions[0];
                turn.Target = player;
            }
            else // defend
            {
                turn.Action = actions[1];
                turn.Target = this;
            }
        }
        else // half health, defend, wait, then brimstone
        {
            if (brimstoneCounter == 0)
            {
                turn.Action = actions[1];
                turn.Target = this;
                brimstoneCounter++;
            }
            else if (brimstoneCounter == 1)
            {
                turn.Action = actions[2];
                turn.Target = this;
                brimstoneCounter++;
            }
            else
            {
                turn.Action = actions[3];
                turn.Target = player;
                brimstoneCounter = 0;
            }
        }

        return turn;
    }

    public void BrimstoneParticles()
    {
        brimstoneParticles.Play();
        audioSource.Play();
    }
}
