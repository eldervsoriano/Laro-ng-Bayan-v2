//using UnityEngine;

//public class ThirdPersonCamera : MonoBehaviour
//{
//    [Header("Follow Settings")]
//    [SerializeField] private Transform target;
//    [SerializeField] private float distanceFromTarget = 5.0f;
//    [SerializeField] private float heightOffset = 1.5f;
//    [SerializeField] private float smoothTime = 0.1f;

//    [Header("Rotation Settings")]
//    [SerializeField] private float rotationSpeed = 3.0f;
//    [SerializeField] private bool invertY = false;
//    [SerializeField] private float minVerticalAngle = -30.0f;
//    [SerializeField] private float maxVerticalAngle = 60.0f;

//    [Header("Collision Settings")]
//    [SerializeField] private bool enableCollisionDetection = true;
//    [SerializeField] private float collisionRadius = 0.2f;
//    [SerializeField] private LayerMask collisionLayers;

//    // Camera position variables
//    private Vector3 currentVelocity = Vector3.zero;
//    private float currentRotationX = 0f;
//    private float currentRotationY = 0f;

//    // Input variables
//    private float mouseX;
//    private float mouseY;

//    private void Start()
//    {
//        // If no target is assigned, try to find the player
//        if (target == null)
//        {
//            var player = GameObject.FindGameObjectWithTag("Player");
//            if (player != null)
//            {
//                target = player.transform;
//            }
//            else
//            {
//                Debug.LogWarning("No target assigned to ThirdPersonCamera and no GameObject with 'Player' tag found.");
//            }
//        }

//        // Initialize rotation based on initial camera angle
//        Vector3 angles = transform.eulerAngles;
//        currentRotationX = angles.y;
//        currentRotationY = angles.x;

//        // Lock and hide cursor
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    private void LateUpdate()
//    {
//        if (target == null)
//            return;

//        HandleInput();
//        RotateCamera();
//        PositionCamera();
//    }

//    private void HandleInput()
//    {
//        // Get mouse input
//        mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
//        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * (invertY ? 1 : -1);
//    }

//    private void RotateCamera()
//    {
//        // Update camera rotation based on mouse input
//        currentRotationX += mouseX;
//        currentRotationY += mouseY;

//        // Clamp vertical rotation
//        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
//    }

//    private void PositionCamera()
//    {
//        if (target == null)
//            return;

//        // Calculate rotation
//        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);

//        // Calculate target position
//        Vector3 targetPosition = target.position + Vector3.up * heightOffset;
//        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * distanceFromTarget;

//        // Handle collision detection
//        if (enableCollisionDetection)
//        {
//            desiredPosition = HandleCameraCollision(targetPosition, desiredPosition);
//        }

//        // Smoothly move the camera to the desired position
//        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

//        // Look at target (slightly above the target position for better framing)
//        transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
//    }

//    private Vector3 HandleCameraCollision(Vector3 targetPosition, Vector3 desiredPosition)
//    {
//        RaycastHit hit;
//        Vector3 direction = (desiredPosition - targetPosition).normalized;
//        float distance = Vector3.Distance(targetPosition, desiredPosition);

//        // Check for collision between target and desired camera position
//        if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, distance, collisionLayers))
//        {
//            // If there's a collision, place the camera at the hit point offset by collision radius
//            return targetPosition + direction * (hit.distance - collisionRadius);
//        }

//        return desiredPosition;
//    }

//    // Public method to set the target
//    public void SetTarget(Transform newTarget)
//    {
//        target = newTarget;
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (target == null)
//            return;

//        // Draw a line to show the camera's target
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(transform.position, target.position + Vector3.up * heightOffset);

//        // Draw spheres to show collision detection points
//        if (enableCollisionDetection)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(transform.position, collisionRadius);
//        }
//    }
//}

//using UnityEngine;

//public class ThirdPersonCamera : MonoBehaviour
//{
//    [Header("Follow Settings")]
//    [SerializeField] private Transform target;
//    [SerializeField] private float distanceFromTarget = 5.0f;
//    [SerializeField] private float heightOffset = 1.5f;
//    [SerializeField] private float smoothTime = 0.1f;

//    [Header("Rotation Settings")]
//    [SerializeField] private float rotationSpeed = 3.0f;
//    [SerializeField] private bool invertY = false;
//    [SerializeField] private float minVerticalAngle = -30.0f;
//    [SerializeField] private float maxVerticalAngle = 60.0f;

//    [Header("Collision Settings")]
//    [SerializeField] private bool enableCollisionDetection = true;
//    [SerializeField] private float collisionRadius = 0.4f; // Increased for better detection
//    [SerializeField] private LayerMask collisionLayers;
//    [SerializeField] private float minDistanceFromTarget = 0.5f; // Minimum distance allowed

//    // Camera position variables
//    private Vector3 currentVelocity = Vector3.zero;
//    private float currentRotationX = 0f;
//    private float currentRotationY = 0f;
//    private float currentDistance; // Current adjusted distance due to collisions

//    // Input variables
//    private float mouseX;
//    private float mouseY;

//    private void Start()
//    {
//        // If no target is assigned, try to find the player
//        if (target == null)
//        {
//            var player = GameObject.FindGameObjectWithTag("Player");
//            if (player != null)
//            {
//                target = player.transform;
//            }
//            else
//            {
//                Debug.LogWarning("No target assigned to ThirdPersonCamera and no GameObject with 'Player' tag found.");
//            }
//        }

//        // Initialize rotation based on initial camera angle
//        Vector3 angles = transform.eulerAngles;
//        currentRotationX = angles.y;
//        currentRotationY = angles.x;
//        currentDistance = distanceFromTarget;

//        // Lock and hide cursor
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    private void LateUpdate()
//    {
//        if (target == null || PauseButton.isPaused)
//            return;

//        HandleInput();
//        RotateCamera();
//        PositionCamera();
//    }


//    private void HandleInput()
//    {
//        // Get mouse input
//        mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
//        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * (invertY ? 1 : -1);
//    }

//    private void RotateCamera()
//    {
//        // Update camera rotation based on mouse input
//        currentRotationX += mouseX;
//        currentRotationY += mouseY;

//        // Clamp vertical rotation
//        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
//    }

//    private void PositionCamera()
//    {
//        if (target == null)
//            return;

//        // Calculate rotation
//        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);

//        // Calculate target position
//        Vector3 targetPosition = target.position + Vector3.up * heightOffset;
//        Vector3 direction = rotation * Vector3.back; // Using Vector3.back instead of -Vector3.forward
//        Vector3 desiredPosition = targetPosition + direction * distanceFromTarget;

//        // Handle collision detection
//        if (enableCollisionDetection)
//        {
//            desiredPosition = HandleCameraCollision(targetPosition, desiredPosition);
//        }

//        // Smoothly move the camera to the desired position
//        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

//        // Look at target (slightly above the target position for better framing)
//        transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
//    }

//    private Vector3 HandleCameraCollision(Vector3 targetPosition, Vector3 desiredPosition)
//    {
//        Vector3 direction = desiredPosition - targetPosition;
//        float targetDistance = direction.magnitude;
//        direction.Normalize();

//        // Use SphereCast to detect collisions
//        if (Physics.SphereCast(targetPosition, collisionRadius, direction, out RaycastHit hit, targetDistance, collisionLayers))
//        {
//            // Move camera in front of obstacle (keeping a small distance to avoid clipping)
//            float adjustedDistance = Mathf.Max(hit.distance - collisionRadius, minDistanceFromTarget);
//            Vector3 newPosition = targetPosition + direction * adjustedDistance;
//            return newPosition;
//        }

//        return desiredPosition;
//    }


//    // Public method to set the target
//    public void SetTarget(Transform newTarget)
//    {
//        target = newTarget;
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (target == null)
//            return;

//        // Draw a line to show the camera's target
//        Gizmos.color = Color.yellow;
//        Vector3 targetPos = target.position + Vector3.up * heightOffset;
//        Gizmos.DrawLine(transform.position, targetPos);

//        // Draw spheres to show collision detection points
//        if (enableCollisionDetection)
//        {
//            // Draw the collision sphere at the camera position
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(transform.position, collisionRadius);

//            // Draw a sphere at the target to show the origin of the SphereCast
//            Gizmos.color = Color.green;
//            Gizmos.DrawWireSphere(targetPos, collisionRadius);

//            // Draw the direction ray
//            Gizmos.color = Color.blue;
//            Vector3 direction = (transform.position - targetPos).normalized;
//            Gizmos.DrawRay(targetPos, direction * distanceFromTarget);
//        }
//    }
//} // KAY ELDER TOH!


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
    [SerializeField] private float collisionOffset = 0.3f;  // Slightly increased for stability
    [SerializeField] private float minDistanceFromTarget = 0.6f;
    [SerializeField] private LayerMask collisionLayers;

    private Vector3 currentVelocity;
    private float currentRotationX;
    private float currentRotationY;
    private float desiredDistance;
    private float currentDistance;

    private float mouseX;
    private float mouseY;

    // NEW: Camera anchor inside the player
    private Transform cameraAnchor;

    private void Start()
    {
        // If no target assigned, find by tag
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player) target = player.transform;
            else Debug.LogWarning("ThirdPersonCamera: No target assigned or found with tag 'Player'");
        }

        // Try to find a child named "CameraTarget"
        if (target != null)
        {
            cameraAnchor = target.Find("CameraTarget");
            if (cameraAnchor == null)
            {
                Debug.LogWarning("ThirdPersonCamera: No 'CameraTarget' child found under player. Using default height offset.");
            }
        }

        Vector3 angles = transform.eulerAngles;
        currentRotationX = angles.y;
        currentRotationY = angles.x;

        desiredDistance = distanceFromTarget;
        currentDistance = desiredDistance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null || PauseButton.isPaused)
            return;

        HandleInput();
        RotateCamera();
        UpdateCameraPosition();
    }

    private void HandleInput()
    {
        mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * (invertY ? 1 : -1);
    }

    private void RotateCamera()
    {
        currentRotationX += mouseX;
        currentRotationY += mouseY;
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0f);

        // Use camera anchor if available, otherwise use target + heightOffset
        Vector3 targetPos = cameraAnchor
            ? cameraAnchor.position
            : target.position + Vector3.up * heightOffset;

        Vector3 desiredCamPos = targetPos - rotation * Vector3.forward * desiredDistance;

        if (enableCollisionDetection)
        {
            desiredCamPos = HandleCollision(targetPos, desiredCamPos);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredCamPos, ref currentVelocity, smoothTime);
        transform.rotation = rotation;
    }

    private Vector3 HandleCollision(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        float targetDistance = direction.magnitude;

        if (Physics.Raycast(from, direction.normalized, out RaycastHit hit, targetDistance, collisionLayers))
        {
            float adjustedDist = Mathf.Max(hit.distance - collisionOffset, minDistanceFromTarget);
            currentDistance = Mathf.Lerp(currentDistance, adjustedDist, Time.deltaTime * 10f);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * 2f);
        }

        return from - (direction.normalized * currentDistance);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        cameraAnchor = newTarget.Find("CameraTarget");
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Vector3 targetPos = cameraAnchor
            ? cameraAnchor.position
            : target.position + Vector3.up * heightOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(targetPos, transform.position);
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
