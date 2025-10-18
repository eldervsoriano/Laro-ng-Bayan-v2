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

    //[Header("UI Sliders (Optional)")]
    //public Slider musicSlider;
    //public Slider sfxSlider;

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
        if (scene.name == "GabitoLoadingScreen")
            return;

        // Switch music instantly before waiting
        SwitchBGMForScene(scene.name);

        // Still do the reconnect delay for sliders and buttons
        StartCoroutine(ReconnectAfterDelay(scene.name));
    }




    //private void Start()
    //{
    //    if (musicSlider != null)
    //    {
    //        musicSlider.value = musicVolume;
    //        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    //    }

    //    if (sfxSlider != null)
    //    {
    //        sfxSlider.value = sfxVolume;
    //        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    //    }
    //}

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
        //var musicUI = GameObject.FindWithTag("MusicSlider");
        //if (musicUI != null)
        //{
        //    musicSlider = musicUI.GetComponent<Slider>();
        //    musicSlider.onValueChanged.RemoveAllListeners();
        //    musicSlider.onValueChanged.AddListener(SetMusicVolume);
        //    musicSlider.value = musicVolume;
        //}

        //var sfxUI = GameObject.FindWithTag("SFXSlider");
        //if (sfxUI != null)
        //{
        //    sfxSlider = sfxUI.GetComponent<Slider>();
        //    sfxSlider.onValueChanged.RemoveAllListeners();
        //    sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        //    sfxSlider.value = sfxVolume;
        //}

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

    private void SwitchBGMForScene(string sceneName)
    {
        // Stop ALL BGM tracks first (no exceptions)
        foreach (Transform child in transform)
        {
            AudioSource src = child.GetComponent<AudioSource>();
            if (src != null)
                src.Stop();
        }

        // Determine which BGM to play based on scene
        AudioSource newBGM = null;

        // Group similar scenes under the same BGM
        switch (sceneName)
        {
            case "Gabito3DMenu":
                newBGM = transform.Find("BGM_Menu")?.GetComponent<AudioSource>();
                break;

            // Turompo group
            case "PVPTurumpoScene":
            case "AITurumpoScene":
                newBGM = transform.Find("BGM_Turompo")?.GetComponent<AudioSource>();
                break;


            // Turompo group
            case "PVPTumbangPresoScene":
            case "AITumbangPresoScene":
                newBGM = transform.Find("BGM_Tumbang")?.GetComponent<AudioSource>();
                break;


            // Jolen group
            case "PVPJolenStreet":
            case "PVPJolenTable":
            case "PVPJolenGrass":
            case "AIJolenStreet":
            case "AIJolenGrass":
            case "AIJolenTable":
                newBGM = transform.Find("BGM_Jolen")?.GetComponent<AudioSource>();
                break;


            // Spider Derby group
            case "PVPSpider":
            case "AISpider":
                newBGM = transform.Find("BGM_Spider_Derby")?.GetComponent<AudioSource>();
                break;


            // Open world
            case "GabitoOpenWorld":
                newBGM = transform.Find("BGM_OpenWorld")?.GetComponent<AudioSource>();
                break;



            // Default fallback
            default:
                Debug.Log($"[AudioManager] No specific BGM for scene '{sceneName}', keeping previous track.");
                return;
        }

        // Play the correct one. Avoid restarting the same track unnecessarily
        if (newBGM != null)
        {
            if (musicSource != newBGM)
            {
                musicSource = newBGM;
                musicSource.volume = musicVolume;
                musicSource.Play();
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    private System.Collections.IEnumerator ReconnectAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.1f); // tiny delay for UI to spawn

        ReconnectSceneAudioObjects();
        HookButtonsToAudio();
        ApplyVolumeSettings();
        SwitchBGMForScene(sceneName);
    }



}
