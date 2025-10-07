using UnityEngine;

public class ChoiceIndicator : MonoBehaviour
{
    [Header("Indicator Settings")]
    public GameObject chosenIndicator;  // The glow/checkmark or bar
    public float checkInterval = 0.05f; // Frequent check for responsiveness

    private PlayerInputManager inputManager;
    private bool indicatorShown = false;
    private int lastChoice = 0;

    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();

        if (chosenIndicator != null)
            chosenIndicator.SetActive(false);

        InvokeRepeating(nameof(CheckChoiceState), 0f, checkInterval);
    }

    void CheckChoiceState()
    {
        if (inputManager == null || chosenIndicator == null)
            return;

        bool isSelecting = inputManager.IsCurrentlySelecting();
        int currentChoice = inputManager.GetPlayerChoice();

        // When player picks a choice while selecting
        if (isSelecting && currentChoice > 0 && !indicatorShown)
        {
            chosenIndicator.SetActive(true);
            indicatorShown = true;
            lastChoice = currentChoice;
        }

        // When the player changes their mind (optional: still selecting but picks a new choice)
        if (isSelecting && indicatorShown && currentChoice != lastChoice && currentChoice > 0)
        {
            lastChoice = currentChoice; // Update tracking (you could add an effect here)
        }

        // When the reveal starts (StopSelecting is called → isSelecting = false)
        if (!isSelecting && indicatorShown)
        {
            chosenIndicator.SetActive(false);
            indicatorShown = false;
        }

        // When new round starts (ResetSelection called)
        if (isSelecting && currentChoice == 0 && indicatorShown)
        {
            chosenIndicator.SetActive(false);
            indicatorShown = false;
        }
    }
}
