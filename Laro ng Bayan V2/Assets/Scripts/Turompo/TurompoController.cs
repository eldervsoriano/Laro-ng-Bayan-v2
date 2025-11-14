//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoController : MonoBehaviour
//{
//    // Player identification
//    public int playerIndex = 1; // 1 for player 1, 2 for player 2

//    // Spin mechanics
//    public float maxSpinSpeed = 720f; // max speed reference
//    public float currentSpinSpeed;
//    public float spinDecayRate = 30f;
//    public float spinBoostPerMatch = 50f;
//    private float minSpinSpeed = 50f;

//    // Animator reference
//    [Header("Animator")]
//    public Animator torompoAnimator;
//    public float animationTransitionSpeed = 0.3f; // Duration for smooth transitions

//    // Visual feedback
//    public ParticleSystem spinParticles;
//    public AudioSource spinAudio;
//    public AudioClip successSound;
//    public AudioClip failSound;

//    // Speed Bar UI
//    [Header("Speed Bar UI")]
//    public Image speedBarFill;
//    public Color highSpeedColor = Color.green;
//    public Color mediumSpeedColor = Color.yellow;
//    public Color lowSpeedColor = Color.red;
//    public Color criticalSpeedColor = Color.red;

//    // Attack Animation System
//    private TurompoAttackAnimator attackAnimator;

//    // Gameplay state
//    private bool isSpinning = false;
//    private string currentAnimationState = "";

//    void Start()
//    {
//        attackAnimator = GetComponent<TurompoAttackAnimator>();
//        InitializeSpeedBar();
//        ResetTorompo();
//    }

//    void Update()
//    {
//        if (isSpinning && TurompoGameManager.Instance.IsGameActive())
//        {
//            // Apply decay
//            currentSpinSpeed -= spinDecayRate * Time.deltaTime;
//            if (currentSpinSpeed < 0f)
//                currentSpinSpeed = 0f;

//            UpdateSpeedBar();
//            UpdateSpinAnimation();

//            // Check game over
//            if (currentSpinSpeed <= minSpinSpeed)
//            {
//                StopSpinning();
//                TurompoGameManager.Instance.PlayerGameOver(playerIndex);
//                Debug.Log("Player " + playerIndex + " stopped spinning. Game over triggered.");
//            }
//        }
//    }

//    private void InitializeSpeedBar()
//    {
//        if (speedBarFill != null)
//            speedBarFill.fillAmount = 1f;
//    }

//    private void UpdateSpeedBar()
//    {
//        if (speedBarFill != null)
//        {
//            float speedPercentage = currentSpinSpeed / maxSpinSpeed;
//            speedBarFill.fillAmount = speedPercentage;

//            if (speedPercentage > 0.7f)
//                speedBarFill.color = highSpeedColor;
//            else if (speedPercentage > 0.4f)
//                speedBarFill.color = mediumSpeedColor;
//            else if (speedPercentage > 0.2f)
//                speedBarFill.color = lowSpeedColor;
//            else
//            {
//                speedBarFill.color = criticalSpeedColor;
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

//    private void UpdateSpinAnimation()
//    {
//        if (torompoAnimator == null) return;

//        float normalizedSpeed = currentSpinSpeed / maxSpinSpeed;
//        string targetAnimationState = "";

//        // Determine target animation based on speed
//        if (normalizedSpeed > 0.7f)
//            targetAnimationState = "HighSpin";
//        else if (normalizedSpeed > 0.4f)
//            targetAnimationState = "MidSpin";
//        else if (normalizedSpeed > 0.2f)
//            targetAnimationState = "LowSpin";
//        else
//            targetAnimationState = "NoSpin";

//        // Only transition if we need to change to a different animation
//        if (targetAnimationState != currentAnimationState)
//        {
//            torompoAnimator.CrossFade(targetAnimationState, animationTransitionSpeed);
//            currentAnimationState = targetAnimationState;
//        }

//        // Optionally: Adjust animation speed to match spin speed
//        // Uncomment if you want the animation itself to speed up/slow down
//        // torompoAnimator.speed = Mathf.Lerp(0.3f, 2.0f, normalizedSpeed);
//    }

//    public void ResetTorompo()
//    {
//        currentSpinSpeed = maxSpinSpeed;
//        isSpinning = true;

//        if (spinParticles != null) spinParticles.Play();
//        if (spinAudio != null) spinAudio.Play();

//        if (attackAnimator != null)
//            attackAnimator.ResetToOriginal();

//        // Force immediate animation state on reset
//        if (torompoAnimator != null)
//        {
//            torompoAnimator.Play("HighSpin");
//            currentAnimationState = "HighSpin";
//        }

//        UpdateSpeedBar();
//    }

//    public void StopSpinning()
//    {
//        isSpinning = false;

//        if (spinParticles != null) spinParticles.Stop();
//        if (spinAudio != null) spinAudio.Stop();

//        if (speedBarFill != null)
//            speedBarFill.fillAmount = 0f;

//        // Smooth transition to stopped state
//        if (torompoAnimator != null)
//        {
//            torompoAnimator.CrossFade("NoSpin", 0.5f);
//            currentAnimationState = "NoSpin";
//        }

//        Debug.Log("Player " + playerIndex + " torompo stopped spinning.");
//    }

//    public void BoostSpin()
//    {
//        currentSpinSpeed += spinBoostPerMatch;
//        if (currentSpinSpeed > maxSpinSpeed)
//            currentSpinSpeed = maxSpinSpeed;

//        if (attackAnimator != null)
//            attackAnimator.TriggerAttack();

//        if (spinAudio != null && successSound != null)
//            spinAudio.PlayOneShot(successSound);

//        UpdateSpeedBar();
//        UpdateSpinAnimation();
//    }

//    public void BoostSpinWithCollision(Vector3 collisionPoint)
//    {
//        currentSpinSpeed += spinBoostPerMatch;
//        if (currentSpinSpeed > maxSpinSpeed)
//            currentSpinSpeed = maxSpinSpeed;

//        if (attackAnimator != null)
//            attackAnimator.TriggerAttackWithCollision(collisionPoint);

//        if (spinAudio != null && successSound != null)
//            spinAudio.PlayOneShot(successSound);

//        UpdateSpeedBar();
//        UpdateSpinAnimation();
//    }

//    public void MissedMatch()
//    {
//        if (spinAudio != null && failSound != null)
//            spinAudio.PlayOneShot(failSound);
//    }

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
    public int playerIndex = 1;

    // Spin mechanics
    public float maxSpinSpeed = 720f;
    public float currentSpinSpeed;
    public float spinDecayRate = 30f;
    public float spinBoostPerMatch = 50f;
    private float minSpinSpeed = 50f;

    // Animator reference
    [Header("Animator")]
    public Animator torompoAnimator;
    public float animationTransitionSpeed = 0.3f;

    // Speed thresholds for animation states
    [Header("Animation Thresholds")]
    [Range(0f, 1f)] public float highestSpinThreshold = 0.9f;  // 90%+
    [Range(0f, 1f)] public float highSpinThreshold = 0.7f;     // 70-90%
    [Range(0f, 1f)] public float midSpinThreshold = 0.4f;      // 40-70%
    [Range(0f, 1f)] public float lowSpinThreshold = 0.2f;      // 20-40%
    // Below 20% = NoSpin

    // Visual feedback
    public ParticleSystem spinParticles;
    public AudioSource spinAudio;
    public AudioClip successSound;
    public AudioClip failSound;

    // Speed Bar UI
    [Header("Speed Bar UI")]
    public Image speedBarFill;
    public Color highestSpeedColor = Color.cyan; // Cyan for highest speed!
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

            // Adjust particle emission based on speed
            UpdateParticleEffects();

            // Adjust audio pitch based on speed
            UpdateAudioPitch();

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

            // Updated color system with HighestSpin
            if (speedPercentage >= highestSpinThreshold)
                speedBarFill.color = highestSpeedColor;
            else if (speedPercentage >= highSpinThreshold)
                speedBarFill.color = highSpeedColor;
            else if (speedPercentage >= midSpinThreshold)
                speedBarFill.color = mediumSpeedColor;
            else if (speedPercentage >= lowSpinThreshold)
                speedBarFill.color = lowSpeedColor;
            else
            {
                speedBarFill.color = criticalSpeedColor;
                // Blinking effect when critical
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

        // Determine target animation based on speed thresholds
        if (normalizedSpeed >= highestSpinThreshold)
            targetAnimationState = "HighestSpin";
        else if (normalizedSpeed >= highSpinThreshold)
            targetAnimationState = "HighSpin";
        else if (normalizedSpeed >= midSpinThreshold)
            targetAnimationState = "MidSpin";
        else if (normalizedSpeed >= lowSpinThreshold)
            targetAnimationState = "LowSpin";
        else
            targetAnimationState = "NoSpin";

        // Only transition if we need to change to a different animation
        if (targetAnimationState != currentAnimationState)
        {
            torompoAnimator.CrossFade(targetAnimationState, animationTransitionSpeed);
            currentAnimationState = targetAnimationState;
            Debug.Log($"Player {playerIndex} animation transition: {currentAnimationState} at {(normalizedSpeed * 100):F1}% speed");
        }

        // Optional: Dynamic animation speed scaling
        // Uncomment to make animations play faster/slower based on spin speed
        // torompoAnimator.speed = Mathf.Lerp(0.5f, 2.0f, normalizedSpeed);
    }

    private void UpdateParticleEffects()
    {
        if (spinParticles != null)
        {
            var emission = spinParticles.emission;
            float normalizedSpeed = currentSpinSpeed / maxSpinSpeed;

            // More particles at higher speeds
            emission.rateOverTime = normalizedSpeed * 50f;

            // Optional: Change particle color based on speed tier
            var main = spinParticles.main;
            if (normalizedSpeed >= highestSpinThreshold)
                main.startColor = Color.cyan; // Cyan for highest!
            else if (normalizedSpeed >= highSpinThreshold)
                main.startColor = new Color(0f, 1f, 0f, 0.8f); // Green
            else if (normalizedSpeed >= midSpinThreshold)
                main.startColor = new Color(1f, 1f, 0f, 0.8f); // Yellow
            else
                main.startColor = new Color(1f, 0f, 0f, 0.8f); // Red
        }
    }

    private void UpdateAudioPitch()
    {
        if (spinAudio != null)
        {
            float normalizedSpeed = currentSpinSpeed / maxSpinSpeed;
            // Pitch ranges from 0.5 to 1.5 based on speed
            spinAudio.pitch = Mathf.Lerp(0.5f, 1.5f, normalizedSpeed);
        }
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
            // Start at HighestSpin since we're at max speed
            torompoAnimator.Play("HighestSpin");
            currentAnimationState = "HighestSpin";
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

    // Helper method to get current animation tier (useful for UI/debugging)
    public string GetCurrentSpeedTier()
    {
        float normalizedSpeed = currentSpinSpeed / maxSpinSpeed;
        if (normalizedSpeed >= highestSpinThreshold) return "Highest";
        if (normalizedSpeed >= highSpinThreshold) return "High";
        if (normalizedSpeed >= midSpinThreshold) return "Medium";
        if (normalizedSpeed >= lowSpinThreshold) return "Low";
        return "Critical";
    }
}