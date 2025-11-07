using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JolenCollisionSound : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip tickSound;          // Assign in Inspector
    public float minVelocity = 1f;       // Ignore tiny bumps
    public float localVolumeMultiplier = 0.7f; // Extra control per object

    [Header("SFX Audio Source")]
    [Tooltip("Optional: drag a specific AudioSource GameObject here (with 'SFX' tag)")]
    public AudioSource customSFXSource;  // For separate GameObject

    private void Start()
    {
        // If no custom source assigned, find any SFX-tagged AudioSource in the scene
        if (customSFXSource == null)
        {
            GameObject sfxObj = GameObject.FindGameObjectWithTag("SFX");
            if (sfxObj != null)
                customSFXSource = sfxObj.GetComponent<AudioSource>();
        }

        // If still null, just create one dynamically (failsafe)
        if (customSFXSource == null)
        {
            GameObject temp = new GameObject($"{gameObject.name}_SFXSource");
            temp.tag = "SFX";
            customSFXSource = temp.AddComponent<AudioSource>();
            customSFXSource.playOnAwake = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < minVelocity || tickSound == null)
            return;

        // If AudioManager exists, use its controlled volume
        float globalSFXVolume = AudioManager.Instance != null ? AudioManager.Instance.sfxVolume : 1f;
        float finalVolume = globalSFXVolume * localVolumeMultiplier;

        // Play sound using AudioManager (recommended)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(tickSound);
        }
        else if (customSFXSource != null)
        {
            customSFXSource.volume = finalVolume;
            customSFXSource.PlayOneShot(tickSound, finalVolume);
        }
    }
}
