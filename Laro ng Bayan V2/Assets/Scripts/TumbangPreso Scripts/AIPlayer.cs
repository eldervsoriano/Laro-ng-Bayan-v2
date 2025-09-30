using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour
{
    [Header("AI Settings")]
    public bool isAI = false;
    public int playerNumber = 2; // Which player this AI controls

    [Header("Throwing AI")]
    public float aimThinkTime = 1.5f; // Time AI takes to "aim"
    public float throwAccuracy = 0.8f; // 0-1, how accurate the AI is
    public float aimRandomness = 2f; // Random offset in world units

    [Header("Defense AI")]
    public float defenseReactionTime = 0.3f; // Seconds delay before AI reacts
    public float defenseAccuracy = 0.75f; // 0-1, how good AI is at timing
    public float perfectTimingWindow = 0.05f; // How close AI tries to get to target

    [Header("Difficulty Scaling")]
    public bool scaleDifficulty = true;
    public float minAccuracy = 0.5f;
    public float maxAccuracy = 0.95f;
    public int scoreThresholdForMax = 5; // Enemy score needed for max difficulty

    private SlipperThrow slipperThrow;
    private bool isThinking = false;

    void Start()
    {
        slipperThrow = GetComponent<SlipperThrow>();

        if (slipperThrow != null)
        {
            slipperThrow.enabled = !isAI; // Disable manual control if AI
        }

        if (isAI)
        {
            Debug.Log($"🤖 AI Player {playerNumber} initialized - Throw Acc: {throwAccuracy}, Defense Acc: {defenseAccuracy}");
        }
    }

    void Update()
    {
        if (!isAI) return;

        // Check if it's AI's turn to throw
        if (TumbangGameManager.Instance.GetCurrentPlayer() == playerNumber && !isThinking)
        {
            StartCoroutine(AIThrowSequence());
        }
    }

    private IEnumerator AIThrowSequence()
    {
        isThinking = true;

        Debug.Log($"🤖 AI Player {playerNumber} is thinking...");

        // Wait to simulate "thinking"
        yield return new WaitForSeconds(aimThinkTime);

        // Find the can
        CanTarget can = FindObjectOfType<CanTarget>();
        if (can == null)
        {
            Debug.LogWarning("AI: No can found!");
            isThinking = false;
            yield break;
        }

        // Calculate accuracy based on opponent's score
        float currentAccuracy = throwAccuracy;
        if (scaleDifficulty)
        {
            int opponentScore = (playerNumber == 1)
                ? TumbangGameManager.Instance.GetPlayer2Score()
                : TumbangGameManager.Instance.GetPlayer1Score();

            float difficultyProgress = Mathf.Clamp01((float)opponentScore / scoreThresholdForMax);
            currentAccuracy = Mathf.Lerp(minAccuracy, maxAccuracy, difficultyProgress);
            Debug.Log($"🎯 AI scaled throw accuracy: {currentAccuracy:F2} (opponent score: {opponentScore})");
        }

        // Calculate throw direction with some randomness
        Vector3 targetPos = can.transform.position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-aimRandomness, aimRandomness) * (1f - currentAccuracy),
            Random.Range(-aimRandomness * 0.5f, aimRandomness * 0.5f) * (1f - currentAccuracy),
            Random.Range(-aimRandomness, aimRandomness) * (1f - currentAccuracy)
        );

        Vector3 aimPoint = targetPos + randomOffset;
        Vector3 direction = (aimPoint - transform.position).normalized;
        direction.y += 0.5f; // Add arc
        direction = direction.normalized;

        Debug.Log($"🎯 AI Player {playerNumber} throwing! Accuracy: {currentAccuracy:F2}");

        // Execute throw
        Rigidbody rb = slipperThrow.rb;
        rb.isKinematic = false;
        rb.AddForce(direction * slipperThrow.throwForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.up * slipperThrow.spinSpeed, ForceMode.Impulse);

        TumbangGameManager.Instance.NotifySlipperThrown(rb);

        yield return new WaitForSeconds(0.5f);
        isThinking = false;
    }
}