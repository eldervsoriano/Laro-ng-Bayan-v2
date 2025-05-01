using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 5.0f;
    [SerializeField] private float heightOffset = 1.5f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private float minVerticalAngle = -30.0f;
    [SerializeField] private float maxVerticalAngle = 60.0f;

    [Header("Collision Settings")]
    [SerializeField] private bool enableCollisionDetection = true;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private LayerMask collisionLayers;

    // Camera position variables
    private Vector3 currentVelocity = Vector3.zero;
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    // Input variables
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        // If no target is assigned, try to find the player
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("No target assigned to ThirdPersonCamera and no GameObject with 'Player' tag found.");
            }
        }

        // Initialize rotation based on initial camera angle
        Vector3 angles = transform.eulerAngles;
        currentRotationX = angles.y;
        currentRotationY = angles.x;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        HandleInput();
        RotateCamera();
        PositionCamera();
    }

    private void HandleInput()
    {
        // Get mouse input
        mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * (invertY ? 1 : -1);
    }

    private void RotateCamera()
    {
        // Update camera rotation based on mouse input
        currentRotationX += mouseX;
        currentRotationY += mouseY;

        // Clamp vertical rotation
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
    }

    private void PositionCamera()
    {
        if (target == null)
            return;

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);

        // Calculate target position
        Vector3 targetPosition = target.position + Vector3.up * heightOffset;
        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * distanceFromTarget;

        // Handle collision detection
        if (enableCollisionDetection)
        {
            desiredPosition = HandleCameraCollision(targetPosition, desiredPosition);
        }

        // Smoothly move the camera to the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

        // Look at target (slightly above the target position for better framing)
        transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
    }

    private Vector3 HandleCameraCollision(Vector3 targetPosition, Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 direction = (desiredPosition - targetPosition).normalized;
        float distance = Vector3.Distance(targetPosition, desiredPosition);

        // Check for collision between target and desired camera position
        if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, distance, collisionLayers))
        {
            // If there's a collision, place the camera at the hit point offset by collision radius
            return targetPosition + direction * (hit.distance - collisionRadius);
        }

        return desiredPosition;
    }

    // Public method to set the target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null)
            return;

        // Draw a line to show the camera's target
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position + Vector3.up * heightOffset);

        // Draw spheres to show collision detection points
        if (enableCollisionDetection)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }
}