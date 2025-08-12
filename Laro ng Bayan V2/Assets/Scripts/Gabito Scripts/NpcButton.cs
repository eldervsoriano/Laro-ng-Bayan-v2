using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcButton : MonoBehaviour
{
    // GABITO SCRIPT :D

    [Header("3D Prompt (Near NPC)")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("Confirmation UI Panel")]
    [SerializeField] private GameObject confirmationPanel;

    [Header("Scene To Load")]
    [SerializeField] private string sceneToLoad = "YourNextSceneName";

    private bool isPlayerNearby = false;
    private GameObject playerObject; // To find and store reference to player
    private SimpleCharacterController playerController;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Show UI and disable movement
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            if (confirmationPanel != null)
                confirmationPanel.SetActive(true);

            if (playerController != null)
                playerController.enabled = false;
        }
    }

    public void OnYesClicked()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnNoClicked()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);

        if (isPlayerNearby && interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (playerController != null)
            playerController.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerObject = other.gameObject;
            playerController = playerObject.GetComponent<SimpleCharacterController>();

            if (interactionPrompt != null && confirmationPanel != null && !confirmationPanel.activeSelf)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            if (confirmationPanel != null)
                confirmationPanel.SetActive(false);

            if (playerController != null)
                playerController.enabled = true; // Ensure control is restored when walking away
        }
    }
}
