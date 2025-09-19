using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GabitoLoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;            // Assign in Inspector
    [SerializeField] private TextMeshProUGUI progressText;  // TMP version

    // This is the name of the scene you want to load
    public static string nextScene = "GabitoOpenWorld";

    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    // Call this if you want to load another scene later
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("GabitoLoadingScreen"); // Always load your loading screen scene
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            if (operation.progress >= 0.9f)
            {
                if (progressBar != null) progressBar.value = 1f;
                if (progressText != null) progressText.text = "100%";

                yield return new WaitForSeconds(0.5f); // Optional pause
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
