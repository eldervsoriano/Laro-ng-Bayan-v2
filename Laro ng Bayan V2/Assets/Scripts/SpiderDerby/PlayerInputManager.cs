//using UnityEngine;
//using UnityEngine.UI;  // For UI Image handling
//using TMPro;  // For TextMeshPro functionality (UI)

//public class PlayerInputManager : MonoBehaviour
//{
//    public int playerNumber = 1;  // 1 for Player 1, 2 for Player 2
//    public Image rockImage;       // Image for Rock
//    public Image paperImage;      // Image for Paper
//    public Image scissorsImage;   // Image for Scissors
//    public Image rockImagePlaceholder; // Placeholder for Rock
//    public Image paperImagePlaceholder; // Placeholder for Paper
//    public Image scissorsImagePlaceholder; // Placeholder for Scissors

//    // Timer UI for showing countdown
//    public TextMeshProUGUI timerText;

//    // Player 1 keys (Q, W, E) and Player 2 keys (Keypad1, Keypad2, Keypad3)
//    public KeyCode keyToSelectRock = KeyCode.Q;
//    public KeyCode keyToSelectPaper = KeyCode.W;
//    public KeyCode keyToSelectScissors = KeyCode.E;

//    public KeyCode keyToSelectRockPlayer2 = KeyCode.Keypad1;
//    public KeyCode keyToSelectPaperPlayer2 = KeyCode.Keypad2;
//    public KeyCode keyToSelectScissorsPlayer2 = KeyCode.Keypad3;

//    // Single variable to store player's choice (1=Rock, 2=Paper, 3=Scissors)
//    private int playerChoice = 0;  // Tracks the player's current choice (1 for Rock, 2 for Paper, 3 for Scissors)
//    private bool isSelecting = true;  // Flag to allow selection of the player's action

//    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
//    private float timer;  // Timer to show countdown

//    void Start()
//    {
//        // Initially, hide all the images (choices are hidden at the start)
//        rockImage.gameObject.SetActive(false);
//        paperImage.gameObject.SetActive(false);
//        scissorsImage.gameObject.SetActive(false);

//        // Initially show placeholders for selection
//        rockImagePlaceholder.gameObject.SetActive(true);
//        paperImagePlaceholder.gameObject.SetActive(true);
//        scissorsImagePlaceholder.gameObject.SetActive(true);

//        timer = timeToChoose;  // Set timer

//        // Start a countdown for selection time
//        Invoke("StopSelecting", timeToChoose);
//    }

//    void Update()
//    {
//        if (isSelecting)
//        {
//            // Handle player input based on the player number
//            if (playerNumber == 1)  // Player 1 input (Q, W, E for Rock, Paper, Scissors)
//            {
//                if (Input.GetKeyDown(keyToSelectRock))
//                {
//                    SetChoice(1);  // Rock
//                }
//                else if (Input.GetKeyDown(keyToSelectPaper))
//                {
//                    SetChoice(2);  // Paper
//                }
//                else if (Input.GetKeyDown(keyToSelectScissors))
//                {
//                    SetChoice(3);  // Scissors
//                }
//            }
//            else if (playerNumber == 2)  // Player 2 input (Keypad1, Keypad2, Keypad3 for Rock, Paper, Scissors)
//            {
//                if (Input.GetKeyDown(keyToSelectRockPlayer2))
//                {
//                    SetChoice(1);  // Rock
//                }
//                else if (Input.GetKeyDown(keyToSelectPaperPlayer2))
//                {
//                    SetChoice(2);  // Paper
//                }
//                else if (Input.GetKeyDown(keyToSelectScissorsPlayer2))
//                {
//                    SetChoice(3);  // Scissors
//                }
//            }
//        }

//        // Update the timer text during the selection phase
//        if (isSelecting)
//        {
//            timer -= Time.deltaTime;
//            timerText.text = Mathf.Ceil(timer).ToString();  // Update the UI with remaining time
//        }
//    }

//    // Method to handle player's choice
//    // Make this method public so SpiderGameManager can access it
//    public void SetChoice(int choice)
//    {
//        playerChoice = choice;

//        // During selection, don't show the choice yet
//        // We'll reveal it when time runs out
//    }

//    // New method to reveal the player's choice after time runs out
//    public void RevealChoice()
//    {
//        // Show the corresponding image based on the choice
//        rockImage.gameObject.SetActive(playerChoice == 1);
//        paperImage.gameObject.SetActive(playerChoice == 2);
//        scissorsImage.gameObject.SetActive(playerChoice == 3);
//    }

//    // Stop the player's ability to select after the time runs out
//    public void StopSelecting()
//    {
//        isSelecting = false;

//        // Hide the placeholders
//        rockImagePlaceholder.gameObject.SetActive(false);
//        paperImagePlaceholder.gameObject.SetActive(false);
//        scissorsImagePlaceholder.gameObject.SetActive(false);

//        // Reveal the player's choice immediately when time runs out
//        RevealChoice();

//        // Trigger the game logic to compare selections
//        SpiderGameManager.Instance.CompareSelections();
//    }

//    // Getter method for playerChoice
//    public int GetPlayerChoice()
//    {
//        return playerChoice;
//    }

//    // Reset the player's choices after a round ends
//    public void ResetSelection()
//    {
//        // Reset the player's current selection (so they can pick again)
//        playerChoice = 0;

//        // Reset the UI to show placeholders and hide selections
//        rockImage.gameObject.SetActive(false);
//        paperImage.gameObject.SetActive(false);
//        scissorsImage.gameObject.SetActive(false);

//        rockImagePlaceholder.gameObject.SetActive(true);
//        paperImagePlaceholder.gameObject.SetActive(true);
//        scissorsImagePlaceholder.gameObject.SetActive(true);
//    }

//    // Method to allow player to start selecting again
//    public void StartSelecting()
//    {
//        isSelecting = true;  // Re-enable selection
//        timer = timeToChoose;  // Reset the timer for this new round
//    }
//}

//using UnityEngine;
//using UnityEngine.UI;  // For UI Image handling
//using TMPro;  // For TextMeshPro functionality (UI)

//public class PlayerInputManager : MonoBehaviour
//{
//    public int playerNumber = 1;  // 1 for Player 1, 2 for Player 2
//    public Image rockImage;       // Image for Rock
//    public Image paperImage;      // Image for Paper
//    public Image scissorsImage;   // Image for Scissors
//    public Image rockImagePlaceholder; // Placeholder for Rock
//    public Image paperImagePlaceholder; // Placeholder for Paper
//    public Image scissorsImagePlaceholder; // Placeholder for Scissors

//    // Timer UI for showing countdown
//    public TextMeshProUGUI timerText;

//    // Player 1 keys (Q, W, E) and Player 2 keys (Keypad1, Keypad2, Keypad3)
//    public KeyCode keyToSelectRock = KeyCode.Q;
//    public KeyCode keyToSelectPaper = KeyCode.W;
//    public KeyCode keyToSelectScissors = KeyCode.E;

//    public KeyCode keyToSelectRockPlayer2 = KeyCode.Keypad1;
//    public KeyCode keyToSelectPaperPlayer2 = KeyCode.Keypad2;
//    public KeyCode keyToSelectScissorsPlayer2 = KeyCode.Keypad3;

//    // Single variable to store player's choice (1=Rock, 2=Paper, 3=Scissors)
//    private int playerChoice = 0;  // Tracks the player's current choice (1 for Rock, 2 for Paper, 3 for Scissors)
//    private bool isSelecting = true;  // Flag to allow selection of the player's action

//    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
//    private float timer;  // Timer to show countdown

//    void Start()
//    {
//        // Initially, hide all the images (choices are hidden at the start)
//        rockImage.gameObject.SetActive(false);
//        paperImage.gameObject.SetActive(false);
//        scissorsImage.gameObject.SetActive(false);

//        // Initially show placeholders for selection
//        rockImagePlaceholder.gameObject.SetActive(true);
//        paperImagePlaceholder.gameObject.SetActive(true);
//        scissorsImagePlaceholder.gameObject.SetActive(true);

//        timer = timeToChoose;  // Set timer

//        // Start a countdown for selection time
//        Invoke("StopSelecting", timeToChoose);
//    }

//    void Update()
//    {
//        if (isSelecting)
//        {
//            // Handle player input based on the player number
//            if (playerNumber == 1)  // Player 1 input (Q, W, E for Rock, Paper, Scissors)
//            {
//                if (Input.GetKeyDown(keyToSelectRock))
//                {
//                    SetChoice(1);  // Rock
//                }
//                else if (Input.GetKeyDown(keyToSelectPaper))
//                {
//                    SetChoice(2);  // Paper
//                }
//                else if (Input.GetKeyDown(keyToSelectScissors))
//                {
//                    SetChoice(3);  // Scissors
//                }
//            }
//            else if (playerNumber == 2)  // Player 2 input (Keypad1, Keypad2, Keypad3 for Rock, Paper, Scissors)
//            {
//                if (Input.GetKeyDown(keyToSelectRockPlayer2))
//                {
//                    SetChoice(1);  // Rock
//                }
//                else if (Input.GetKeyDown(keyToSelectPaperPlayer2))
//                {
//                    SetChoice(2);  // Paper
//                }
//                else if (Input.GetKeyDown(keyToSelectScissorsPlayer2))
//                {
//                    SetChoice(3);  // Scissors
//                }
//            }
//        }

//        // Update the timer text during the selection phase
//        if (isSelecting)
//        {
//            timer -= Time.deltaTime;
//            timerText.text = Mathf.Ceil(timer).ToString();  // Update the UI with remaining time
//        }
//    }

//    // Method to handle player's choice
//    // Make this method public so SpiderGameManager can access it
//    public void SetChoice(int choice)
//    {
//        playerChoice = choice;

//        // During selection, don't show the choice yet
//        // We'll reveal it when time runs out
//    }

//    // New method to reveal the player's choice after time runs out
//    public void RevealChoice()
//    {
//        // Show the corresponding image based on the choice
//        rockImage.gameObject.SetActive(playerChoice == 1);
//        paperImage.gameObject.SetActive(playerChoice == 2);
//        scissorsImage.gameObject.SetActive(playerChoice == 3);
//    }

//    // Stop the player's ability to select after the time runs out
//    public void StopSelecting()
//    {
//        isSelecting = false;

//        // Hide the placeholders
//        rockImagePlaceholder.gameObject.SetActive(false);
//        paperImagePlaceholder.gameObject.SetActive(false);
//        scissorsImagePlaceholder.gameObject.SetActive(false);

//        // Reveal the player's choice immediately when time runs out
//        RevealChoice();

//        // Trigger the game logic to compare selections
//        SpiderGameManager.Instance.CompareSelections();
//    }

//    // Getter method for playerChoice
//    public int GetPlayerChoice()
//    {
//        return playerChoice;
//    }

//    // Reset the player's choices after a round ends
//    public void ResetSelection()
//    {
//        // Reset the player's current selection (so they can pick again)
//        playerChoice = 0;

//        // Reset the UI to show placeholders and hide selections
//        rockImage.gameObject.SetActive(false);
//        paperImage.gameObject.SetActive(false);
//        scissorsImage.gameObject.SetActive(false);

//        rockImagePlaceholder.gameObject.SetActive(true);
//        paperImagePlaceholder.gameObject.SetActive(true);
//        scissorsImagePlaceholder.gameObject.SetActive(true);
//    }

//    // Method to allow player to start selecting again
//    public void StartSelecting()
//    {
//        isSelecting = true;  // Re-enable selection
//        timer = timeToChoose;  // Reset the timer for this new round
//    }
//}




//AI



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

    // AI Controller reference (for Player 2 only)
    private AIPlayerController aiController;

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

        // Get AI controller if this is Player 2
        if (playerNumber == 2)
        {
            aiController = GetComponent<AIPlayerController>();
        }

        // Start a countdown for selection time
        Invoke("StopSelecting", timeToChoose);
    }

    void Update()
    {
        // Only handle human input if AI is not enabled for this player
        bool isAIControlled = (aiController != null && aiController.IsAIEnabled());

        if (isSelecting && !isAIControlled)
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

        // Update the timer text during the selection phase (only if we have a timer text and we're not AI controlled)
        if (isSelecting && timerText != null && !isAIControlled)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();  // Update the UI with remaining time
        }
    }

    // Method to handle player's choice
    // Make this method public so SpiderGameManager and AI can access it
    public void SetChoice(int choice)
    {
        if (isSelecting)  // Only allow choice if still selecting
        {
            playerChoice = choice;

            // Visual feedback for selection (subtle highlight or animation could be added here)
            UpdateSelectionVisuals();
        }
    }

    // Method to provide visual feedback when a choice is made (optional enhancement)
    private void UpdateSelectionVisuals()
    {
        // You could add visual feedback here like highlighting the selected placeholder
        // For now, we'll keep the original behavior of not showing until time runs out
    }

    // New method to reveal the player's choice after time runs out
    public void RevealChoice()
    {
        // Show the corresponding image based on the choice
        rockImage.gameObject.SetActive(playerChoice == 1);
        paperImage.gameObject.SetActive(playerChoice == 2);
        scissorsImage.gameObject.SetActive(playerChoice == 3);
    }

    // Stop the player's ability to select after the time runs out
    public void StopSelecting()
    {
        isSelecting = false;

        // Hide the placeholders
        rockImagePlaceholder.gameObject.SetActive(false);
        paperImagePlaceholder.gameObject.SetActive(false);
        scissorsImagePlaceholder.gameObject.SetActive(false);

        // Reveal the player's choice immediately when time runs out
        RevealChoice();

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

        // Cancel any pending StopSelecting calls and set a new one
        CancelInvoke("StopSelecting");
        Invoke("StopSelecting", timeToChoose);
    }

    // Method to check if this player is AI controlled
    public bool IsAIControlled()
    {
        return aiController != null && aiController.IsAIEnabled();
    }

    // Method to get the display name for this player
    public string GetPlayerDisplayName()
    {
        if (playerNumber == 1)
            return "Player 1";
        else
            return IsAIControlled() ? "AI" : "Player 2";
    }

    // Method to set timer text reference (useful for UI setup)
    public void SetTimerText(TextMeshProUGUI timerTextComponent)
    {
        timerText = timerTextComponent;
    }

    // Method to get current selection status
    public bool IsCurrentlySelecting()
    {
        return isSelecting;
    }

    // Method to get remaining time
    public float GetRemainingTime()
    {
        return timer;
    }

    // Method to force stop selecting (called by game manager)
    public void ForceStopSelecting()
    {
        CancelInvoke("StopSelecting");
        StopSelecting();
    }
}