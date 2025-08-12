using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Only if you use TextMesh Pro

public class NpcButton : MonoBehaviour
{
    [Header("3D Prompt (Near NPC)")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("UI Panels")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject dialoguePanel;

    [Header("Dialogue Elements")]
    [SerializeField] private TMP_Text dialogueText; // TMP recommended
    [TextArea]
    [SerializeField] private List<string> npcLines;

    private int currentLineIndex = 0;

    [Header("Scene To Load")]
    [SerializeField] private string sceneToLoad = "YourNextSceneName";

    private bool isPlayerNearby = false;
    private GameObject playerObject;
    private SimpleCharacterController playerController;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
                currentLineIndex = 0;        // Reset dialogue index here before showing dialogue
                ShowDialogueLine(currentLineIndex);
            }

            if (playerController != null)
                playerController.enabled = false;
        }
    }

    // Called when "Next" button is pressed
    public void OnNextClicked()
    {
        currentLineIndex++;

        if (currentLineIndex < npcLines.Count)
        {
            ShowDialogueLine(currentLineIndex);
        }
        else
        {
            // End of dialogue — show Yes/No choices
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (confirmationPanel != null) confirmationPanel.SetActive(true);
        }
    }

    void ShowDialogueLine(int index)
    {
        if (dialogueText != null && index >= 0 && index < npcLines.Count)
        {
            dialogueText.text = npcLines[index];
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

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (isPlayerNearby && interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (playerController != null)
            playerController.enabled = true;

        currentLineIndex = 0; // Reset dialogue index here as well
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerObject = other.gameObject;
            playerController = playerObject.GetComponent<SimpleCharacterController>();

            if (interactionPrompt != null && !confirmationPanel.activeSelf)
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

            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);

            if (playerController != null)
                playerController.enabled = true;
        }
    }
}
