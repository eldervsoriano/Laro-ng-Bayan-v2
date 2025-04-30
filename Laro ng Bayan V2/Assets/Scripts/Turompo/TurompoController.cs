//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoController : MonoBehaviour
//{
//    // Player identification
//    public int playerIndex = 1; // 1 for player 1, 2 for player 2

//    // Spin mechanics
//    public float maxSpinSpeed = 720f; // degrees per second (positive for clockwise rotation)
//    public float currentSpinSpeed;
//    public float spinDecayRate = 30f; // speed reduction per second (changed from 50f to 30f)
//    public float spinBoostPerMatch = 50f; // speed boost for successful matches

//    // Visual feedback
//    public GameObject torompoModel;
//    public ParticleSystem spinParticles;
//    public AudioSource spinAudio;
//    public AudioClip successSound;
//    public AudioClip failSound;

//    // Gameplay state
//    private bool isSpinning = false;
//    private float minSpinSpeed = 50f; // minimum speed before game over

//    void Start()
//    {
//        ResetTorompo();
//    }

//    void Update()
//    {
//        if (isSpinning)
//        {
//            // Apply continuous spin decay (reducing positive value)
//            currentSpinSpeed -= spinDecayRate * Time.deltaTime;

//            // Ensure spin speed doesn't go negative
//            if (currentSpinSpeed < 0f)
//            {
//                currentSpinSpeed = 0f;
//            }

//            // Update visual spin speed (using positive rotation)
//            torompoModel.transform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime);

//            // Adjust particle effects based on speed
//            var emission = spinParticles.emission;
//            emission.rateOverTime = (currentSpinSpeed / maxSpinSpeed) * 50;

//            // Adjust audio pitch based on speed
//            spinAudio.pitch = 0.5f + (currentSpinSpeed / maxSpinSpeed);

//            // Check for game over condition (when speed drops below minimum)
//            if (currentSpinSpeed <= minSpinSpeed)
//            {
//                StopSpinning();
//                // Notify the game manager that this player lost (other player wins)
//                int winnerIndex = (playerIndex == 1) ? 2 : 1;
//                TurompoGameManager.Instance.DeclareWinner(winnerIndex);
//            }
//        }
//    }

//    public void ResetTorompo()
//    {
//        currentSpinSpeed = maxSpinSpeed;
//        isSpinning = true;
//        spinParticles.Play();
//        spinAudio.Play();
//    }

//    public void StopSpinning()
//    {
//        isSpinning = false;
//        spinParticles.Stop();
//        spinAudio.Stop();
//    }

//    public void BoostSpin()
//    {
//        // Increase spin speed on successful matches
//        currentSpinSpeed += spinBoostPerMatch;
//        if (currentSpinSpeed > maxSpinSpeed)
//        {
//            currentSpinSpeed = maxSpinSpeed;
//        }
//        spinAudio.PlayOneShot(successSound);
//    }

//    public void MissedMatch()
//    {
//        // Play the fail sound but don't modify the spin speed on misses
//        // This ensures rotation speed only increases on successful matches
//        spinAudio.PlayOneShot(failSound);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurompoController : MonoBehaviour
{
    // Player identification
    public int playerIndex = 1; // 1 for player 1, 2 for player 2

    // Spin mechanics
    public float maxSpinSpeed = 720f; // degrees per second (positive for clockwise rotation)
    public float currentSpinSpeed;
    public float spinDecayRate = 30f; // speed reduction per second (changed from 50f to 30f)
    public float spinBoostPerMatch = 50f; // speed boost for successful matches

    // Visual feedback
    public GameObject torompoModel;
    public ParticleSystem spinParticles;
    public AudioSource spinAudio;
    public AudioClip successSound;
    public AudioClip failSound;

    // Gameplay state
    private bool isSpinning = false;
    private float minSpinSpeed = 50f; // minimum speed before game over

    void Start()
    {
        ResetTorompo();
    }

    void Update()
    {
        if (isSpinning && TurompoGameManager.Instance.IsGameActive())
        {
            // Apply continuous spin decay (reducing positive value)
            currentSpinSpeed -= spinDecayRate * Time.deltaTime;

            // Ensure spin speed doesn't go negative
            if (currentSpinSpeed < 0f)
            {
                currentSpinSpeed = 0f;
            }

            // Update visual spin speed (using positive rotation)
            if (torompoModel != null)
            {
                torompoModel.transform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime);
            }

            // Adjust particle effects based on speed
            if (spinParticles != null)
            {
                var emission = spinParticles.emission;
                emission.rateOverTime = (currentSpinSpeed / maxSpinSpeed) * 50;
            }

            // Adjust audio pitch based on speed
            if (spinAudio != null)
            {
                spinAudio.pitch = 0.5f + (currentSpinSpeed / maxSpinSpeed);
            }

            // Check for game over condition (when speed drops below minimum)
            if (currentSpinSpeed <= minSpinSpeed)
            {
                StopSpinning();

                // Notify the game manager that this player lost (other player wins)
                if (TurompoGameManager.Instance != null)
                {
                    // Call PlayerGameOver directly with the current player index
                    TurompoGameManager.Instance.PlayerGameOver(playerIndex);

                    // Log for debugging
                    Debug.Log("Player " + playerIndex + " stopped spinning. Game over triggered.");
                }
            }
        }
    }

    public void ResetTorompo()
    {
        currentSpinSpeed = maxSpinSpeed;
        isSpinning = true;

        if (spinParticles != null)
            spinParticles.Play();

        if (spinAudio != null)
            spinAudio.Play();
    }

    public void StopSpinning()
    {
        isSpinning = false;

        if (spinParticles != null)
            spinParticles.Stop();

        if (spinAudio != null)
            spinAudio.Stop();

        // Log for debugging
        Debug.Log("Player " + playerIndex + " torompo stopped spinning.");
    }

    public void BoostSpin()
    {
        // Increase spin speed on successful matches
        currentSpinSpeed += spinBoostPerMatch;

        if (currentSpinSpeed > maxSpinSpeed)
        {
            currentSpinSpeed = maxSpinSpeed;
        }

        if (spinAudio != null && successSound != null)
            spinAudio.PlayOneShot(successSound);
    }

    public void MissedMatch()
    {
        // Play the fail sound but don't modify the spin speed on misses
        // This ensures rotation speed only increases on successful matches
        if (spinAudio != null && failSound != null)
            spinAudio.PlayOneShot(failSound);
    }
}