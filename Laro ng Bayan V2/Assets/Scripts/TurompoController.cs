//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoController : MonoBehaviour
//{
//    // Player identification
//    public int playerIndex = 1; // 1 for player 1, 2 for player 2

//    // Spin mechanics
//    public float maxSpinSpeed = -720f; // degrees per second (negative for clockwise rotation)
//    public float currentSpinSpeed;
//    public float spinDecayRate = 10f; // speed reduction per second
//    public float spinBoostPerMatch = 50f; // speed boost for successful matches

//    // Visual feedback
//    public GameObject torompoModel;
//    public ParticleSystem spinParticles;
//    public AudioSource spinAudio;
//    public AudioClip successSound;
//    public AudioClip failSound;

//    // Gameplay state
//    private bool isSpinning = false;
//    private float minSpinSpeed = -50f; // minimum speed before game over (closer to zero)

//    void Start()
//    {
//        ResetTorompo();
//    }

//    void Update()
//    {
//        if (isSpinning)
//        {
//            // Apply continuous spin decay (moving toward zero from negative)
//            currentSpinSpeed += spinDecayRate * Time.deltaTime;

//            // Ensure spin speed doesn't go positive (staying negative or zero)
//            if (currentSpinSpeed > 0f)
//            {
//                currentSpinSpeed = 0f;
//            }

//            // Update visual spin speed
//            torompoModel.transform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime);

//            // Adjust particle effects based on speed
//            var emission = spinParticles.emission;
//            emission.rateOverTime = Mathf.Abs(currentSpinSpeed / maxSpinSpeed) * 50;

//            // Adjust audio pitch based on speed
//            spinAudio.pitch = 0.5f + (Mathf.Abs(currentSpinSpeed / maxSpinSpeed));

//            // Check for game over condition
//            if (currentSpinSpeed >= minSpinSpeed)
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
//        currentSpinSpeed -= spinBoostPerMatch;
//        if (currentSpinSpeed < maxSpinSpeed)
//        {
//            currentSpinSpeed = maxSpinSpeed;
//        }
//        spinAudio.PlayOneShot(successSound);
//    }

//    public void MissedMatch()
//    {
//        // Extra penalty for missing a note
//        currentSpinSpeed += spinBoostPerMatch * 0.5f;

//        // Ensure spin speed doesn't go positive
//        if (currentSpinSpeed > 0f)
//        {
//            currentSpinSpeed = 0f;
//        }

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
        if (isSpinning)
        {
            // Apply continuous spin decay (reducing positive value)
            currentSpinSpeed -= spinDecayRate * Time.deltaTime;

            // Ensure spin speed doesn't go negative
            if (currentSpinSpeed < 0f)
            {
                currentSpinSpeed = 0f;
            }

            // Update visual spin speed (using positive rotation)
            torompoModel.transform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime);

            // Adjust particle effects based on speed
            var emission = spinParticles.emission;
            emission.rateOverTime = (currentSpinSpeed / maxSpinSpeed) * 50;

            // Adjust audio pitch based on speed
            spinAudio.pitch = 0.5f + (currentSpinSpeed / maxSpinSpeed);

            // Check for game over condition (when speed drops below minimum)
            if (currentSpinSpeed <= minSpinSpeed)
            {
                StopSpinning();
                // Notify the game manager that this player lost (other player wins)
                int winnerIndex = (playerIndex == 1) ? 2 : 1;
                TurompoGameManager.Instance.DeclareWinner(winnerIndex);
            }
        }
    }

    public void ResetTorompo()
    {
        currentSpinSpeed = maxSpinSpeed;
        isSpinning = true;
        spinParticles.Play();
        spinAudio.Play();
    }

    public void StopSpinning()
    {
        isSpinning = false;
        spinParticles.Stop();
        spinAudio.Stop();
    }

    public void BoostSpin()
    {
        // Increase spin speed on successful matches
        currentSpinSpeed += spinBoostPerMatch;
        if (currentSpinSpeed > maxSpinSpeed)
        {
            currentSpinSpeed = maxSpinSpeed;
        }
        spinAudio.PlayOneShot(successSound);
    }

    public void MissedMatch()
    {
        // Play the fail sound but don't modify the spin speed on misses
        // This ensures rotation speed only increases on successful matches
        spinAudio.PlayOneShot(failSound);
    }
}