using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float gravity = -9.81f;

    [Header("Character Controller Settings")]
    [SerializeField] private float controllerHeight = 1.8f;
    [SerializeField] private float controllerRadius = 0.3f;
    [SerializeField] private float controllerSkinWidth = 0.08f;
    [SerializeField] private float controllerStepOffset = 0.3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    // Component references
    private CharacterController characterController;
    private Transform cameraTransform;
    private Transform modelTransform;

    // Animation parameter IDs
    private int speedParameterID;
    private int isRunningParameterID;

    // Movement variables
    private Vector3 movementDirection;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    private bool isRunning;

    private void Awake()
    {
        // Get components
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // Find model transform (assuming it's the first child)
        if (transform.childCount > 0)
        {
            modelTransform = transform.GetChild(0);
        }

        // Configure CharacterController to prevent blinking during rotation
        if (characterController != null)
        {
            characterController.height = controllerHeight;
            characterController.radius = controllerRadius;
            characterController.skinWidth = controllerSkinWidth;
            characterController.stepOffset = controllerStepOffset;
            characterController.center = new Vector3(0, controllerHeight / 2, 0);
        }

        // If animator wasn't assigned in inspector, try to get it
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Cache animator parameter IDs for better performance
        speedParameterID = Animator.StringToHash("Speed");
        isRunningParameterID = Animator.StringToHash("IsRunning");
    }

    private void Update()
    {
        CheckGrounded();
        HandleInput();
        HandleMovement();
        HandleRotation();
        UpdateAnimations();
    }

    private void CheckGrounded()
    {
        // Simple ground check
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);

        // Apply gravity if not grounded
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // Small negative value to keep the character grounded
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
    }

    private void HandleInput()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Project vectors onto horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction
        movementDirection = forward * vertical + right * horizontal;

        // Check if running (sprint)
        isRunning = Input.GetKey(KeyCode.LeftShift) && movementDirection.magnitude > 0.1f;
    }

    private void HandleMovement()
    {
        // Apply movement
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 horizontalMovement = movementDirection * currentSpeed;

        // Apply horizontal and vertical movement in a single Move call to prevent jitter
        characterController.Move((horizontalMovement + verticalVelocity) * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Only rotate if we're moving
        if (movementDirection.magnitude > 0.1f)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            // Smoothly rotate towards movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            // Set animation parameters
            float normalizedSpeed = movementDirection.magnitude;
            animator.SetFloat(speedParameterID, normalizedSpeed);
            animator.SetBool(isRunningParameterID, isRunning);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the CharacterController shape in the Scene view
        if (characterController != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * (characterController.height / 2), characterController.radius);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * characterController.skinWidth, characterController.radius);
        }
    }
}