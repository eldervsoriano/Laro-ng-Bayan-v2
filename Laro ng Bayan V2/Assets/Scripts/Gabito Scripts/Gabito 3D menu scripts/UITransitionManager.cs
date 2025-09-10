using UnityEngine;
using Cinemachine;

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

    private void SetActiveCamera(CinemachineVirtualCamera target)
    {
        // Reset all cameras to priority 0
        vcamPlay.Priority = 0;
        vcamOptions.Priority = 0;
        vcamCredits.Priority = 0;
        vcamModes.Priority = 0;
        vcamSelect.Priority = 0;

        // Activate target
        target.Priority = 10;
    }

    private void ShowOnly(GameObject targetUI)
    {
        // Disable all UIs first
        uiMainMenu.SetActive(false);
        uiOptions.SetActive(false);
        uiCredits.SetActive(false);
        uiGameModes.SetActive(false);

        // Then enable the one we want
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
        SetActiveCamera(vcamSelect);
        ShowOnly(uiGameSelect);
    }
}
