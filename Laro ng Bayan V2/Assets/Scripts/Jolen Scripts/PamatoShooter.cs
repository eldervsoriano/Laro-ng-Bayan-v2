using UnityEngine;

public class PamatoShooter : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 5f; // was 1.5, boost it for better response

    public LineRenderer aimLine;

    private Vector3 dragStartWorld;
    private bool isDragging = false;
    private bool hasShot = false;
    private Camera cam;



    void Start()
    {
        cam = Camera.main;
        if (aimLine != null)
            aimLine.enabled = false;
    }

    void OnMouseDown()
    {
        if (hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            dragStartWorld = new Vector3(hit.point.x, 0f, hit.point.z);
            isDragging = true;

            if (aimLine != null)
                aimLine.enabled = true;
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging || hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dragCurrent = new Vector3(hit.point.x, 0f, hit.point.z);
            Vector3 direction = dragStartWorld - dragCurrent;

            Vector3 lineEnd = transform.position + direction;

            if (aimLine != null)
            {
                aimLine.SetPosition(0, transform.position);
                aimLine.SetPosition(1, lineEnd);

                // Optional: Color based on power
                float power = Mathf.Clamp(direction.magnitude, 0, 2f);
                aimLine.startColor = aimLine.endColor = Color.Lerp(Color.green, Color.red, power / 2f);
            }
        }
    }

    void OnMouseUp()
    {
        if (!isDragging || hasShot) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dragEnd = new Vector3(hit.point.x, 0f, hit.point.z);
            Vector3 force = dragStartWorld - dragEnd;
            rb.AddForce(force * forceMultiplier, ForceMode.Impulse);

            GameManager.Instance.NotifyShot(rb); // ✅ Let GameManager watch it
            hasShot = true;

            if (aimLine != null)
                aimLine.enabled = false;
        }

        isDragging = false;
    }


    void Update()
    {
        // Remove turn-ending logic here
    }

    public void ResetTurn()
    {
        hasShot = false;
        isDragging = false;
        if (aimLine != null)
            aimLine.enabled = false;
    }


}
