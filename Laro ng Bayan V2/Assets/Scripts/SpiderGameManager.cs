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

    private int player1Health = 100; // Player 1's starting health (can adjust based on gameplay)
    private int player2Health = 100; // Player 2's starting health (can adjust based on gameplay)

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
    }

    // Method to compare selections after players make their choice
    public void CompareSelections()
    {
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
    }


    // Method to reduce the player's health when they take damage
    public void TakeDamage(int player)
    {
        if (player == 1)
        {
            player1Health -= 10; // Reduce Player 1's health by 10
            player1Health = Mathf.Clamp(player1Health, 0, 100); // Ensure health stays between 0 and 100

            // Debug log to check health value
            Debug.Log("Player 1 Health: " + player1Health);

            // Update the health bar's fill amount
            player1HealthSlider.value = player1Health; // Update slider value based on health

            // Update health text
            player1HealthText.text = "Player 1: " + player1Health + "%";
        }
        else if (player == 2)
        {
            player2Health -= 10; // Reduce Player 2's health by 10
            player2Health = Mathf.Clamp(player2Health, 0, 100); // Ensure health stays between 0 and 100

            // Debug log to check health value
            Debug.Log("Player 2 Health: " + player2Health);

            // Update the health bar's fill amount
            player2HealthSlider.value = player2Health; // Update slider value based on health

            // Update health text
            player2HealthText.text = "Player 2: " + player2Health + "%";
        }

        // Check if any player's health has reached 0 and declare the winner
        if (player1Health <= 0)
        {
            resultText.text = "Player 2 Wins!";
        }
        else if (player2Health <= 0)
        {
            resultText.text = "Player 1 Wins!";
        }
    }





}
