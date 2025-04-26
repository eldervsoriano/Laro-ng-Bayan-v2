using UnityEngine;
using TMPro; // For displaying result and health text in TextMeshPro
using UnityEngine.UI; // For Health Bar UI

public class SpiderGameManager : MonoBehaviour
{
    public static SpiderGameManager Instance;

    // Player Input Managers
    public PlayerInputManager player1Input;
    public PlayerInputManager player2Input;

    // UI Elements
    public TextMeshProUGUI resultText;  // Result UI text for displaying winner of the round
    public TextMeshProUGUI roundText;  // Text to display the current round
    public Slider player1HealthSlider; // Player 1 health bar
    public Slider player2HealthSlider; // Player 2 health bar

    public TextMeshProUGUI player1HealthText; // Player 1 health text
    public TextMeshProUGUI player2HealthText; // Player 2 health text

    public GameObject winnerPanel; // Winner panel to display the winner
    public TextMeshProUGUI winnerText; // Text to show the winner message
    public TextMeshProUGUI roundWinnerText; // Text to display winner of each round

    // Editable values in Inspector
    [Header("Player Settings")]
    [SerializeField] private int player1Health = 100; // Player 1's starting health
    [SerializeField] private int player2Health = 100; // Player 2's starting health
    [SerializeField] private int damagePerAttack = 20; // Damage per attack (adjustable)
    [SerializeField] private float winnerDelay = 2f; // Delay time before showing winner panel (adjustable)

    private bool gameOver = false;  // Flag to track if the game is over
    private int round = 1;  // Track the current round
    private float timeToChoose = 5f;  // Time limit for making a choice (in seconds)
    private float timer;  // Timer to show countdown

    private bool isSelecting = true;  // Flag to track if players can select their moves

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Set initial health values
        player1HealthSlider.value = player1Health;
        player2HealthSlider.value = player2Health;

        // Initialize health text (whole numbers)
        player1HealthText.text = "Player 1 Health: " + player1Health;
        player2HealthText.text = "Player 2 Health: " + player2Health;

        winnerPanel.SetActive(false); // Hide winner panel at the start

        // Initialize timer for selection phase
        timer = timeToChoose;
    }

    void Update()
    {
        // Update the timer text during the selection phase
        if (!gameOver && isSelecting)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                resultText.text = "Time: " + Mathf.Ceil(timer).ToString();
            }
            else
            {
                StopSelecting();
            }
        }

        // Update round text
        roundText.text = "Round: " + round;
    }

    // Method to compare selections after players make their choice
    public void CompareSelections()
    {
        if (gameOver) return;  // If the game is over, stop comparing selections

        int player1Choice = player1Input.GetPlayerChoice();
        int player2Choice = player2Input.GetPlayerChoice();

        string result = "It's a draw!";

        // Compare choices for player 1 and player 2
        if (player1Choice == player2Choice)
        {
            result = "It's a draw!";
        }
        else if ((player1Choice == 1 && player2Choice == 3) || // Rock beats Scissors
                 (player1Choice == 2 && player2Choice == 1) || // Paper beats Rock
                 (player1Choice == 3 && player2Choice == 2))   // Scissors beats Paper
        {
            result = "Player 1 Wins Round " + round + "!";
            TakeDamage(2);  // Player 1 wins, so Player 2 takes damage
        }
        else
        {
            result = "Player 2 Wins Round " + round + "!";
            TakeDamage(1);  // Player 2 wins, so Player 1 takes damage
        }

        resultText.text = result;
        roundWinnerText.text = result;

        // Check if any player's health has reached 0 and declare the winner
        if (player1Health <= 0)
        {
            Invoke("ShowWinnerPanelWithDelay", winnerDelay);
            winnerText.text = "Player 2 Wins the Game!";
            gameOver = true; // End the game
        }
        else if (player2Health <= 0)
        {
            Invoke("ShowWinnerPanelWithDelay", winnerDelay);
            winnerText.text = "Player 1 Wins the Game!";
            gameOver = true; // End the game
        }
        else
        {
            // Proceed to the next round regardless of the result (even if it's a draw)
            Invoke("StartNewRound", 2f);  // Wait 2 seconds before allowing new round
        }
    }

    // Method to start a new round
    void StartNewRound()
    {
        if (gameOver) return;

        // Reset player selections
        player1Input.ResetSelection();
        player2Input.ResetSelection();

        // Reset the timer for the new round
        timer = timeToChoose;

        // Increment round number
        round++;

        // Re-enable player selection
        isSelecting = true;
        player1Input.StartSelecting();
        player2Input.StartSelecting();
    }

    // Method to reduce the player's health when they take damage
    public void TakeDamage(int player)
    {
        if (player == 1)
        {
            player1Health -= damagePerAttack;
            player1Health = Mathf.Clamp(player1Health, 0, 100); // Ensure health doesn't go below 0

            player1HealthSlider.value = player1Health;
            player1HealthText.text = "Player 1 Health: " + player1Health;
        }
        else if (player == 2)
        {
            player2Health -= damagePerAttack;
            player2Health = Mathf.Clamp(player2Health, 0, 100); // Ensure health doesn't go below 0

            player2HealthSlider.value = player2Health;
            player2HealthText.text = "Player 2 Health: " + player2Health;
        }
    }

    // Stop the player's ability to select after the time runs out
    public void StopSelecting()
    {
        isSelecting = false;
        player1Input.StopSelecting();
        player2Input.StopSelecting();

        // Show the players' choices after the time runs out using SetChoice
        player1Input.SetChoice(player1Input.GetPlayerChoice());
        player2Input.SetChoice(player2Input.GetPlayerChoice());

        CompareSelections();
    }

    // Method to show the winner panel with a delay
    private void ShowWinnerPanelWithDelay()
    {
        winnerPanel.SetActive(true);  // Show winner panel after delay
    }
}