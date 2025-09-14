using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalPanelManager : MonoBehaviour
{
    public GameObject finalPanel;

    void Start()
    {
        if (finalPanel != null) finalPanel.SetActive(false);

        if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.showFinalPanel)
        {
            ShowFinalPanel();
            ObjectiveManager.Instance.showFinalPanel = false; // reset so it won’t repeat
        }
    }

    public void ShowFinalPanel()
    {
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
            Debug.Log("Final panel activated!");
        }
        else
        {
            Debug.LogWarning("Final panel reference is missing!");
        }
    }

    // Hook this up to your Button's OnClick()
    public void HideFinalPanel()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);
    }
}
