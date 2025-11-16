//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TurompoRhythmController : MonoBehaviour
//{
//    // Player identification
//    public int playerIndex = 1;

//    // Key configuration
//    public KeyCode[] playerKeys;

//    // Note spawning
//    public float noteSpeed = 5f;
//    public float spawnRate = 1f;
//    public GameObject[] notePrefabs; // Different prefabs for different keys
//    public Transform spawnPoint;
//    public Transform targetLine;

//    // Game references
//    public TurompoController playerTorompo;

//    // Anti-spam protection
//    public float keyPressDelay = 0.3f; // Minimum time between key presses in seconds
//    private Dictionary<int, float> keyLastPressTime = new Dictionary<int, float>();

//    // AI Settings
//    [Header("AI Settings")]
//    public bool enableAI = false;
//    [Range(0f, 1f)]
//    public float aiAccuracy = 0.8f; // How accurate the AI is
//    [Range(0f, 1f)]
//    public float aiReactionSpeed = 0.7f; // How fast AI reacts
//    [Range(0f, 1f)]
//    public float aiConsistency = 0.85f; // How consistent AI performance is
//    public float aiPerfectHitRange = 0.3f; // Range for AI to consider a "perfect" hit
//    public float aiGoodHitRange = 0.6f; // Range for AI to consider a "good" hit

//    // Internal state
//    private float nextSpawnTime = 0f;
//    private List<TurompoNoteController> activeNotes = new List<TurompoNoteController>();

//    // AI State
//    private Dictionary<int, float> aiKeyLastAttempt = new Dictionary<int, float>();
//    private float aiMinTimeBetweenAttempts = 0.2f;

//    // 3D Target Highlight
//    [Header("3D Target Highlight Settings")]
//    public Color highlightColor = Color.yellow;
//    public float highlightDuration = 0.15f;

//    private Renderer targetRenderer;
//    private Color originalColor;

//    // Gabito added stuffs
//    private Coroutine spawnRoutine = null;
//    private bool isSpawning = false;



//    void Start()
//    {
//        Debug.Log("Rythm Controller Script Started (From void start)");
//        if (playerIndex == 1)
//            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
//        else
//            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

//        for (int i = 0; i < playerKeys.Length; i++)
//        {
//            keyLastPressTime[i] = -keyPressDelay;
//            aiKeyLastAttempt[i] = -aiMinTimeBetweenAttempts;
//        }

//        // Detect target line's renderer for 3D highlight
//        targetRenderer = targetLine.GetComponent<Renderer>();
//        if (targetRenderer != null)
//        {
//            originalColor = targetRenderer.material.color;
//        }
//    }

//    // Public method you call AFTER countdown ends. Also Gabito edits
//    public void BeginSpawning()
//    {
//        Debug.Log("BeginSpawning() called.");
//        if (isSpawning)
//        {
//            Debug.Log("Already spawning.");
//            return; // Already spawning — prevent duplicates
//        }

//        isSpawning = true;
//        spawnRoutine = StartCoroutine(SpawnNotesRoutine());
//    }

//    // Added by gabito
//    public void StopSpawning()
//    {
//        if (spawnRoutine != null)
//            StopCoroutine(spawnRoutine);

//        spawnRoutine = null;
//        isSpawning = false;
//    }


//    // Gabito edits
//    IEnumerator SpawnNotesRoutine()
//    {
//        Debug.Log("SpawnNotesRoutine started");
//        yield return new WaitForSeconds(1f);

//        while (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            Debug.Log("Spawning note...");
//            SpawnRandomNote();
//            yield return new WaitForSeconds(spawnRate);
//        }

//        isSpawning = false;
//        spawnRoutine = null;
//    }


//    //Edited byG Gabito
//    void Update()
//    {
//        if (TurompoGameManager.IsInputLocked)
//            return;

//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            if (enableAI)
//                HandleAIInput();
//            else
//                HandleHumanInput();
//        }
//    }


//    void HandleHumanInput()
//    {
//        // Handle key presses with anti-spam protection
//        for (int i = 0; i < playerKeys.Length; i++)
//        {
//            if (Input.GetKeyDown(playerKeys[i]))
//            {
//                if (Time.time - keyLastPressTime[i] >= keyPressDelay)
//                {
//                    keyLastPressTime[i] = Time.time;
//                    HandleKeyPress(i);
//                }
//            }
//        }
//    }

//    void HandleAIInput()
//    {
//        for (int keyIndex = 0; keyIndex < playerKeys.Length; keyIndex++)
//        {
//            if (Time.time - aiKeyLastAttempt[keyIndex] < aiMinTimeBetweenAttempts)
//                continue;

//            TurompoNoteController bestNote = FindBestNoteForKey(keyIndex);

//            if (bestNote != null)
//            {
//                float distanceToTarget = Mathf.Abs(bestNote.transform.position.y - targetLine.position.y);
//                if (ShouldAIAttemptNote(bestNote, distanceToTarget))
//                {
//                    float reactionDelay = CalculateAIReactionDelay(distanceToTarget);
//                    StartCoroutine(DelayedAIKeyPress(keyIndex, reactionDelay));
//                    aiKeyLastAttempt[keyIndex] = Time.time;
//                }
//            }
//        }
//    }

//    TurompoNoteController FindBestNoteForKey(int keyIndex)
//    {
//        TurompoNoteController bestNote = null;
//        float bestDistance = float.MaxValue;

//        foreach (TurompoNoteController note in activeNotes)
//        {
//            if (note.keyIndex == keyIndex)
//            {
//                float distanceToTarget = Mathf.Abs(note.transform.position.y - targetLine.position.y);
//                if (note.transform.position.y >= targetLine.position.y - 2f && distanceToTarget < bestDistance)
//                {
//                    bestDistance = distanceToTarget;
//                    bestNote = note;
//                }
//            }
//        }
//        return bestNote;
//    }

//    bool ShouldAIAttemptNote(TurompoNoteController note, float distanceToTarget)
//    {
//        float attemptChance = 0f;

//        if (distanceToTarget <= aiPerfectHitRange)
//            attemptChance = aiAccuracy * 0.9f;
//        else if (distanceToTarget <= aiGoodHitRange)
//            attemptChance = aiAccuracy * 0.7f;
//        else if (distanceToTarget <= 1.0f)
//            attemptChance = aiAccuracy * 0.4f;
//        else
//            attemptChance = aiAccuracy * 0.1f;

//        float consistencyRoll = Random.Range(0f, 1f);
//        if (consistencyRoll > aiConsistency)
//            attemptChance *= 0.5f;

//        return Random.Range(0f, 1f) < attemptChance;
//    }

//    float CalculateAIReactionDelay(float distanceToTarget)
//    {
//        float baseDelay = (1f - aiReactionSpeed) * 0.3f;
//        float distanceDelay = distanceToTarget * 0.05f;
//        float randomDelay = Random.Range(-0.05f, 0.1f);
//        return Mathf.Max(0f, baseDelay + distanceDelay + randomDelay);
//    }

//    IEnumerator DelayedAIKeyPress(int keyIndex, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            HandleKeyPress(keyIndex);
//        }
//    }

//    void SpawnRandomNote()
//    {
//        int keyIndex = Random.Range(0, playerKeys.Length);
//        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
//        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

//        note.keyIndex = keyIndex;
//        note.speed = noteSpeed;
//        note.targetPosition = targetLine.position;
//        note.rhythmController = this;

//        activeNotes.Add(note);
//    }

//    void HandleKeyPress(int keyIndex)
//    {
//        TurompoNoteController closestNote = null;
//        float closestDistance = float.MaxValue;

//        foreach (TurompoNoteController note in activeNotes)
//        {
//            if (note.keyIndex == keyIndex)
//            {
//                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestNote = note;
//                }
//            }
//        }

//        if (closestNote != null && closestDistance < 1.0f)
//        {
//            int scoreAmount = CalculateScore(closestDistance);
//            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);

//            Vector3 collisionPoint = closestNote.transform.position;
//            playerTorompo.BoostSpinWithCollision(collisionPoint);

//            activeNotes.Remove(closestNote);
//            Destroy(closestNote.gameObject);

//            // 🔥 Trigger 3D highlight
//            StartCoroutine(HighlightTarget3D());

//            if (enableAI)
//                NotifyAISystem(true);
//        }
//        else
//        {
//            playerTorompo.MissedMatch();
//            if (enableAI)
//                NotifyAISystem(false);
//        }
//    }

//    void NotifyAISystem(bool wasHit)
//    {
//        TurompoAIController aiController = FindObjectOfType<TurompoAIController>();
//        if (aiController != null && aiController.aiPlayerIndex == playerIndex)
//        {
//            if (wasHit)
//                aiController.RegisterAIHit();
//            else
//                aiController.RegisterAIMiss();
//        }
//    }

//    int CalculateScore(float distance)
//    {
//        if (distance < 0.2f)
//            return 100; // Perfect
//        else if (distance < 0.5f)
//            return 50;  // Good
//        else
//            return 25;  // Okay
//    }

//    public void NoteMissed(TurompoNoteController note)
//    {
//        playerTorompo.MissedMatch();
//    }

//    public void RemoveNote(TurompoNoteController note)
//    {
//        if (activeNotes.Contains(note))
//            activeNotes.Remove(note);
//    }

//    public void ClearAllNotes()
//    {
//        foreach (var note in new List<TurompoNoteController>(activeNotes))
//        {
//            if (note != null)
//                Destroy(note.gameObject);
//        }
//        activeNotes.Clear();
//    }

//    public void AIKeyPress(int keyIndex)
//    {
//        if (enableAI && keyIndex >= 0 && keyIndex < playerKeys.Length)
//        {
//            HandleKeyPress(keyIndex);
//        }
//    }

//    public List<TurompoNoteController> GetActiveNotes()
//    {
//        return new List<TurompoNoteController>(activeNotes);
//    }

//    public Vector3 GetTargetLinePosition()
//    {
//        return targetLine.position;
//    }

//    public void SetAIEnabled(bool enabled)
//    {
//        enableAI = enabled;
//    }

//    public void SetAIDifficulty(float accuracy, float reactionSpeed, float consistency)
//    {
//        aiAccuracy = Mathf.Clamp01(accuracy);
//        aiReactionSpeed = Mathf.Clamp01(reactionSpeed);
//        aiConsistency = Mathf.Clamp01(consistency);
//    }

//    // 🔥 3D Highlight Coroutine
//    IEnumerator HighlightTarget3D()
//    {
//        if (targetRenderer != null)
//        {
//            targetRenderer.material.color = highlightColor;
//            yield return new WaitForSeconds(highlightDuration);
//            targetRenderer.material.color = originalColor;
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurompoRhythmController : MonoBehaviour
{
    // Player identification
    public int playerIndex = 1;

    // Key configuration
    public KeyCode[] playerKeys;

    // Note spawning
    public float noteSpeed = 5f;
    public float spawnRate = 1f;
    public GameObject[] notePrefabs;
    public Transform spawnPoint;
    public Transform targetLine;

    // Game references
    public TurompoController playerTorompo;

    // Anti-spam protection
    public float keyPressDelay = 0.3f;
    private Dictionary<int, float> keyLastPressTime = new Dictionary<int, float>();

    // AI Settings
    [Header("AI Settings")]
    public bool enableAI = false;
    [Range(0f, 1f)] public float aiAccuracy = 0.8f;
    [Range(0f, 1f)] public float aiReactionSpeed = 0.7f;
    [Range(0f, 1f)] public float aiConsistency = 0.85f;
    public float aiPerfectHitRange = 0.3f;
    public float aiGoodHitRange = 0.6f;

    // Internal state
    private float nextSpawnTime = 0f;
    private List<TurompoNoteController> activeNotes = new List<TurompoNoteController>();

    // AI State
    private Dictionary<int, float> aiKeyLastAttempt = new Dictionary<int, float>();
    private float aiMinTimeBetweenAttempts = 0.2f;

    // 3D Target Highlight
    [Header("3D Target Highlight Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightDuration = 0.15f;

    private Renderer targetRenderer;
    private Color originalColor;

    // Gabito added
    private Coroutine spawnRoutine = null;
    private bool isSpawning = false;

    // FEEDBACK UI
    [Header("Match Feedback UI")]
    public TextMeshProUGUI feedbackText;
    public float feedbackDuration = 0.5f;
    private Coroutine feedbackRoutine;


    void Start()
    {
        if (playerIndex == 1)
            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        else
            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

        for (int i = 0; i < playerKeys.Length; i++)
        {
            keyLastPressTime[i] = -keyPressDelay;
            aiKeyLastAttempt[i] = -aiMinTimeBetweenAttempts;
        }

        targetRenderer = targetLine.GetComponent<Renderer>();
        if (targetRenderer != null)
            originalColor = targetRenderer.material.color;

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    public void BeginSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnNotesRoutine());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = null;
        isSpawning = false;
    }

    IEnumerator SpawnNotesRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
        {
            SpawnRandomNote();
            yield return new WaitForSeconds(spawnRate);
        }

        isSpawning = false;
        spawnRoutine = null;
    }

    void Update()
    {
        if (TurompoGameManager.IsInputLocked)
            return;

        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
        {
            if (enableAI)
                HandleAIInput();
            else
                HandleHumanInput();
        }
    }

    void HandleHumanInput()
    {
        for (int i = 0; i < playerKeys.Length; i++)
        {
            if (Input.GetKeyDown(playerKeys[i]))
            {
                if (Time.time - keyLastPressTime[i] >= keyPressDelay)
                {
                    keyLastPressTime[i] = Time.time;
                    HandleKeyPress(i);
                }
            }
        }
    }

    void HandleAIInput()
    {
        for (int keyIndex = 0; keyIndex < playerKeys.Length; keyIndex++)
        {
            if (Time.time - aiKeyLastAttempt[keyIndex] < aiMinTimeBetweenAttempts)
                continue;

            TurompoNoteController bestNote = FindBestNoteForKey(keyIndex);

            if (bestNote != null)
            {
                float distanceToTarget = Mathf.Abs(bestNote.transform.position.y - targetLine.position.y);

                if (ShouldAIAttemptNote(bestNote, distanceToTarget))
                {
                    float reactionDelay = CalculateAIReactionDelay(distanceToTarget);
                    StartCoroutine(DelayedAIKeyPress(keyIndex, reactionDelay));
                    aiKeyLastAttempt[keyIndex] = Time.time;
                }
            }
        }
    }

    TurompoNoteController FindBestNoteForKey(int keyIndex)
    {
        TurompoNoteController bestNote = null;
        float bestDistance = float.MaxValue;

        foreach (var note in activeNotes)
        {
            if (note.keyIndex == keyIndex)
            {
                float distanceToTarget = Mathf.Abs(note.transform.position.y - targetLine.position.y);

                if (note.transform.position.y >= targetLine.position.y - 2f &&
                    distanceToTarget < bestDistance)
                {
                    bestDistance = distanceToTarget;
                    bestNote = note;
                }
            }
        }
        return bestNote;
    }

    bool ShouldAIAttemptNote(TurompoNoteController note, float distanceToTarget)
    {
        float attemptChance = 0f;

        if (distanceToTarget <= aiPerfectHitRange) attemptChance = aiAccuracy * 0.9f;
        else if (distanceToTarget <= aiGoodHitRange) attemptChance = aiAccuracy * 0.7f;
        else if (distanceToTarget <= 1f) attemptChance = aiAccuracy * 0.4f;
        else attemptChance = aiAccuracy * 0.1f;

        float consistencyRoll = Random.Range(0f, 1f);
        if (consistencyRoll > aiConsistency)
            attemptChance *= 0.5f;

        return Random.Range(0f, 1f) < attemptChance;
    }

    float CalculateAIReactionDelay(float distanceToTarget)
    {
        float baseDelay = (1f - aiReactionSpeed) * 0.3f;
        float distanceDelay = distanceToTarget * 0.05f;
        float randomDelay = Random.Range(-0.05f, 0.1f);
        return Mathf.Max(0f, baseDelay + distanceDelay + randomDelay);
    }

    IEnumerator DelayedAIKeyPress(int keyIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
            HandleKeyPress(keyIndex);
    }

    void SpawnRandomNote()
    {
        int keyIndex = Random.Range(0, playerKeys.Length);

        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

        note.keyIndex = keyIndex;
        note.speed = noteSpeed;
        note.targetPosition = targetLine.position;
        note.rhythmController = this;

        activeNotes.Add(note);
    }

    void HandleKeyPress(int keyIndex)
    {
        TurompoNoteController closestNote = null;
        float closestDistance = float.MaxValue;

        foreach (TurompoNoteController note in activeNotes)
        {
            if (note.keyIndex == keyIndex)
            {
                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNote = note;
                }
            }
        }

        if (closestNote != null && closestDistance < 1.0f)
        {
            int scoreAmount = CalculateScore(closestDistance);
            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);

            Vector3 collisionPoint = closestNote.transform.position;
            playerTorompo.BoostSpinWithCollision(collisionPoint);

            activeNotes.Remove(closestNote);
            Destroy(closestNote.gameObject);

            StartCoroutine(HighlightTarget3D());

            if (enableAI)
                NotifyAISystem(true);
        }
        else
        {
            playerTorompo.MissedMatch(); // NO FEEDBACK DISPLAY
            if (enableAI)
                NotifyAISystem(false);
        }
    }

    void NotifyAISystem(bool wasHit)
    {
        TurompoAIController aiController = FindObjectOfType<TurompoAIController>();
        if (aiController != null && aiController.aiPlayerIndex == playerIndex)
        {
            if (wasHit) aiController.RegisterAIHit();
            else aiController.RegisterAIMiss();
        }
    }

    int CalculateScore(float distance)
    {
        if (distance < 0.2f)
        {
            ShowFeedback("PERFECT!", Color.yellow);
            return 100;
        }
        else if (distance < 0.5f)
        {
            ShowFeedback("GOOD!", Color.green);
            return 50;
        }
        else
        {
            ShowFeedback("POOR!", Color.cyan);
            return 25;
        }
    }

    public void NoteMissed(TurompoNoteController note)
    {
        // Removed MISSED feedback
        playerTorompo.MissedMatch();
    }

    public void RemoveNote(TurompoNoteController note)
    {
        if (activeNotes.Contains(note))
            activeNotes.Remove(note);
    }

    public void ClearAllNotes()
    {
        foreach (var note in new List<TurompoNoteController>(activeNotes))
        {
            if (note != null)
                Destroy(note.gameObject);
        }
        activeNotes.Clear();
    }

    public void AIKeyPress(int keyIndex)
    {
        if (enableAI && keyIndex >= 0 && keyIndex < playerKeys.Length)
            HandleKeyPress(keyIndex);
    }

    public List<TurompoNoteController> GetActiveNotes()
    {
        return new List<TurompoNoteController>(activeNotes);
    }

    public Vector3 GetTargetLinePosition()
    {
        return targetLine.position;
    }

    public void SetAIEnabled(bool enabled)
    {
        enableAI = enabled;
    }

    public void SetAIDifficulty(float accuracy, float reactionSpeed, float consistency)
    {
        aiAccuracy = Mathf.Clamp01(accuracy);
        aiReactionSpeed = Mathf.Clamp01(reactionSpeed);
        aiConsistency = Mathf.Clamp01(consistency);
    }

    IEnumerator HighlightTarget3D()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = highlightColor;
            yield return new WaitForSeconds(highlightDuration);
            targetRenderer.material.color = originalColor;
        }
    }

    // FEEDBACK POPUP
    void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(ShowFeedbackRoutine(message, color));
    }

    IEnumerator ShowFeedbackRoutine(string message, Color color)
    {
        feedbackText.text = message;

        color.a = 1;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(feedbackDuration);

        float t = 0;
        Color start = color;
        Color end = new Color(color.r, color.g, color.b, 0);

        while (t < 1f)
        {
            feedbackText.color = Color.Lerp(start, end, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }

        feedbackText.gameObject.SetActive(false);
    }
}
