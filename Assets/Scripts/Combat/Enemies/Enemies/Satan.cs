using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class Satan : Enemy
{
    [SerializeField] private ParticleSystem sinParticles;
    [SerializeField] private ParticleSystem attackParticles;
    private readonly Tween deathTween = new();

    public override bool CorruptHealth => true;

    // 0 is sin
    // 1 is big attack (consumes all sins)
    protected override Turn ChooseTurnInternal()
    {
        Turn turn = new(this, player, null);

        if (player.SinCount < 7)
            turn.Action = actions[0];
        else
            turn.Action = actions[1];

        return turn;
    }

    public void SinAction()
    {
        sinParticles.Play();
    }
    
    public void AttackAction()
    {
        attackParticles.Play();
    }

    protected override void Die()
    {
        Destroy(healthbar.gameObject);

        transform.Shake(1, 3f, deathTween);
    }
}
