using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the button is clicked")]
    [SerializeField] private string targetSceneName = "YourSceneName";

    // This method can be called by a UI Button via the OnClick event in the inspector
    public void ChangeScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is not set!");
            return;
        }

        Debug.Log("Changing to scene: " + targetSceneName);
        SceneManager.LoadScene(targetSceneName);
    }

    // Optional: You can also create a method that accepts a scene name parameter
    // This allows you to use one script for multiple buttons that go to different scenes
    public void ChangeToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name parameter is empty!");
            return;
        }

        Debug.Log("Changing to scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}