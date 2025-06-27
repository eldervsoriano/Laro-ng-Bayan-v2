//using UnityEngine;
//using UnityEngine.UI;

//public class PanelButtonController : MonoBehaviour
//{
//    [SerializeField] private GameObject panel;
//    [SerializeField] private Button openButton;
//    [SerializeField] private Button closeButton;
//    [SerializeField] private bool startClosed = true;

//    private void Start()
//    {
//        // Add click listeners to buttons
//        if (openButton != null)
//            openButton.onClick.AddListener(OpenPanel);

//        if (closeButton != null)
//            closeButton.onClick.AddListener(ClosePanel);

//        // Set initial panel state
//        if (panel != null && startClosed)
//            panel.SetActive(false);
//    }

//    public void OpenPanel()
//    {
//        if (panel != null)
//            panel.SetActive(true);
//    }

//    public void ClosePanel()
//    {
//        if (panel != null)
//            panel.SetActive(false);
//    }

//    // Toggle panel visibility
//    public void TogglePanel()
//    {
//        if (panel != null)
//            panel.SetActive(!panel.activeSelf);
//    }

//    private void OnDestroy()
//    {
//        // Clean up listeners when object is destroyed
//        if (openButton != null)
//            openButton.onClick.RemoveListener(OpenPanel);

//        if (closeButton != null)
//            closeButton.onClick.RemoveListener(ClosePanel);
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class PanelButtonController : MonoBehaviour
{
    [SerializeField] private GameObject panel; // This should be your pause menu panel
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private bool startClosed = true;

    private bool isPaused = false;

    private void Start()
    {
        // Add click listeners to buttons
        if (openButton != null)
            openButton.onClick.AddListener(OpenPanel);
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        // Set initial panel state
        if (panel != null && startClosed)
            panel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // Toggle panel visibility
    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            // Currently paused - resume game and hide pause menu
            ResumeGame();
        }
        else
        {
            // Currently playing - pause game and show pause menu
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // Show pause menu immediately
        if (panel != null)
        {
            panel.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }

        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;

        // Update pause button text
        if (pauseButton != null)
        {
            Text buttonText = pauseButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "Resume";
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Hide the panel when resuming
        if (panel != null)
            panel.SetActive(false);

        // Optional: Update pause button text
        if (pauseButton != null)
        {
            Text buttonText = pauseButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "Pause";
        }
    }

    // Public getter for pause state
    public bool IsPaused
    {
        get { return isPaused; }
    }

    private void OnDestroy()
    {
        // Clean up listeners when object is destroyed
        if (openButton != null)
            openButton.onClick.RemoveListener(OpenPanel);
        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(TogglePause);

        // Ensure time scale is reset when object is destroyed
        Time.timeScale = 1f;
    }
}