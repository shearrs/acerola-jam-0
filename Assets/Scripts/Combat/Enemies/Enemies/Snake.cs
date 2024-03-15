using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    [SerializeField] private AudioSource audioSource;
    private bool hasGivenEnvy = false;

    private bool attackedLastTurn = false;
    private bool hiss = false;

    public override bool CorruptHealth => false;

    // 0 is attack
    // 1 is defend
    // 2 is envy inflict
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, null, null);

        bool onlySnake = true;
        
        foreach (Enemy enemy in Battle.Enemies)
        {
            if (enemy != this && enemy is Snake)
            {
                onlySnake = false;
                break;
            }
        }

        // if this is the only snake and player does not have envy, 75% chance to inflict player with envy
        if (onlySnake && !hasGivenEnvy && !player.HasSin(SinType.ENVY) && EnvyRoll())
        {
            turn.Action = actions[2];
            hasGivenEnvy = true;

            turn.Target = player;
        }
        else if (attackedLastTurn) // else flip between attacking and defending
        {
            turn.Action = actions[1];
            attackedLastTurn = false;

            hiss = true;
            turn.Target = this;
        }
        else
        {
            turn.Action = actions[0];
            attackedLastTurn = true;

            turn.Target = player;
        }

        return turn;
    }

    private void Hiss()
    {
        AudioManager.RandomizePitch(audioSource);
        audioSource.Play();
    }

    private bool EnvyRoll()
    {
        return Random.Range(0, 5) == 4;
    }

    public override void OnExecutingTurn()
    {
        base.OnExecutingTurn();

        if (hiss)
        {
            Invoke(nameof(Hiss), .35f);
            hiss = false;
        }
    }
}
