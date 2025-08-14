using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NpcButton : MonoBehaviour
{
    [Header("3D Prompt (Near NPC)")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("UI Panels")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject dialoguePanel;

    [Header("Dialogue Elements")]
    [SerializeField] private TMP_Text dialogueText;
    [TextArea]
    [SerializeField] private List<string> npcLines;

    private int currentLineIndex = 0;

    [Header("Scene To Load")]
    [SerializeField] private string sceneToLoad = "YourNextSceneName";

    private bool isPlayerNearby = false;
    private GameObject playerObject;
    private SimpleCharacterController playerController;
    private ThirdPersonCamera cameraController;
    private Animator playerAnimator; // NEW — for animation control
    private int speedParamID;
    private int isRunningParamID;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // Cache animator parameter hashes
        speedParamID = Animator.StringToHash("Speed");
        isRunningParamID = Animator.StringToHash("IsRunning");
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionPrompt != null) interactionPrompt.SetActive(false);

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
                currentLineIndex = 0;
                ShowDialogueLine(currentLineIndex);
            }

            if (playerController != null)
                playerController.enabled = false; // stop movement script

            if (cameraController != null)
                cameraController.enabled = false; // stop camera movement

            if (playerAnimator != null)
            {
                // Force idle animation
                playerAnimator.SetFloat(speedParamID, 0f);
                playerAnimator.SetBool(isRunningParamID, false);
            }

            // Show mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnNextClicked()
    {
        currentLineIndex++;

        if (currentLineIndex < npcLines.Count)
        {
            ShowDialogueLine(currentLineIndex);
        }
        else
        {
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
        // Optional: reset before scene change
        if (playerController != null) playerController.enabled = true;
        if (cameraController != null) cameraController.enabled = true;
        if (playerAnimator != null) playerAnimator.SetFloat(speedParamID, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnNoClicked()
    {
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (isPlayerNearby && interactionPrompt != null) interactionPrompt.SetActive(true);

        if (playerController != null)
            playerController.enabled = true;

        if (cameraController != null)
            cameraController.enabled = true;

        if (playerAnimator != null)
        {
            // Let movement script control animations again (no extra changes needed here)
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentLineIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerObject = other.gameObject;
            playerController = playerObject.GetComponent<SimpleCharacterController>();
            playerAnimator = playerObject.GetComponentInChildren<Animator>();
            cameraController = FindObjectOfType<ThirdPersonCamera>();

            if (interactionPrompt != null && !confirmationPanel.activeSelf)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            if (confirmationPanel != null) confirmationPanel.SetActive(false);
            if (dialoguePanel != null) dialoguePanel.SetActive(false);

            if (playerController != null) playerController.enabled = true;
            if (cameraController != null) cameraController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
