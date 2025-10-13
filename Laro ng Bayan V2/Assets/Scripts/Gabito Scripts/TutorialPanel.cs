using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video; 

[System.Serializable]
public class TutorialPage
{
    [TextArea] public string text;  // Tutorial text
    public GameObject imageOrVideoPanel;   // A panel from the scene
}

public class TutorialPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject doneButton;
    [SerializeField] private GameObject skipButton;

    [Header("Tutorial Pages")]
    [SerializeField] private List<TutorialPage> tutorialPages;

    private int currentIndex = 0;
    private CountdownManager countdownManager;

    public PamatoShooter[] shooters; // assign Player1 & Player2 in Inspector

    void Start()
    {
        PauseButton.canPause = false;
        countdownManager = FindObjectOfType<CountdownManager>();

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game during tutorial
        }

        // Disable gameplay while tutorial is showing
        foreach (var shooter in shooters)
            shooter.enabled = false;

        currentIndex = 0;
        ShowPage(currentIndex);
    }



    private void ShowPage(int index)
    {
        if (index >= 0 && index < tutorialPages.Count)
        {
            if (tutorialText != null)
                tutorialText.text = tutorialPages[index].text;

            // Only show the active page image
            for (int i = 0; i < tutorialPages.Count; i++)
            {
                var page = tutorialPages[i];
                if (page.imageOrVideoPanel != null)
                {
                    bool isActive = (i == index);
                    page.imageOrVideoPanel.SetActive(isActive);

                    // Control video playback based on visibility
                    var videoPlayer = page.imageOrVideoPanel.GetComponentInChildren<VideoPlayer>();
                    if (videoPlayer != null)
                    {
                        if (isActive)
                        {
                            videoPlayer.frame = 0; // rewind to beginning each time
                            videoPlayer.Play();
                        }
                        else
                        {
                            videoPlayer.Stop();
                            videoPlayer.frame = 0; // reset preview to first frame
                        }

                    }
                }
            }
        }

        // Toggle navigation buttons
        if (backButton != null) backButton.SetActive(index > 0);  // hide Back on first page
        if (nextButton != null) nextButton.SetActive(index < tutorialPages.Count - 1);
        if (doneButton != null) doneButton.SetActive(index == tutorialPages.Count - 1);
    }


    public void OnNextClicked()
    {
        currentIndex++;
        if (currentIndex < tutorialPages.Count)
            ShowPage(currentIndex);
    }

    public void OnBackClicked()
    {
        currentIndex--;
        if (currentIndex >= 0)
            ShowPage(currentIndex);
    }

    public void OnDoneClicked() => CloseTutorial();
    public void OnSkipClicked() => CloseTutorial();

    private void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // Hide all tutorial images
        foreach (var page in tutorialPages)
            if (page.imageOrVideoPanel != null)
                page.imageOrVideoPanel.SetActive(false);

        Time.timeScale = 1f; // Resume game

        // Allow pausing AFTER tutorial is done
        PauseButton.canPause = true;

        // Begin countdown after tutorial
        if (countdownManager != null)
            countdownManager.BeginCountdown();
    }

    public void ReopenTutorial()
    {
        Time.timeScale = 0f; // Pause again
        PauseButton.canPause = false;


        // Reset to first page or last viewed page (your choice)
        currentIndex = 0;
        ShowPage(currentIndex);
    }



}
