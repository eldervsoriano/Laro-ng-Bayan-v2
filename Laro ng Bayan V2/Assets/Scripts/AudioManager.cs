//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    [Header("Audio Sources")]
//    [Tooltip("Single AudioSource for background music (keep in Hierarchy)")]
//    public AudioSource musicSource;

//    [Tooltip("One AudioSource used to play all SFX clips (auto-created if empty)")]
//    public AudioSource sfxSource;

//    [Header("UI Sliders (Optional)")]
//    public Slider musicSlider;
//    public Slider sfxSlider;

//    [Header("Volume Settings")]
//    [Range(0f, 1f)] public float musicVolume = 1f;
//    [Range(0f, 1f)] public float sfxVolume = 1f;

//    [Header("Sound Effects Library")]
//    [Tooltip("Assign all your SFX clips (jump, attack, etc.) here")]
//    public List<AudioClip> sfxClips = new List<AudioClip>();

//    private Dictionary<string, AudioClip> sfxLibrary = new Dictionary<string, AudioClip>();

//    private void Awake()
//    {
//        // Singleton
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//        DontDestroyOnLoad(gameObject);

//        // Ensure we have a SFX AudioSource
//        if (sfxSource == null)
//        {
//            GameObject sfxObj = new GameObject("SFX_AudioSource");
//            sfxObj.transform.SetParent(transform);
//            sfxSource = sfxObj.AddComponent<AudioSource>();
//        }

//        LoadVolumeSettings();
//        ApplyVolumeSettings();
//        BuildSFXLibrary();

//        SceneManager.sceneLoaded += (scene, mode) => ApplyVolumeSettings();
//    }

//    private void Start()
//    {
//        if (musicSlider != null)
//        {
//            musicSlider.value = musicVolume;
//            musicSlider.onValueChanged.AddListener(SetMusicVolume);
//        }

//        if (sfxSlider != null)
//        {
//            sfxSlider.value = sfxVolume;
//            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
//        }
//    }

//    private void BuildSFXLibrary()
//    {
//        sfxLibrary.Clear();
//        foreach (var clip in sfxClips)
//        {
//            if (clip != null && !sfxLibrary.ContainsKey(clip.name))
//                sfxLibrary.Add(clip.name, clip);
//        }
//    }

//    // 🎵 Set Music Volume
//    public void SetMusicVolume(float volume)
//    {
//        musicVolume = Mathf.Clamp01(volume);
//        if (musicSource != null)
//            musicSource.volume = musicVolume;

//        SaveVolumeSettings();
//    }

//    // 🔊 Set SFX Volume
//    public void SetSFXVolume(float volume)
//    {
//        sfxVolume = Mathf.Clamp01(volume);
//        if (sfxSource != null)
//            sfxSource.volume = sfxVolume;

//        SaveVolumeSettings();
//    }

//    // 💥 Play SFX by name
//    public void PlaySFX(string clipName)
//    {
//        if (sfxLibrary.TryGetValue(clipName, out AudioClip clip))
//            PlaySFX(clip);
//        else
//            Debug.LogWarning($"[AudioManager] SFX '{clipName}' not found!");
//    }

//    // 💥 Play SFX by clip
//    public void PlaySFX(AudioClip clip)
//    {
//        if (clip == null) return;
//        sfxSource.PlayOneShot(clip, sfxVolume);
//    }

//    // 💾 Save volumes
//    private void SaveVolumeSettings()
//    {
//        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
//        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
//        PlayerPrefs.Save();
//    }

//    // 📖 Load volumes
//    private void LoadVolumeSettings()
//    {
//        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
//        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
//    }

//    private void ApplyVolumeSettings()
//    {
//        if (musicSource != null)
//            musicSource.volume = musicVolume;

//        if (sfxSource != null)
//            sfxSource.volume = sfxVolume;
//    }
//}



using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Single AudioSource for background music (keep in Hierarchy)")]
    public AudioSource musicSource;

    [Tooltip("All AudioSources that will play SFX (add more if you want)")]
    public List<AudioSource> sfxSources = new List<AudioSource>();

    [Header("UI Sliders (Optional)")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Sound Effects Library")]
    [Tooltip("Assign all your SFX clips (jump, attack, etc.) here")]
    public List<AudioClip> sfxClips = new List<AudioClip>();

    private Dictionary<string, AudioClip> sfxLibrary = new Dictionary<string, AudioClip>();



    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // If no SFX sources, create one
        if (sfxSources.Count == 0)
        {
            GameObject sfxObj = new GameObject("SFX_AudioSource");
            sfxObj.transform.SetParent(transform);
            AudioSource sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }

        LoadVolumeSettings();
        ApplyVolumeSettings();
        BuildSFXLibrary();

        // Reconnect sliders and audio sources when a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReconnectSceneAudioObjects();
        ApplyVolumeSettings();
        HookButtonsToAudio();   // For buttons
    }


    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void BuildSFXLibrary()
    {
        sfxLibrary.Clear();
        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxLibrary.ContainsKey(clip.name))
                sfxLibrary.Add(clip.name, clip);
        }
    }

    // 🎵 Set Music Volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;

        SaveVolumeSettings();
    }

    // 🔊 Set SFX Volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.volume = sfxVolume;
        }

        SaveVolumeSettings();
    }

    // 💥 Play SFX by name
    public void PlaySFX(string clipName)
    {
        if (sfxLibrary.TryGetValue(clipName, out AudioClip clip))
            PlaySFX(clip);
        else
            Debug.LogWarning($"[AudioManager] SFX '{clipName}' not found!");
    }



    // 💥 Play SFX by clip
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSFXSource();
        if (source == null)
        {
            Debug.LogWarning("[AudioManager] No available SFX source found!");
            return;
        }

        source.volume = sfxVolume;
        source.PlayOneShot(clip);
    }

    private AudioSource GetAvailableSFXSource()
    {
        // Find any valid AudioSource in the list
        foreach (AudioSource src in sfxSources)
        {
            if (src != null && !src.isPlaying)
                return src;
        }

        // If none found, create a new one safely
        GameObject newSFXObj = new GameObject("SFX_AudioSource_Extra");
        newSFXObj.transform.SetParent(transform);
        AudioSource newSource = newSFXObj.AddComponent<AudioSource>();
        newSource.volume = sfxVolume;
        sfxSources.Add(newSource);
        return newSource;
    }

    // 💾 Save volumes
    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    // 📖 Load volumes
    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void ApplyVolumeSettings()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.volume = sfxVolume;
        }
    }

    private void ReconnectSceneAudioObjects()
    {
        // Reconnect music source
        if (musicSource == null)
        {
            var foundMusic = GameObject.FindWithTag("MusicSource");
            if (foundMusic != null)
                musicSource = foundMusic.GetComponent<AudioSource>();
        }

        // Reconnect all tagged SFX sources
        GameObject[] foundSFXObjects = GameObject.FindGameObjectsWithTag("SFXSource");
        foreach (var obj in foundSFXObjects)
        {
            AudioSource src = obj.GetComponent<AudioSource>();
            if (src != null && !sfxSources.Contains(src))
            {
                sfxSources.Add(src);
                src.volume = sfxVolume;
            }
        }

        // Reconnect sliders
        var musicUI = GameObject.FindWithTag("MusicSlider");
        if (musicUI != null)
        {
            var newSlider = musicUI.GetComponent<Slider>();
            newSlider.onValueChanged.RemoveAllListeners();
            newSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider = newSlider;
            musicSlider.value = musicVolume;
        }

        var sfxUI = GameObject.FindWithTag("SFXSlider");
        if (sfxUI != null)
        {
            var newSlider = sfxUI.GetComponent<Slider>();
            newSlider.onValueChanged.RemoveAllListeners();
            newSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxSlider = newSlider;
            sfxSlider.value = sfxVolume;
        }

        ApplyVolumeSettings();
    }

    private void HookButtonsToAudio()
    {
        // Find all Buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button btn in buttons)
        {
            // Remove any old listeners to prevent stacking
            btn.onClick.RemoveAllListeners();
            // Add the click sound
            btn.onClick.AddListener(() =>
            {
                if (sfxLibrary.ContainsKey("Click"))
                    PlaySFX(sfxLibrary["Click"]);
            });
        }
    }


}
