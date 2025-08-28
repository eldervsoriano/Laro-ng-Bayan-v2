using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static bool isPaused = false;
    public static bool canPause = true; // NEW: Block pause during tutorial/countdown


    void Update()
    {
        // Only allow ESC if pausing is allowed AND game is not already paused
        if (canPause && !isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return; // Prevent double-pausing

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Only the Resume button will call this
    public void ResumeGame()
    {
        if (!isPaused) return; // Prevent resume when not paused

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGameWithCursor()
    {
        if (!isPaused) return;

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
