using System.Collections;
using UnityEngine;

public class TargetLineKeyHighlighter : MonoBehaviour
{
    public Outline outline;                       // The outline component on this box
    public TurompoRhythmController rhythmController;  // Drag Player1 or Player2 here
    public int playerIndex = 1;                   // 1 = Player1, 2 = Player2

    private void Awake()
    {
        // Try auto-get the outline if not assigned
        if (outline == null)
            outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;
    }

    private void Update()
    {
        if (rhythmController == null || outline == null)
            return;

        // Loop through all player keys
        foreach (KeyCode key in rhythmController.playerKeys)
        {
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(FlashOutline());
                break; // Only flash once per frame even if multiple keys are pressed
            }
        }
    }

    private IEnumerator FlashOutline()
    {
        outline.enabled = true;
        yield return new WaitForSeconds(0.1f);
        outline.enabled = false;
    }
}
