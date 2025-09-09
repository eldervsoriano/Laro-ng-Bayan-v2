using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera vcamPlay;
    public CinemachineVirtualCamera vcamOptions;
    public CinemachineVirtualCamera vcamCredits;

    private void SetActiveCamera(CinemachineVirtualCamera target)
    {
        // Reset all cameras to priority 0
        vcamPlay.Priority = 0;
        vcamOptions.Priority = 0;
        vcamCredits.Priority = 0;

        // Set the target one higher so it activates
        target.Priority = 10;
    }

    // These methods can be hooked up to Unity UI Buttons
    public void GoToPlay()
    {
        SetActiveCamera(vcamPlay);
    }

    public void GoToOptions()
    {
        SetActiveCamera(vcamOptions);
    }

    public void GoToCredits()
    {
        SetActiveCamera(vcamCredits);
    }
}
