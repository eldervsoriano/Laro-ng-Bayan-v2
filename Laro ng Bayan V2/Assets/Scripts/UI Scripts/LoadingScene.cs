//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class LoadingScene : MonoBehaviour
//{
//    public GameObject LoadingScreen;
//    public Image LoadingBarFill;

//    public void LoadScene(int sceneId)
//    {
//        StartCoroutine(LoadSceneAsync(sceneId));
//    }

//    IEnumerator LoadSceneAsync(int sceneId)
//    {
//        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

//        while (!operation.isDone)
//        {
//            Debug.Log("tite");
//            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
//            LoadingBarFill.fillAmount = progressValue;
//            yield return null;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add TextMeshPro namespace

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public TextMeshProUGUI LoadingText; // Changed to TextMeshProUGUI

    // Minimum time to show loading screen (seconds)
    public float minimumLoadingTime = 1.5f;

    public void LoadScene(int sceneId)
    {
        // Make sure the loading screen is active
        LoadingScreen.SetActive(true);

        // Reset the loading bar
        LoadingBarFill.fillAmount = 0f;

        // Start the loading coroutine
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        // Start loading the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        // Don't let the scene activate until we allow it
        operation.allowSceneActivation = false;

        // Track loading start time
        float startTime = Time.time;
        float progress = 0f;

        // Wait until the load is nearly complete
        while (progress < 0.9f)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progress;

            // Update the loading text if available
            if (LoadingText != null)
            {
                LoadingText.text = string.Format("Loading: {0}%", Mathf.RoundToInt(progress * 100));
            }

            yield return null;
        }

        // Ensure we show the loading screen for at least the minimum time
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadingTime)
        {
            // Animate from 90% to 100% during remaining time
            float remainingTime = minimumLoadingTime - elapsedTime;
            float startProgress = LoadingBarFill.fillAmount;

            while (elapsedTime < minimumLoadingTime)
            {
                elapsedTime = Time.time - startTime;
                float t = 1.0f - ((minimumLoadingTime - elapsedTime) / remainingTime);

                LoadingBarFill.fillAmount = Mathf.Lerp(startProgress, 1.0f, t);

                if (LoadingText != null)
                {
                    LoadingText.text = string.Format("Loading: {0}%", Mathf.RoundToInt(LoadingBarFill.fillAmount * 100));
                }

                yield return null;
            }
        }

        // Make sure the bar is filled completely
        LoadingBarFill.fillAmount = 1.0f;
        if (LoadingText != null)
        {
            LoadingText.text = "Loading: 100%";
        }

        // Allow the scene to activate
        operation.allowSceneActivation = true;

        // Wait one frame to ensure everything is properly set
        yield return null;

        // Hide the loading screen (optional - you might want to keep this commented out if the new scene has its own way to hide it)
        // LoadingScreen.SetActive(false);
    }
}