using System.Collections;
using UnityEngine;

public class NoteHighlighter : MonoBehaviour
{
    private Outline outline;
    private TurompoRhythmController turompoRhythmController; // orrect reference
    private TurompoNoteController noteController; // also correct naming
    private bool insideTarget = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false; // start off

        noteController = GetComponent<TurompoNoteController>();
    }

    void Start()
    {
        // make sure these GameObject names match exactly in the scene
        outline = GameObject.Find("Target Line p1").GetComponent<Outline>();
        turompoRhythmController = GameObject.Find("Player1").GetComponent<TurompoRhythmController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TargetLine"))
        {
            insideTarget = true;
            Debug.Log($"{gameObject.name} entered target line!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TargetLine"))
        {
            insideTarget = false;
            Debug.Log($"{gameObject.name} exited target line!");
        }
    }

    private void Update()
    {
        if (!insideTarget || turompoRhythmController == null || noteController == null)
            return;

        // Detect if the corresponding key for this note is pressed
        KeyCode keyForNote = turompoRhythmController.playerKeys[noteController.keyIndex];
        if (Input.GetKeyDown(keyForNote))
        {
            StartCoroutine(FlashOutline());
        }
    }

    private IEnumerator FlashOutline()
    {
        if (outline == null) yield break;

        outline.enabled = true;
        yield return new WaitForSeconds(0.1f);
        outline.enabled = false;
    }
}
