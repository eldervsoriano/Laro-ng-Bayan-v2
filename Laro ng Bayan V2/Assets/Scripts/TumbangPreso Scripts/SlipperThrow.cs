//using UnityEngine;

//public class SlipperThrow : MonoBehaviour
//{
//    public Rigidbody rb;
//    public float throwForce = 12f;
//    public float spinSpeed = 720f;
//    public int playerNumber = 1; // Assign in Inspector: 1 or 2

//    private bool isDragging = false;
//    private bool hasThrown = false;
//    private Vector3 dragStart;
//    private Camera cam;
//    [HideInInspector] public Vector3 startPosition;
//    [HideInInspector] public Quaternion startRotation;



//    void Awake()
//    {
//        startPosition = transform.position;
//        startRotation = transform.rotation; // Save rotation from scene
//    }


//    void Start()
//    {
//        cam = Camera.main;
//        rb.isKinematic = true;
//    }



//    void OnMouseDown()
//    {
//        if (hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
//            return;

//        dragStart = Input.mousePosition;
//        isDragging = true;
//    }

//    void OnMouseUp()
//    {
//        if (!isDragging || hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
//            return;

//        // Check drag distance
//        float dragDistance = (Input.mousePosition - dragStart).magnitude;
//        if (dragDistance < 30f) // threshold in pixels (tweak this value)
//        {
//            isDragging = false;
//            return; // Do nothing if just a click
//        }

//        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            Vector3 direction = (hit.point - transform.position).normalized;
//            direction.y += 0.5f;
//            direction = direction.normalized;

//            rb.isKinematic = false;
//            rb.AddForce(direction * throwForce, ForceMode.Impulse);
//            rb.AddTorque(Vector3.up * spinSpeed, ForceMode.Impulse);

//            TumbangGameManager.Instance.NotifySlipperThrown(rb);
//            hasThrown = true;
//        }

//        isDragging = false;
//    }

//    public void ResetTurn()
//    {
//        hasThrown = false;
//        isDragging = false;

//        rb.isKinematic = false; // temporarily make it non-kinematic
//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;

//        rb.isKinematic = true; // now safe to freeze it again

//        transform.position = startPosition;
//        transform.rotation = startRotation;
//    }




//}



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
    private AIPlayer aiPlayer;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        aiPlayer = GetComponent<AIPlayer>();
    }

    void Start()
    {
        cam = Camera.main;
        rb.isKinematic = true;
    }

    void OnMouseDown()
    {
        // Block player input if this is an AI slipper
        if (aiPlayer != null && aiPlayer.isAI)
        {
            Debug.Log($"⛔ AI slipper cannot be controlled by mouse");
            return;
        }

        if (hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
            return;

        dragStart = Input.mousePosition;
        isDragging = true;
    }

    void OnMouseUp()
    {
        // Block player input if this is an AI slipper
        if (aiPlayer != null && aiPlayer.isAI)
        {
            return;
        }

        if (!isDragging || hasThrown || TumbangGameManager.Instance.GetCurrentPlayer() != playerNumber)
            return;

        // Check drag distance
        float dragDistance = (Input.mousePosition - dragStart).magnitude;
        if (dragDistance < 30f) // threshold in pixels
        {
            isDragging = false;
            return; // Do nothing if just a click
        }

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

        rb.isKinematic = false; // temporarily make it non-kinematic
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // now safe to freeze it again

        transform.position = startPosition;
        transform.rotation = startRotation;

        // Also reset AI state if this is an AI slipper
        if (aiPlayer != null && aiPlayer.isAI)
        {
            aiPlayer.ResetAITurn();
        }
    }
}