using UnityEngine;
using UnityEngine.UI;
using TMPro; // Remove this line if you're using standard Unity Text

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Volume Text (Optional)")]
    public TextMeshProUGUI musicVolumeText; // Change to Text if not using TextMeshPro
    public TextMeshProUGUI sfxVolumeText;   // Change to Text if not using TextMeshPro

    private void Start()
    {
        // Initialize sliders with current volume values
        if (MusicManager.Instance != null)
        {
            musicSlider.value = MusicManager.Instance.GetMusicVolume();
            sfxSlider.value = MusicManager.Instance.GetSFXVolume();

            // Add listeners to sliders
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            // Update text displays
            UpdateVolumeText();
        }
        else
        {
            Debug.LogError("MusicManager instance not found! Make sure MusicManager exists in the scene.");
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusicVolume(value);
            UpdateMusicVolumeText(value);
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetSFXVolume(value);
            UpdateSFXVolumeText(value);

            // Optional: Play a test sound when adjusting SFX volume
            // MusicManager.Instance.PlaySFX(0);
        }
    }

    private void UpdateVolumeText()
    {
        if (MusicManager.Instance != null)
        {
            UpdateMusicVolumeText(MusicManager.Instance.GetMusicVolume());
            UpdateSFXVolumeText(MusicManager.Instance.GetSFXVolume());
        }
    }

    private void UpdateMusicVolumeText(float value)
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }

    private void UpdateSFXVolumeText(float value)
    {
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }
}