using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
using UnityEngine.Rendering;

public class Level : Singleton<Level>
{
    [Header("Gameplay")]
    [SerializeField] private Player player;
    [SerializeField] private List<Encounter> encounters;

    [Header("Environment")]
    [SerializeField] private Volume defaultVolume;
    [SerializeField] private Volume stormVolume;
    private AudioManager audioManager;

    public Encounter CurrentEncounter => encounters[0];
    public Player Player => player;

    public void Start()
    {
        player.Move();
        audioManager = AudioManager.Instance;

        audioManager.PlaySong(audioManager.AmbientMusic);
        audioManager.PlayAmbience(audioManager.AmbientForest);
    }

    public void StartEncounter()
    {
        UIManager.Instance.EnterEncounter();
        CurrentEncounter.Enter();
    }

    public void EndEncounter()
    {
        UIManager.Instance.EndEncounter();
        AudioManager.Instance.PlaySong(AudioManager.Instance.AmbientMusic);
        encounters.RemoveAt(0);

        if (!Player.IsDead)
            Player.Move();
    }

    public void StartStorm(float time)
    {
        StartCoroutine(IETransitionStorm(true, time));
    }

    private IEnumerator IETransitionStorm(bool active, float time)
    {
        float elapsedTime = 0;
        float percent = 0;
        Volume volumeOn;
        Volume volumeOff;

        if (active)
        {
            volumeOn = stormVolume;
            volumeOff = defaultVolume;
        }
        else
        {
            volumeOn = defaultVolume;
            volumeOff = stormVolume;
        }

        volumeOn.gameObject.SetActive(true);

        while(elapsedTime < time)
        {
            volumeOn.weight = Mathf.Lerp(0, 1, percent);
            volumeOff.weight = Mathf.Lerp(1, 0, percent);

            percent = elapsedTime / time;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        volumeOn.weight = 1;
        volumeOff.weight = 0;

        volumeOff.gameObject.SetActive(false);
    }

    public void EndStorm(float time)
    {
        StartCoroutine(IETransitionStorm(false, time));
    }
}