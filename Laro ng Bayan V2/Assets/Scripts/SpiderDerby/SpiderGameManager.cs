using UnityEngine;
using TMPro; // For displaying result and health text in TextMeshPro
using UnityEngine.UI; // For Health Bar UI

public class SpiderGameManager : MonoBehaviour
{
    public static SpiderGameManager Instance;

    public PlayerInputManager player1Input;
    public PlayerInputManager player2Input;

    public TextMeshProUGUI resultText;  // Result UI text for displaying winner
    public Slider player1HealthSlider; // Player 1 health bar
    public Slider player2HealthSlider; // Player 2 health bar

    public TextMeshProUGUI player1HealthText; // Player 1 health text (percentage or numeric)
    public TextMeshProUGUI player2HealthText; // Player 2 health text (percentage or numeric)

    public GameObject winnerPanel; // Winner panel to display the winner
    public TextMeshProUGUI winnerText; // Text to show the winner message

    private int player1Health = 100; // Player 1's starting health
    private int player2Health = 100; // Player 2's starting health

    private bool gameOver = false;  // Flag to track if the game is over
    private int round = 1;  // Track the current round
    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
    private float timer;  // Timer to show countdown

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Set initial health values
        player1HealthSlider.value = player1Health; // Set Player 1's health slider to 100
        player2HealthSlider.value = player2Health; // Set Player 2's health slider to 100

        // Initialize health text
        player1HealthText.text = "Player 1: " + player1Health + "%";
        player2HealthText.text = "Player 2: " + player2Health + "%";

        winnerPanel.SetActive(false); // Make sure the winner panel is hidden at the start

        // Initialize timer for selection phase
        timer = timeToChoose;
    }

    void Update()
    {
        // Update the timer text during the selection phase
        if (!gameOver)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                // Show the timer countdown in the UI
                resultText.text = "Time: " + Mathf.Ceil(timer).ToString();
            }
            else
            {
                // When time runs out, stop selecting and compare the selections
                StopSelecting();
            }
        }
    }

    // Method to compare selections after players make their choice
    public void CompareSelections()
    {
        if (gameOver) return;  // If the game is over, stop comparing selections

        int[] player1Choices = player1Input.GetPlayerChoices();
        int[] player2Choices = player2Input.GetPlayerChoices();

        string result = "It's a draw!";  // Default result is a draw

        for (int i = 0; i < 3; i++)  // Loop through all 3 rounds (Rock, Paper, Scissors)
        {
            if (player1Choices[i] == player2Choices[i])  // If both players chose the same
            {
                result = "It's a draw!";
            }
            else if ((player1Choices[i] == 1 && player2Choices[i] == 3) || // Rock beats Scissors
                     (player1Choices[i] == 2 && player2Choices[i] == 1) || // Paper beats Rock
                     (player1Choices[i] == 3 && player2Choices[i] == 2))   // Scissors beats Paper
            {
                result = "Player 1 Wins!";
                TakeDamage(2);  // Player 1 wins, so Player 2 takes damage
                break;
            }
            else
            {
                result = "Player 2 Wins!";
                TakeDamage(1);  // Player 2 wins, so Player 1 takes damage
                break;
            }
        }

        resultText.text = result;  // Display the result

        // Check if any player's health has reached 0 and declare the winner
        if (player1Health <= 0)
        {
            winnerPanel.SetActive(true); // Show winner panel
            winnerText.text = "Player 2 Wins!"; // Display Player 2 wins message
            gameOver = true;  // End the game
        }
        else if (player2Health <= 0)
        {
            winnerPanel.SetActive(true); // Show winner panel
            winnerText.text = "Player 1 Wins!"; // Display Player 1 wins message
            gameOver = true;  // End the game
        }
        else
        {
            // If no player is dead, proceed to the next round
            Invoke("ResetRound", 2f);  // Wait 2 seconds before resetting
        }
    }

    // Method to reduce the player's health when they take damage
    public void TakeDamage(int player)
    {
        if (player == 1)
        {
            player1Health -= 10; // Reduce Player 1's health by 10
            player1Health = Mathf.Clamp(player1Health, 0, 100); // Ensure health stays between 0 and 100

            // Update the health bar's fill amount
            player1HealthSlider.value = player1Health;

            // Update health text
            player1HealthText.text = "Player 1: " + player1Health + "%";
        }
        else if (player == 2)
        {
            player2Health -= 10; // Reduce Player 2's health by 10
            player2Health = Mathf.Clamp(player2Health, 0, 100); // Ensure health stays between 0 and 100

            // Update the health bar's fill amount
            player2HealthSlider.value = player2Health;

            // Update health text
            player2HealthText.text = "Player 2: " + player2Health + "%";
        }
    }

    // Method to reset the round for the next round
    void ResetRound()
    {
        // Reset health values to 100
        player1Health = 100;
        player2Health = 100;

        // Reset sliders
        player1HealthSlider.value = player1Health;
        player2HealthSlider.value = player2Health;

        // Reset health text
        player1HealthText.text = "Player 1: " + player1Health + "%";
        player2HealthText.text = "Player 2: " + player2Health + "%";

        // Hide winner panel
        winnerPanel.SetActive(false);

        // Reset the selection phase (allow players to choose again)
        player1Input.ResetSelection();
        player2Input.ResetSelection();

        // Reset the round counter
        round++; // Increment round number

        // Restart the timer
        timer = timeToChoose;

        gameOver = false; // Reset the game-over flag
    }

    // Stop the player's ability to select after the time runs out
    public void StopSelecting()
    {
        player1Input.StopSelecting();
        player2Input.StopSelecting();
    }
}
