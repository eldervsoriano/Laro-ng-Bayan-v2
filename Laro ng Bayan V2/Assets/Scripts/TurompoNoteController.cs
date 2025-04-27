////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.UI;

////// Controls individual notes moving down the rhythm highway
////public class TurompoNoteController : MonoBehaviour
////{
////    public int keyIndex;
////    public float speed;
////    public Vector3 targetPosition;
////    public TurompoRhythmController rhythmController;

////    private float destroyTimer = 5.0f; // Destroy note after 5 seconds
////    private bool passedTargetLine = false;

////    void Update()
////    {
////        // Countdown destroy timer
////        destroyTimer -= Time.deltaTime;
////        if (destroyTimer <= 0)
////        {
////            // Time's up, destroy this note
////            if (rhythmController != null)
////                rhythmController.RemoveNote(this);
////            Destroy(gameObject);
////            return;
////        }

////        // Continue moving downward regardless of target line
////        transform.position = new Vector3(
////            transform.position.x,
////            transform.position.y - speed * Time.deltaTime,
////            transform.position.z
////        );

////        // Apply a -180 degree rotation around the y-axis
////        transform.rotation = Quaternion.Euler(0, -180, 0);

////        // Check if we've passed the target line
////        if (!passedTargetLine && transform.position.y < targetPosition.y)
////        {
////            passedTargetLine = true;
////            // Notify rhythm controller that this note was missed
////            if (rhythmController != null)
////                rhythmController.NoteMissed(this);
////        }
////    }

////}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// Controls individual notes moving down the rhythm highway
//public class TurompoNoteController : MonoBehaviour
//{
//    public int keyIndex;
//    public float speed;
//    public Vector3 targetPosition;
//    public TurompoRhythmController rhythmController;

//    [Header("Rotation Settings")]
//    [Tooltip("The rotation to apply to the note")]
//    public Vector3 noteRotation = new Vector3(0, -180, 0);

//    private float destroyTimer = 5.0f; // Destroy note after 5 seconds
//    private bool passedTargetLine = false;

//    void Update()
//    {
//        // Countdown destroy timer
//        destroyTimer -= Time.deltaTime;
//        if (destroyTimer <= 0)
//        {
//            // Time's up, destroy this note
//            if (rhythmController != null)
//                rhythmController.RemoveNote(this);
//            Destroy(gameObject);
//            return;
//        }

//        // Continue moving downward regardless of target line
//        transform.position = new Vector3(
//            transform.position.x,
//            transform.position.y - speed * Time.deltaTime,
//            transform.position.z
//        );

//        // Apply the customizable rotation
//        transform.rotation = Quaternion.Euler(noteRotation);

//        // Check if we've passed the target line
//        if (!passedTargetLine && transform.position.y < targetPosition.y)
//        {
//            passedTargetLine = true;
//            // Notify rhythm controller that this note was missed
//            if (rhythmController != null)
//                rhythmController.NoteMissed(this);
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// Controls individual notes moving down the rhythm highway
//public class TurompoNoteController : MonoBehaviour
//{
//    public int keyIndex;
//    public float speed;
//    public Vector3 targetPosition;
//    public TurompoRhythmController rhythmController;

//    [Header("Rotation Settings")]
//    [Tooltip("The rotation to apply to the note")]
//    public Vector3 noteRotation = new Vector3(0, -180, 0);

//    private float destroyTimer = 5.0f; // Destroy note after 5 seconds
//    private bool passedTargetLine = false;

//    void Update()
//    {
//        // Countdown destroy timer
//        destroyTimer -= Time.deltaTime;
//        if (destroyTimer <= 0)
//        {
//            // Time's up, destroy this note
//            if (rhythmController != null)
//                rhythmController.RemoveNote(this);
//            Destroy(gameObject);
//            return;
//        }

//        // Continue moving downward regardless of target line
//        transform.position = new Vector3(
//            transform.position.x,
//            transform.position.y - speed * Time.deltaTime,
//            transform.position.z
//        );

//        // Apply the customizable rotation
//        transform.rotation = Quaternion.Euler(noteRotation);

//        // Check if we've passed the target line
//        if (!passedTargetLine && transform.position.y < targetPosition.y)
//        {
//            passedTargetLine = true;
//            // Notify rhythm controller that this note was missed
//            if (rhythmController != null)
//                rhythmController.NoteMissed(this);
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls individual notes moving down the rhythm highway
public class TurompoNoteController : MonoBehaviour
{
    public int keyIndex;
    public float speed;
    public Vector3 targetPosition;
    public TurompoRhythmController rhythmController;

    [Header("Rotation Settings")]
    [Tooltip("The rotation to apply to the note")]
    public Vector3 noteRotation = new Vector3(0, -180, 0);

    // Flag to apply custom rotation from inspector
    [Tooltip("Enable this to use the rotation values from the inspector")]
    public bool useCustomRotation = true;

    private float destroyTimer = 5.0f; // Destroy note after 5 seconds
    private bool passedTargetLine = false;

    void Start()
    {
        // Apply initial rotation if using custom rotation
        if (useCustomRotation)
        {
            transform.rotation = Quaternion.Euler(noteRotation);
        }
    }

    void Update()
    {
        // Countdown destroy timer
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0)
        {
            // Time's up, destroy this note
            if (rhythmController != null)
                rhythmController.RemoveNote(this);
            Destroy(gameObject);
            return;
        }

        // Continue moving downward regardless of target line
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - speed * Time.deltaTime,
            transform.position.z
        );

        // Apply the customizable rotation only if using custom rotation
        // This prevents overriding any animations or other rotation changes
        if (useCustomRotation)
        {
            transform.rotation = Quaternion.Euler(noteRotation);
        }

        // Check if we've passed the target line
        if (!passedTargetLine && transform.position.y < targetPosition.y)
        {
            passedTargetLine = true;
            // Notify rhythm controller that this note was missed
            if (rhythmController != null)
                rhythmController.NoteMissed(this);
        }
    }
}