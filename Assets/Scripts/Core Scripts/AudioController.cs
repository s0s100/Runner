using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public static AudioController Instance { get { return instance; } }

    private const float DEFAULT_MUSIC_VOLUME = 1.0f;
    private const float DEFAULT_REDUCED_MUSIC_VOLUME = 0.5f;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource uiSource;

    [SerializeField]
    private AudioClip buttonClick;
    [SerializeField]
    private AudioClip buttonClickTwo;

    [SerializeField]
    private AudioClip menuMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            instance.PlayMenuMusic();
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;

        uiSource.clip = buttonClick;
        uiSource.loop = false;

        PlayMenuMusic();
    }

    private void PlayMenuMusic()
    {
        MaxMusicVolume();
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlaySpecifiedMusic(AudioClip musicClip)
    {
        MaxMusicVolume();
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void PlayEffect(AudioClip effect, Vector3 effectLocation)
    {
        if (effect == null)
        {
            Debug.Log("No audio clip found");
            return;
        }

        AudioSource.PlayClipAtPoint(effect, effectLocation);
    }

    public void ReducedMusicVolume()
    {
        musicSource.volume = DEFAULT_REDUCED_MUSIC_VOLUME;
    }

    public void MaxMusicVolume()
    {
        musicSource.volume = DEFAULT_MUSIC_VOLUME;
    }

    public void PlayButtonClick()
    {
        uiSource.clip = buttonClick;
        uiSource.Play();
    }

    public void PlayButtonClickTwo()
    {
        uiSource.clip = buttonClickTwo;
        uiSource.Play();
    }

    public void PlayUIEffect(AudioClip effect)
    {
        if (effect == null)
        {
            Debug.Log("No audio clip found");
            return;
        }

        uiSource.clip = effect;
        uiSource.Play();
    }
}
