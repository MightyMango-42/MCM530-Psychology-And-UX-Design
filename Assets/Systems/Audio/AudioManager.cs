using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] public AudioMixer DefaultAudioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    //[Header("SFX")]

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StopAllMusic();
            PlayMusic(menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        StopAllSFX();
        _sfxSource.PlayOneShot(clip);
    }

    public void PauseAllAudio()
    {
        _musicSource.Pause();
        _sfxSource.Pause();
    }

    public void ResumeAllAudio()
    {
        _musicSource.Play();
        _sfxSource.Play();
    }

    public void StopAllSFX()
    {
        _sfxSource.Stop();
    }

    public void StopAllMusic()
    {
        _musicSource.Stop();
    }

    public void StopAllAudio()
    {
        StopAllMusic();
        StopAllSFX();
    }
}
