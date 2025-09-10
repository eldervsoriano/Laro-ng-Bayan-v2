using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UnlockUIManager : MonoBehaviour
{
    [SerializeField] private GameObject unlockPanel;
    [SerializeField] private TMP_Text unlockText;

    void Start()
    {
        if (unlockPanel != null) unlockPanel.SetActive(false);

        if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.turumpoJustUnlocked)
        {
            ShowUnlockMessage("You can now play Turumpo! Talk to Andrea.");
            ObjectiveManager.Instance.turumpoJustUnlocked = false; // reset so it won’t show again
        }
    }


    public void ShowUnlockMessage(string message)
    {
        if (unlockPanel != null) unlockPanel.SetActive(true);
        if (unlockText != null) unlockText.text = message;

        // Hide after few seconds
        StartCoroutine(HideAfterDelay(5f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (unlockPanel != null) unlockPanel.SetActive(false);
    }

}
