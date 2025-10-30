using UnityEngine;

public class PamatoShooter : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 2f; // was 1.5, boost it for better response

    public LineRenderer aimLine;

    private Vector3 dragStartWorld;
    private bool isDragging = false;
    private bool hasShot = false;
    private Camera cam;

    private Plane dragPlane; // NEW: imaginary flat ground
    private RigidbodyConstraints originalConstraints; // save original settings


    void Start()
    {
        cam = Camera.main;
        dragPlane = new Plane(Vector3.up, Vector3.zero); // flat plane at Y = 0

        if (aimLine != null)
            aimLine.enabled = false;

        // store the default constraints (important if you set some in Inspector)
        originalConstraints = rb.constraints;
    }



    void OnMouseDown()
    {
        // --- NEW: Prevent input if not this pamato's turn ---
        int activePlayer = JolenGameManager.Instance.GetCurrentPlayer();
        if ((activePlayer == 1 && gameObject != JolenGameManager.Instance.player1Pamato) ||
            (activePlayer == 2 && gameObject != JolenGameManager.Instance.player2Pamato))
            return;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Roof"))
                return; // ignore clicks hitting the roof
        }

        if (hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dragStartWorld = new Vector3(hitPoint.x, 0f, hitPoint.z);
            isDragging = true;

            if (aimLine != null)
                aimLine.enabled = true;

            // Freeze position so it won’t roll while aiming
            rb.constraints = RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezePositionZ |
                             RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnMouseDrag()
    {
        // --- NEW: Prevent input if not this pamato's turn ---
        int activePlayer = JolenGameManager.Instance.GetCurrentPlayer();
        if ((activePlayer == 1 && gameObject != JolenGameManager.Instance.player1Pamato) ||
            (activePlayer == 2 && gameObject != JolenGameManager.Instance.player2Pamato))
            return;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Roof"))
                return; // ignore clicks hitting the roof
        }

        if (!isDragging || hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dragCurrent = new Vector3(hitPoint.x, 0f, hitPoint.z);
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

    void OnMouseUp()
    {
        // --- NEW: Prevent input if not this pamato's turn ---
        int activePlayer = JolenGameManager.Instance.GetCurrentPlayer();
        if ((activePlayer == 1 && gameObject != JolenGameManager.Instance.player1Pamato) ||
            (activePlayer == 2 && gameObject != JolenGameManager.Instance.player2Pamato))
            return;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Roof"))
                return; // ignore clicks hitting the roof
        }

        if (!isDragging || hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dragEnd = new Vector3(hitPoint.x, 0f, hitPoint.z);
            Vector3 force = dragStartWorld - dragEnd;

            rb.constraints = originalConstraints; // Unfreeze before shooting
            rb.AddForce(force * forceMultiplier, ForceMode.Impulse);

            JolenGameManager.Instance.NotifyShot(rb);
            hasShot = true;

            if (aimLine != null)
                aimLine.enabled = false;
        }

        isDragging = false;
    }

    public void ResetTurn()
    {
        hasShot = false;
        isDragging = false;
        if (aimLine != null)
            aimLine.enabled = false;

        rb.constraints = originalConstraints; // ensure unfrozen at turn reset
    }
}
