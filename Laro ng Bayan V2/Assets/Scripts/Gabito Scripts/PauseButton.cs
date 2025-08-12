using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // GABITO SCRIPT :D

    [SerializeField] private GameObject pausePanel;
    public static bool isPaused = false; // Make it static so other scripts can check it

    // If you want ESC back, uncomment this
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         if (isPaused)
    //             ResumeGame();
    //         else
    //             PauseGame();
    //     }
    // }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        // Hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGameWithCursor()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        // Hide cursor
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
