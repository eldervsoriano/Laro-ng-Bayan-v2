using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsSwitchButton : MonoBehaviour
{
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject targetPanel;

    public void SwitchPanel()
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        if (targetPanel != null)
            targetPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed. Exiting game...");

        Application.Quit();

    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);

        // Reset time and pause states
        Time.timeScale = 1f;
        PauseButton.isPaused = false;
        PauseButton.canPause = true;

        SceneManager.LoadScene(sceneName);
    }

}
