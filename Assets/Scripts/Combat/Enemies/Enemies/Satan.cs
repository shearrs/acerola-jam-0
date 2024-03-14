using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class Satan : Enemy
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem sinParticles;
    [SerializeField] private ParticleSystem attackParticles;

    [Header("Sounds")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip sinSound;

    protected override AudioClip HitSound => AudioManager.Instance.SatanHit;
    public override bool CorruptHealth => true;
    public ParticleSystem DeathParticles => deathParticles;

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

        AudioManager audioManager = AudioManager.Instance;
        audioManager.UISource.pitch = 1;
        audioManager.PlaySound(sinSound, 0.65f);
    }
    
    public void AttackAction()
    {
        sinParticles.Play();
        attackParticles.Play();

        AudioManager audioManager = AudioManager.Instance;
        audioManager.UISource.pitch = 1;
        audioManager.PlaySound(attackSound, 0.65f);
    }

    protected override void Die()
    {
        List<Enemy> enemies = Battle.Enemies;
        int count = enemies.Count;
        for (int i = 0; i < count; i++)
        {
            if (enemies[i] == this)
                continue;

            enemies[0].Damage(999);
            enemies.RemoveAt(0);
        }

        intent.gameObject.SetActive(false);
        Destroy(healthbar.gameObject);

        GameEndManager.Instance.SatanDeathSequence(this);
    }
}
