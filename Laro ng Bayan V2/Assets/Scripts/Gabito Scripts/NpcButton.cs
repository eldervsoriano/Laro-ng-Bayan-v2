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

    [Header("Unlock Requirements")]
    [SerializeField] private bool requiresTurumpoUnlocked = false;


    private int currentLineIndex = 0;

    [Header("Scene To Load")]
    [SerializeField] private string sceneToLoad = "YourNextSceneName";

    [Header("Optional: Pause UI Button GameObject")]
    [Tooltip("Assign the pause button GameObject (the whole UI button). We'll SetActive(false/true) while dialogue is open.")]



    [SerializeField] private GameObject pauseUIButton; // << just a GameObject

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

        // Make sure pause is allowed at start (safe default)
        PauseButton.canPause = true;
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

            // Block pausing & hide pause button while in dialogue
            PauseButton.canPause = false;
            if (pauseUIButton != null) pauseUIButton.SetActive(false);

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

        // Allow pressing Space to advance dialogue
        if (dialoguePanel != null && dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            OnNextClicked();
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
            // keep Pause blocked until player chooses Yes/No
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
        // Re-enable before leaving scene (optional)
        PauseButton.canPause = true;
        if (pauseUIButton != null) pauseUIButton.SetActive(true);

        // Optional: reset before scene change
        if (playerController != null) playerController.enabled = true;
        if (cameraController != null) cameraController.enabled = true;
        if (playerAnimator != null) playerAnimator.SetFloat(speedParamID, 0f);

        // Force cursor to be visible and unlocked for the next scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnNoClicked()
    {
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (isPlayerNearby && interactionPrompt != null) interactionPrompt.SetActive(true);

        // Re-enable pause after dialogue is fully closed
        PauseButton.canPause = true;
        if (pauseUIButton != null) pauseUIButton.SetActive(true);

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

        // Re-enable pause once dialogue fully ends
        PauseButton.canPause = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if this NPC requires Turumpo unlocked
            if (requiresTurumpoUnlocked && !ObjectiveManager.Instance.turumpoUnlocked)
            {
                // Do NOT show prompt if locked
                return;
            }

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

            // Safety: restore pause ability if player walks away
            PauseButton.canPause = true;
            if (pauseUIButton != null) pauseUIButton.SetActive(true);

            if (playerController != null) playerController.enabled = true;
            if (cameraController != null) cameraController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Just in case: allow pause again when leaving NPC
            PauseButton.canPause = true;
        }
    }
}
