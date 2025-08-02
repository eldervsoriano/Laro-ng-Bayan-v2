using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // GABITO SCRIPT 

    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        pausePanel.SetActive(true); // Show UI first
        Time.timeScale = 0f;        // THEN pause time
        isPaused = true;
    }

    // Now public so buttons can call it
    public void ResumeGame()
    {
        Time.timeScale = 1f;             // Unpause time first
        pausePanel.SetActive(false);     // THEN hide UI
        isPaused = false;
    }
}
