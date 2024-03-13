using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] private ParticleSystem brimstoneParticles;
    [SerializeField] private AudioSource audioSource;
    int attackCounter = 0;
    int brimstoneCounter = 0;

    public override bool CorruptHealth => true;

    // 0 is attack
    // 1 is defend
    // 2 is brimstone
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        if (Health > MaxHealth / 2)
        {
            if (attackCounter < 2) // attack
            {
                turn.Action = actions[0];
                turn.Target = player;
                attackCounter++;
            }
            else // defend
            {
                turn.Action = actions[1];
                turn.Target = this;
                attackCounter = 0;
            }
        }
        else // half health, defend, then brimstone
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
                turn.Target = player;
                brimstoneCounter++;
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
