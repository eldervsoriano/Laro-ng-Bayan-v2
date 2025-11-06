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

//    // Speed Bar UI
//    [Header("Speed Bar UI")]
//    public Image speedBarFill; // The fill image of the speed bar
//    public Color highSpeedColor = Color.green;
//    public Color mediumSpeedColor = Color.yellow;
//    public Color lowSpeedColor = Color.red;
//    public Color criticalSpeedColor = Color.red;

//    // Attack Animation System
//    private TurompoAttackAnimator attackAnimator;

//    // Gameplay state
//    private bool isSpinning = false;
//    private float minSpinSpeed = 50f; // minimum speed before game over

//    void Start()
//    {
//        // Get the attack animator component
//        attackAnimator = GetComponent<TurompoAttackAnimator>();

//        // Initialize speed bar
//        InitializeSpeedBar();

//        ResetTorompo();
//    }

//    void Update()
//    {
//        if (isSpinning && TurompoGameManager.Instance.IsGameActive())
//        {
//            // Apply continuous spin decay (reducing positive value)
//            currentSpinSpeed -= spinDecayRate * Time.deltaTime;

//            // Ensure spin speed doesn't go negative
//            if (currentSpinSpeed < 0f)
//            {
//                currentSpinSpeed = 0f;
//            }

//            // Update visual spin speed (using positive rotation)
//            if (torompoModel != null)
//            {
//                torompoModel.transform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime);
//            }

//            // Adjust particle effects based on speed
//            if (spinParticles != null)
//            {
//                var emission = spinParticles.emission;
//                emission.rateOverTime = (currentSpinSpeed / maxSpinSpeed) * 50;
//            }

//            // Adjust audio pitch based on speed
//            if (spinAudio != null)
//            {
//                spinAudio.pitch = 0.5f + (currentSpinSpeed / maxSpinSpeed);
//            }

//            // Update speed bar
//            UpdateSpeedBar();

//            // Check for game over condition (when speed drops below minimum)
//            if (currentSpinSpeed <= minSpinSpeed)
//            {
//                StopSpinning();

//                // Notify the game manager that this player lost (other player wins)
//                if (TurompoGameManager.Instance != null)
//                {
//                    // Call PlayerGameOver directly with the current player index
//                    TurompoGameManager.Instance.PlayerGameOver(playerIndex);

//                    // Log for debugging
//                    Debug.Log("Player " + playerIndex + " stopped spinning. Game over triggered.");
//                }
//            }
//        }
//    }

//    private void InitializeSpeedBar()
//    {
//        if (speedBarFill != null)
//        {
//            speedBarFill.fillAmount = 1f; // Start at full speed
//        }
//    }

//    private void UpdateSpeedBar()
//    {
//        if (speedBarFill != null)
//        {
//            // Calculate speed percentage
//            float speedPercentage = currentSpinSpeed / maxSpinSpeed;

//            // Update the fill amount (0 to 1)
//            speedBarFill.fillAmount = speedPercentage;

//            // Update the color based on speed percentage
//            if (speedPercentage > 0.7f)
//            {
//                speedBarFill.color = highSpeedColor;
//            }
//            else if (speedPercentage > 0.4f)
//            {
//                speedBarFill.color = mediumSpeedColor;
//            }
//            else if (speedPercentage > 0.2f)
//            {
//                speedBarFill.color = lowSpeedColor;
//            }
//            else
//            {
//                speedBarFill.color = criticalSpeedColor;

//                // Optional: Add blinking effect when critical
//                if (speedPercentage <= (minSpinSpeed / maxSpinSpeed))
//                {
//                    float blinkAlpha = Mathf.Sin(Time.time * 8f) * 0.5f + 0.5f;
//                    Color blinkColor = criticalSpeedColor;
//                    blinkColor.a = blinkAlpha;
//                    speedBarFill.color = blinkColor;
//                }
//            }
//        }
//    }

//    public void ResetTorompo()
//    {
//        currentSpinSpeed = maxSpinSpeed;
//        isSpinning = true;

//        if (spinParticles != null)
//            spinParticles.Play();

//        if (spinAudio != null)
//            spinAudio.Play();

//        // Reset attack animator to original state
//        if (attackAnimator != null)
//        {
//            attackAnimator.ResetToOriginal();
//        }

//        // Update speed bar on reset
//        UpdateSpeedBar();
//    }

//    public void StopSpinning()
//    {
//        isSpinning = false;

//        if (spinParticles != null)
//            spinParticles.Stop();

//        if (spinAudio != null)
//            spinAudio.Stop();

//        // Update speed bar to show empty
//        if (speedBarFill != null)
//        {
//            speedBarFill.fillAmount = 0f;
//        }

//        // Log for debugging
//        Debug.Log("Player " + playerIndex + " torompo stopped spinning.");
//    }

//    public void BoostSpin()
//    {
//        // Increase spin speed on successful matches
//        currentSpinSpeed += spinBoostPerMatch;

//        if (currentSpinSpeed > maxSpinSpeed)
//        {
//            currentSpinSpeed = maxSpinSpeed;
//        }

//        // Trigger attack animation
//        if (attackAnimator != null)
//        {
//            attackAnimator.TriggerAttack();
//        }

//        if (spinAudio != null && successSound != null)
//            spinAudio.PlayOneShot(successSound);

//        // Update speed bar immediately after boost
//        UpdateSpeedBar();
//    }

//    /// <summary>
//    /// Boost spin with collision point for directional attack animation
//    /// </summary>
//    public void BoostSpinWithCollision(Vector3 collisionPoint)
//    {
//        // Increase spin speed on successful matches
//        currentSpinSpeed += spinBoostPerMatch;

//        if (currentSpinSpeed > maxSpinSpeed)
//        {
//            currentSpinSpeed = maxSpinSpeed;
//        }

//        // Trigger attack animation with collision effects
//        if (attackAnimator != null)
//        {
//            attackAnimator.TriggerAttackWithCollision(collisionPoint);
//        }

//        if (spinAudio != null && successSound != null)
//            spinAudio.PlayOneShot(successSound);

//        // Update speed bar immediately after boost
//        UpdateSpeedBar();
//    }

//    public void MissedMatch()
//    {
//        // Play the fail sound but don't modify the spin speed on misses
//        // This ensures rotation speed only increases on successful matches
//        if (spinAudio != null && failSound != null)
//            spinAudio.PlayOneShot(failSound);
//    }

//    // Public getter for current speed percentage (useful for UI or other components)
//    public float GetSpeedPercentage()
//    {
//        return currentSpinSpeed / maxSpinSpeed;
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurompoController : MonoBehaviour
{
    // Player identification
    public int playerIndex = 1; // 1 for player 1, 2 for player 2

    // Spin mechanics
    public float maxSpinSpeed = 720f; // max speed reference
    public float currentSpinSpeed;
    public float spinDecayRate = 30f;
    public float spinBoostPerMatch = 50f;
    private float minSpinSpeed = 50f;

    // Animator reference
    [Header("Animator")]
    public Animator torompoAnimator;
    public float animationTransitionSpeed = 0.3f; // Duration for smooth transitions

    // Visual feedback
    public ParticleSystem spinParticles;
    public AudioSource spinAudio;
    public AudioClip successSound;
    public AudioClip failSound;

    // Speed Bar UI
    [Header("Speed Bar UI")]
    public Image speedBarFill;
    public Color highSpeedColor = Color.green;
    public Color mediumSpeedColor = Color.yellow;
    public Color lowSpeedColor = Color.red;
    public Color criticalSpeedColor = Color.red;

    // Attack Animation System
    private TurompoAttackAnimator attackAnimator;

    // Gameplay state
    private bool isSpinning = false;
    private string currentAnimationState = "";

    void Start()
    {
        attackAnimator = GetComponent<TurompoAttackAnimator>();
        InitializeSpeedBar();
        ResetTorompo();
    }

    void Update()
    {
        if (isSpinning && TurompoGameManager.Instance.IsGameActive())
        {
            // Apply decay
            currentSpinSpeed -= spinDecayRate * Time.deltaTime;
            if (currentSpinSpeed < 0f)
                currentSpinSpeed = 0f;

            UpdateSpeedBar();
            UpdateSpinAnimation();

            // Check game over
            if (currentSpinSpeed <= minSpinSpeed)
            {
                StopSpinning();
                TurompoGameManager.Instance.PlayerGameOver(playerIndex);
                Debug.Log("Player " + playerIndex + " stopped spinning. Game over triggered.");
            }
        }
    }

    private void InitializeSpeedBar()
    {
        if (speedBarFill != null)
            speedBarFill.fillAmount = 1f;
    }

    private void UpdateSpeedBar()
    {
        if (speedBarFill != null)
        {
            float speedPercentage = currentSpinSpeed / maxSpinSpeed;
            speedBarFill.fillAmount = speedPercentage;

            if (speedPercentage > 0.7f)
                speedBarFill.color = highSpeedColor;
            else if (speedPercentage > 0.4f)
                speedBarFill.color = mediumSpeedColor;
            else if (speedPercentage > 0.2f)
                speedBarFill.color = lowSpeedColor;
            else
            {
                speedBarFill.color = criticalSpeedColor;
                if (speedPercentage <= (minSpinSpeed / maxSpinSpeed))
                {
                    float blinkAlpha = Mathf.Sin(Time.time * 8f) * 0.5f + 0.5f;
                    Color blinkColor = criticalSpeedColor;
                    blinkColor.a = blinkAlpha;
                    speedBarFill.color = blinkColor;
                }
            }
        }
    }

    private void UpdateSpinAnimation()
    {
        if (torompoAnimator == null) return;

        float normalizedSpeed = currentSpinSpeed / maxSpinSpeed;
        string targetAnimationState = "";

        // Determine target animation based on speed
        if (normalizedSpeed > 0.7f)
            targetAnimationState = "HighSpin";
        else if (normalizedSpeed > 0.4f)
            targetAnimationState = "MidSpin";
        else if (normalizedSpeed > 0.2f)
            targetAnimationState = "LowSpin";
        else
            targetAnimationState = "NoSpin";

        // Only transition if we need to change to a different animation
        if (targetAnimationState != currentAnimationState)
        {
            torompoAnimator.CrossFade(targetAnimationState, animationTransitionSpeed);
            currentAnimationState = targetAnimationState;
        }

        // Optionally: Adjust animation speed to match spin speed
        // Uncomment if you want the animation itself to speed up/slow down
        // torompoAnimator.speed = Mathf.Lerp(0.3f, 2.0f, normalizedSpeed);
    }

    public void ResetTorompo()
    {
        currentSpinSpeed = maxSpinSpeed;
        isSpinning = true;

        if (spinParticles != null) spinParticles.Play();
        if (spinAudio != null) spinAudio.Play();

        if (attackAnimator != null)
            attackAnimator.ResetToOriginal();

        // Force immediate animation state on reset
        if (torompoAnimator != null)
        {
            torompoAnimator.Play("HighSpin");
            currentAnimationState = "HighSpin";
        }

        UpdateSpeedBar();
    }

    public void StopSpinning()
    {
        isSpinning = false;

        if (spinParticles != null) spinParticles.Stop();
        if (spinAudio != null) spinAudio.Stop();

        if (speedBarFill != null)
            speedBarFill.fillAmount = 0f;

        // Smooth transition to stopped state
        if (torompoAnimator != null)
        {
            torompoAnimator.CrossFade("NoSpin", 0.5f);
            currentAnimationState = "NoSpin";
        }

        Debug.Log("Player " + playerIndex + " torompo stopped spinning.");
    }

    public void BoostSpin()
    {
        currentSpinSpeed += spinBoostPerMatch;
        if (currentSpinSpeed > maxSpinSpeed)
            currentSpinSpeed = maxSpinSpeed;

        if (attackAnimator != null)
            attackAnimator.TriggerAttack();

        if (spinAudio != null && successSound != null)
            spinAudio.PlayOneShot(successSound);

        UpdateSpeedBar();
        UpdateSpinAnimation();
    }

    public void BoostSpinWithCollision(Vector3 collisionPoint)
    {
        currentSpinSpeed += spinBoostPerMatch;
        if (currentSpinSpeed > maxSpinSpeed)
            currentSpinSpeed = maxSpinSpeed;

        if (attackAnimator != null)
            attackAnimator.TriggerAttackWithCollision(collisionPoint);

        if (spinAudio != null && successSound != null)
            spinAudio.PlayOneShot(successSound);

        UpdateSpeedBar();
        UpdateSpinAnimation();
    }

    public void MissedMatch()
    {
        if (spinAudio != null && failSound != null)
            spinAudio.PlayOneShot(failSound);
    }

    public float GetSpeedPercentage()
    {
        return currentSpinSpeed / maxSpinSpeed;
    }
}