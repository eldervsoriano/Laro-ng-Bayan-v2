using UnityEngine;

public class GrassSlowEffect : MonoBehaviour
{
    public float slowMultiplier = 0.5f; // 50% speed while in grass

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pamato"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity *= slowMultiplier;
            }
        }
    }

    //private void OnTriggerStay(Collider other)
  /*  {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity *= slowMultiplier;
            }
        }
    }*/
}
