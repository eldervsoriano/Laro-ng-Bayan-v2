using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    [Header("Countdown UI")]
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;
    public float countdownTime = 3f;

    [Header("Gameplay References")]
    public PamatoShooter[] shooters; // assign both Player 1 and 2 in inspector

    [Header("Options")]
    [Tooltip("Check if this level has a tutorial panel.")]
    public bool hasTutorial = true;

    void Start()
    {
        if (!hasTutorial)
        {
            // If no tutorial, start countdown immediately
            BeginCountdown();
        }
        // If hasTutorial = true then TutorialPanel handles it and calls BeginCountdown() later
    }

    public void BeginCountdown()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Disable gameplay while counting down
        foreach (var shooter in shooters)
            shooter.enabled = false;

        countdownPanel.SetActive(true);

        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);

        countdownPanel.SetActive(false);

        // Enable gameplay
        foreach (var shooter in shooters)
            shooter.enabled = true;

        // Initialize UI after countdown
        if (UIJolen.Instance != null)
        {
            UIJolen.Instance.SetProfilesVisible(true);
            UIJolen.Instance.UpdateTurn(1); // Player 1 starts
        }
    }
}
