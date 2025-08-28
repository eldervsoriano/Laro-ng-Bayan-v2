using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TutorialPage
{
    [TextArea] public string text;  // Tutorial text
    public GameObject imagePanel;   // A panel from the scene
}

public class TutorialPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject nextButton;
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
                if (tutorialPages[i].imagePanel != null)
                    tutorialPages[i].imagePanel.SetActive(i == index);
        }

        // Toggle navigation buttons
        if (nextButton != null) nextButton.SetActive(index < tutorialPages.Count - 1);
        if (doneButton != null) doneButton.SetActive(index == tutorialPages.Count - 1);
    }

    public void OnNextClicked()
    {
        currentIndex++;
        if (currentIndex < tutorialPages.Count)
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
            if (page.imagePanel != null)
                page.imagePanel.SetActive(false);

        Time.timeScale = 1f; // Resume game

        // Begin countdown after tutorial
        if (countdownManager != null)
            countdownManager.BeginCountdown();
    }
}
