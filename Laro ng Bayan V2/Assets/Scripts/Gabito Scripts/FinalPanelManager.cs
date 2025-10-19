using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPanelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject finalPanel;
    public MonoBehaviour cameraController; // Drag your camera controller script here

    private bool isFinalPanelActive = false;


    void Start()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);

        if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.showFinalPanel)
        {
            ShowFinalPanel();
            ObjectiveManager.Instance.showFinalPanel = false; // reset so it won’t repeat
        }
    }

    void Update()
    {
        // This keeps the cursor visible if the final panel is active
        if (isFinalPanelActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ShowFinalPanel()
    {
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
            isFinalPanelActive = true;

            // Pause game
            Time.timeScale = 0f;


            // Disable camera movement
            if (cameraController != null)
                cameraController.enabled = false;

            // Force cursor visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Final panel activated!");
        }
        else
        {
            Debug.LogWarning("Final panel reference is missing!");
        }
    }


    // Hook this up to your Okay Button's OnClick()
    public void HideFinalPanel()
    {
        if (finalPanel != null)
            finalPanel.SetActive(false);

        isFinalPanelActive = false;

        // Resume game
        Time.timeScale = 1f;

        // Re-enable camera movement
        if (cameraController != null)
            cameraController.enabled = true;

        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Final panel hidden, game resumed.");
    }
}
