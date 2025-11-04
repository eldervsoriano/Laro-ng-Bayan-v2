using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JolenLoseOnTouch : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    private Rigidbody rb;

    [Header("Player Settings")]
    [Tooltip("Set this to 1 for Player 1, or 2 for Player 2.")]
    public int playerNumber = 1;

    [Header("UI Settings")]
    [Tooltip("Drag the TextMeshProUGUI object that shows the fall message for this player.")]
    public TextMeshProUGUI fellText; // Each Jolen has its own message text

    [Tooltip("Color of the player's message text.")]
    public Color messageColor = Color.white;

    void Start()
    {
        // Save spawn position and rotation
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();

        if (fellText != null)
        {
            fellText.gameObject.SetActive(false);
            fellText.color = messageColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fall")) return;
        if (JolenGameManager.Instance == null) return;

        Debug.Log($"Player {playerNumber} fell! Respawning pamato...");

        // Show player-specific message
        if (fellText != null)
            StartCoroutine(ShowFellMessage());

        // Respawn pamato
        Respawn();

        // Notify game manager
        JolenGameManager.Instance.NotifyPamatoFell();
    }

    private IEnumerator ShowFellMessage()
    {
        string playerName = playerNumber == 1 ? "<color=#BBB2FF>Player 1</color>" : "<color=#FF9E90>Player 2</color>";
        fellText.text = $"{playerName} fell! Respawning to the original location...";
        fellText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        fellText.gameObject.SetActive(false);
    }

    private void Respawn()
    {
        transform.position = startPos;
        transform.rotation = startRot;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
