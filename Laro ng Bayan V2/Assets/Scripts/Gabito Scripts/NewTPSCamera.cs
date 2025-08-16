using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTPSCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float minYAngle = -20f;
    [SerializeField] private float maxYAngle = 60f;

    [Header("Smoothness")]
    [SerializeField] private float smoothTime = 0.1f;

    private float currentYaw;
    private float currentPitch;
    private Vector3 velocity;

    private void Start()
    {
        if (!target)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player) target = player.transform;
        }

        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (!target || PauseButton.isPaused) return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * rotationSpeed * Time.deltaTime;
        currentPitch -= mouseY * rotationSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);

        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPos = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // Smooth move
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
        transform.rotation = rotation;
    }
}
