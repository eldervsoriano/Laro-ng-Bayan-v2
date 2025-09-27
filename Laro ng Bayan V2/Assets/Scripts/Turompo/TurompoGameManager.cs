//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro; // Add TextMeshPro namespace

//public class TurompoGameManager : MonoBehaviour
//{
//    // Singleton instance
//    public static TurompoGameManager Instance { get; private set; }

//    // Player references
//    public TurompoController player1Torompo;
//    public TurompoController player2Torompo;
//    public TurompoRhythmController player1Rhythm;
//    public TurompoRhythmController player2Rhythm;

//    // UI elements - changed from Text to TextMeshProUGUI
//    public TextMeshProUGUI player1ScoreText;
//    public TextMeshProUGUI player2ScoreText;
//    public TextMeshProUGUI timerText;
//    public TextMeshProUGUI levelText;
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI winnerText;

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

//            // Prevent timer from going negative
//            if (remainingTime < 0)
//            {
//                remainingTime = 0;
//            }

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

//        // Make sure the game over panel is active
//        gameOverPanel.SetActive(true);

//        Debug.Log("Game Over: " + winner + " Wins!");

//        // Stop all coroutines to prevent further difficulty increases
//        StopAllCoroutines();
//    }

//    public void GameTimeOver()
//    {
//        if (!isGameActive) return;

//        // Set remaining time to exactly zero to ensure display shows 00:00
//        remainingTime = 0;
//        UpdateTimerUI();

//        isGameActive = false;

//        // Determine winner based on score
//        string winner;
//        int winnerIndex;

//        if (player1Score > player2Score)
//        {
//            winner = "Player 1";
//            winnerIndex = 1;
//            // Keep player 1's torompo spinning
//            if (player2Torompo != null)
//                player2Torompo.StopSpinning();
//        }
//        else if (player2Score > player1Score)
//        {
//            winner = "Player 2";
//            winnerIndex = 2;
//            // Keep player 2's torompo spinning
//            if (player1Torompo != null)
//                player1Torompo.StopSpinning();
//        }
//        else
//        {
//            winner = "DRAW!";
//            winnerIndex = 0; // No winner
//            // In case of a draw, stop both torompos
//            if (player1Torompo != null)
//                player1Torompo.StopSpinning();
//            if (player2Torompo != null)
//                player2Torompo.StopSpinning();
//        }

//        winnerText.text = winner + " Wins!";

//        // Make sure the game over panel is active
//        gameOverPanel.SetActive(true);

//        Debug.Log("Time's Up! " + winner + " Wins!");

//        // Clear all rhythm notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Stop progression
//        StopAllCoroutines();

//        // If we have a UI Manager, inform it of the game over state
//        TurompoUIManager uiManager = FindObjectOfType<TurompoUIManager>();
//        if (uiManager != null)
//        {
//            uiManager.ShowGameOver(winner + " Wins!", player1Score, player2Score);
//        }
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
//using TMPro; // Add TextMeshPro namespace

//public class TurompoGameManager : MonoBehaviour
//{
//    // Singleton instance
//    public static TurompoGameManager Instance { get; private set; }

//    // Player references
//    public TurompoController player1Torompo;
//    public TurompoController player2Torompo;
//    public TurompoRhythmController player1Rhythm;
//    public TurompoRhythmController player2Rhythm;

//    // UI elements - changed from Text to TextMeshProUGUI
//    public TextMeshProUGUI player1ScoreText;
//    public TextMeshProUGUI player2ScoreText;
//    public TextMeshProUGUI timerText;
//    public TextMeshProUGUI levelText;
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI winnerText;

//    // Pre-Challenge Integration
//    [Header("Pre-Challenge")]
//    public TurompoPreChallenge preChallenge;
//    public bool usePreChallenge = true; // Toggle to enable/disable pre-challenge

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
//        // Start with pre-challenge if enabled, otherwise start main game directly
//        if (usePreChallenge && preChallenge != null)
//        {
//            preChallenge.StartPreChallenge();
//        }
//        else
//        {
//            StartGame();
//        }
//    }

//    void Update()
//    {
//        if (isGameActive)
//        {
//            // Update timer
//            remainingTime -= Time.deltaTime;

//            // Prevent timer from going negative
//            if (remainingTime < 0)
//            {
//                remainingTime = 0;
//            }

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
//        if (player1Torompo != null)
//            player1Torompo.ResetTorompo();
//        if (player2Torompo != null)
//            player2Torompo.ResetTorompo();

//        // Clear any active notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Hide game over panel
//        if (gameOverPanel != null)
//            gameOverPanel.SetActive(false);

//        // Start the progressive difficulty system
//        StartCoroutine(ProgressiveDifficulty());
//    }

//    // Method called by pre-challenge when it wants to start the main game
//    public void StartGameFromPreChallenge()
//    {
//        StartGame();
//    }

//    public void UpdateScoreUI()
//    {
//        if (player1ScoreText != null)
//            player1ScoreText.text = "P1 Score: " + player1Score;
//        if (player2ScoreText != null)
//            player2ScoreText.text = "P2 Score: " + player2Score;
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
//        if (playerIndex == 1)
//        {
//            player1Score += points;
//        }
//        else if (playerIndex == 2)
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
//        if (winnerText != null)
//            winnerText.text = winner + " Wins!";

//        // Make sure the game over panel is active
//        if (gameOverPanel != null)
//            gameOverPanel.SetActive(true);

//        Debug.Log("Game Over: " + winner + " Wins!");

//        // Call ObjectiveManager when Turumpo finishes (GABITO ITO LANG YUNG SCRIPT KO DITO
//        if (ObjectiveManager.Instance != null)
//        {
//            ObjectiveManager.Instance.CompleteTurumpo();
//        }
//        /// GABITO, ITO LANG DINAGDAG KO

//        // Stop all coroutines to prevent further difficulty increases
//        StopAllCoroutines();
//    }

//    public void GameTimeOver()
//    {
//        if (!isGameActive) return;

//        // Set remaining time to exactly zero to ensure display shows 00:00
//        remainingTime = 0;
//        UpdateTimerUI();

//        isGameActive = false;

//        // Determine winner based on score
//        string winner;
//        int winnerIndex;

//        if (player1Score > player2Score)
//        {
//            winner = "Player 1";
//            winnerIndex = 1;
//            // Keep player 1's torompo spinning
//            if (player2Torompo != null)
//                player2Torompo.StopSpinning();
//        }
//        else if (player2Score > player1Score)
//        {
//            winner = "Player 2";
//            winnerIndex = 2;
//            // Keep player 2's torompo spinning
//            if (player1Torompo != null)
//                player1Torompo.StopSpinning();
//        }
//        else
//        {
//            winner = "DRAW!";
//            winnerIndex = 0; // No winner
//            // In case of a draw, stop both torompos
//            if (player1Torompo != null)
//                player1Torompo.StopSpinning();
//            if (player2Torompo != null)
//                player2Torompo.StopSpinning();
//        }

//        if (winnerText != null)
//            winnerText.text = winner + " Wins!";

//        // Make sure the game over panel is active
//        if (gameOverPanel != null)
//            gameOverPanel.SetActive(true);

//        Debug.Log("Time's Up! " + winner + " Wins!");

//        // Clear all rhythm notes
//        if (player1Rhythm != null)
//            player1Rhythm.ClearAllNotes();
//        if (player2Rhythm != null)
//            player2Rhythm.ClearAllNotes();

//        // Call ObjectiveManager when Turumpo finishes (GABITO ITO LANG YUNG SCRIPT KO DITO
//        if (ObjectiveManager.Instance != null)
//        {
//            ObjectiveManager.Instance.CompleteTurumpo();
//        }
//        /// GABITO, ITO LANG DINAGDAG KO

//        // Stop progression
//        StopAllCoroutines();
//    }

//    public void RestartGame()
//    {
//        // Restart with pre-challenge if enabled
//        if (usePreChallenge && preChallenge != null)
//        {
//            preChallenge.StartPreChallenge();
//        }
//        else
//        {
//            StartGame();
//        }
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




//AI



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

    // Pre-Challenge Integration
    [Header("Pre-Challenge")]
    public TurompoPreChallenge preChallenge;
    public bool usePreChallenge = true; // Toggle to enable/disable pre-challenge

    // AI Integration
    [Header("AI Settings")]
    public bool enableSinglePlayerMode = false; // Toggle for single player vs AI
    public TurompoAIController aiController;
    [Range(0f, 1f)]
    public float aiDifficultyLevel = 0.7f; // Overall AI difficulty
    public bool adaptiveAIDifficulty = true; // Should AI adapt to player performance

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

    // AI Performance tracking for adaptive difficulty
    private Queue<float> player1RecentScores = new Queue<float>();
    private Queue<float> aiRecentScores = new Queue<float>();
    private int maxScoreHistory = 10;
    private float lastDifficultyAdjustment = 0f;
    private float difficultyAdjustmentInterval = 10f;

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
        // Initialize AI system
        InitializeAISystem();

        // Start with pre-challenge if enabled, otherwise start main game directly
        if (usePreChallenge && preChallenge != null)
        {
            preChallenge.StartPreChallenge();
        }
        else
        {
            StartGame();
        }
    }

    void Update()
    {
        if (isGameActive)
        {
            // Update timer
            remainingTime -= Time.deltaTime;

            // Prevent timer from going negative
            if (remainingTime < 0)
            {
                remainingTime = 0;
            }

            UpdateTimerUI();

            // Update adaptive AI difficulty
            if (adaptiveAIDifficulty && enableSinglePlayerMode)
            {
                UpdateAdaptiveAIDifficulty();
            }

            // Check if time is up
            if (remainingTime <= 0)
            {
                GameTimeOver();
            }
        }
    }

    void InitializeAISystem()
    {
        if (aiController == null)
        {
            aiController = FindObjectOfType<TurompoAIController>();
        }

        if (enableSinglePlayerMode)
        {
            // Configure AI for single player mode
            if (aiController != null)
            {
                aiController.EnableAI(true);
                SetAIDifficultyFromLevel(aiDifficultyLevel);
            }

            // Enable AI for player 2 rhythm controller
            if (player2Rhythm != null)
            {
                player2Rhythm.SetAIEnabled(true);
                player2Rhythm.SetAIDifficulty(aiDifficultyLevel, aiDifficultyLevel, aiDifficultyLevel * 0.9f);
            }

            // Enable AI for pre-challenge if it exists
            if (preChallenge != null)
            {
                preChallenge.SetPlayer2AI(true);
                preChallenge.SetAIDifficulty(aiDifficultyLevel, aiDifficultyLevel, 0.2f);
            }
        }
        else
        {
            // Disable AI for multiplayer mode
            if (aiController != null)
            {
                aiController.EnableAI(false);
            }

            if (player2Rhythm != null)
            {
                player2Rhythm.SetAIEnabled(false);
            }

            if (preChallenge != null)
            {
                preChallenge.SetPlayer2AI(false);
            }
        }
    }

    void SetAIDifficultyFromLevel(float level)
    {
        // Convert 0-1 difficulty level to AI parameters
        float accuracy = Mathf.Lerp(0.3f, 0.95f, level);
        float reactionSpeed = Mathf.Lerp(0.4f, 0.9f, level);
        float consistency = Mathf.Lerp(0.6f, 0.95f, level);

        if (aiController != null)
        {
            aiController.SetAIDifficulty(accuracy, reactionSpeed, consistency);
        }

        if (player2Rhythm != null)
        {
            player2Rhythm.SetAIDifficulty(accuracy, reactionSpeed, consistency);
        }

        if (preChallenge != null)
        {
            preChallenge.SetAIDifficulty(level, accuracy, (1f - reactionSpeed) * 0.3f);
        }
    }

    void UpdateAdaptiveAIDifficulty()
    {
        if (Time.time - lastDifficultyAdjustment < difficultyAdjustmentInterval)
            return;

        lastDifficultyAdjustment = Time.time;

        // Compare player vs AI performance
        if (player1RecentScores.Count > 0 && aiRecentScores.Count > 0)
        {
            float playerAverage = 0f;
            float aiAverage = 0f;

            foreach (float score in player1RecentScores)
                playerAverage += score;
            playerAverage /= player1RecentScores.Count;

            foreach (float score in aiRecentScores)
                aiAverage += score;
            aiAverage /= aiRecentScores.Count;

            // Adjust AI difficulty to maintain challenge
            float performanceRatio = playerAverage / (aiAverage + 1f); // +1 to avoid division by zero

            if (performanceRatio > 1.5f) // Player is doing much better than AI
            {
                aiDifficultyLevel = Mathf.Min(1f, aiDifficultyLevel + 0.05f);
                SetAIDifficultyFromLevel(aiDifficultyLevel);
                Debug.Log($"Increased AI difficulty to {aiDifficultyLevel:F2}");
            }
            else if (performanceRatio < 0.7f) // AI is doing much better than player
            {
                aiDifficultyLevel = Mathf.Max(0.2f, aiDifficultyLevel - 0.05f);
                SetAIDifficultyFromLevel(aiDifficultyLevel);
                Debug.Log($"Decreased AI difficulty to {aiDifficultyLevel:F2}");
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

        // Clear performance history
        player1RecentScores.Clear();
        aiRecentScores.Clear();

        // Reset all difficulty parameters
        ResetDifficulty();

        // Re-initialize AI system in case settings changed
        InitializeAISystem();

        // Update all UI
        UpdateScoreUI();
        UpdateTimerUI();
        UpdateLevelUI();

        // Reset torompos
        if (player1Torompo != null)
            player1Torompo.ResetTorompo();
        if (player2Torompo != null)
            player2Torompo.ResetTorompo();

        // Clear any active notes
        if (player1Rhythm != null)
            player1Rhythm.ClearAllNotes();
        if (player2Rhythm != null)
            player2Rhythm.ClearAllNotes();

        // Hide game over panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Start the progressive difficulty system
        StartCoroutine(ProgressiveDifficulty());
    }

    // Method called by pre-challenge when it wants to start the main game
    public void StartGameFromPreChallenge()
    {
        StartGame();
    }

    public void UpdateScoreUI()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = "P1 Score: " + player1Score;

        if (player2ScoreText != null)
        {
            string player2Label = enableSinglePlayerMode ? "AI Score: " : "P2 Score: ";
            player2ScoreText.text = player2Label + player2Score;
        }
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
        if (playerIndex == 1)
        {
            player1Score += points;

            // Track performance for adaptive AI
            if (enableSinglePlayerMode && adaptiveAIDifficulty)
            {
                player1RecentScores.Enqueue(points);
                if (player1RecentScores.Count > maxScoreHistory)
                    player1RecentScores.Dequeue();
            }
        }
        else if (playerIndex == 2)
        {
            player2Score += points;

            // Track AI performance for adaptive difficulty
            if (enableSinglePlayerMode && adaptiveAIDifficulty)
            {
                aiRecentScores.Enqueue(points);
                if (aiRecentScores.Count > maxScoreHistory)
                    aiRecentScores.Dequeue();
            }
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
        string winnerName = "";
        if (winnerIndex == 1)
        {
            winnerName = "Player 1";
        }
        else if (winnerIndex == 2)
        {
            winnerName = enableSinglePlayerMode ? "AI" : "Player 2";
        }

        if (winnerText != null)
            winnerText.text = winnerName + " Wins!";

        // Make sure the game over panel is active
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Game Over: " + winnerName + " Wins!");

        // Call ObjectiveManager when Turumpo finishes (GABITO ITO LANG YUNG SCRIPT KO DITO
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteTurumpo();
        }
        /// GABITO, ITO LANG DINAGDAG KO

        // Stop all coroutines to prevent further difficulty increases
        StopAllCoroutines();
    }

    public void GameTimeOver()
    {
        if (!isGameActive) return;

        // Set remaining time to exactly zero to ensure display shows 00:00
        remainingTime = 0;
        UpdateTimerUI();

        isGameActive = false;

        // Determine winner based on score
        string winnerName;
        int winnerIndex;

        if (player1Score > player2Score)
        {
            winnerName = "Player 1";
            winnerIndex = 1;
            // Keep player 1's torompo spinning
            if (player2Torompo != null)
                player2Torompo.StopSpinning();
        }
        else if (player2Score > player1Score)
        {
            winnerName = enableSinglePlayerMode ? "AI" : "Player 2";
            winnerIndex = 2;
            // Keep player 2's torompo spinning
            if (player1Torompo != null)
                player1Torompo.StopSpinning();
        }
        else
        {
            winnerName = "DRAW!";
            winnerIndex = 0; // No winner
            // In case of a draw, stop both torompos
            if (player1Torompo != null)
                player1Torompo.StopSpinning();
            if (player2Torompo != null)
                player2Torompo.StopSpinning();
        }

        if (winnerText != null)
            winnerText.text = winnerName + " Wins!";

        // Make sure the game over panel is active
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Time's Up! " + winnerName + " Wins!");

        // Clear all rhythm notes
        if (player1Rhythm != null)
            player1Rhythm.ClearAllNotes();
        if (player2Rhythm != null)
            player2Rhythm.ClearAllNotes();

        // Call ObjectiveManager when Turumpo finishes (GABITO ITO LANG YUNG SCRIPT KO DITO
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteTurumpo();
        }
        /// GABITO, ITO LANG DINAGDAG KO

        // Stop progression
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        // Restart with pre-challenge if enabled
        if (usePreChallenge && preChallenge != null)
        {
            preChallenge.StartPreChallenge();
        }
        else
        {
            StartGame();
        }
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

            // 4. Slightly adjust AI difficulty as game progresses (if enabled)
            if (enableSinglePlayerMode && !adaptiveAIDifficulty)
            {
                // Gradually increase AI difficulty over time
                float difficultyIncrease = 0.02f; // Small increase per level
                aiDifficultyLevel = Mathf.Min(0.95f, aiDifficultyLevel + difficultyIncrease);
                SetAIDifficultyFromLevel(aiDifficultyLevel);
            }

            Debug.Log($"Difficulty increased to level {currentLevel}");
        }
    }

    // Public methods for external AI configuration
    public void SetSinglePlayerMode(bool enabled)
    {
        enableSinglePlayerMode = enabled;
        InitializeAISystem();
    }

    public void SetAIDifficultyLevel(float level)
    {
        aiDifficultyLevel = Mathf.Clamp01(level);
        SetAIDifficultyFromLevel(aiDifficultyLevel);
    }

    public void SetAdaptiveAI(bool enabled)
    {
        adaptiveAIDifficulty = enabled;
    }

    // Getter methods for AI system
    public bool IsSinglePlayerMode()
    {
        return enableSinglePlayerMode;
    }

    public float GetCurrentAIDifficulty()
    {
        return aiDifficultyLevel;
    }
}