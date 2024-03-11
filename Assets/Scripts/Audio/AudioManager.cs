using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Sounds")]
    [Header("Buttons")]
    [SerializeField] private AudioClip buttonSound1;
    [SerializeField] private AudioClip buttonSound2;
    [SerializeField] private AudioClip buttonSound3;

    [Header("Combat")]
    [SerializeField] private AudioClip sinSound;
    [SerializeField] private AudioClip purifySound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip highlightSound1;
    [SerializeField] private AudioClip highlightSound2;

    [Header("Songs")]
    [SerializeField] private float songTransitionTime;
    [SerializeField] private AudioClip ambientMusic;
    [SerializeField] private AudioClip battleMusic;
    private Coroutine songCoroutine;

    [Header("Ambience")]
    [SerializeField] private AudioClip ambientForest;

    public AudioClip AmbientForest => ambientForest;

    public AudioClip AmbientMusic => ambientMusic;
    public AudioClip BattleMusic => battleMusic;
    public AudioSource UISource => uiSource;

    public void PlayAmbience(AudioClip ambience)
    {
        ambientSource.Stop();
        ambientSource.clip = ambience;
        ambientSource.Play();
    }

    public void PlaySong(AudioClip song, float transitionTime = -1)
    {
        if (songCoroutine != null)
            StopCoroutine(songCoroutine);

        if (transitionTime < 0)
            transitionTime = songTransitionTime;

        songCoroutine = StartCoroutine(IETransitionSong(song, transitionTime));
    }

    private IEnumerator IETransitionSong(AudioClip song, float transitionTime)
    {
        float elapsedTime = 0;

        if (musicSource.isPlaying)
        {
            while (elapsedTime < transitionTime)
            {
                float percentage = elapsedTime / transitionTime;
                musicSource.volume = Mathf.Lerp(.3f, 0, percentage);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }

        musicSource.Stop();

        if (song == null)
            yield break;

        elapsedTime = 0;
        musicSource.clip = song;
        musicSource.Play();

        while(elapsedTime < transitionTime)
        {
            float percentage = elapsedTime / transitionTime;
            musicSource.volume = Mathf.Lerp(0, .3f, percentage);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = .3f;
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1)
    {
        uiSource.PlayOneShot(clip, volumeScale);
    }

    public void DeathSound()
    {
        uiSource.PlayOneShot(deathSound, 0.55f);
    }

    public void HitSound()
    {
        RandomizePitch();
        uiSource.PlayOneShot(hitSound, 0.75f);
    }

    public void SinSound()
    {
        RandomizePitch();
        uiSource.PlayOneShot(sinSound, 0.2f);
    }

    public void PurifySound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(purifySound, 0.2f);
    }

    public void HighlightSound(int index = 1)
    {
        if (index == 1)
        {
            uiSource.pitch = 2.5f;
            uiSource.PlayOneShot(highlightSound1, 0.4f);
        }
        else
        {
            uiSource.pitch = 0.75f;
            uiSource.PlayOneShot(highlightSound2, 0.4f);
        }
    }

    public void ButtonSound(int index = 1, bool randomize = true, float volumeScale = 1)
    {
        if (index == 1) // default, low, shaky
        {
            if (randomize)
                RandomizePitch(uiSource, 1.25f, 2);

            uiSource.PlayOneShot(buttonSound1, volumeScale);
        }
        else if (index == 2) // high blip
        {
            if (randomize)
                RandomizePitch(uiSource, 0.75f, 1.25f);

            uiSource.PlayOneShot(buttonSound2, volumeScale);
        }
        else if (index == 3) // low blip
        {
            if (randomize)
                RandomizePitch(uiSource, 1, 1.5f);

            uiSource.PlayOneShot(buttonSound3, volumeScale);
        }
    }

    public static void RandomizePitch(AudioSource source = null, float lowRange = 0.75f, float highRange = 1.25f)
    {
        if (source == null)
            source = Instance.uiSource;

        source.pitch = Random.Range(lowRange, highRange);
    }
}