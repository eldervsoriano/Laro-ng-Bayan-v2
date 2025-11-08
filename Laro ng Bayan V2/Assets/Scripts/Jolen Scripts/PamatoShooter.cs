using UnityEngine;
using UnityEngine.EventSystems;

public class PamatoShooter : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 2f;
    public LineRenderer aimLine;

    private Vector3 dragStartWorld;
    private bool isDragging = false;
    private bool hasShot = false;
    private Camera cam;

    private Plane dragPlane;
    private RigidbodyConstraints originalConstraints;


    void Start()
    {
        cam = Camera.main;
        dragPlane = new Plane(Vector3.up, Vector3.zero);

        if (aimLine != null)
            aimLine.enabled = false;

        originalConstraints = rb.constraints;
    }

    void Update()
    {
        if (!enabled) return;
        if (hasShot) Debug.Log($"{name} drag locked: hasShot = true");

        // Ignore UI
        if (IsPointerBlockingUI())
            return;


        // Only let the current player aim
        int activePlayer = JolenGameManager.Instance.GetCurrentPlayer();
        if ((activePlayer == 1 && gameObject != JolenGameManager.Instance.player1Pamato) ||
            (activePlayer == 2 && gameObject != JolenGameManager.Instance.player2Pamato))
            return;

        if (CountdownManager.InputLocked) return;

        // --- Handle mouse down ---
        if (Input.GetMouseButtonDown(0) && !hasShot)
        {
            if (TryGetGroundPoint(out Vector3 hitPoint))
            {
                dragStartWorld = hitPoint;
                isDragging = true;

                if (aimLine != null)
                    aimLine.enabled = true;

                rb.constraints = RigidbodyConstraints.FreezePositionX |
                                 RigidbodyConstraints.FreezePositionY |
                                 RigidbodyConstraints.FreezePositionZ |
                                 RigidbodyConstraints.FreezeRotation;
            }
        }

        // --- Handle mouse drag ---
        if (isDragging && Input.GetMouseButton(0) && !hasShot)
        {
            if (TryGetGroundPoint(out Vector3 hitPoint))
            {
                Vector3 dragCurrent = hitPoint;
                Vector3 direction = dragStartWorld - dragCurrent;
                Vector3 lineEnd = transform.position + direction;

                if (aimLine != null)
                {
                    aimLine.SetPosition(0, transform.position);
                    aimLine.SetPosition(1, lineEnd);

                    float power = Mathf.Clamp(direction.magnitude, 0, 2f);
                    aimLine.startColor = aimLine.endColor = Color.Lerp(Color.green, Color.red, power / 2f);
                }
            }
        }

        // --- Handle mouse release ---
        if (isDragging && Input.GetMouseButtonUp(0) && !hasShot)
        {
            if (TryGetGroundPoint(out Vector3 hitPoint))
            {
                Vector3 dragEnd = hitPoint;
                Vector3 force = dragStartWorld - dragEnd;

                rb.constraints = originalConstraints;
                rb.AddForce(force * forceMultiplier, ForceMode.Impulse);

                JolenGameManager.Instance.NotifyShot(rb);
                hasShot = true;

                if (aimLine != null)
                    aimLine.enabled = false;
            }

            isDragging = false;
        }


    }

    // Cast ray from mouse to ground, skipping the Roof layer
    private bool TryGetGroundPoint(out Vector3 point)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~LayerMask.GetMask("Roof")))
        {
            // Project to Y = 0 plane
            dragPlane.Raycast(ray, out float enter);
            point = ray.GetPoint(enter);
            point.y = 0f;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    public void ResetTurn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = originalConstraints;
        hasShot = false;
        isDragging = false;
        hasShot = false;
        isDragging = false;
        if (aimLine != null)
            aimLine.enabled = false;

        rb.constraints = originalConstraints;
    }

    private bool IsPointerBlockingUI()
    {
        // If there's no EventSystem or no UI under the mouse, don't block
        if (EventSystem.current == null)
            return false;

        // Get all UI elements under pointer
        var pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // If none of them are "Raycast Target" UI elements that we care about, don't block
        foreach (var r in results)
        {
            // You can filter specific UI names or tags here if needed
            if (r.gameObject.CompareTag("BlockUI"))
                return true;
        }
        return false;
    }

}
