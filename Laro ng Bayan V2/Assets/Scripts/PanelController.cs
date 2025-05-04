using UnityEngine;

public class PanelController : MonoBehaviour
{
    [Header("Panel Settings")]
    [Tooltip("Drag and drop your UI panel here")]
    [SerializeField] private GameObject targetPanel;

    [Header("Cursor Settings")]
    [Tooltip("Should the cursor be visible when panel is open")]
    [SerializeField] private bool showCursorOnOpen = true;

    [Header("Game Settings")]
    [Tooltip("Pause the game when panel is open")]
    [SerializeField] private bool pauseGameOnOpen = true;

    // Reference to player controller (optional)
    [Header("Player Control")]
    [Tooltip("Optional: Reference to your player controller to disable when panel is open")]
    [SerializeField] private MonoBehaviour playerController;

    private bool isPanelOpen = false;
    private CursorLockMode previousLockState;
    private bool previousCursorVisible;

    private void Start()
    {
        // Store initial cursor state
        previousLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        // Make sure the panel is hidden when the game starts
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Target panel is not assigned in the inspector!");
        }
    }

    private void Update()
    {
        // Check if Q key was pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;

        // Show/hide the panel
        if (targetPanel != null)
        {
            targetPanel.SetActive(isPanelOpen);
        }

        // Handle cursor visibility
        if (isPanelOpen && showCursorOnOpen)
        {
            // Store current cursor state before changing it
            previousLockState = Cursor.lockState;
            previousCursorVisible = Cursor.visible;

            // Show and unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Restore previous cursor state
            Cursor.lockState = previousLockState;
            Cursor.visible = previousCursorVisible;
        }

        // Handle game pause
        if (pauseGameOnOpen)
        {
            Time.timeScale = isPanelOpen ? 0f : 1f;
        }

        // Handle player controller
        if (playerController != null)
        {
            playerController.enabled = !isPanelOpen;
        }
    }

    // Method to close the panel (can be called from UI button)
    public void ClosePanel()
    {
        if (isPanelOpen)
        {
            TogglePanel(); // This will handle everything
        }
    }
}