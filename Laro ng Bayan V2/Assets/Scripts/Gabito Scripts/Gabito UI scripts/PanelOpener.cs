using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    [Header("Panel to Open")]
    [SerializeField] public GameObject targetPanel; // the panel you want to open (e.g. TutorialPanel)

    [Header("Optional Settings")]
    [Tooltip("Pause the game when the panel opens")]
    [SerializeField] private bool pauseGame = true;

    [Tooltip("Show the mouse cursor while panel is open")]
    [SerializeField] private bool showCursor = true;

    [Tooltip("Temporarily disable PauseButton script while panel is open")]
    [SerializeField] private bool disablePauseScript = true;

    private PauseButton pauseButtonRef; // reference to the PauseButton script

    void Start()
    {
        // Find the PauseButton script in the scene (optional but handy)
        pauseButtonRef = FindObjectOfType<PauseButton>();
    }

    // Called by your Info Button
    public void OpenPanel()
    {
        if (targetPanel == null)
        {
            Debug.LogWarning($"{nameof(PanelOpener)}: No panel assigned!");
            return;
        }

        // Enable the target panel
        targetPanel.SetActive(true);

        // Pause game if chosen
        if (pauseGame)
            Time.timeScale = 0f;

        // Disable the pause button script so ESC won’t stack
        if (disablePauseScript && pauseButtonRef != null)
        {
            pauseButtonRef.enabled = false;
            PauseButton.canPause = false;
        }

        // Show cursor for UI interaction
        if (showCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Optional close helper (for your Close button)
    public void ClosePanel()
    {
        if (targetPanel == null)
            return;

        targetPanel.SetActive(false);

        if (pauseGame)
            Time.timeScale = 1f;

        // Re-enable pause script
        if (disablePauseScript && pauseButtonRef != null)
        {
            pauseButtonRef.enabled = true;
            PauseButton.canPause = true;
        }

        //// Optional: hide cursor back to gameplay
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
