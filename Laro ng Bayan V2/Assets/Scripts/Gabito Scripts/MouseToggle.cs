using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToggle : MonoBehaviour
{
    // GABITO SCRIPT :D
    void Update()
    {
        // Only allow Z toggle if not paused
        if (!PauseButton.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
