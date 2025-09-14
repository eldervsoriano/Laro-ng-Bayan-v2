using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAccessManager : MonoBehaviour
{
    [SerializeField] public GameObject playPanel;  // Regular play panel
    [SerializeField] public GameObject selectGameModesPanel;
    [SerializeField] public GameObject storyNotCompletePanel;

    private void Start()
    {
        // Make sure optional panels start hidden
        if (selectGameModesPanel != null) selectGameModesPanel.SetActive(false);
        if (storyNotCompletePanel != null) storyNotCompletePanel.SetActive(false);
    }

    // Hook this to your "Select Game Modes" button
    public void OnSelectGameModesPressed()
    {
        if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.spiderDerbyCompleted)
        {
            // Story complete -> show select modes
            if (selectGameModesPanel != null) selectGameModesPanel.SetActive(true);
            if (playPanel != null) playPanel.SetActive(false);
        }
        else
        {
            // Not finished -> show warning
            if (storyNotCompletePanel != null) storyNotCompletePanel.SetActive(true);
            if (playPanel != null) playPanel.SetActive(false);
        }
    }

    // Hook this to the Close button inside your warning panel
    public void CloseWarning()
    {
        if (storyNotCompletePanel != null) storyNotCompletePanel.SetActive(false);
        if (playPanel != null) playPanel.SetActive(true); // Bring back play panel
    }

    // Hook this to the Close button inside the game modes panel
    public void CloseGameModes()
    {
        if (selectGameModesPanel != null) selectGameModesPanel.SetActive(false);
        if (playPanel != null) playPanel.SetActive(true); // Bring back play panel
    }
}
