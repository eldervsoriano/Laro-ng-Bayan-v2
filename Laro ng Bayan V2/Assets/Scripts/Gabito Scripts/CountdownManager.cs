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
        // Disable pause during countdown
        PauseButton.canPause = false;

        // Disable gameplay scripts
        foreach (var script in scriptsToDisable)
            if (script != null) script.enabled = false;

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

        // Re-enable gameplay scripts
        foreach (var script in scriptsToDisable)
            if (script != null) script.enabled = true;

        // Re-enable pause after countdown finishes
        PauseButton.canPause = true;

        // Optional: only Jolen cares about UIJolen, so wrap it in null check
        if (UIJolen.Instance != null)
        {
            UIJolen.Instance.SetProfilesVisible(true);
            UIJolen.Instance.UpdateTurn(1);
        }
    }
}
