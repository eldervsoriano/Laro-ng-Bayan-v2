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
    [Tooltip("Drop any scripts that should be disabled during countdown.")]
    public MonoBehaviour[] scriptsToDisable; // works for any component
    public static bool InputLocked = false; // add this at the top of CountdownManager


    [Header("Options")]
    [Tooltip("Check if this level has a tutorial panel.")]
    public bool hasTutorial = true;

    void Start()
    {
        if (!hasTutorial)
        {
            BeginCountdown();
        }
        // If hasTutorial = true, TutorialPanel will call BeginCountdown() later
    }

    public void BeginCountdown()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Freeze time
        Time.timeScale = 0f;

        // Disable pause during countdown
        PauseButton.canPause = false;

        // Disable gameplay scripts
        foreach (var script in scriptsToDisable)
            if (script != null) script.enabled = false;

        CountdownManager.InputLocked = true; // Lock inputs globally

        countdownPanel.SetActive(true);

        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            yield return new WaitForSecondsRealtime(1f); // works even when timeScale = 0
            timer--;
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);

        countdownPanel.SetActive(false);

        // Re-enable gameplay scripts
        foreach (var script in scriptsToDisable)
            if (script != null) script.enabled = true;

        // Unfreeze time
        Time.timeScale = 1f;
        PauseButton.canPause = true;
        CountdownManager.InputLocked = false; // Unlock inputs

        // --- Delay showing UIJolen panels a bit ---
        yield return new WaitForSecondsRealtime(0.5f);

        if (UIJolen.Instance != null)
        {
            UIJolen.Instance.SetProfilesVisible(true);
            UIJolen.Instance.AllowTurnUI();   // add this line
            UIJolen.Instance.UpdateTurn(1);
        }

    }
}
