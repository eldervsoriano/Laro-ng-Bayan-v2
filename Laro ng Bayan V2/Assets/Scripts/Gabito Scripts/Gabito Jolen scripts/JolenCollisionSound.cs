using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JolenCollisionSound : MonoBehaviour
{
    public AudioClip tickSound; // assign in Inspector
    public float minVelocity = 1f; // ignore tiny bumps
    public float volume = 0.7f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // only play sound if impact is strong enough
        if (collision.relativeVelocity.magnitude >= minVelocity)
        {
            audioSource.PlayOneShot(tickSound, volume);
        }
    }
}
