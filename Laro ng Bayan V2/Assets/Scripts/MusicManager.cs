using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] musicTracks;
    public AudioClip[] sfxClips;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;

    // PlayerPrefs keys for saving
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
    {
        // Singleton pattern - persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioSources()
    {
        // Create audio sources if they don't exist
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        // Apply initial volumes
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    private void LoadVolumeSettings()
    {
        // Load saved volume settings or use defaults
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
        {
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            musicSource.volume = musicVolume;
        }

        if (PlayerPrefs.HasKey(SFX_VOLUME_KEY))
        {
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
            sfxSource.volume = sfxVolume;
        }
    }

    #region Music Controls

    public void PlayMusic(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= musicTracks.Length)
        {
            Debug.LogWarning("Track index out of range!");
            return;
        }

        musicSource.clip = musicTracks[trackIndex];
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null!");
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    #endregion

    #region SFX Controls

    public void PlaySFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= sfxClips.Length)
        {
            Debug.LogWarning("SFX index out of range!");
            return;
        }

        sfxSource.PlayOneShot(sfxClips[clipIndex], sfxVolume);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    #endregion

    #region Volume Getters

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    #endregion
}