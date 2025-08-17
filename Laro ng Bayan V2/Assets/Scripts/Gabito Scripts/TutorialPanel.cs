using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TutorialPage
{
    [TextArea] public string text;       // Tutorial text
    public GameObject imagePanel;        // A panel from the scene hierarchy
}

public class TutorialPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;   // Parent panel
    [SerializeField] private TMP_Text tutorialText;      // Text field
    [SerializeField] private GameObject nextButton;      // Next button
    [SerializeField] private GameObject doneButton;      // Done/Okay button
    [SerializeField] private GameObject skipButton;      // Skip button

    [Header("Tutorial Pages")]
    [SerializeField] private List<TutorialPage> tutorialPages;

    private int currentIndex = 0;
    private CountdownManager countdownManager;

    public PamatoShooter[] shooters; // assign Player1 & Player2

    void Start()
    {
        countdownManager = FindObjectOfType<CountdownManager>();

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game during tutorial
        }

        // Disable gameplay during tutorial
        foreach (var shooter in shooters)
        {
            shooter.enabled = false;
        }

        currentIndex = 0;
        ShowPage(currentIndex);
    }

    void ShowPage(int index)
    {
        if (index >= 0 && index < tutorialPages.Count)
        {
            // Update tutorial text
            if (tutorialText != null)
                tutorialText.text = tutorialPages[index].text;

            // Activate only the current image panel
            for (int i = 0; i < tutorialPages.Count; i++)
            {
                if (tutorialPages[i].imagePanel != null)
                    tutorialPages[i].imagePanel.SetActive(i == index);
            }
        }

        // Toggle buttons
        if (nextButton != null) nextButton.SetActive(index < tutorialPages.Count - 1);
        if (doneButton != null) doneButton.SetActive(index == tutorialPages.Count - 1);
    }

    public void OnNextClicked()
    {
        currentIndex++;
        if (currentIndex < tutorialPages.Count)
        {
            ShowPage(currentIndex);
        }
    }

    public void OnDoneClicked()
    {
        CloseTutorial();
    }

    public void OnSkipClicked()
    {
        CloseTutorial();
    }

    void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // Hide all image panels when closing
        foreach (var page in tutorialPages)
        {
            if (page.imagePanel != null)
                page.imagePanel.SetActive(false);
        }

        // Resume game
        Time.timeScale = 1f;

        if (countdownManager != null)
            countdownManager.BeginCountdown();
    }
}
