using UnityEngine;
using Cinemachine;
using UnityEngine.UI; // needed for Button


public class UITransitionManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCamera vcamPlay;
    public CinemachineVirtualCamera vcamOptions;
    public CinemachineVirtualCamera vcamCredits;
    public CinemachineVirtualCamera vcamModes;
    public CinemachineVirtualCamera vcamSelect;

    [Header("UI Panels")]
    public GameObject uiMainMenu;
    public GameObject uiOptions;
    public GameObject uiCredits;
    public GameObject uiGameModes;
    public GameObject uiGameSelect;

    [Header("Buttons")]
    public Button selectModesButton; // Drag your Select Modes button here in Inspector

    /*  Optional: keep reference to "story not complete" panel in case you want to restore later
    [Header("For Select Modes")]
    public GameObject uiStoryNotComplete; // Warning panel
    public GameObject uiPlayPanel;        // The "regular play panel" */

    private void Update()
    {
        // Continuously check if progress changed (simple but safe)
        UpdateSelectModesButton();
    }


    private void SetActiveCamera(CinemachineVirtualCamera target)
    {
        vcamPlay.Priority = 0;
        vcamOptions.Priority = 0;
        vcamCredits.Priority = 0;
        vcamModes.Priority = 0;
        vcamSelect.Priority = 0;

        if (target != null)
            target.Priority = 10;
    }

    private void ShowOnly(GameObject targetUI)
    {
        uiMainMenu.SetActive(false);
        uiOptions.SetActive(false);
        uiCredits.SetActive(false);
        uiGameModes.SetActive(false);
        uiGameSelect.SetActive(false);

        if (targetUI != null)
            targetUI.SetActive(true);
    }

    public void GoToPlay()
    {
        SetActiveCamera(vcamPlay);
        ShowOnly(uiMainMenu);
    }

    public void GoToOptions()
    {
        SetActiveCamera(vcamOptions);
        ShowOnly(uiOptions);
    }

    public void GoToCredits()
    {
        SetActiveCamera(vcamCredits);
        ShowOnly(uiCredits);
    }

    public void GoToGameModes()
    {
        SetActiveCamera(vcamModes);
        ShowOnly(uiGameModes);
    }

    public void GoToSelectModes()
    {
        // Only works if story completed
        if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.spiderDerbyCompleted)
        {
            SetActiveCamera(vcamSelect);
            ShowOnly(uiGameSelect);
        }
        else
        {
            Debug.Log("Select Modes button should be greyed out. Story not complete.");
        }

        //if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.spiderDerbyCompleted)
        //{
        //    // Story completed -> move to game modes
        //    SetActiveCamera(vcamSelect);
        //if (uiGameSelect != null) uiGameSelect.SetActive(true);
        //if (uiPlayPanel != null) uiPlayPanel.SetActive(false);
        //}
        //else
        //{
        //    // Not completed -> show warning, stay on Play camera
        //    if (uiStoryNotComplete != null) uiStoryNotComplete.SetActive(true);
        //    if (uiPlayPanel != null) uiPlayPanel.SetActive(false);

        //    Debug.Log("Finish the story first!");
        //}
    }

    //// Hook this to the Close button in your warning panel
    //public void CloseWarning()
    //{
    //    if (uiStoryNotComplete != null) uiStoryNotComplete.SetActive(false);
    //    if (uiPlayPanel != null) uiPlayPanel.SetActive(true);
    //}

    //// Hook this to the Close button in your game modes panel
    //public void CloseGameModes()
    //{
    //    if (uiGameModes != null) uiGameModes.SetActive(false);
    //    if (uiPlayPanel != null) uiPlayPanel.SetActive(true);
    //    SetActiveCamera(vcamPlay);
    //}

    // === Enable/disable the Select Modes button ===
    private void UpdateSelectModesButton()
    {
        if (selectModesButton != null)
        {
            if (ObjectiveManager.Instance != null && ObjectiveManager.Instance.spiderDerbyCompleted)
                selectModesButton.interactable = true;
            else
                selectModesButton.interactable = false;
        }
    }
}
