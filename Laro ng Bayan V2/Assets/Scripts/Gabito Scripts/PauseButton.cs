using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed for scene reload

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static bool isPaused = false;
    public static bool canPause = true; // NEW: Block pause during tutorial/countdown

    //void Start()
    //{
    //    // Reset time scale in case coming from menu
    //    Time.timeScale = 1f;
    //}

    void Update()
    {
        // Only allow ESC if pausing is allowed AND game is not already paused
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return; // Prevent double-pausing

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
}