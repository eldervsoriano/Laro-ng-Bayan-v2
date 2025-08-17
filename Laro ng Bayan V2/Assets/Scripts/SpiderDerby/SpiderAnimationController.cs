//using UnityEngine;

//public class SpiderAnimationController : MonoBehaviour
//{
//    // Reference to the Animator component
//    private Animator animator;

//    // Animation parameter names (constants)
//    private const string TRIGGER_ATTACK = "Attack";
//    private const string TRIGGER_DAMAGE = "TakeDamage";
//    private const string TRIGGER_DEATH = "Death";
//    private const string BOOL_IS_DEAD = "IsDead";

//    // Flag to track if spider is dead
//    private bool isDead = false;

//    void Start()
//    {
//        // Get the Animator component
//        animator = GetComponent<Animator>();

//        // Ensure we have an animator
//        if (animator == null)
//        {
//            Debug.LogError("Animator component not found on spider model!");
//        }

//        // Start in idle state by default
//        ResetToIdle();
//    }

//    // Method to trigger the Attack animation
//    public void PlayAttackAnimation()
//    {
//        if (animator != null && !isDead)
//        {
//            animator.SetTrigger(TRIGGER_ATTACK);
//        }
//    }

//    // Method to trigger the Damage Taken animation
//    public void PlayDamageTakenAnimation()
//    {
//        if (animator != null && !isDead)
//        {
//            animator.SetTrigger(TRIGGER_DAMAGE);
//        }
//    }

//    // Method to trigger the Death animation
//    public void PlayDeathAnimation()
//    {
//        if (animator != null && !isDead)
//        {
//            isDead = true;
//            animator.SetBool(BOOL_IS_DEAD, true);
//            animator.SetTrigger(TRIGGER_DEATH);
//        }
//    }

//    // Method to reset to Idle animation
//    public void ResetToIdle()
//    {
//        if (animator != null)
//        {
//            animator.ResetTrigger(TRIGGER_ATTACK);
//            animator.ResetTrigger(TRIGGER_DAMAGE);
//            animator.ResetTrigger(TRIGGER_DEATH);

//            // Only reset the IsDead bool if we're actually resetting the spider
//            // (This would typically happen at the start of a new game)
//            if (isDead)
//            {
//                isDead = false;
//                animator.SetBool(BOOL_IS_DEAD, false);
//            }
//        }
//    }

//    // Method to check if spider is currently dead
//    public bool IsDead()
//    {
//        return isDead;
//    }
//}

using UnityEngine;
using System.Collections;

public class SpiderAnimationController : MonoBehaviour
{
    [Header("Animation References")]
    // Reference to the Animator component
    private Animator animator;

    [Header("Effect References")]
    // Particle Systems for different effects
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private ParticleSystem hitImpactParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private ParticleSystem damageParticles;

    // Audio Sources for sound effects
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitImpactSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    // Camera shake reference
    [SerializeField] private CameraShake cameraShake;

    [Header("Effect Settings")]
    // Camera shake parameters
    [SerializeField] private float attackShakeDuration = 0.3f;
    [SerializeField] private float attackShakeIntensity = 1.0f;
    [SerializeField] private float hitImpactShakeDuration = 0.5f;
    [SerializeField] private float hitImpactShakeIntensity = 1.5f;
    [SerializeField] private float deathShakeDuration = 0.8f;
    [SerializeField] private float deathShakeIntensity = 2.0f;

    // Hit impact settings
    [SerializeField] private float hitFlashDuration = 0.2f;
    [SerializeField] private Color hitFlashColor = Color.red;

    [Header("Hit Knockback Settings")]
    [SerializeField] private float hitKnockbackForce = 5.0f;
    [SerializeField] private Vector3 knockbackDirection = Vector3.back; // Editable in Inspector

    // Material references for hit flash effect
    private Renderer spiderRenderer;
    private Material originalMaterial;
    private Material flashMaterial;

    // Animation parameter names (constants)
    private const string TRIGGER_ATTACK = "Attack";
    private const string TRIGGER_DAMAGE = "TakeDamage";
    private const string TRIGGER_DEATH = "Death";
    private const string BOOL_IS_DEAD = "IsDead";

    // Flag to track if spider is dead
    private bool isDead = false;

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Get the renderer for hit flash effects
        spiderRenderer = GetComponent<Renderer>();
        if (spiderRenderer != null)
        {
            originalMaterial = spiderRenderer.material;
            // Create a flash material with the hit flash color
            flashMaterial = new Material(originalMaterial);
            flashMaterial.color = hitFlashColor;
        }

        // Get audio source if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Find camera shake component if not assigned
        if (cameraShake == null)
            cameraShake = FindObjectOfType<CameraShake>();

        // Ensure we have an animator
        if (animator == null)
        {
            Debug.LogError("Animator component not found on spider model!");
        }

        // Auto-find particle systems if not assigned
        SetupParticleSystems();

        // Start in idle state by default
        ResetToIdle();
    }

    // Method to automatically find and setup particle systems
    private void SetupParticleSystems()
    {
        if (attackParticles == null)
        {
            Transform attackParticleTransform = transform.Find("AttackParticles");
            if (attackParticleTransform != null)
                attackParticles = attackParticleTransform.GetComponent<ParticleSystem>();
        }

        if (hitImpactParticles == null)
        {
            Transform hitParticleTransform = transform.Find("HitImpactParticles");
            if (hitParticleTransform != null)
                hitImpactParticles = hitParticleTransform.GetComponent<ParticleSystem>();
        }

        if (deathParticles == null)
        {
            Transform deathParticleTransform = transform.Find("DeathParticles");
            if (deathParticleTransform != null)
                deathParticles = deathParticleTransform.GetComponent<ParticleSystem>();
        }

        if (damageParticles == null)
        {
            Transform damageParticleTransform = transform.Find("DamageParticles");
            if (damageParticleTransform != null)
                damageParticles = damageParticleTransform.GetComponent<ParticleSystem>();
        }
    }

    // Method to trigger the Attack animation with effects
    public void PlayAttackAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.SetTrigger(TRIGGER_ATTACK);
            PlayAttackEffects();
        }
    }

    private void PlayAttackEffects()
    {
        if (attackParticles != null) attackParticles.Play();
        if (audioSource != null && attackSound != null) audioSource.PlayOneShot(attackSound);
        if (cameraShake != null) cameraShake.Shake(attackShakeDuration, attackShakeIntensity);
    }

    public void PlayDamageTakenAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.SetTrigger(TRIGGER_DAMAGE);
            PlayHitImpactEffects();
        }
    }

    private void PlayHitImpactEffects()
    {
        if (hitImpactParticles != null) hitImpactParticles.Play();
        if (damageParticles != null) damageParticles.Play();

        if (audioSource != null && hitImpactSound != null) audioSource.PlayOneShot(hitImpactSound);
        if (audioSource != null && damageSound != null) audioSource.PlayOneShot(damageSound, 0.7f);

        if (cameraShake != null) cameraShake.Shake(hitImpactShakeDuration, hitImpactShakeIntensity);

        StartCoroutine(HitFlashEffect());
        StartCoroutine(HitKnockback());
    }

    private IEnumerator HitFlashEffect()
    {
        if (spiderRenderer != null && flashMaterial != null)
        {
            spiderRenderer.material = flashMaterial;
            yield return new WaitForSeconds(hitFlashDuration);
            spiderRenderer.material = originalMaterial;
        }
    }

    private IEnumerator HitKnockback()
    {
        Vector3 originalPosition = transform.position;

        // Use inspector direction, normalized
        Vector3 direction = knockbackDirection.normalized;
        Vector3 targetPosition = originalPosition + direction * hitKnockbackForce;

        float elapsedTime = 0f;
        float knockbackDuration = 0.2f;

        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition,
                elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition,
                elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    public void PlayDeathAnimation()
    {
        if (animator != null && !isDead)
        {
            isDead = true;
            animator.SetBool(BOOL_IS_DEAD, true);
            animator.SetTrigger(TRIGGER_DEATH);
            PlayDeathEffects();
        }
    }

    private void PlayDeathEffects()
    {
        if (deathParticles != null) deathParticles.Play();
        if (audioSource != null && deathSound != null) audioSource.PlayOneShot(deathSound);
        if (cameraShake != null) cameraShake.Shake(deathShakeDuration, deathShakeIntensity);
        StartCoroutine(DeathEffectSequence());
    }

    private IEnumerator DeathEffectSequence()
    {
        if (spiderRenderer != null)
        {
            Color originalColor = spiderRenderer.material.color;
            for (int i = 0; i < 3; i++)
            {
                spiderRenderer.material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spiderRenderer.material.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }

            float fadeTime = 1.0f;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                float alpha = Mathf.Lerp(1f, 0.3f, elapsedTime / fadeTime);
                Color fadeColor = originalColor;
                fadeColor.a = alpha;
                spiderRenderer.material.color = fadeColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void ResetToIdle()
    {
        if (animator != null)
        {
            animator.ResetTrigger(TRIGGER_ATTACK);
            animator.ResetTrigger(TRIGGER_DAMAGE);
            animator.ResetTrigger(TRIGGER_DEATH);

            if (isDead)
            {
                isDead = false;
                animator.SetBool(BOOL_IS_DEAD, false);
                if (spiderRenderer != null && originalMaterial != null)
                {
                    Color resetColor = originalMaterial.color;
                    resetColor.a = 1f;
                    spiderRenderer.material.color = resetColor;
                }
            }
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TriggerHitImpact()
    {
        PlayHitImpactEffects();
    }

    public void SetParticleSystems(ParticleSystem attack, ParticleSystem hitImpact,
                                  ParticleSystem death, ParticleSystem damage)
    {
        attackParticles = attack;
        hitImpactParticles = hitImpact;
        deathParticles = death;
        damageParticles = damage;
    }

    public void SetAudioClips(AudioClip attack, AudioClip hitImpact,
                             AudioClip damage, AudioClip death)
    {
        attackSound = attack;
        hitImpactSound = hitImpact;
        damageSound = damage;
        deathSound = death;
    }
}
