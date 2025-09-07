using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed for scene reload

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static bool isPaused = false;
    public static bool canPause = true; // NEW: Block pause during tutorial/countdown

    [SerializeField] private MonoBehaviour[] scriptsToDisable; // drag certain scripts to disable here

    [SerializeField] private GameObject[] objectsToDisable; // drag full GameObjects here



    void Start()
    {
        // Reset time scale in case coming from menu
        Time.timeScale = 1f;
    }

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (isPaused) return; // Prevent double-pausing

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;



        // Disable target scripts
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }

        // Disable target GameObjects
        foreach (var obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }


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

        // Re-enable target scripts
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = true;
        }

        // Re-enable target GameObjects
        foreach (var obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(true);
        }

    }

    public void ResumeGameWithCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!isPaused) return;

        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        // Re-enable target scripts
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = true;
        }

        // Re-enable target GameObjects
        foreach (var obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(true);
        }

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
