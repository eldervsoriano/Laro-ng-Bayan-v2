using UnityEngine;
using System.Collections;

public class AIPlayerController : MonoBehaviour
{
    [Header("AI Settings")]
    public bool isAIEnabled = false;
    public AIPlayerDifficulty difficulty = AIPlayerDifficulty.Medium;

    [Header("Timing Settings")]
    [SerializeField] private float minDecisionTime = 1f;
    [SerializeField] private float maxDecisionTime = 4f;
    [SerializeField] private float lastSecondChance = 0.3f; // Chance to wait until last second

    [Header("Strategy Settings")]
    [SerializeField] private float randomChoiceWeight = 0.4f;
    [SerializeField] private float counterStrategyWeight = 0.4f;
    [SerializeField] private float patternFollowWeight = 0.2f;

    private PlayerInputManager playerInput;
    private int[] playerHistoryChoices = new int[5]; // Track last 5 choices of human player
    private int[] aiHistoryChoices = new int[5]; // Track AI's last 5 choices
    private int historyIndex = 0;
    private bool hasDecided = false;
    private int aiChoice = 0;

    // Strategy patterns for different difficulties
    private float[] difficultyMultipliers = new float[] { 0.6f, 0.8f, 1.0f, 1.2f }; // Easy, Medium, Hard, Expert

    void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();
        if (playerInput == null)
        {
            Debug.LogError("AIPlayerController requires a PlayerInputManager component!");
            enabled = false;
            return;
        }

        // Initialize history arrays
        for (int i = 0; i < playerHistoryChoices.Length; i++)
        {
            playerHistoryChoices[i] = 0;
            aiHistoryChoices[i] = 0;
        }
    }

    public void StartAIDecision(float timeLimit)
    {
        if (!isAIEnabled) return;

        hasDecided = false;
        aiChoice = 0;

        // Calculate when AI should make its decision
        float decisionTime = CalculateDecisionTime(timeLimit);

        StartCoroutine(MakeDecisionCoroutine(decisionTime));
    }

    private float CalculateDecisionTime(float timeLimit)
    {
        float baseTime = Random.Range(minDecisionTime, maxDecisionTime);

        // Apply difficulty modifier
        float difficultyMod = difficultyMultipliers[(int)difficulty];
        baseTime *= difficultyMod;

        // Sometimes wait until the last second for dramatic effect
        if (Random.Range(0f, 1f) < lastSecondChance)
        {
            baseTime = timeLimit - 0.5f;
        }

        // Ensure we don't exceed time limit
        return Mathf.Clamp(baseTime, 0.5f, timeLimit - 0.1f);
    }

    private IEnumerator MakeDecisionCoroutine(float decisionTime)
    {
        yield return new WaitForSeconds(decisionTime);

        if (!hasDecided)
        {
            int choice = DetermineAIChoice();
            MakeChoice(choice);
        }
    }

    private int DetermineAIChoice()
    {
        float random = Random.Range(0f, 1f);

        switch (difficulty)
        {
            case AIPlayerDifficulty.Easy:
                return DetermineEasyChoice();

            case AIPlayerDifficulty.Medium:
                return DetermineMediumChoice();

            case AIPlayerDifficulty.Hard:
                return DetermineHardChoice();

            case AIPlayerDifficulty.Expert:
                return DetermineExpertChoice();

            default:
                return Random.Range(1, 4);
        }
    }

    private int DetermineEasyChoice()
    {
        // Easy AI: Mostly random with slight bias
        int lastPlayerChoice = GetLastPlayerChoice();

        if (lastPlayerChoice == 0)
        {
            // First round or no pattern - just pick randomly
            return Random.Range(1, 4);
        }

        float rand = Random.Range(0f, 1f);

        if (rand < 0.8f)
        {
            // 80% random choice
            return Random.Range(1, 4);
        }
        else
        {
            // 20% try to counter last player choice (simple strategy)
            return GetCounterChoice(lastPlayerChoice);
        }
    }

    private int DetermineMediumChoice()
    {
        // Medium AI: Balanced approach
        int lastPlayerChoice = GetLastPlayerChoice();

        if (lastPlayerChoice == 0)
        {
            // First round - use random choice
            return Random.Range(1, 4);
        }

        float rand = Random.Range(0f, 1f);

        if (rand < 0.5f)
        {
            // 50% random
            return Random.Range(1, 4);
        }
        else if (rand < 0.8f)
        {
            // 30% counter strategy
            return DetermineCounterStrategy();
        }
        else
        {
            // 20% pattern recognition
            return DeterminePatternChoice();
        }
    }

    private int DetermineHardChoice()
    {
        // Hard AI: Strategic with good pattern recognition
        int lastPlayerChoice = GetLastPlayerChoice();

        if (lastPlayerChoice == 0)
        {
            // First round - slightly biased random choice
            // Hard AI might have slight preferences even in first round
            float firstRoundRand = Random.Range(0f, 1f);
            if (firstRoundRand < 0.4f) return 1; // Slight bias toward Rock
            else if (firstRoundRand < 0.7f) return 2; // Paper
            else return 3; // Scissors
        }

        float rand = Random.Range(0f, 1f);

        if (rand < 0.3f)
        {
            // 30% random
            return Random.Range(1, 4);
        }
        else if (rand < 0.7f)
        {
            // 40% advanced counter strategy
            return DetermineAdvancedCounterStrategy();
        }
        else
        {
            // 30% pattern prediction
            return DeterminePatternChoice();
        }
    }

    private int DetermineExpertChoice()
    {
        // Expert AI: Highly strategic with meta-game awareness
        int lastPlayerChoice = GetLastPlayerChoice();

        if (lastPlayerChoice == 0)
        {
            // First round - Expert AI uses opening theory
            // Use a strategic opening based on psychological factors
            float openingRand = Random.Range(0f, 1f);
            if (openingRand < 0.35f) return 2; // Paper (beats most common first choice: Rock)
            else if (openingRand < 0.65f) return 1; // Rock (aggressive opening)
            else return 3; // Scissors (unexpected opening)
        }

        float rand = Random.Range(0f, 1f);

        if (rand < 0.2f)
        {
            // 20% random (unpredictability)
            return Random.Range(1, 4);
        }
        else if (rand < 0.5f)
        {
            // 30% meta-strategy (counter the counter)
            return DetermineMetaStrategy();
        }
        else if (rand < 0.8f)
        {
            // 30% pattern prediction
            return DeterminePatternChoice();
        }
        else
        {
            // 20% frequency analysis
            return DetermineFrequencyBasedChoice();
        }
    }

    private int DetermineCounterStrategy()
    {
        int lastPlayerChoice = GetLastPlayerChoice();
        if (lastPlayerChoice > 0)
            return GetCounterChoice(lastPlayerChoice);
        else
            return Random.Range(1, 4);
    }

    private int DetermineAdvancedCounterStrategy()
    {
        // Look at last 2-3 moves and try to predict
        int[] recentChoices = GetRecentPlayerChoices(3);

        // If we don't have enough history, fall back to simple counter
        if (recentChoices[0] == 0)
        {
            return DetermineCounterStrategy();
        }

        int predictedChoice = PredictNextChoice(recentChoices);

        if (predictedChoice > 0)
            return GetCounterChoice(predictedChoice);
        else
            return DetermineCounterStrategy();
    }

    private int DetermineMetaStrategy()
    {
        // Try to counter what the player might expect us to do
        int lastPlayerChoice = GetLastPlayerChoice();
        if (lastPlayerChoice > 0)
        {
            // If player expects us to counter, we counter their counter
            int expectedCounter = GetCounterChoice(lastPlayerChoice);
            return GetCounterChoice(expectedCounter);
        }
        else
        {
            return Random.Range(1, 4);
        }
    }

    private int DeterminePatternChoice()
    {
        int[] recentChoices = GetRecentPlayerChoices(5);

        // Look for simple patterns
        if (HasPattern(recentChoices))
        {
            int predictedNext = PredictFromPattern(recentChoices);
            if (predictedNext > 0)
                return GetCounterChoice(predictedNext);
        }

        return Random.Range(1, 4);
    }

    private int DetermineFrequencyBasedChoice()
    {
        // Analyze player's most frequent choice and counter it
        int[] frequency = new int[4]; // Index 0 unused, 1=Rock, 2=Paper, 3=Scissors

        for (int i = 0; i < playerHistoryChoices.Length; i++)
        {
            if (playerHistoryChoices[i] > 0 && playerHistoryChoices[i] < 4)
                frequency[playerHistoryChoices[i]]++;
        }

        // Check if we have any history
        int totalChoices = frequency[1] + frequency[2] + frequency[3];
        if (totalChoices == 0)
        {
            // No history available, use random
            return Random.Range(1, 4);
        }

        // Find most frequent choice
        int mostFrequent = 1;
        for (int i = 2; i < 4; i++)
        {
            if (frequency[i] > frequency[mostFrequent])
                mostFrequent = i;
        }

        return GetCounterChoice(mostFrequent);
    }

    private int GetCounterChoice(int choice)
    {
        switch (choice)
        {
            case 1: return 2; // Rock -> Paper
            case 2: return 3; // Paper -> Scissors  
            case 3: return 1; // Scissors -> Rock
            default: return Random.Range(1, 4);
        }
    }

    private int GetLastPlayerChoice()
    {
        // Get the most recent non-zero choice
        for (int i = historyIndex - 1; i >= 0; i--)
        {
            if (playerHistoryChoices[i] > 0)
                return playerHistoryChoices[i];
        }

        // Check wrapped around entries
        for (int i = playerHistoryChoices.Length - 1; i >= historyIndex; i--)
        {
            if (playerHistoryChoices[i] > 0)
                return playerHistoryChoices[i];
        }

        return 0; // No previous choice found
    }

    private int[] GetRecentPlayerChoices(int count)
    {
        int[] recent = new int[count];
        int recentIndex = 0;

        // Get recent choices (newest first)
        for (int i = 0; i < playerHistoryChoices.Length && recentIndex < count; i++)
        {
            int checkIndex = (historyIndex - 1 - i + playerHistoryChoices.Length) % playerHistoryChoices.Length;
            if (playerHistoryChoices[checkIndex] > 0)
            {
                recent[recentIndex] = playerHistoryChoices[checkIndex];
                recentIndex++;
            }
        }

        return recent;
    }

    private int PredictNextChoice(int[] recentChoices)
    {
        if (recentChoices.Length < 2) return 0;

        // Simple pattern recognition: if last two are the same, predict same
        if (recentChoices[0] == recentChoices[1] && recentChoices[0] > 0)
        {
            return recentChoices[0];
        }

        // Look for alternating pattern
        if (recentChoices.Length >= 3 && recentChoices[0] > 0 && recentChoices[2] > 0)
        {
            if (recentChoices[0] == recentChoices[2])
            {
                return recentChoices[0]; // Predict continuation of alternating pattern
            }
        }

        return 0; // No clear pattern
    }

    private bool HasPattern(int[] choices)
    {
        if (choices.Length < 3) return false;

        // Check for repetitive patterns
        for (int i = 0; i < choices.Length - 2; i++)
        {
            if (choices[i] == choices[i + 1] && choices[i] > 0)
                return true;
        }

        return false;
    }

    private int PredictFromPattern(int[] choices)
    {
        // Simple prediction based on recent patterns
        if (choices.Length >= 2 && choices[0] > 0)
            return choices[0]; // Predict last choice will repeat

        return 0;
    }

    private void MakeChoice(int choice)
    {
        if (!hasDecided)
        {
            hasDecided = true;
            aiChoice = choice;

            // Record AI's choice in history
            aiHistoryChoices[historyIndex] = choice;

            // Make the choice through PlayerInputManager
            playerInput.SetChoice(choice);
        }
    }

    public void RecordPlayerChoice(int playerChoice)
    {
        if (isAIEnabled)
        {
            playerHistoryChoices[historyIndex] = playerChoice;
            historyIndex = (historyIndex + 1) % playerHistoryChoices.Length;
        }
    }

    public void ResetAI()
    {
        hasDecided = false;
        aiChoice = 0;
        StopAllCoroutines();
    }

    public void SetDifficulty(AIPlayerDifficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }

    public bool IsAIEnabled()
    {
        return isAIEnabled;
    }

    public void EnableAI(bool enable)
    {
        isAIEnabled = enable;
        if (!enable)
        {
            ResetAI();
        }
    }
}

public enum AIPlayerDifficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2,
    Expert = 3
}