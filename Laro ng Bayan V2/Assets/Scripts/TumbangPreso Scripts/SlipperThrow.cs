using UnityEngine;

public class SlipperThrow : MonoBehaviour
{
    public Rigidbody rb;
    public float throwForce = 12f;
    public float spinSpeed = 720f;
    public int playerNumber = 1; // Assign in Inspector: 1 or 2

    private bool isDragging = false;
    private bool hasThrown = false;
    private Vector3 dragStart;
    private Camera cam;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;



    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation; // ✅ Save rotation from scene
    }


    void Start()
    {
        cam = Camera.main;
        rb.isKinematic = true;
    }



    void OnMouseDown()
    {
        if (hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
            return;

        dragStart = Input.mousePosition;
        isDragging = true;
    }

    void OnMouseUp()
    {
        if (!isDragging || hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y += 0.5f;
            direction = direction.normalized;

            rb.isKinematic = false;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
            rb.AddTorque(Vector3.up * spinSpeed, ForceMode.Impulse);

            TumbangGameManager.Instance.NotifySlipperThrown(rb);
            hasThrown = true;
        }

        isDragging = false;
    }

    public void ResetTurn()
    {
        hasThrown = false;
        isDragging = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; // ✅ clear spin before disabling physics

        rb.isKinematic = true; // ✅ move this after velocities

        transform.position = startPosition;
        transform.rotation = startRotation;
    }



}
