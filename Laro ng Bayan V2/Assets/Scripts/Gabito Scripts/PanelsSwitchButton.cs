using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsSwitchButton : MonoBehaviour
{
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject targetPanel;

    public void SwitchPanel()
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        if (targetPanel != null)
            targetPanel.SetActive(true);
    }
}
