using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
using UnityEngine.Rendering;

public enum LevelVolume { DEFAULT, STORM, HEAVENLY };

public class Level : Singleton<Level>
{
    [Header("Gameplay")]
    [SerializeField] private Player player;
    [SerializeField] private List<Encounter> encounters;

    [Header("Environment")]
    [SerializeField] private Volume defaultVolume;
    [SerializeField] private Volume stormVolume;
    [SerializeField] private Volume heavenVolume;
    private Volume currentVolume;
    private AudioManager audioManager;

    public bool RamFight { get; set; } = false;
    public bool SatanFight { get; set; } = false;
    public bool HasStarted { get; set; } = false;
    public Encounter CurrentEncounter => encounters[0];
    public Player Player => player;

    public void Start()
    {
        audioManager = AudioManager.Instance;

        audioManager.PlayAmbience(audioManager.AmbientForest);
        currentVolume = defaultVolume;
    }

    public void StartEncounter()
    {
        UIManager.Instance.EnterEncounter();
        CurrentEncounter.Enter();
    }

    public void EndEncounter()
    {
        UIManager.Instance.EndEncounter();

        if (!SatanFight && !Player.IsDead)
        {
            audioManager.PlayAmbientMusic();
            encounters.RemoveAt(0);
            Player.Move();
        }
    }

    public void SetVolume(LevelVolume volume, float time)
    {
        Volume volumeToTransitionTo = null;

        switch(volume)
        {
            case LevelVolume.DEFAULT:
                volumeToTransitionTo = defaultVolume;
                break;
            case LevelVolume.STORM:
                volumeToTransitionTo = stormVolume;
                break;
            case LevelVolume.HEAVENLY:
                volumeToTransitionTo = heavenVolume;
                break;
        }

        StartCoroutine(TransitionVolumes(volumeToTransitionTo, time));
    }

    private IEnumerator TransitionVolumes(Volume newVolume, float time)
    {
        float elapsedTime = 0;
        float percent = 0;

        newVolume.gameObject.SetActive(true);

        while(elapsedTime < time)
        {
            newVolume.weight = Mathf.Lerp(0, 1, percent);
            currentVolume.weight = Mathf.Lerp(1, 0, percent);

            percent = elapsedTime / time;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        newVolume.weight = 1;
        currentVolume.weight = 0;

        currentVolume.gameObject.SetActive(false);
    }
}