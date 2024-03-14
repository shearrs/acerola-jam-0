using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] private ParticleSystem brimstoneParticles;
    [SerializeField] private AudioSource audioSource;
    bool hasAttackedOnce = false;
    bool defendedLastTurn = false;
    bool brimstoneMode = false;

    public override bool CorruptHealth => true;

    // attack the first turn
    // after that, 50% chance to go into brimstone mode

    // 0 is attack
    // 1 is defend
    // 2 is brimstone
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        int random = Random.Range(0, 2);

        if (!brimstoneMode && (!hasAttackedOnce || random == 0)) // attack at least once
        {
            turn.Action = actions[0];
            turn.Target = player;
            hasAttackedOnce = true;
        }
        else // go into brimstone mode where we defend then attack
        {
            brimstoneMode = true;

            if (!defendedLastTurn)
            {
                turn.Action = actions[1];
                turn.Target = this;
                defendedLastTurn = true;
            }
            else
            {
                turn.Action = actions[2];
                turn.Target = player;
                defendedLastTurn = false;
                brimstoneMode = false;
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
