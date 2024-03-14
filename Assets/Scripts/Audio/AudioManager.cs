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

    [Header("Combat")]
    [SerializeField] private AudioClip encounterSound;
    [SerializeField] private AudioClip sinSound;
    [SerializeField] private AudioClip purifySound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip highlightSound1;
    [SerializeField] private AudioClip highlightSound2;
    [SerializeField] private AudioClip lotSound;
    [SerializeField] private AudioClip healthSound;
    [SerializeField] private AudioClip defenseSound;
    [SerializeField] private AudioClip satanHit;

    [Header("Songs")]
    [SerializeField] private float songTransitionTime;
    [SerializeField] private AudioClip happyAmbientMusic;
    [SerializeField] private AudioClip scaryAmbientMusic;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private AudioClip satanMusic;
    [SerializeField] private AudioClip heavenMusic;
    private AudioClip currentSong = null;
    private Coroutine songCoroutine;

    [Header("Ambience")]
    [SerializeField] private AudioClip ambientForest;
    [SerializeField] private AudioClip ambientOoo;

    public AudioClip SatanHit => satanHit;

    public AudioClip AmbientForest => ambientForest;
    public AudioClip AmbientOoo => ambientOoo;
    public AudioClip HappyAmbientMusic => happyAmbientMusic;
    public AudioClip ScaryAmbientMusic => scaryAmbientMusic;
    public AudioClip BattleMusic => battleMusic;
    public AudioClip SatanMusic => satanMusic;
    public AudioClip HeavenMusic => heavenMusic;
    public AudioSource UISource => uiSource;

    private void Start()
    {
        PlayAmbientMusic();
    }

    public void PlayAmbience(AudioClip ambience)
    {
        ambientSource.Stop();
        ambientSource.clip = ambience;
        ambientSource.Play();
    }

    public void PlayAmbientMusic()
    {
        if (musicSource.isPlaying && (musicSource.clip == happyAmbientMusic || musicSource.clip == scaryAmbientMusic))
            return;

        if (Level.Instance.RamFight)
        {
            PlaySong(ScaryAmbientMusic);
        }
        else
        {
            PlaySong(HappyAmbientMusic);
        }
    }

    public void PlaySong(AudioClip song, float transitionTime = -1)
    {
        if (song == currentSong)
            return;
        else
            currentSong = song;

        if (songCoroutine != null)
            StopCoroutine(songCoroutine);

        if (transitionTime < 0)
            transitionTime = songTransitionTime;

        songCoroutine = StartCoroutine(IETransitionSong(song, transitionTime));
    }

    private IEnumerator IETransitionSong(AudioClip song, float transitionTime)
    {
        float elapsedTime = 0;
        float maxAudio;

        if (song == satanMusic || song == heavenMusic)
            maxAudio = 0.7f;
        else
            maxAudio = 0.5f;

        if (musicSource.isPlaying)
        {
            while (elapsedTime < transitionTime)
            {
                float percentage = elapsedTime / transitionTime;
                musicSource.volume = Mathf.Lerp(maxAudio, 0, percentage);

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
            musicSource.volume = Mathf.Lerp(0, maxAudio, percentage);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = maxAudio;
    }

    private IEnumerator IEHeavenMusic()
    {
        float quietPoint = 13f; // 13 seconds in, go quiet

        while (musicSource.time < quietPoint)
            yield return null;

        StartCoroutine(IETransitionSong(null, 3));
        Invoke(nameof(ShowEndScreen), 1);
    }

    private void ShowEndScreen()
    {
        GameEndManager.Instance.ShowEndScreen();
    }

    public void PlayHeavenMusic()
    {
        StartCoroutine(IETransitionSong(heavenMusic, 0.25f));

        StartCoroutine(IEHeavenMusic());
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1)
    {
        uiSource.PlayOneShot(clip, volumeScale);
    }

    public void EncounterSound()
    {
        uiSource.PlayOneShot(encounterSound, 0.75f);
    }

    public void DeathSound()
    {
        uiSource.PlayOneShot(deathSound, 0.55f);
    }

    public void ShieldSound()
    {
        RandomizePitch(uiSource, 0.75f, 1f);
        uiSource.PlayOneShot(shieldSound, 0.3f);
    }

    public void HitSound(AudioClip sound)
    {
        if (sound == null)
        {
            RandomizePitch();
            uiSource.PlayOneShot(hitSound, 0.55f);
        }
        else
        {
            RandomizePitch(uiSource, 0.75f, 0.95f);
            uiSource.PlayOneShot(sound, 0.55f);
        }
    }

    public void SinSound()
    {
        RandomizePitch();
        uiSource.PlayOneShot(sinSound, 0.4f);
    }

    public void PurifySound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(purifySound, 0.4f);
    }

    public void LotSound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(lotSound, 0.45f);
    }

    public void HealthSound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(healthSound, .65f);
    }

    public void HighlightSound(int index = 1)
    {
        if (index == 1)
        {
            RandomizePitch(UISource, 1, 1.05f);
            uiSource.PlayOneShot(highlightSound1, 0.4f);
        }
        else
        {
            uiSource.pitch = 0.75f;
            uiSource.PlayOneShot(highlightSound2, 0.4f);
        }
    }

    public void DefenseSound()
    {
        uiSource.pitch = 1;
        uiSource.PlayOneShot(defenseSound, 0.65f);
    }

    public void ButtonSound(int index = 1, bool randomize = true, float volumeScale = -1)
    {
        if (index == 1) // default, low, shaky
        {
            if (randomize)
                RandomizePitch(uiSource, 1.25f, 2);

            if (volumeScale == -1)
                volumeScale = 0.5f;

            uiSource.PlayOneShot(buttonSound1, volumeScale);
        }
        else if (index == 2) // high blip
        {
            if (randomize)
                RandomizePitch(uiSource, 0.95f, 1.25f);

            if (volumeScale == -1)
                volumeScale = 0.35f;

            uiSource.PlayOneShot(buttonSound2, volumeScale);
        }
    }

    public static void RandomizePitch(AudioSource source = null, float lowRange = 0.75f, float highRange = 1.25f)
    {
        if (source == null)
            source = Instance.uiSource;

        source.pitch = Random.Range(lowRange, highRange);
    }
}