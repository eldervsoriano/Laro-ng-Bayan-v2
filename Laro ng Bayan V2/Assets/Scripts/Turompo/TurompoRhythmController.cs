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

//    // Internal state
//    private float nextSpawnTime = 0f;
//    private List<TurompoNoteController> activeNotes = new List<TurompoNoteController>();

//    void Start()
//    {
//        // Set up key configurations based on player index
//        if (playerIndex == 1)
//        {
//            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
//        }
//        else
//        {
//            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
//        }

//        // Initialize the key press timers
//        for (int i = 0; i < playerKeys.Length; i++)
//        {
//            keyLastPressTime[i] = -keyPressDelay; // Allow immediate first press
//        }
//    }

//    void Update()
//    {
//        // Only process game logic if the game is active
//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            // Spawn new notes
//            if (Time.time >= nextSpawnTime)
//            {
//                SpawnRandomNote();
//                nextSpawnTime = Time.time + spawnRate;
//            }

//            // Handle key presses with anti-spam protection
//            for (int i = 0; i < playerKeys.Length; i++)
//            {
//                if (Input.GetKeyDown(playerKeys[i]))
//                {
//                    // Check if enough time has passed since the last press of this key
//                    if (Time.time - keyLastPressTime[i] >= keyPressDelay)
//                    {
//                        keyLastPressTime[i] = Time.time; // Update the last press time
//                        HandleKeyPress(i);
//                    }
//                    // If not enough time has passed, ignore this key press (anti-spam)
//                }
//            }
//        }
//    }

//    void SpawnRandomNote()
//    {
//        // Choose a random key to spawn
//        int keyIndex = Random.Range(0, playerKeys.Length);

//        // Instantiate the corresponding note
//        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
//        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

//        // Set up the note
//        note.keyIndex = keyIndex;
//        note.speed = noteSpeed;
//        note.targetPosition = targetLine.position;
//        note.rhythmController = this;

//        // Add to active notes
//        activeNotes.Add(note);
//    }

//    void HandleKeyPress(int keyIndex)
//    {
//        // Find closest note for this key
//        TurompoNoteController closestNote = null;
//        float closestDistance = float.MaxValue;

//        foreach (TurompoNoteController note in activeNotes)
//        {
//            if (note.keyIndex == keyIndex)
//            {
//                // Calculate distance based on Y position difference from target line
//                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestNote = note;
//                }
//            }
//        }

//        // Check if note is within hit range (distance to target line)
//        if (closestNote != null && closestDistance < 1.0f)
//        {
//            // Successfully hit the note
//            int scoreAmount = CalculateScore(closestDistance);
//            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);

//            // Only boost spin on successful note matches
//            playerTorompo.BoostSpin();

//            // Remove the note
//            activeNotes.Remove(closestNote);
//            Destroy(closestNote.gameObject);
//        }
//        else
//        {
//            // Missed - no matching note or too far
//            // We just report a miss but don't change the spin speed - that only happens on successful hits
//            playerTorompo.MissedMatch();
//        }
//    }

//    int CalculateScore(float distance)
//    {
//        // Calculate score based on precision
//        if (distance < 0.2f)
//            return 100; // Perfect
//        else if (distance < 0.5f)
//            return 50;  // Good
//        else
//            return 25;  // Okay
//    }

//    public void NoteMissed(TurompoNoteController note)
//    {
//        // Player missed this note as it passed the target line
//        playerTorompo.MissedMatch();
//        // Note: we don't remove the note here - it will continue falling and be destroyed by its timer
//    }

//    public void RemoveNote(TurompoNoteController note)
//    {
//        if (activeNotes.Contains(note))
//        {
//            activeNotes.Remove(note);
//        }
//    }

//    // Method to clear all notes when game ends or restarts
//    public void ClearAllNotes()
//    {
//        foreach (var note in new List<TurompoNoteController>(activeNotes))
//        {
//            if (note != null)
//            {
//                Destroy(note.gameObject);
//            }
//        }
//        activeNotes.Clear();
//    }
//}

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

//    // Internal state
//    private float nextSpawnTime = 0f;
//    private List<TurompoNoteController> activeNotes = new List<TurompoNoteController>();

//    void Start()
//    {
//        // Set up key configurations based on player index
//        if (playerIndex == 1)
//        {
//            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
//        }
//        else
//        {
//            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
//        }

//        // Initialize the key press timers
//        for (int i = 0; i < playerKeys.Length; i++)
//        {
//            keyLastPressTime[i] = -keyPressDelay; // Allow immediate first press
//        }
//    }

//    void Update()
//    {
//        // Only process game logic if the game is active
//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            // Spawn new notes
//            if (Time.time >= nextSpawnTime)
//            {
//                SpawnRandomNote();
//                nextSpawnTime = Time.time + spawnRate;
//            }

//            // Handle key presses with anti-spam protection
//            for (int i = 0; i < playerKeys.Length; i++)
//            {
//                if (Input.GetKeyDown(playerKeys[i]))
//                {
//                    // Check if enough time has passed since the last press of this key
//                    if (Time.time - keyLastPressTime[i] >= keyPressDelay)
//                    {
//                        keyLastPressTime[i] = Time.time; // Update the last press time
//                        HandleKeyPress(i);
//                    }
//                    // If not enough time has passed, ignore this key press (anti-spam)
//                }
//            }
//        }
//    }

//    void SpawnRandomNote()
//    {
//        // Choose a random key to spawn
//        int keyIndex = Random.Range(0, playerKeys.Length);

//        // Instantiate the corresponding note
//        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
//        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

//        // Set up the note
//        note.keyIndex = keyIndex;
//        note.speed = noteSpeed;
//        note.targetPosition = targetLine.position;
//        note.rhythmController = this;

//        // Add to active notes
//        activeNotes.Add(note);
//    }

//    void HandleKeyPress(int keyIndex)
//    {
//        // Find closest note for this key
//        TurompoNoteController closestNote = null;
//        float closestDistance = float.MaxValue;

//        foreach (TurompoNoteController note in activeNotes)
//        {
//            if (note.keyIndex == keyIndex)
//            {
//                // Calculate distance based on Y position difference from target line
//                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestNote = note;
//                }
//            }
//        }

//        // Check if note is within hit range (distance to target line)
//        if (closestNote != null && closestDistance < 1.0f)
//        {
//            // Successfully hit the note
//            int scoreAmount = CalculateScore(closestDistance);
//            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);

//            // Get the collision point (note's position)
//            Vector3 collisionPoint = closestNote.transform.position;

//            // Boost spin with collision animation at the note's position
//            playerTorompo.BoostSpinWithCollision(collisionPoint);

//            // Remove the note
//            activeNotes.Remove(closestNote);
//            Destroy(closestNote.gameObject);
//        }
//        else
//        {
//            // Missed - no matching note or too far
//            // We just report a miss but don't change the spin speed - that only happens on successful hits
//            playerTorompo.MissedMatch();
//        }
//    }

//    int CalculateScore(float distance)
//    {
//        // Calculate score based on precision
//        if (distance < 0.2f)
//            return 100; // Perfect
//        else if (distance < 0.5f)
//            return 50;  // Good
//        else
//            return 25;  // Okay
//    }

//    public void NoteMissed(TurompoNoteController note)
//    {
//        // Player missed this note as it passed the target line
//        playerTorompo.MissedMatch();
//        // Note: we don't remove the note here - it will continue falling and be destroyed by its timer
//    }

//    public void RemoveNote(TurompoNoteController note)
//    {
//        if (activeNotes.Contains(note))
//        {
//            activeNotes.Remove(note);
//        }
//    }

//    // Method to clear all notes when game ends or restarts
//    public void ClearAllNotes()
//    {
//        foreach (var note in new List<TurompoNoteController>(activeNotes))
//        {
//            if (note != null)
//            {
//                Destroy(note.gameObject);
//            }
//        }
//        activeNotes.Clear();
//    }
//}



//AI





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


//    void Start()
//    {
//        if (playerIndex == 1)
//            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
//        else
//            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

//        for (int i = 0; i < playerKeys.Length; i++)
//        {
//            keyLastPressTime[i] = -keyPressDelay;
//            aiKeyLastAttempt[i] = -aiMinTimeBetweenAttempts;
//        }

//        // REMOVE THIS LINE
//        // StartCoroutine(SpawnNotesRoutine());
//    }


//    // Public method you call AFTER countdown ends
//    public void BeginSpawning()
//    {
//        StartCoroutine(SpawnNotesRoutine());
//    }

//    IEnumerator SpawnNotesRoutine()
//    {
//        // delay before the first note drops
//        yield return new WaitForSeconds(1f);

//        while (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            SpawnRandomNote();
//            yield return new WaitForSeconds(spawnRate);
//        }
//    }


//    void Update()
//    {
//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            // Only handle input (human or AI)
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
//                // Check if enough time has passed since the last press of this key
//                if (Time.time - keyLastPressTime[i] >= keyPressDelay)
//                {
//                    keyLastPressTime[i] = Time.time; // Update the last press time
//                    HandleKeyPress(i);
//                }
//                // If not enough time has passed, ignore this key press (anti-spam)
//            }
//        }
//    }

//    void HandleAIInput()
//    {
//        // AI logic for each key type
//        for (int keyIndex = 0; keyIndex < playerKeys.Length; keyIndex++)
//        {
//            // Check if enough time has passed since last attempt for this key
//            if (Time.time - aiKeyLastAttempt[keyIndex] < aiMinTimeBetweenAttempts)
//                continue;

//            // Find the best note for this key
//            TurompoNoteController bestNote = FindBestNoteForKey(keyIndex);

//            if (bestNote != null)
//            {
//                float distanceToTarget = Mathf.Abs(bestNote.transform.position.y - targetLine.position.y);

//                // Determine if AI should attempt this note
//                if (ShouldAIAttemptNote(bestNote, distanceToTarget))
//                {
//                    // Calculate reaction delay based on AI settings
//                    float reactionDelay = CalculateAIReactionDelay(distanceToTarget);

//                    // Start coroutine for delayed AI input
//                    StartCoroutine(DelayedAIKeyPress(keyIndex, reactionDelay));

//                    // Update last attempt time
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

//                // Only consider notes that are approaching or near the target
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
//        // Base decision factors
//        float attemptChance = 0f;

//        // Distance-based chance
//        if (distanceToTarget <= aiPerfectHitRange)
//        {
//            attemptChance = aiAccuracy * 0.9f; // High chance for perfect range
//        }
//        else if (distanceToTarget <= aiGoodHitRange)
//        {
//            attemptChance = aiAccuracy * 0.7f; // Medium chance for good range
//        }
//        else if (distanceToTarget <= 1.0f)
//        {
//            attemptChance = aiAccuracy * 0.4f; // Low chance for acceptable range
//        }
//        else
//        {
//            attemptChance = aiAccuracy * 0.1f; // Very low chance for poor range
//        }

//        // Apply consistency factor
//        float consistencyRoll = Random.Range(0f, 1f);
//        if (consistencyRoll > aiConsistency)
//        {
//            attemptChance *= 0.5f; // Reduce chance on consistency failure
//        }

//        // Random decision
//        return Random.Range(0f, 1f) < attemptChance;
//    }

//    float CalculateAIReactionDelay(float distanceToTarget)
//    {
//        // Base reaction time based on AI reaction speed
//        float baseDelay = (1f - aiReactionSpeed) * 0.3f;

//        // Add distance-based delay (closer notes = faster reaction)
//        float distanceDelay = distanceToTarget * 0.05f;

//        // Add some randomness
//        float randomDelay = Random.Range(-0.05f, 0.1f);

//        return Mathf.Max(0f, baseDelay + distanceDelay + randomDelay);
//    }

//    IEnumerator DelayedAIKeyPress(int keyIndex, float delay)
//    {
//        yield return new WaitForSeconds(delay);

//        // Check if game is still active and this is still a valid attempt
//        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
//        {
//            HandleKeyPress(keyIndex);
//        }
//    }

//    void SpawnRandomNote()
//    {
//        // Choose a random key to spawn
//        int keyIndex = Random.Range(0, playerKeys.Length);

//        // Instantiate the corresponding note
//        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
//        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

//        // Set up the note
//        note.keyIndex = keyIndex;
//        note.speed = noteSpeed;
//        note.targetPosition = targetLine.position;
//        note.rhythmController = this;

//        // Add to active notes
//        activeNotes.Add(note);
//    }

//    void HandleKeyPress(int keyIndex)
//    {
//        // Find closest note for this key
//        TurompoNoteController closestNote = null;
//        float closestDistance = float.MaxValue;

//        foreach (TurompoNoteController note in activeNotes)
//        {
//            if (note.keyIndex == keyIndex)
//            {
//                // Calculate distance based on Y position difference from target line
//                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    closestNote = note;
//                }
//            }
//        }

//        // Check if note is within hit range (distance to target line)
//        if (closestNote != null && closestDistance < 1.0f)
//        {
//            // Successfully hit the note
//            int scoreAmount = CalculateScore(closestDistance);
//            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);

//            // Get the collision point (note's position)
//            Vector3 collisionPoint = closestNote.transform.position;

//            // Boost spin with collision animation at the note's position
//            playerTorompo.BoostSpinWithCollision(collisionPoint);

//            // Remove the note
//            activeNotes.Remove(closestNote);
//            Destroy(closestNote.gameObject);

//            // Notify AI system of successful hit if this is an AI player
//            if (enableAI)
//            {
//                NotifyAISystem(true);
//            }
//        }
//        else
//        {
//            // Missed - no matching note or too far
//            // We just report a miss but don't change the spin speed - that only happens on successful hits
//            playerTorompo.MissedMatch();

//            // Notify AI system of miss if this is an AI player
//            if (enableAI)
//            {
//                NotifyAISystem(false);
//            }
//        }
//    }

//    void NotifyAISystem(bool wasHit)
//    {
//        // Find AI controller and notify it of the result
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
//        // Calculate score based on precision
//        if (distance < 0.2f)
//            return 100; // Perfect
//        else if (distance < 0.5f)
//            return 50;  // Good
//        else
//            return 25;  // Okay
//    }

//    public void NoteMissed(TurompoNoteController note)
//    {
//        // Player missed this note as it passed the target line
//        playerTorompo.MissedMatch();
//        // Note: we don't remove the note here - it will continue falling and be destroyed by its timer
//    }

//    public void RemoveNote(TurompoNoteController note)
//    {
//        if (activeNotes.Contains(note))
//        {
//            activeNotes.Remove(note);
//        }
//    }

//    // Method to clear all notes when game ends or restarts
//    public void ClearAllNotes()
//    {
//        foreach (var note in new List<TurompoNoteController>(activeNotes))
//        {
//            if (note != null)
//            {
//                Destroy(note.gameObject);
//            }
//        }
//        activeNotes.Clear();
//    }

//    // Public method for AI to directly attempt key presses
//    public void AIKeyPress(int keyIndex)
//    {
//        if (enableAI && keyIndex >= 0 && keyIndex < playerKeys.Length)
//        {
//            HandleKeyPress(keyIndex);
//        }
//    }

//    // Public getter for active notes (for AI analysis)
//    public List<TurompoNoteController> GetActiveNotes()
//    {
//        return new List<TurompoNoteController>(activeNotes);
//    }

//    // Public getter for target line position (for AI analysis)
//    public Vector3 GetTargetLinePosition()
//    {
//        return targetLine.position;
//    }

//    // Method to enable/disable AI for this controller
//    public void SetAIEnabled(bool enabled)
//    {
//        enableAI = enabled;
//    }

//    // Method to adjust AI difficulty
//    public void SetAIDifficulty(float accuracy, float reactionSpeed, float consistency)
//    {
//        aiAccuracy = Mathf.Clamp01(accuracy);
//        aiReactionSpeed = Mathf.Clamp01(reactionSpeed);
//        aiConsistency = Mathf.Clamp01(consistency);
//    }
//}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurompoRhythmController : MonoBehaviour
{
    // Player identification
    public int playerIndex = 1;

    // Key configuration
    public KeyCode[] playerKeys;

    // Note spawning
    public float noteSpeed = 5f;
    public float spawnRate = 1f;
    public GameObject[] notePrefabs; // Different prefabs for different keys
    public Transform spawnPoint;
    public Transform targetLine;

    // Game references
    public TurompoController playerTorompo;

    // Anti-spam protection
    public float keyPressDelay = 0.3f; // Minimum time between key presses in seconds
    private Dictionary<int, float> keyLastPressTime = new Dictionary<int, float>();

    // AI Settings
    [Header("AI Settings")]
    public bool enableAI = false;
    [Range(0f, 1f)]
    public float aiAccuracy = 0.8f; // How accurate the AI is
    [Range(0f, 1f)]
    public float aiReactionSpeed = 0.7f; // How fast AI reacts
    [Range(0f, 1f)]
    public float aiConsistency = 0.85f; // How consistent AI performance is
    public float aiPerfectHitRange = 0.3f; // Range for AI to consider a "perfect" hit
    public float aiGoodHitRange = 0.6f; // Range for AI to consider a "good" hit

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

    // Gabito added stuffs
    private Coroutine spawnRoutine = null;
    private bool isSpawning = false;



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

        // Detect target line's renderer for 3D highlight
        targetRenderer = targetLine.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }
    }

    // Public method you call AFTER countdown ends. Also Gabito edits
    public void BeginSpawning()
    {
        if (isSpawning)
            return; // Already spawning — prevent duplicates

        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnNotesRoutine());
    }

    // Added by gabito
    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = null;
        isSpawning = false;
    }


    // Gabito edits
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


    //Edited byG Gabito
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
        // Handle key presses with anti-spam protection
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

        foreach (TurompoNoteController note in activeNotes)
        {
            if (note.keyIndex == keyIndex)
            {
                float distanceToTarget = Mathf.Abs(note.transform.position.y - targetLine.position.y);
                if (note.transform.position.y >= targetLine.position.y - 2f && distanceToTarget < bestDistance)
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

        if (distanceToTarget <= aiPerfectHitRange)
            attemptChance = aiAccuracy * 0.9f;
        else if (distanceToTarget <= aiGoodHitRange)
            attemptChance = aiAccuracy * 0.7f;
        else if (distanceToTarget <= 1.0f)
            attemptChance = aiAccuracy * 0.4f;
        else
            attemptChance = aiAccuracy * 0.1f;

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
        {
            HandleKeyPress(keyIndex);
        }
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

            // 🔥 Trigger 3D highlight
            StartCoroutine(HighlightTarget3D());

            if (enableAI)
                NotifyAISystem(true);
        }
        else
        {
            playerTorompo.MissedMatch();
            if (enableAI)
                NotifyAISystem(false);
        }
    }

    void NotifyAISystem(bool wasHit)
    {
        TurompoAIController aiController = FindObjectOfType<TurompoAIController>();
        if (aiController != null && aiController.aiPlayerIndex == playerIndex)
        {
            if (wasHit)
                aiController.RegisterAIHit();
            else
                aiController.RegisterAIMiss();
        }
    }

    int CalculateScore(float distance)
    {
        if (distance < 0.2f)
            return 100; // Perfect
        else if (distance < 0.5f)
            return 50;  // Good
        else
            return 25;  // Okay
    }

    public void NoteMissed(TurompoNoteController note)
    {
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
        {
            HandleKeyPress(keyIndex);
        }
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

    // 🔥 3D Highlight Coroutine
    IEnumerator HighlightTarget3D()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = highlightColor;
            yield return new WaitForSeconds(highlightDuration);
            targetRenderer.material.color = originalColor;
        }
    }
}
