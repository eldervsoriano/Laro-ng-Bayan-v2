using UnityEngine;
using UnityEngine.UI;  // For UI Image handling
using TMPro;  // For TextMeshPro functionality (UI)

public class PlayerInputManager : MonoBehaviour
{
    public int playerNumber = 1;  // 1 for Player 1, 2 for Player 2
    public Image rockImage;       // Image for Rock
    public Image paperImage;      // Image for Paper
    public Image scissorsImage;   // Image for Scissors
    public Image rockImagePlaceholder; // Placeholder for Rock
    public Image paperImagePlaceholder; // Placeholder for Paper
    public Image scissorsImagePlaceholder; // Placeholder for Scissors

    // Timer UI for showing countdown
    public TextMeshProUGUI timerText;

    // Player 1 keys (Q, W, E) and Player 2 keys (Keypad1, Keypad2, Keypad3)
    public KeyCode keyToSelectRock = KeyCode.Q;
    public KeyCode keyToSelectPaper = KeyCode.W;
    public KeyCode keyToSelectScissors = KeyCode.E;

    public KeyCode keyToSelectRockPlayer2 = KeyCode.Keypad1;
    public KeyCode keyToSelectPaperPlayer2 = KeyCode.Keypad2;
    public KeyCode keyToSelectScissorsPlayer2 = KeyCode.Keypad3;

    // Single variable to store player's choice (1=Rock, 2=Paper, 3=Scissors)
    private int playerChoice = 0;  // Tracks the player's current choice (1 for Rock, 2 for Paper, 3 for Scissors)
    private bool isSelecting = true;  // Flag to allow selection of the player's action

    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
    private float timer;  // Timer to show countdown

    void Start()
    {
        // Initially, hide all the images (choices are hidden at the start)
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);

        // Initially show placeholders for selection
        rockImagePlaceholder.gameObject.SetActive(true);
        paperImagePlaceholder.gameObject.SetActive(true);
        scissorsImagePlaceholder.gameObject.SetActive(true);

        timer = timeToChoose;  // Set timer

        // Start a countdown for selection time
        Invoke("StopSelecting", timeToChoose);
    }

    void Update()
    {
        if (isSelecting)
        {
            // Handle player input based on the player number
            if (playerNumber == 1)  // Player 1 input (Q, W, E for Rock, Paper, Scissors)
            {
                if (Input.GetKeyDown(keyToSelectRock))
                {
                    SetChoice(1);  // Rock
                }
                else if (Input.GetKeyDown(keyToSelectPaper))
                {
                    SetChoice(2);  // Paper
                }
                else if (Input.GetKeyDown(keyToSelectScissors))
                {
                    SetChoice(3);  // Scissors
                }
            }
            else if (playerNumber == 2)  // Player 2 input (Keypad1, Keypad2, Keypad3 for Rock, Paper, Scissors)
            {
                if (Input.GetKeyDown(keyToSelectRockPlayer2))
                {
                    SetChoice(1);  // Rock
                }
                else if (Input.GetKeyDown(keyToSelectPaperPlayer2))
                {
                    SetChoice(2);  // Paper
                }
                else if (Input.GetKeyDown(keyToSelectScissorsPlayer2))
                {
                    SetChoice(3);  // Scissors
                }
            }
        }

        // Update the timer text during the selection phase
        if (isSelecting)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();  // Update the UI with remaining time
        }
    }

    // Method to handle player's choice
    // Make this method public so SpiderGameManager can access it
    public void SetChoice(int choice)
    {
        playerChoice = choice;

        // Show the corresponding image based on the choice
        switch (choice)
        {
            case 1:
                rockImage.gameObject.SetActive(true);
                break;
            case 2:
                paperImage.gameObject.SetActive(true);
                break;
            case 3:
                scissorsImage.gameObject.SetActive(true);
                break;
        }

        // Hide the images for the other selections (only one choice can be shown at a time)
        rockImage.gameObject.SetActive(choice == 1);
        paperImage.gameObject.SetActive(choice == 2);
        scissorsImage.gameObject.SetActive(choice == 3);
    }

    // Stop the player's ability to select after the time runs out
    public void StopSelecting()
    {
        isSelecting = false;

        // Hide the placeholders and reveal actual selections after time is up
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);

        // Hide placeholders
        rockImagePlaceholder.gameObject.SetActive(false);
        paperImagePlaceholder.gameObject.SetActive(false);
        scissorsImagePlaceholder.gameObject.SetActive(false);

        // Trigger the game logic to compare selections
        SpiderGameManager.Instance.CompareSelections();
    }

    // Getter method for playerChoice
    public int GetPlayerChoice()
    {
        return playerChoice;
    }

    // Reset the player's choices after a round ends
    public void ResetSelection()
    {
        // Reset the player's current selection (so they can pick again)
        playerChoice = 0;

        // Reset the UI to show placeholders and hide selections
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);

        rockImagePlaceholder.gameObject.SetActive(true);
        paperImagePlaceholder.gameObject.SetActive(true);
        scissorsImagePlaceholder.gameObject.SetActive(true);
    }

    // Method to allow player to start selecting again
    public void StartSelecting()
    {
        isSelecting = true;  // Re-enable selection
        timer = timeToChoose;  // Reset the timer for this new round
    }
}
