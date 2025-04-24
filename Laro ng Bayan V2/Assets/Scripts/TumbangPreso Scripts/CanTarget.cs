using UnityEngine;

public class CanTarget : MonoBehaviour
{
    private bool hasBeenHit = false;
    public float destroyDelay = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit) return;

        if (collision.gameObject.CompareTag("Slipper"))
        {
            hasBeenHit = true;

            TumbangGameManager.Instance.PlayerScored();

            // Optional: apply extra force
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 knockback = collision.relativeVelocity * 0.5f;
                rb.AddForce(knockback, ForceMode.Impulse);
            }

            // Start delayed destroy and spawn
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private System.Collections.IEnumerator DestroyAndRespawn()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Respawn new can
        CanSpawner spawner = FindObjectOfType<CanSpawner>();
        if (spawner != null)
        {
            spawner.SpawnNewCan();
        }

        Destroy(gameObject);
    }
}
