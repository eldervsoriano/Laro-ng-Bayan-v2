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

    // Make playerChoices public so that SpiderGameManager can access it
    public int[] playerChoices = new int[3];  // Array to store the player's 3 choices (1=Rock, 2=Paper, 3=Scissors)
    private int currentSelection = 0;  // Tracks which choice the player is making (0 for first, 1 for second, 2 for third)
    private bool isSelecting = true;  // Flag to allow selection of the player's action

    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
    private float timer;  // Timer to show countdown

    void Start()
    {
        // Initially, hide all the images (choices are hidden at the start)
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);

        rockImagePlaceholder.gameObject.SetActive(true);  // Show placeholders for selection
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
    void SetChoice(int choice)
    {
        if (currentSelection < 3)
        {
            playerChoices[currentSelection] = choice;

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

            // Move to the next selection
            currentSelection++;

            // Hide the images for the other selections
            if (currentSelection < 3)
            {
                rockImage.gameObject.SetActive(false);
                paperImage.gameObject.SetActive(false);
                scissorsImage.gameObject.SetActive(false);
            }
        }
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

    // Getter method for playerChoices
    public int[] GetPlayerChoices()
    {
        return playerChoices;
    }

    // Reset the player's choices after a round ends
    public void ResetSelection()
    {
        playerChoices = new int[3];  // Reset the array
        currentSelection = 0;  // Reset the current selection index

        // Hide all choices and reset placeholders
        rockImage.gameObject.SetActive(false);
        paperImage.gameObject.SetActive(false);
        scissorsImage.gameObject.SetActive(false);

        rockImagePlaceholder.gameObject.SetActive(true);
        paperImagePlaceholder.gameObject.SetActive(true);
        scissorsImagePlaceholder.gameObject.SetActive(true);
    }
}
