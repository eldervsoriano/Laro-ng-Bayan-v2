using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRotateName : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        // Find the main camera once at start
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No MainCamera found. Make sure your camera has the 'MainCamera' tag.");
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // Make the text face the camera (full 360 rotation)
        transform.LookAt(transform.position + cam.rotation * Vector3.forward,
                         cam.rotation * Vector3.up);
    }
}
