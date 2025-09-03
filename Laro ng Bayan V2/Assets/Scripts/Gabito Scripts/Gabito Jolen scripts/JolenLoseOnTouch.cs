using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JolenLoseOnTouch : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    private Rigidbody rb;

    void Start()
    {
        // Save the original spawn point & rotation
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fall")) return;

        if (JolenGameManager.Instance == null) return;

        int loser = JolenGameManager.Instance.GetCurrentPlayer();
        Debug.Log("Player " + loser + " fell! Respawning pamato...");

        // Respawn pamato at starting point
        Respawn();

        // End the turn (switches player)
        JolenGameManager.Instance.NotifyPamatoFell();
    }

    private void Respawn()
    {
        // Reset position & rotation
        transform.position = startPos;
        transform.rotation = startRot;

        // Reset physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
