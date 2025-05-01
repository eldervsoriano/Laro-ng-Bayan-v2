//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class SpiderGameManager : MonoBehaviour
//{
//    public static SpiderGameManager Instance;

//    // Player Input Managers
//    public PlayerInputManager player1Input;
//    public PlayerInputManager player2Input;

//    // Spider Animation Controllers
//    public SpiderAnimationController player1Spider;
//    public SpiderAnimationController player2Spider;

//    // UI Elements
//    public TextMeshProUGUI resultText;
//    public TextMeshProUGUI roundText;
//    public Image player1HealthImage;
//    public Image player2HealthImage;
//    public TextMeshProUGUI player1HealthText;
//    public TextMeshProUGUI player2HealthText;
//    public GameObject winnerPanel;
//    public TextMeshProUGUI winnerText;
//    public TextMeshProUGUI roundWinnerText;

//    // Editable values in Inspector
//    [Header("Player Settings")]
//    [SerializeField] private int player1Health = 100;
//    [SerializeField] private int player2Health = 100;
//    [SerializeField] private int damagePerAttack = 20;
//    [SerializeField] private float winnerDelay = 2f;
//    [SerializeField] private float animationDelay = 0.5f; // Delay for smoother animation timing
//    [SerializeField] private float deathAnimationDelay = 1.0f; // Delay before playing death animation
//    [SerializeField] private float drawDelay = 2.0f; // Delay before starting new round after a draw

//    private bool gameOver = false;
//    private int round = 1;
//    private float timeToChoose = 5f;
//    private float timer;
//    private bool isSelecting = true;
//    private bool isProcessingRound = false;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        // Set initial health values
//        player1HealthImage.fillAmount = player1Health / 100f;
//        player2HealthImage.fillAmount = player2Health / 100f;
//        player1HealthText.text = "Player 1 Health: " + player1Health;
//        player2HealthText.text = "Player 2 Health: " + player2Health;

//        winnerPanel.SetActive(false);
//        timer = timeToChoose;

//        // Hide result text at the start
//        resultText.gameObject.SetActive(false);
//        roundWinnerText.gameObject.SetActive(false);

//        // Ensure spiders are in idle animations to start
//        if (player1Spider != null) player1Spider.ResetToIdle();
//        if (player2Spider != null) player2Spider.ResetToIdle();
//    }

//    void Update()
//    {
//        if (!gameOver && isSelecting)
//        {
//            if (timer > 0f)
//            {
//                timer -= Time.deltaTime;
//                // Only show time remaining during selection phase
//                resultText.gameObject.SetActive(true);
//                resultText.text = "Time: " + Mathf.Ceil(timer).ToString();
//            }
//            else
//            {
//                StopSelecting();
//            }
//        }

//        roundText.text = "Round: " + round;
//    }

//    // Method to compare selections after players make their choice
//    public void CompareSelections()
//    {
//        if (gameOver || isProcessingRound) return;

//        isProcessingRound = true;

//        // Hide the time display text
//        resultText.gameObject.SetActive(false);

//        int player1Choice = player1Input.GetPlayerChoice();
//        int player2Choice = player2Input.GetPlayerChoice();

//        // Note: The choices are now revealed when StopSelecting() is called
//        // so no need to call RevealChoice() here

//        string result = "It's a draw!";

//        // Compare choices and play appropriate animations with small delay
//        if (player1Choice == player2Choice)
//        {
//            result = "It's a draw!";
//            // No animations for a draw, spiders remain in idle

//            // Show result text after a short delay
//            Invoke("ShowResultText", animationDelay);

//            // FIX: Add a call to start a new round after a short delay
//            Invoke("StartNewRound", drawDelay);
//        }
//        else if ((player1Choice == 1 && player2Choice == 3) || // Rock beats Scissors
//                 (player1Choice == 2 && player2Choice == 1) || // Paper beats Rock
//                 (player1Choice == 3 && player2Choice == 2))   // Scissors beats Paper
//        {
//            result = "Player 1 Wins Round " + round + "!";

//            // Player 1 attacks
//            Invoke("PlayPlayer1AttackAnimation", animationDelay);

//            // Player 2 takes damage after a short delay
//            Invoke("PlayPlayer2DamageAnimation", animationDelay * 2);

//            // Player 2 takes damage
//            Invoke("Player2TakesDamage", animationDelay * 3);

//            // Show result text after animations start
//            Invoke("ShowResultText", animationDelay * 2);
//        }
//        else
//        {
//            result = "Player 2 Wins Round " + round + "!";

//            // Player 2 attacks
//            Invoke("PlayPlayer2AttackAnimation", animationDelay);

//            // Player 1 takes damage after a short delay
//            Invoke("PlayPlayer1DamageAnimation", animationDelay * 2);

//            // Player 1 takes damage
//            Invoke("Player1TakesDamage", animationDelay * 3);

//            // Show result text after animations start
//            Invoke("ShowResultText", animationDelay * 2);
//        }

//        // Store the result but don't show it yet
//        resultText.text = result;
//        roundWinnerText.text = result;
//    }

//    // Method to show result text at the appropriate time
//    private void ShowResultText()
//    {
//        resultText.gameObject.SetActive(true);
//        roundWinnerText.gameObject.SetActive(true);
//    }

//    // Animation trigger methods
//    private void PlayPlayer1AttackAnimation()
//    {
//        if (player1Spider != null) player1Spider.PlayAttackAnimation();
//    }

//    private void PlayPlayer2AttackAnimation()
//    {
//        if (player2Spider != null) player2Spider.PlayAttackAnimation();
//    }

//    private void PlayPlayer1DamageAnimation()
//    {
//        if (player1Spider != null) player1Spider.PlayDamageTakenAnimation();
//    }

//    private void PlayPlayer2DamageAnimation()
//    {
//        if (player2Spider != null) player2Spider.PlayDamageTakenAnimation();
//    }

//    private void PlayPlayer1DeathAnimation()
//    {
//        if (player1Spider != null) player1Spider.PlayDeathAnimation();
//    }

//    private void PlayPlayer2DeathAnimation()
//    {
//        if (player2Spider != null) player2Spider.PlayDeathAnimation();
//    }

//    // Damage handlers with animation synchronization
//    private void Player1TakesDamage()
//    {
//        TakeDamage(1);
//        CheckGameOver();
//    }

//    private void Player2TakesDamage()
//    {
//        TakeDamage(2);
//        CheckGameOver();
//    }

//    // Check if game is over
//    private void CheckGameOver()
//    {
//        if (player1Health <= 0)
//        {
//            // Play death animation for player 1 spider
//            Invoke("PlayPlayer1DeathAnimation", deathAnimationDelay);

//            // Show winner panel after death animation has time to play
//            Invoke("ShowWinnerPanelWithDelay", winnerDelay + deathAnimationDelay);
//            winnerText.text = "Player 1 Loses the Game!";
//            gameOver = true;
//        }
//        else if (player2Health <= 0)
//        {
//            // Play death animation for player 2 spider
//            Invoke("PlayPlayer2DeathAnimation", deathAnimationDelay);

//            // Show winner panel after death animation has time to play
//            Invoke("ShowWinnerPanelWithDelay", winnerDelay + deathAnimationDelay);
//            winnerText.text = "Player 2 Loses the Game!";
//            gameOver = true;
//        }
//        else
//        {
//            // Proceed to the next round after animations finish
//            Invoke("StartNewRound", 3f);  // Increased delay to account for animations
//        }
//    }

//    // Method to start a new round
//    void StartNewRound()
//    {
//        if (gameOver) return;

//        // Reset player selections
//        player1Input.ResetSelection();
//        player2Input.ResetSelection();

//        // Hide result texts for new round
//        resultText.gameObject.SetActive(false);
//        roundWinnerText.gameObject.SetActive(false);

//        // Reset animations to idle
//        if (player1Spider != null && !player1Spider.IsDead()) player1Spider.ResetToIdle();
//        if (player2Spider != null && !player2Spider.IsDead()) player2Spider.ResetToIdle();

//        // Reset the timer for the new round
//        timer = timeToChoose;

//        // Increment round number
//        round++;

//        // Re-enable player selection
//        isSelecting = true;
//        isProcessingRound = false;
//        player1Input.StartSelecting();
//        player2Input.StartSelecting();
//    }

//    // Method to reduce the player's health when they take damage
//    public void TakeDamage(int player)
//    {
//        if (player == 1)
//        {
//            player1Health -= damagePerAttack;
//            player1Health = Mathf.Clamp(player1Health, 0, 100);

//            player1HealthImage.fillAmount = player1Health / 100f;
//            player1HealthText.text = "Player 1 Health: " + player1Health;
//        }
//        else if (player == 2)
//        {
//            player2Health -= damagePerAttack;
//            player2Health = Mathf.Clamp(player2Health, 0, 100);

//            player2HealthImage.fillAmount = player2Health / 100f;
//            player2HealthText.text = "Player 2 Health: " + player2Health;
//        }
//    }

//    // Stop the player's ability to select after the time runs out
//    public void StopSelecting()
//    {
//        if (!isSelecting) return;

//        isSelecting = false;
//        player1Input.StopSelecting();
//        player2Input.StopSelecting();

//        // Compare selections after both players have made their choices
//        CompareSelections();
//    }

//    // Method to show the winner panel with a delay
//    private void ShowWinnerPanelWithDelay()
//    {
//        winnerPanel.SetActive(true);
//    }

//    // Reset the entire game (call this from a button click)
//    public void ResetGame()
//    {
//        // Reset health
//        player1Health = 100;
//        player2Health = 100;

//        // Reset UI
//        player1HealthImage.fillAmount = player1Health / 100f;
//        player2HealthImage.fillAmount = player2Health / 100f;
//        player1HealthText.text = "Player 1 Health: " + player1Health;
//        player2HealthText.text = "Player 2 Health: " + player2Health;

//        // Hide result texts
//        resultText.gameObject.SetActive(false);
//        roundWinnerText.gameObject.SetActive(false);

//        // Reset round
//        round = 1;

//        // Reset game state
//        gameOver = false;
//        isProcessingRound = false;

//        // Reset animations
//        if (player1Spider != null) player1Spider.ResetToIdle();
//        if (player2Spider != null) player2Spider.ResetToIdle();

//        // Hide winner panel
//        winnerPanel.SetActive(false);

//        // Reset player selections
//        player1Input.ResetSelection();
//        player2Input.ResetSelection();

//        // Start new round
//        timer = timeToChoose;
//        isSelecting = true;
//        player1Input.StartSelecting();
//        player2Input.StartSelecting();
//    }
//}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpiderGameManager : MonoBehaviour
{
    public static SpiderGameManager Instance;

    // Player Input Managers
    public PlayerInputManager player1Input;
    public PlayerInputManager player2Input;

    // Spider Animation Controllers
    public SpiderAnimationController player1Spider;
    public SpiderAnimationController player2Spider;

    // UI Elements
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI roundText;
    public Image player1HealthImage;
    public Image player2HealthImage;
    public TextMeshProUGUI player1HealthText;
    public TextMeshProUGUI player2HealthText;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI roundWinnerText;

    // Editable values in Inspector
    [Header("Player Settings")]
    [SerializeField] private int player1Health = 100;
    [SerializeField] private int player2Health = 100;
    [SerializeField] private int damagePerAttack = 20;
    [SerializeField] private float winnerDelay = 2f;
    [SerializeField] private float animationDelay = 0.5f; // Delay for smoother animation timing
    [SerializeField] private float deathAnimationDelay = 1.0f; // Delay before playing death animation
    [SerializeField] private float drawDelay = 2.0f; // Delay before starting new round after a draw

    private bool gameOver = false;
    private int round = 1;
    private float timeToChoose = 5f;
    private float timer;
    private bool isSelecting = true;
    private bool isProcessingRound = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Set initial health values
        player1HealthImage.fillAmount = player1Health / 100f;
        player2HealthImage.fillAmount = player2Health / 100f;
        player1HealthText.text = "Player 1 Health: " + player1Health;
        player2HealthText.text = "Player 2 Health: " + player2Health;

        winnerPanel.SetActive(false);
        timer = timeToChoose;

        // Hide result text at the start
        resultText.gameObject.SetActive(false);
        roundWinnerText.gameObject.SetActive(false);

        // Ensure spiders are in idle animations to start
        if (player1Spider != null) player1Spider.ResetToIdle();
        if (player2Spider != null) player2Spider.ResetToIdle();
    }

    void Update()
    {
        if (!gameOver && isSelecting)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                // Only show time remaining during selection phase
                resultText.gameObject.SetActive(true);
                resultText.text = "Time: " + Mathf.Ceil(timer).ToString();
            }
            else
            {
                StopSelecting();
            }
        }

        roundText.text = "Round: " + round;
    }

    // Method to compare selections after players make their choice
    public void CompareSelections()
    {
        if (gameOver || isProcessingRound) return;

        isProcessingRound = true;

        // Hide the time display text
        resultText.gameObject.SetActive(false);

        int player1Choice = player1Input.GetPlayerChoice();
        int player2Choice = player2Input.GetPlayerChoice();

        // Note: The choices are now revealed when StopSelecting() is called
        // so no need to call RevealChoice() here

        string result = "It's a draw!";

        // Fix: Check if either player didn't make a choice (choice = 0)
        if (player1Choice == 0 && player2Choice == 0)
        {
            // Both players didn't choose - draw
            result = "Neither player chose - It's a draw!";
            Invoke("ShowResultText", animationDelay);
            Invoke("StartNewRound", drawDelay);
        }
        else if (player1Choice == 0)
        {
            // Player 1 didn't choose - Player 2 wins by default
            result = "Player 1 didn't choose - Player 2 Wins Round " + round + "!";

            // Player 2 attacks
            Invoke("PlayPlayer2AttackAnimation", animationDelay);

            // Player 1 takes damage after a short delay
            Invoke("PlayPlayer1DamageAnimation", animationDelay * 2);

            // Player 1 takes damage
            Invoke("Player1TakesDamage", animationDelay * 3);

            // Show result text after animations start
            Invoke("ShowResultText", animationDelay * 2);
        }
        else if (player2Choice == 0)
        {
            // Player 2 didn't choose - Player 1 wins by default
            result = "Player 2 didn't choose - Player 1 Wins Round " + round + "!";

            // Player 1 attacks
            Invoke("PlayPlayer1AttackAnimation", animationDelay);

            // Player 2 takes damage after a short delay
            Invoke("PlayPlayer2DamageAnimation", animationDelay * 2);

            // Player 2 takes damage
            Invoke("Player2TakesDamage", animationDelay * 3);

            // Show result text after animations start
            Invoke("ShowResultText", animationDelay * 2);
        }
        // Compare choices only if both players made a selection
        else if (player1Choice == player2Choice)
        {
            result = "It's a draw!";
            // No animations for a draw, spiders remain in idle

            // Show result text after a short delay
            Invoke("ShowResultText", animationDelay);

            // Start a new round after a short delay
            Invoke("StartNewRound", drawDelay);
        }
        else if ((player1Choice == 1 && player2Choice == 3) || // Rock beats Scissors
                 (player1Choice == 2 && player2Choice == 1) || // Paper beats Rock
                 (player1Choice == 3 && player2Choice == 2))   // Scissors beats Paper
        {
            result = "Player 1 Wins Round " + round + "!";

            // Player 1 attacks
            Invoke("PlayPlayer1AttackAnimation", animationDelay);

            // Player 2 takes damage after a short delay
            Invoke("PlayPlayer2DamageAnimation", animationDelay * 2);

            // Player 2 takes damage
            Invoke("Player2TakesDamage", animationDelay * 3);

            // Show result text after animations start
            Invoke("ShowResultText", animationDelay * 2);
        }
        else
        {
            result = "Player 2 Wins Round " + round + "!";

            // Player 2 attacks
            Invoke("PlayPlayer2AttackAnimation", animationDelay);

            // Player 1 takes damage after a short delay
            Invoke("PlayPlayer1DamageAnimation", animationDelay * 2);

            // Player 1 takes damage
            Invoke("Player1TakesDamage", animationDelay * 3);

            // Show result text after animations start
            Invoke("ShowResultText", animationDelay * 2);
        }

        // Store the result but don't show it yet
        resultText.text = result;
        roundWinnerText.text = result;
    }

    // Method to show result text at the appropriate time
    private void ShowResultText()
    {
        resultText.gameObject.SetActive(true);
        roundWinnerText.gameObject.SetActive(true);
    }

    // Animation trigger methods
    private void PlayPlayer1AttackAnimation()
    {
        if (player1Spider != null) player1Spider.PlayAttackAnimation();
    }

    private void PlayPlayer2AttackAnimation()
    {
        if (player2Spider != null) player2Spider.PlayAttackAnimation();
    }

    private void PlayPlayer1DamageAnimation()
    {
        if (player1Spider != null) player1Spider.PlayDamageTakenAnimation();
    }

    private void PlayPlayer2DamageAnimation()
    {
        if (player2Spider != null) player2Spider.PlayDamageTakenAnimation();
    }

    private void PlayPlayer1DeathAnimation()
    {
        if (player1Spider != null) player1Spider.PlayDeathAnimation();
    }

    private void PlayPlayer2DeathAnimation()
    {
        if (player2Spider != null) player2Spider.PlayDeathAnimation();
    }

    // Damage handlers with animation synchronization
    private void Player1TakesDamage()
    {
        TakeDamage(1);
        CheckGameOver();
    }

    private void Player2TakesDamage()
    {
        TakeDamage(2);
        CheckGameOver();
    }

    // Check if game is over
    private void CheckGameOver()
    {
        if (player1Health <= 0)
        {
            // Play death animation for player 1 spider
            Invoke("PlayPlayer1DeathAnimation", deathAnimationDelay);

            // Show winner panel after death animation has time to play
            Invoke("ShowWinnerPanelWithDelay", winnerDelay + deathAnimationDelay);
            winnerText.text = "Player 1 Loses the Game!";
            gameOver = true;
        }
        else if (player2Health <= 0)
        {
            // Play death animation for player 2 spider
            Invoke("PlayPlayer2DeathAnimation", deathAnimationDelay);

            // Show winner panel after death animation has time to play
            Invoke("ShowWinnerPanelWithDelay", winnerDelay + deathAnimationDelay);
            winnerText.text = "Player 2 Loses the Game!";
            gameOver = true;
        }
        else
        {
            // Proceed to the next round after animations finish
            Invoke("StartNewRound", 3f);  // Increased delay to account for animations
        }
    }

    // Method to start a new round
    void StartNewRound()
    {
        if (gameOver) return;

        // Reset player selections
        player1Input.ResetSelection();
        player2Input.ResetSelection();

        // Hide result texts for new round
        resultText.gameObject.SetActive(false);
        roundWinnerText.gameObject.SetActive(false);

        // Reset animations to idle
        if (player1Spider != null && !player1Spider.IsDead()) player1Spider.ResetToIdle();
        if (player2Spider != null && !player2Spider.IsDead()) player2Spider.ResetToIdle();

        // Reset the timer for the new round
        timer = timeToChoose;

        // Increment round number
        round++;

        // Re-enable player selection
        isSelecting = true;
        isProcessingRound = false;
        player1Input.StartSelecting();
        player2Input.StartSelecting();
    }

    // Method to reduce the player's health when they take damage
    public void TakeDamage(int player)
    {
        if (player == 1)
        {
            player1Health -= damagePerAttack;
            player1Health = Mathf.Clamp(player1Health, 0, 100);

            player1HealthImage.fillAmount = player1Health / 100f;
            player1HealthText.text = "Player 1 Health: " + player1Health;
        }
        else if (player == 2)
        {
            player2Health -= damagePerAttack;
            player2Health = Mathf.Clamp(player2Health, 0, 100);

            player2HealthImage.fillAmount = player2Health / 100f;
            player2HealthText.text = "Player 2 Health: " + player2Health;
        }
    }

    // Stop the player's ability to select after the time runs out
    public void StopSelecting()
    {
        if (!isSelecting) return;

        isSelecting = false;
        player1Input.StopSelecting();
        player2Input.StopSelecting();

        // Compare selections after both players have made their choices
        CompareSelections();
    }

    // Method to show the winner panel with a delay
    private void ShowWinnerPanelWithDelay()
    {
        winnerPanel.SetActive(true);
    }

    // Reset the entire game (call this from a button click)
    public void ResetGame()
    {
        // Reset health
        player1Health = 100;
        player2Health = 100;

        // Reset UI
        player1HealthImage.fillAmount = player1Health / 100f;
        player2HealthImage.fillAmount = player2Health / 100f;
        player1HealthText.text = "Player 1 Health: " + player1Health;
        player2HealthText.text = "Player 2 Health: " + player2Health;

        // Hide result texts
        resultText.gameObject.SetActive(false);
        roundWinnerText.gameObject.SetActive(false);

        // Reset round
        round = 1;

        // Reset game state
        gameOver = false;
        isProcessingRound = false;

        // Reset animations
        if (player1Spider != null) player1Spider.ResetToIdle();
        if (player2Spider != null) player2Spider.ResetToIdle();

        // Hide winner panel
        winnerPanel.SetActive(false);

        // Reset player selections
        player1Input.ResetSelection();
        player2Input.ResetSelection();

        // Start new round
        timer = timeToChoose;
        isSelecting = true;
        player1Input.StartSelecting();
        player2Input.StartSelecting();
    }
}