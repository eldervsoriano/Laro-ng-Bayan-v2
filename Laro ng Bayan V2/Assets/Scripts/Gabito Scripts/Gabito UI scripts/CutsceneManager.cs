using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public VideoPlayer videoPlayer;           // Assign your VideoPlayer component
    public string nextSceneName = "LoadingScene";  // Next scene to load after video or skip

    [Header("UI")]
    public Button skipButton;                 // Button to skip the cutscene

    private bool isSkipping = false;

    private void Start()
    {
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipCutscene);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;  // Detect when the video finishes
            videoPlayer.Play();                          // Start playback
        }
        else
        {
            Debug.LogWarning("No VideoPlayer assigned!");
            LoadNextScene(); // fail-safe
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (!isSkipping)
            LoadNextScene();
    }

    public void SkipCutscene()
    {
        if (isSkipping) return;
        isSkipping = true;

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
