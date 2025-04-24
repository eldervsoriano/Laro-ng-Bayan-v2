using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIActions : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Enter the name of your Main Menu scene")]
    public string mainMenuSceneName = "MainMenu"; // editable in Inspector

    [Header("Optional References")]
    public Canvas gameCanvas; // drag your Canvas here in the Inspector

    public void PlayAgain()
    {
        if (gameCanvas != null)
        {
            gameCanvas.enabled = false; // ✅ Hide UI immediately
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("Main Menu scene name not set in UIActions script!");
        }
    }
}
