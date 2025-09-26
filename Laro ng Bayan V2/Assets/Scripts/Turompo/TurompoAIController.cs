using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurompoAIController : MonoBehaviour
{
    [Header("AI Configuration")]
    public bool enableAI = true;
    public int aiPlayerIndex = 2; // Which player the AI controls

    [Header("AI Difficulty Settings")]
    [Range(0f, 1f)]
    public float aiAccuracy = 0.8f; // How accurate the AI is (0 = never hits, 1 = perfect)
    [Range(0f, 1f)]
    public float aiReactionSpeed = 0.7f; // How fast AI reacts (0 = very slow, 1 = instant)
    [Range(0f, 1f)]
    public float aiConsistency = 0.85f; // How consistent AI performance is

    [Header("Dynamic Difficulty")]
    public bool adaptiveDifficulty = true;
    public float difficultyAdjustmentRate = 0.1f;
    public float maxAccuracy = 0.95f;
    public float minAccuracy = 0.3f;

    [Header("AI Behavior Patterns")]
    public float burstChance = 0.3f; // Chance of AI going into burst mode
    public float burstDuration = 2f;
    public float burstAccuracyMultiplier = 1.5f;

    // References
    private TurompoRhythmController aiRhythmController;
    private TurompoPreChallenge preChallenge;
    private TurompoGameManager gameManager;

    // AI State
    private bool isInBurstMode = false;
    private float burstTimer = 0f;
    private float lastPerformanceCheck = 0f;
    private int aiHits = 0;
    private int aiMisses = 0;

    // Performance tracking
    private Queue<float> recentPerformance = new Queue<float>();
    private int maxPerformanceHistory = 10;

    void Start()
    {
        // Get references
        gameManager = TurompoGameManager.Instance;
        preChallenge = FindObjectOfType<TurompoPreChallenge>();

        // Find the AI player's rhythm controller
        TurompoRhythmController[] rhythmControllers = FindObjectsOfType<TurompoRhythmController>();
        foreach (var controller in rhythmControllers)
        {
            if (controller.playerIndex == aiPlayerIndex)
            {
                aiRhythmController = controller;
                break;
            }
        }

        if (aiRhythmController == null)
        {
            Debug.LogWarning("AI Controller: Could not find rhythm controller for AI player " + aiPlayerIndex);
        }
    }

    void Update()
    {
        if (!enableAI) return;

        // Handle burst mode
        HandleBurstMode();

        // Update adaptive difficulty
        if (adaptiveDifficulty)
        {
            UpdateAdaptiveDifficulty();
        }

        // Handle pre-challenge AI
        if (preChallenge != null && preChallenge.IsChallengeActive())
        {
            HandlePreChallengeAI();
        }

        // Handle main game AI
        if (gameManager != null && gameManager.IsGameActive())
        {
            HandleMainGameAI();
        }
    }

    void HandlePreChallengeAI()
    {
        // Get current key for AI player
        KeyCode currentKey = GetCurrentAIKey();
        if (currentKey == KeyCode.None) return;

        // Calculate AI reaction with some randomness
        float reactionDelay = (1f - aiReactionSpeed) * 0.5f + Random.Range(0f, 0.2f);

        // Check if AI should press the key
        if (Random.Range(0f, 1f) < aiAccuracy * GetCurrentAccuracyMultiplier())
        {
            StartCoroutine(DelayedKeyPress(currentKey, reactionDelay));
        }
    }

    void HandleMainGameAI()
    {
        if (aiRhythmController == null) return;

        // Find notes that the AI should try to hit
        var aiNotes = GetAITargetNotes();

        foreach (var noteInfo in aiNotes)
        {
            // Calculate if AI should attempt this note
            float distanceToTarget = noteInfo.distanceToTarget;
            float hitChance = CalculateHitChance(distanceToTarget);

            if (Random.Range(0f, 1f) < hitChance)
            {
                // Calculate reaction time based on difficulty
                float reactionTime = CalculateReactionTime(distanceToTarget);
                StartCoroutine(DelayedNoteHit(noteInfo.keyIndex, reactionTime));
            }
        }
    }

    struct NoteInfo
    {
        public int keyIndex;
        public float distanceToTarget;
        public GameObject noteObject;
    }

    List<NoteInfo> GetAITargetNotes()
    {
        var targetNotes = new List<NoteInfo>();

        // This would need to be adapted based on how you access active notes
        // You might need to make activeNotes public or add a getter method
        // For now, this is a conceptual implementation

        return targetNotes;
    }

    float CalculateHitChance(float distanceToTarget)
    {
        // Calculate base hit chance based on distance and accuracy
        float baseChance = aiAccuracy * GetCurrentAccuracyMultiplier();

        // Adjust based on distance - closer notes are easier to hit
        float distanceModifier = Mathf.Clamp01(2f - distanceToTarget);

        // Add some randomness for consistency setting
        float consistencyFactor = Random.Range(aiConsistency, 1f);

        return baseChance * distanceModifier * consistencyFactor;
    }

    float CalculateReactionTime(float distanceToTarget)
    {
        // Base reaction time based on AI speed setting
        float baseReaction = (1f - aiReactionSpeed) * 0.3f;

        // Add some randomness
        float randomFactor = Random.Range(0.8f, 1.2f);

        // Adjust based on distance - AI reacts faster to closer notes
        float distanceReaction = distanceToTarget * 0.1f;

        return (baseReaction + distanceReaction) * randomFactor;
    }

    KeyCode GetCurrentAIKey()
    {
        // This needs to access the current key from pre-challenge
        // You'll need to make currentPlayer2Key public or add a getter
        if (preChallenge != null)
        {
            // Return preChallenge.GetCurrentPlayer2Key(); // You'll need to add this method
        }
        return KeyCode.None;
    }

    IEnumerator DelayedKeyPress(KeyCode key, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Simulate key press - you'll need to modify your input handling
        // to accept AI input as well as player input
        SimulateKeyPress(key);
    }

    IEnumerator DelayedNoteHit(int keyIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Simulate key press for the specific key index
        if (aiRhythmController != null)
        {
            // You'll need to modify TurompoRhythmController to accept AI input
            SimulateNoteHit(keyIndex);
        }
    }

    void SimulateKeyPress(KeyCode key)
    {
        // This method should trigger the same logic as a real key press
        // You'll need to modify your existing key handling to accept AI input
        Debug.Log($"AI simulating key press: {key}");
    }

    void SimulateNoteHit(int keyIndex)
    {
        // This should trigger the same logic as HandleKeyPress in rhythm controller
        // You'll need to add a public method to TurompoRhythmController for AI input
        Debug.Log($"AI attempting to hit note with key index: {keyIndex}");
    }

    void HandleBurstMode()
    {
        if (isInBurstMode)
        {
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0f)
            {
                isInBurstMode = false;
            }
        }
        else
        {
            // Check if AI should enter burst mode
            if (Random.Range(0f, 1f) < burstChance * Time.deltaTime)
            {
                isInBurstMode = true;
                burstTimer = burstDuration;
            }
        }
    }

    float GetCurrentAccuracyMultiplier()
    {
        float multiplier = 1f;

        if (isInBurstMode)
        {
            multiplier *= burstAccuracyMultiplier;
        }

        return multiplier;
    }

    void UpdateAdaptiveDifficulty()
    {
        // Check performance every few seconds
        if (Time.time - lastPerformanceCheck > 3f)
        {
            lastPerformanceCheck = Time.time;

            // Calculate recent performance
            float totalAttempts = aiHits + aiMisses;
            if (totalAttempts > 0)
            {
                float performance = (float)aiHits / totalAttempts;
                recentPerformance.Enqueue(performance);

                if (recentPerformance.Count > maxPerformanceHistory)
                {
                    recentPerformance.Dequeue();
                }

                // Adjust difficulty based on average performance
                float averagePerformance = 0f;
                foreach (float perf in recentPerformance)
                {
                    averagePerformance += perf;
                }
                averagePerformance /= recentPerformance.Count;

                // Adjust AI accuracy to maintain challenge
                if (averagePerformance > 0.8f) // AI is doing too well
                {
                    aiAccuracy = Mathf.Max(minAccuracy, aiAccuracy - difficultyAdjustmentRate);
                }
                else if (averagePerformance < 0.4f) // AI is doing poorly
                {
                    aiAccuracy = Mathf.Min(maxAccuracy, aiAccuracy + difficultyAdjustmentRate);
                }
            }

            // Reset counters
            aiHits = 0;
            aiMisses = 0;
        }
    }

    public void RegisterAIHit()
    {
        aiHits++;
    }

    public void RegisterAIMiss()
    {
        aiMisses++;
    }

    // Public methods to adjust AI difficulty dynamically
    public void SetAIDifficulty(float accuracy, float reactionSpeed, float consistency)
    {
        aiAccuracy = Mathf.Clamp01(accuracy);
        aiReactionSpeed = Mathf.Clamp01(reactionSpeed);
        aiConsistency = Mathf.Clamp01(consistency);
    }

    public void EnableAI(bool enable)
    {
        enableAI = enable;
    }
}