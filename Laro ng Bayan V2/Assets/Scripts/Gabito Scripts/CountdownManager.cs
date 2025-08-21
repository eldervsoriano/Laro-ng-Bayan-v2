using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public GameObject countdownPanel;   // Parent panel of the text
    public TextMeshProUGUI countdownText;          // The countdown text
    public float countdownTime = 3f;    // Seconds before game starts

    public PamatoShooter[] shooters; // assign both Player 1 and Player 2 in Inspector
    public UIJolen uiJolen;

    // Remove Start() auto-start so tutorial can control it
    // void Start()
    // {
    //     StartCoroutine(StartCountdown());
    // }

    // Public method so TutorialPanel can call this
    public void BeginCountdown()
    {
        StartCoroutine(StartCountdown());
    }


    private IEnumerator StartCountdown()
    {
        PauseButton.canPause = false; // disable pause during countdown

        // disable all PamatoShooter scripts
        foreach (var shooter in shooters)
        {
            shooter.enabled = false;
        }

        // Hide profiles and turn panels
        // Make sure profiles + turn panels are hidden at start of countdown
        UIJolen.Instance.SetProfilesVisible(false);
        if (UIJolen.Instance.player1TurnPanel != null)
            UIJolen.Instance.player1TurnPanel.SetActive(false);

        if (UIJolen.Instance.player2TurnPanel != null)
            UIJolen.Instance.player2TurnPanel.SetActive(false);


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

        // enable all PamatoShooter scripts
        foreach (var shooter in shooters)
        {
            shooter.enabled = true;
        }

        // Show Player profiles now
        UIJolen.Instance.SetProfilesVisible(true);

        // Show Player 1’s turn (this will auto-enable Player1TurnPanel)
        UIJolen.Instance.UpdateTurn(1);
        UIJolen.Instance.turnText.gameObject.SetActive(true);

        PauseButton.canPause = true; // re-enable pause once countdown is done

    }
}
