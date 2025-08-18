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
        // disable all PamatoShooter scripts
        foreach (var shooter in shooters)
        {
            shooter.enabled = false;
        }

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
    }
}
