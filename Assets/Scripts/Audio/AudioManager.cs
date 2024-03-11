using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip buttonSound1;
    [SerializeField] private AudioClip buttonSound2;
    [SerializeField] private AudioClip buttonSound3;
    [SerializeField] private AudioClip sinSound;
    [SerializeField] private AudioClip purifySound;
    [SerializeField] private AudioClip hitSound;

    [Header("Songs")]
    [SerializeField] private float songTransitionTime;
    [SerializeField] private AudioClip battleMusic;
    private Coroutine songCoroutine;

    public AudioClip BattleMusic => battleMusic;
    public AudioSource UISource => uiSource;

    public void PlaySong(AudioClip song, float transitionTime = -1)
    {
        if (songCoroutine != null)
            StopCoroutine(songCoroutine);

        if (transitionTime < 0)
            transitionTime = songTransitionTime;

        songCoroutine = StartCoroutine(IEPlaySong(song, transitionTime));
    }

    private IEnumerator IEPlaySong(AudioClip song, float transitionTime)
    {
        float elapsedTime = 0;

        if (musicSource.isPlaying)
        {
            while (elapsedTime < transitionTime)
            {
                float percentage = elapsedTime / transitionTime;
                musicSource.volume = Mathf.Lerp(.65f, 0, percentage);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }

        elapsedTime = 0;
        musicSource.clip = song;
        musicSource.Play();

        while(elapsedTime < transitionTime)
        {
            float percentage = elapsedTime / transitionTime;
            musicSource.volume = Mathf.Lerp(0, .65f, percentage);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = 1;
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1)
    {
        uiSource.PlayOneShot(clip, volumeScale);
    }

    public void HitSound()
    {
        RandomizePitch();
        uiSource.PlayOneShot(hitSound, 0.75f);
    }

    public void SinSound()
    {
        RandomizePitch();
        uiSource.PlayOneShot(sinSound, 0.35f);
    }

    public void PurifySound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(purifySound, 0.35f);
    }

    public void ButtonSound(int index = 1, bool randomize = true, float volumeScale = 1)
    {
        if (index == 1)
        {
            if (randomize)
                RandomizePitch(uiSource, 1.25f, 2);

            uiSource.PlayOneShot(buttonSound1, volumeScale);
        }
        else if (index == 2)
        {
            if (randomize)
                RandomizePitch(uiSource, 0.75f, 1.25f);

            uiSource.PlayOneShot(buttonSound2, volumeScale);
        }
        else if (index == 3)
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