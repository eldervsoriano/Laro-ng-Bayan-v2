using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRotateName : MonoBehaviour
{
    [Tooltip("Leave empty to auto-find the main camera")]
    public Transform targetCamera;

    [Tooltip("If true, it will rotate only on the Y axis (useful for 3D world labels)")]
    public bool rotateOnlyY = true;

    private void Start()
    {
        if (targetCamera == null)
        {
            if (Camera.main != null)
                targetCamera = Camera.main.transform;
            else
                Debug.LogWarning("BillboardName: No camera found. Please assign one manually.");
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        Vector3 lookDirection;

        if (rotateOnlyY)
        {
            // Keep the object's Y the same, so it only rotates horizontally
            Vector3 targetPosition = targetCamera.position;
            targetPosition.y = transform.position.y;
            lookDirection = transform.position - targetPosition;
        }
        else
        {
            // Full rotation to face the camera
            lookDirection = transform.position - targetCamera.position;
        }

        // Face toward the camera
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;
    }
}
