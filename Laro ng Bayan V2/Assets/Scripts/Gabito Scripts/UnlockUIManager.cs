using System.Collections;
using UnityEngine;
using TMPro;

public class UnlockUIManager : MonoBehaviour
{
    [SerializeField] private GameObject unlockPanel;
    [SerializeField] private TMP_Text unlockText;

    void Start()
    {
        if (unlockPanel != null) unlockPanel.SetActive(false);

        if (ObjectiveManager.Instance != null)
        {
            // Turumpo unlock
            if (ObjectiveManager.Instance.turumpoJustUnlocked)
            {
                ShowUnlockMessage("You can now play Turumpo! Talk to Andrea.");
                ObjectiveManager.Instance.turumpoJustUnlocked = false; // reset so it won’t show again
            }

            // Tumbang Preso unlock
            if (ObjectiveManager.Instance.tumbangPresoJustUnlocked)
            {
                ShowUnlockMessage("You can now play Tumbang Preso! Talk to Charles.");
                ObjectiveManager.Instance.tumbangPresoJustUnlocked = false; // reset so it won’t show again
            }

            // Spider Derby unlock
            if (ObjectiveManager.Instance.spiderDerbyJustUnlocked)
            {
                ShowUnlockMessage("You can now play Spider Derby! Talk to Michael.");
                ObjectiveManager.Instance.spiderDerbyJustUnlocked = false; // reset so it won’t show again
            }
        }
    }

    public void ShowUnlockMessage(string message)
    {
        if (unlockPanel != null) unlockPanel.SetActive(true);
        if (unlockText != null) unlockText.text = message;

        // Hide after a few seconds
        StartCoroutine(HideAfterDelay(5f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (unlockPanel != null) unlockPanel.SetActive(false);
    }
}
