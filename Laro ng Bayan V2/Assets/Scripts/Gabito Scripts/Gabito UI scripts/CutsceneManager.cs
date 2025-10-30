using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public Image displayImage;           // The UI Image that displays the cutscene visuals
    public Sprite[] cutsceneSprites;     // The sequence of images
    public float displayDuration = 3f;   // How long each image shows before changing

    [Header("UI")]
    public Button skipButton;            // Button for skipping the cutscene
    public string nextSceneName = "LoadingScene";  // Scene to load after cutscene or skip

    private int currentIndex = 0;
    private bool isSkipping = false;

    private void Start()
    {
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipCutscene);

        if (cutsceneSprites.Length > 0)
        {
            displayImage.sprite = cutsceneSprites[0];
            StartCoroutine(PlayCutscene());
        }
        else
        {
            Debug.LogWarning("No cutscene sprites assigned!");
        }
    }

    private IEnumerator PlayCutscene()
    {
        while (currentIndex < cutsceneSprites.Length && !isSkipping)
        {
            displayImage.sprite = cutsceneSprites[currentIndex];
            yield return new WaitForSeconds(displayDuration);
            currentIndex++;
        }

        LoadNextScene();
    }

    public void SkipCutscene()
    {
        if (isSkipping) return;
        isSkipping = true;
        StopAllCoroutines();
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
