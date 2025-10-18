using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed for scene reload

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;   // parent pause panel
    [SerializeField] private GameObject mainPanel;    // the one with Resume, Options, Exit
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPanel; // optional
    [SerializeField] private GameObject exitPanel;     // optional


    public static bool isPaused = false;
    public static bool canPause = true; // NEW: Block pause during tutorial/countdown

    //void Start()
    //{
    //    // Reset time scale in case coming from menu
    //    Time.timeScale = 1f;
    //}


    void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Only resume if currently in the main panel
                if (mainPanel != null && mainPanel.activeSelf)
                {
                    ResumeGame();
                }
                else
                {
                    // Otherwise, return to main panel instead
                    ShowMainPanel();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (isPaused) return; // Prevent double-pausing

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowMainPanel(); // always start with the main panel visible


            pausePanel.SetActive(true);
        isPaused = true;
    }

    // Only the Resume button will call this
    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!isPaused) return; // Prevent resume when not paused

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;



    }

    public void ResumeGameWithCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!isPaused) return;

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Restart function
    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time is running again
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowMainPanel()
    {
        // Main panel ON, others OFF
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        //restartPanel.SetActive(false);
        exitPanel.SetActive(false);
    }
}