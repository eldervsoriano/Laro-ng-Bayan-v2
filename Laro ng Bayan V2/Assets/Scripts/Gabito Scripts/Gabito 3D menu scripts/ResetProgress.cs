using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgress : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject resetPromptPanel;        // "Are you sure?" panel
    [SerializeField] private GameObject resetConfirmationPanel;  // "Progress reset!" panel
    [SerializeField] private GameObject settingsPanel; // Settings panel

    // Called when clicking the main Reset button in Settings
    public void ShowResetPrompt()
    {
        if (resetPromptPanel != null)
            resetPromptPanel.SetActive(true);
    }

    // Called when pressing "Yes" in the prompt
    public void ConfirmReset()
    {
        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.ResetProgress(); // clear progress in OM

        if (resetPromptPanel != null)
            resetPromptPanel.SetActive(false);

        if (resetConfirmationPanel != null)
            resetConfirmationPanel.SetActive(true);
    }

    // Called when pressing "No" in the prompt
    public void CancelReset()
    {
        if (resetPromptPanel != null)
            resetPromptPanel.SetActive(false);
    }

    // Called when pressing "OK" in the confirmation panel
    public void CloseResetConfirmation()
    {
        if (resetConfirmationPanel != null)
            resetConfirmationPanel.SetActive(false);

        settingsPanel.SetActive(true);
    }
}
