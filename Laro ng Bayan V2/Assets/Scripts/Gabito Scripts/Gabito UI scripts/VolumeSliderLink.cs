using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderLink : MonoBehaviour
{
    public enum VolumeType { Music, SFX }
    public VolumeType type;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("No AudioManager found!");
            return;
        }

        // Set the slider's initial value
        if (type == VolumeType.Music)
            slider.value = AudioManager.Instance.musicVolume;
        else
            slider.value = AudioManager.Instance.sfxVolume;

        // Add a listener to update AudioManager when changed
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (AudioManager.Instance == null)
            return;

        if (type == VolumeType.Music)
            AudioManager.Instance.SetMusicVolume(value);
        else
            AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
