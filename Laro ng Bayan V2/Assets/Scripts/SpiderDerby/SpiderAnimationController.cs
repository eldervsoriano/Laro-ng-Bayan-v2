using UnityEngine;

public class SpiderAnimationController : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

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

        // Ensure we have an animator
        if (animator == null)
        {
            Debug.LogError("Animator component not found on spider model!");
        }

        // Start in idle state by default
        ResetToIdle();
    }

    // Method to trigger the Attack animation
    public void PlayAttackAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.SetTrigger(TRIGGER_ATTACK);
        }
    }

    // Method to trigger the Damage Taken animation
    public void PlayDamageTakenAnimation()
    {
        if (animator != null && !isDead)
        {
            animator.SetTrigger(TRIGGER_DAMAGE);
        }
    }

    // Method to trigger the Death animation
    public void PlayDeathAnimation()
    {
        if (animator != null && !isDead)
        {
            isDead = true;
            animator.SetBool(BOOL_IS_DEAD, true);
            animator.SetTrigger(TRIGGER_DEATH);
        }
    }

    // Method to reset to Idle animation
    public void ResetToIdle()
    {
        if (animator != null)
        {
            animator.ResetTrigger(TRIGGER_ATTACK);
            animator.ResetTrigger(TRIGGER_DAMAGE);
            animator.ResetTrigger(TRIGGER_DEATH);

            // Only reset the IsDead bool if we're actually resetting the spider
            // (This would typically happen at the start of a new game)
            if (isDead)
            {
                isDead = false;
                animator.SetBool(BOOL_IS_DEAD, false);
            }
        }
    }

    // Method to check if spider is currently dead
    public bool IsDead()
    {
        return isDead;
    }
}