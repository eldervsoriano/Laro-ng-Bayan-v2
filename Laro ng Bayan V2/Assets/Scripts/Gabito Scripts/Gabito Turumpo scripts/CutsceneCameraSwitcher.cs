using UnityEngine;
using Cinemachine;
using System.Collections;
using System;

public class CutsceneCameraSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneStep
    {
        public CinemachineVirtualCamera vcam;
        public Animator animator;
        public string triggerName;   // e.g. "ReadyShoot"
        public float waitTime = 2f;  // time before next step
    }

    public CinemachineVirtualCamera mainCam; // assign your main/default cam in Inspector
    public CutsceneStep[] steps;

    private int currentStep = 0;
    private Action onCutsceneFinished;

    public void PlayCutscene(Action onFinished = null)
    {
        currentStep = 0;
        onCutsceneFinished = onFinished;
        PlayStep(0);
    }

    private void PlayStep(int index)
    {
        if (index >= steps.Length)
        {
            // Restore main cam when cutscene finishes
            SetActiveVCam(mainCam);

            onCutsceneFinished?.Invoke();
            return;
        }

        // Switch to this step’s camera
        SetActiveVCam(steps[index].vcam);

        // Fire trigger
        if (steps[index].animator && !string.IsNullOrEmpty(steps[index].triggerName))
        {
            steps[index].animator.SetTrigger(steps[index].triggerName);
        }

        // Wait and go next
        StartCoroutine(GoNextAfterDelay(steps[index].waitTime));
    }

    private IEnumerator GoNextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentStep++;
        PlayStep(currentStep);
    }

    private void SetActiveVCam(CinemachineVirtualCamera target)
    {
        // Reset all to low priority
        foreach (var s in steps)
        {
            if (s.vcam != null)
                s.vcam.Priority = 0;
        }
        if (mainCam != null)
            mainCam.Priority = 0;

        // Activate the one we want
        if (target != null)
            target.Priority = 10;
    }
}
