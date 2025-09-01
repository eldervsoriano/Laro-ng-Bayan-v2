using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;   // reference to dialogue panel
    public Button pauseButton;       // reference to your pause button

    private bool isDialogueActive = false;

    void Update()
    {
        if (isDialogueActive)
        {
            // block ESC key input
            if (Input.GetKeyDown(KeyCode.Escape))
                return;
        }
    }

    public void ShowDialogue()
    {
        dialogueBox.SetActive(true);
        isDialogueActive = true;

        // disable pause button
        if (pauseButton != null)
            pauseButton.interactable = false;
    }

    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;

        // re-enable pause button
        if (pauseButton != null)
            pauseButton.interactable = true;
    }
}
