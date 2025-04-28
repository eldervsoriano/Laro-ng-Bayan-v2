//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoGameManager : MonoBehaviour
//{
//    // Singleton instance
//    public static TurompoGameManager Instance { get; private set; }

//    // Player references
//    public TurompoController player1Torompo;
//    public TurompoController player2Torompo;
//    public TurompoRhythmController player1Rhythm;
//    public TurompoRhythmController player2Rhythm;

//    // UI elements
//    public Text player1ScoreText;
//    public Text player2ScoreText;
//    public Text timerText;
//    public Text levelText;
//    public GameObject gameOverPanel;
//    public Text winnerText;

//    // Game settings
//    [Header("Game Settings")]
//    public float gameDuration = 120f; // 2 minutes default

//    // Progression settings
//    [Header("Progression Settings")]
//    public float progressionInterval = 15f; // Time between difficulty increases
//    public float spinDecayIncrease = 2f; // How much decay rate increases per level
//    public float noteSpeedIncrease = 0.5f; // How much note speed increases per level
//    public float spawnRateDecrease = 0.1f; // How much spawn rate decreases per level

//    // Game state
//    private bool isGameActive = false;
//    private int player1Score = 0;
//    private int player2Score = 0;
//    private float remainingTime;
//    private int currentLevel = 1;

//    private void Awake()
//    {
//        // Singleton pattern
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Start()
//    {
//        StartGame();
//    }

//    void Update()
//    {
//        if (isGameActive)
//        {
//            // Update timer
//            remainingTime -= Time.deltaTime;
//            UpdateTimerUI();

//            // Check if time is up
//            if (remainingTime <= 0)
//            {
//                GameTimeOver();
//            }
//        }
//    }

//    public void StartGame()
//    {
//        isGameActive = true;
//        player1Score = 0;
//        player2Score = 0;
//        remainingTime = gameDuration;
//        currentLevel = 1;

//        // Reset all difficulty parameters
//        ResetDifficulty();

//        // Update all UI
//        UpdateScoreUI();
//        UpdateTimerUI();
//        UpdateLevelUI();

//        // Reset torompos
//        player1Torompo.ResetTorompo();
//        player2Torompo.ResetTorompo();

//        // Hide game over panel
//        gameOverPanel.SetActive(false);

//        // Start the progressive difficulty system
//        StartCoroutine(ProgressiveDifficulty());
//    }

//    public void UpdateScoreUI()
//    {
//        player1ScoreText.text = "P1 Score: " + player1Score;
//        player2ScoreText.text = "P2 Score: " + player2Score;
//    }

//    public void UpdateTimerUI()
//    {
//        if (timerText != null)
//        {
//            int minutes = Mathf.FloorToInt(remainingTime / 60);
//            int seconds = Mathf.FloorToInt(remainingTime % 60);
//            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
//        }
//    }

//    public void UpdateLevelUI()
//    {
//        if (levelText != null)
//        {
//            levelText.text = "Level: " + currentLevel;
//        }
//    }

//    public void AddScore(int playerIndex, int points)
//    {
//        if (!isGameActive) return;

//        if (playerIndex == 1)
//        {
//            player1Score += points;
//        }
//        else
//        {
//            player2Score += points;
//        }
//        UpdateScoreUI();
//    }

//    public void PlayerGameOver(int playerIndex)
//    {
//        if (!isGameActive) return;

//        isGameActive = false;
//        string winner = playerIndex == 1 ? "Player 2" : "Player 1";
//        winnerText.text = winner + " Wins!";
//        gameOverPanel.SetActive(true);

//        // Stop all coroutines to prevent further difficulty increases
//        StopAllCoroutines();
//    }

//    public void GameTimeOver()
//    {
//        if (!isGameActive) return;

//        isGameActive = false;

//        // Determine winner based on score
//        string winner;
//        if (player1Score > player2Score)
//        {
//            winner = "Player 1";
//        }
//        else if (player2Score > player1Score)
//        {
//            winner = "Player 2";
//        }
//        else
//        {
//            winner = "No one - It's a tie!";
//        }

//        winnerText.text = winner + " Wins!";
//        gameOverPanel.SetActive(true);

//        // Stop progression
//        StopAllCoroutines();
//    }

//    public void RestartGame()
//    {
//        StartGame();
//    }

//    public bool IsGameActive()
//    {
//        return isGameActive;
//    }

//    private void ResetDifficulty()
//    {
//        // Reset Torompo decay rates
//        if (player1Torompo != null)
//            player1Torompo.spinDecayRate = 10f; // Default value

//        if (player2Torompo != null)
//            player2Torompo.spinDecayRate = 10f; // Default value

//        // Reset Rhythm note speeds and spawn rates
//        if (player1Rhythm != null)
//        {
//            player1Rhythm.noteSpeed = 5f; // Default value
//            player1Rhythm.spawnRate = 1f; // Default value
//        }

//        if (player2Rhythm != null)
//        {
//            player2Rhythm.noteSpeed = 5f; // Default value
//            player2Rhythm.spawnRate = 1f; // Default value
//        }
//    }

//    private IEnumerator ProgressiveDifficulty()
//    {
//        while (isGameActive)
//        {
//            // Wait for the interval
//            yield return new WaitForSeconds(progressionInterval);

//            // Increase difficulty level
//            currentLevel++;
//            UpdateLevelUI();

//            // Increase difficulty parameters

//            // 1. Increase the spin decay rate (makes turompos slow down faster)
//            if (player1Torompo != null)
//                player1Torompo.spinDecayRate += spinDecayIncrease;

//            if (player2Torompo != null)
//                player2Torompo.spinDecayRate += spinDecayIncrease;

//            // 2. Increase note speed (makes notes fall faster)
//            if (player1Rhythm != null)
//                player1Rhythm.noteSpeed += noteSpeedIncrease;

//            if (player2Rhythm != null)
//                player2Rhythm.noteSpeed += noteSpeedIncrease;

//            // 3. Decrease spawn rate (makes notes appear more frequently)
//            if (player1Rhythm != null)
//                player1Rhythm.spawnRate = Mathf.Max(0.2f, player1Rhythm.spawnRate - spawnRateDecrease);

//            if (player2Rhythm != null)
//                player2Rhythm.spawnRate = Mathf.Max(0.2f, player2Rhythm.spawnRate - spawnRateDecrease);

//            Debug.Log($"Difficulty increased to level {currentLevel}");
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoGameManager : MonoBehaviour
//{
//    // Singleton instance
//    public static TurompoGameManager Instance { get; private set; }

//    // Player references
//    public TurompoController player1Torompo;
//    public TurompoController player2Torompo;
//    public TurompoRhythmController player1Rhythm;
//    public TurompoRhythmController player2Rhythm;

//    // UI elements
//    public Text player1ScoreText;
//    public Text player2ScoreText;
//    public Text timerText;
//    public Text levelText;
//    public GameObject gameOverPanel;
//    public Text winnerText;

//    // Game settings
//    [Header("Game Settings")]
//    public float gameDuration = 120f; // 2 minutes default

//    // Progression settings
//    [Header("Progression Settings")]
//    public float progressionInterval = 15f; // Time between difficulty increases
//    public float spinDecayIncrease = 2f; // How much decay rate increases per level
//    public float noteSpeedIncrease = 0.5f; // How much note speed increases per level
//    public float spawnRateDecrease = 0.1f; // How much spawn rate decreases per level

//    // Game state
//    private bool isGameActive = false;
//    private int player1Score = 0;
//    private int player2Score = 0;
//    private float remainingTime;
//    private int currentLevel = 1;

//    private void Awake()
//    {
//        // Singleton pattern
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Start()
//    {
//        StartGame();
//    }

//    void Update()
//    {
//        if (isGameActive)
//        {
//            // Update timer
//            remainingTime -= Time.deltaTime;
//            UpdateTimerUI();

//            // Check if time is up
//            if (remainingTime <= 0)
//            {
//                GameTimeOver();
//            }
//        }
//    }

//    public void StartGame()
//    {
//        isGameActive = true;
//        player1Score = 0;
//        player2Score = 0;
//        remainingTime = gameDuration;
//        currentLevel = 1;

//        // Reset all difficulty parameters
//        ResetDifficulty();

//        // Update all UI
//        UpdateScoreUI();
//        UpdateTimerUI();
//        UpdateLevelUI();

//        // Reset torompos
//        player1Torompo.ResetTorompo();
//        player2Torompo.ResetTorompo();

//        // Clear any active notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Hide game over panel
//        gameOverPanel.SetActive(false);

//        // Start the progressive difficulty system
//        StartCoroutine(ProgressiveDifficulty());
//    }

//    public void UpdateScoreUI()
//    {
//        player1ScoreText.text = "P1 Score: " + player1Score;
//        player2ScoreText.text = "P2 Score: " + player2Score;
//    }

//    public void UpdateTimerUI()
//    {
//        if (timerText != null)
//        {
//            int minutes = Mathf.FloorToInt(remainingTime / 60);
//            int seconds = Mathf.FloorToInt(remainingTime % 60);
//            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
//        }
//    }

//    public void UpdateLevelUI()
//    {
//        if (levelText != null)
//        {
//            levelText.text = "Level: " + currentLevel;
//        }
//    }

//    public void AddScore(int playerIndex, int points)
//    {
//        if (!isGameActive) return;

//        if (playerIndex == 1)
//        {
//            player1Score += points;
//        }
//        else
//        {
//            player2Score += points;
//        }
//        UpdateScoreUI();
//    }

//    public void PlayerGameOver(int playerIndex)
//    {
//        if (!isGameActive) return;

//        // Determine winner based on which player has stopped spinning
//        int winnerIndex = playerIndex == 1 ? 2 : 1;
//        DeclareWinner(winnerIndex);
//    }

//    public void DeclareWinner(int winnerIndex)
//    {
//        // Only process if game is still active
//        if (!isGameActive) return;

//        // Stop the game
//        isGameActive = false;

//        // Stop all torompos except the winner's
//        if (player1Torompo != null && player1Torompo.playerIndex != winnerIndex)
//        {
//            player1Torompo.StopSpinning();
//        }

//        if (player2Torompo != null && player2Torompo.playerIndex != winnerIndex)
//        {
//            player2Torompo.StopSpinning();
//        }

//        // Clear all rhythm notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Display winner message
//        string winner = "Player " + winnerIndex;
//        winnerText.text = winner + " Wins!";
//        gameOverPanel.SetActive(true);

//        // Stop all coroutines to prevent further difficulty increases
//        StopAllCoroutines();
//    }

//    public void GameTimeOver()
//    {
//        if (!isGameActive) return;

//        isGameActive = false;

//        // Determine winner based on score
//        string winner;
//        if (player1Score > player2Score)
//        {
//            winner = "Player 1";
//            // Keep player 1's torompo spinning
//            if (player2Torompo != null)
//                player2Torompo.StopSpinning();
//        }
//        else if (player2Score > player1Score)
//        {
//            winner = "Player 2";
//            // Keep player 2's torompo spinning
//            if (player1Torompo != null)
//                player1Torompo.StopSpinning();
//        }
//        else
//        {
//            winner = "No one - It's a tie!";
//            // You might want to keep both spinning or stop both
//        }

//        winnerText.text = winner + " Wins!";
//        gameOverPanel.SetActive(true);

//        // Clear all rhythm notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Stop progression
//        StopAllCoroutines();
//    }

//    public void RestartGame()
//    {
//        StartGame();
//    }

//    public bool IsGameActive()
//    {
//        return isGameActive;
//    }

//    private void ResetDifficulty()
//    {
//        // Reset Torompo decay rates
//        if (player1Torompo != null)
//            player1Torompo.spinDecayRate = 10f; // Default value

//        if (player2Torompo != null)
//            player2Torompo.spinDecayRate = 10f; // Default value

//        // Reset Rhythm note speeds and spawn rates
//        if (player1Rhythm != null)
//        {
//            player1Rhythm.noteSpeed = 5f; // Default value
//            player1Rhythm.spawnRate = 1f; // Default value
//        }

//        if (player2Rhythm != null)
//        {
//            player2Rhythm.noteSpeed = 5f; // Default value
//            player2Rhythm.spawnRate = 1f; // Default value
//        }
//    }

//    private IEnumerator ProgressiveDifficulty()
//    {
//        while (isGameActive)
//        {
//            // Wait for the interval
//            yield return new WaitForSeconds(progressionInterval);

//            // Increase difficulty level
//            currentLevel++;
//            UpdateLevelUI();

//            // Increase difficulty parameters

//            // 1. Increase the spin decay rate (makes turompos slow down faster)
//            if (player1Torompo != null)
//                player1Torompo.spinDecayRate += spinDecayIncrease;

//            if (player2Torompo != null)
//                player2Torompo.spinDecayRate += spinDecayIncrease;

//            // 2. Increase note speed (makes notes fall faster)
//            if (player1Rhythm != null)
//                player1Rhythm.noteSpeed += noteSpeedIncrease;

//            if (player2Rhythm != null)
//                player2Rhythm.noteSpeed += noteSpeedIncrease;

//            // 3. Decrease spawn rate (makes notes appear more frequently)
//            if (player1Rhythm != null)
//                player1Rhythm.spawnRate = Mathf.Max(0.2f, player1Rhythm.spawnRate - spawnRateDecrease);

//            if (player2Rhythm != null)
//                player2Rhythm.spawnRate = Mathf.Max(0.2f, player2Rhythm.spawnRate - spawnRateDecrease);

//            Debug.Log($"Difficulty increased to level {currentLevel}");
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add TextMeshPro namespace

public class TurompoGameManager : MonoBehaviour
{
    // Singleton instance
    public static TurompoGameManager Instance { get; private set; }

    // Player references
    public TurompoController player1Torompo;
    public TurompoController player2Torompo;
    public TurompoRhythmController player1Rhythm;
    public TurompoRhythmController player2Rhythm;

    // UI elements - changed from Text to TextMeshProUGUI
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;

    // Game settings
    [Header("Game Settings")]
    public float gameDuration = 120f; // 2 minutes default

    // Progression settings
    [Header("Progression Settings")]
    public float progressionInterval = 15f; // Time between difficulty increases
    public float spinDecayIncrease = 2f; // How much decay rate increases per level
    public float noteSpeedIncrease = 0.5f; // How much note speed increases per level
    public float spawnRateDecrease = 0.1f; // How much spawn rate decreases per level

    // Game state
    private bool isGameActive = false;
    private int player1Score = 0;
    private int player2Score = 0;
    private float remainingTime;
    private int currentLevel = 1;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (isGameActive)
        {
            // Update timer
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            // Check if time is up
            if (remainingTime <= 0)
            {
                GameTimeOver();
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        player1Score = 0;
        player2Score = 0;
        remainingTime = gameDuration;
        currentLevel = 1;

        // Reset all difficulty parameters
        ResetDifficulty();

        // Update all UI
        UpdateScoreUI();
        UpdateTimerUI();
        UpdateLevelUI();

        // Reset torompos
        player1Torompo.ResetTorompo();
        player2Torompo.ResetTorompo();

        // Clear any active notes
        if (player1Rhythm != null)
            player1Rhythm.ClearAllNotes();
        if (player2Rhythm != null)
            player2Rhythm.ClearAllNotes();

        // Hide game over panel
        gameOverPanel.SetActive(false);

        // Start the progressive difficulty system
        StartCoroutine(ProgressiveDifficulty());
    }

    public void UpdateScoreUI()
    {
        player1ScoreText.text = "P1 Score: " + player1Score;
        player2ScoreText.text = "P2 Score: " + player2Score;
    }

    public void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
    }

    public void AddScore(int playerIndex, int points)
    {
        if (!isGameActive) return;

        if (playerIndex == 1)
        {
            player1Score += points;
        }
        else
        {
            player2Score += points;
        }
        UpdateScoreUI();
    }

    public void PlayerGameOver(int playerIndex)
    {
        if (!isGameActive) return;

        // Determine winner based on which player has stopped spinning
        int winnerIndex = playerIndex == 1 ? 2 : 1;
        DeclareWinner(winnerIndex);
    }

    public void DeclareWinner(int winnerIndex)
    {
        // Only process if game is still active
        if (!isGameActive) return;

        // Stop the game
        isGameActive = false;

        // Stop all torompos except the winner's
        if (player1Torompo != null && player1Torompo.playerIndex != winnerIndex)
        {
            player1Torompo.StopSpinning();
        }

        if (player2Torompo != null && player2Torompo.playerIndex != winnerIndex)
        {
            player2Torompo.StopSpinning();
        }

        // Clear all rhythm notes
        if (player1Rhythm != null)
            player1Rhythm.ClearAllNotes();
        if (player2Rhythm != null)
            player2Rhythm.ClearAllNotes();

        // Display winner message
        string winner = "Player " + winnerIndex;
        winnerText.text = winner + " Wins!";
        gameOverPanel.SetActive(true);

        // Stop all coroutines to prevent further difficulty increases
        StopAllCoroutines();
    }

    public void GameTimeOver()
    {
        if (!isGameActive) return;

        isGameActive = false;

        // Determine winner based on score
        string winner;
        if (player1Score > player2Score)
        {
            winner = "Player 1";
            // Keep player 1's torompo spinning
            if (player2Torompo != null)
                player2Torompo.StopSpinning();
        }
        else if (player2Score > player1Score)
        {
            winner = "Player 2";
            // Keep player 2's torompo spinning
            if (player1Torompo != null)
                player1Torompo.StopSpinning();
        }
        else
        {
            winner = "DRAW!";
            // You might want to keep both spinning or stop both
        }

        winnerText.text = winner + " Wins!";
        gameOverPanel.SetActive(true);

        // Clear all rhythm notes
        if (player1Rhythm != null)
            player1Rhythm.ClearAllNotes();
        if (player2Rhythm != null)
            player2Rhythm.ClearAllNotes();

        // Stop progression
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        StartGame();
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    private void ResetDifficulty()
    {
        // Reset Torompo decay rates
        if (player1Torompo != null)
            player1Torompo.spinDecayRate = 10f; // Default value

        if (player2Torompo != null)
            player2Torompo.spinDecayRate = 10f; // Default value

        // Reset Rhythm note speeds and spawn rates
        if (player1Rhythm != null)
        {
            player1Rhythm.noteSpeed = 5f; // Default value
            player1Rhythm.spawnRate = 1f; // Default value
        }

        if (player2Rhythm != null)
        {
            player2Rhythm.noteSpeed = 5f; // Default value
            player2Rhythm.spawnRate = 1f; // Default value
        }
    }

    private IEnumerator ProgressiveDifficulty()
    {
        while (isGameActive)
        {
            // Wait for the interval
            yield return new WaitForSeconds(progressionInterval);

            // Increase difficulty level
            currentLevel++;
            UpdateLevelUI();

            // Increase difficulty parameters

            // 1. Increase the spin decay rate (makes turompos slow down faster)
            if (player1Torompo != null)
                player1Torompo.spinDecayRate += spinDecayIncrease;

            if (player2Torompo != null)
                player2Torompo.spinDecayRate += spinDecayIncrease;

            // 2. Increase note speed (makes notes fall faster)
            if (player1Rhythm != null)
                player1Rhythm.noteSpeed += noteSpeedIncrease;

            if (player2Rhythm != null)
                player2Rhythm.noteSpeed += noteSpeedIncrease;

            // 3. Decrease spawn rate (makes notes appear more frequently)
            if (player1Rhythm != null)
                player1Rhythm.spawnRate = Mathf.Max(0.2f, player1Rhythm.spawnRate - spawnRateDecrease);

            if (player2Rhythm != null)
                player2Rhythm.spawnRate = Mathf.Max(0.2f, player2Rhythm.spawnRate - spawnRateDecrease);

            Debug.Log($"Difficulty increased to level {currentLevel}");
        }
    }
}