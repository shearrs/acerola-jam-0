using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class GameEndManager : Singleton<GameEndManager>
{
    [SerializeField] private ParticleSystem heavenParticles;
    [SerializeField] private AudioClip endSound;
    [SerializeField] private EndScreen endScreen;
    private AudioManager audioManager;
    private readonly Tween deathTween = new();

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void SatanDeathSequence(Satan satan)
    {
        audioManager.PlaySong(null, 0.15f);
        CameraManager.Instance.Shake(0.125f, 5f);
        satan.transform.Shake(1, 6f, deathTween).SetOnComplete(() => RemoveSatan(satan));
        heavenParticles.Play();

        PlaySound();
        Invoke(nameof(PlaySound), 1.5f);
        Invoke(nameof(PlaySound), 2.5f);
        Invoke(nameof(PlaySound), 3.5f);
        Invoke(nameof(ChangeVolume), 1.5f);
    }

    private void PlaySound()
    {
        audioManager.PlaySound(endSound, 0.85f);
    }

    private void RemoveSatan(Satan satan)
    {
        audioManager.PlayHeavenMusic();
        CombatManager.Instance.PlayDeathParticles(satan.DeathParticles);
        Destroy(satan.gameObject, 5f);
    }

    private void ChangeVolume()
    {
        Level.Instance.SetVolume(LevelVolume.HEAVENLY, 12f);
    }

    public void ShowEndScreen()
    {
        Level.Instance.SetVolume(LevelVolume.DEFAULT, 24f);
        endScreen.Enable();
    }
}