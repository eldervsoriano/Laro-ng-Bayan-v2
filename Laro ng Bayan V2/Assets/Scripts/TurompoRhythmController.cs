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

    // Internal state
    private float nextSpawnTime = 0f;
    private List<TurompoNoteController> activeNotes = new List<TurompoNoteController>();

    void Start()
    {
        // Set up key configurations based on player index
        if (playerIndex == 1)
        {
            playerKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        }
        else
        {
            playerKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        }
    }

    void Update()
    {
        // Only process game logic if the game is active
        if (TurompoGameManager.Instance != null && TurompoGameManager.Instance.IsGameActive())
        {
            // Spawn new notes
            if (Time.time >= nextSpawnTime)
            {
                SpawnRandomNote();
                nextSpawnTime = Time.time + spawnRate;
            }

            // Handle key presses
            for (int i = 0; i < playerKeys.Length; i++)
            {
                if (Input.GetKeyDown(playerKeys[i]))
                {
                    HandleKeyPress(i);
                }
            }
        }
    }

    void SpawnRandomNote()
    {
        // Choose a random key to spawn
        int keyIndex = Random.Range(0, playerKeys.Length);

        // Instantiate the corresponding note
        GameObject noteObject = Instantiate(notePrefabs[keyIndex], spawnPoint.position, Quaternion.identity);
        TurompoNoteController note = noteObject.GetComponent<TurompoNoteController>();

        // Set up the note
        note.keyIndex = keyIndex;
        note.speed = noteSpeed;
        note.targetPosition = targetLine.position;
        note.rhythmController = this;

        // Add to active notes
        activeNotes.Add(note);
    }

    void HandleKeyPress(int keyIndex)
    {
        // Find closest note for this key
        TurompoNoteController closestNote = null;
        float closestDistance = float.MaxValue;

        foreach (TurompoNoteController note in activeNotes)
        {
            if (note.keyIndex == keyIndex)
            {
                // Calculate distance based on Y position difference from target line
                float distance = Mathf.Abs(note.transform.position.y - targetLine.position.y);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNote = note;
                }
            }
        }

        // Check if note is within hit range (distance to target line)
        if (closestNote != null && closestDistance < 1.0f)
        {
            // Successfully hit the note
            int scoreAmount = CalculateScore(closestDistance);
            TurompoGameManager.Instance.AddScore(playerIndex, scoreAmount);
            playerTorompo.BoostSpin();

            // Remove the note
            activeNotes.Remove(closestNote);
            Destroy(closestNote.gameObject);
        }
        else
        {
            // Missed - no matching note or too far
            playerTorompo.MissedMatch();
        }
    }

    int CalculateScore(float distance)
    {
        // Calculate score based on precision
        if (distance < 0.2f)
            return 100; // Perfect
        else if (distance < 0.5f)
            return 50;  // Good
        else
            return 25;  // Okay
    }

    public void NoteMissed(TurompoNoteController note)
    {
        // Player missed this note as it passed the target line
        playerTorompo.MissedMatch();
        // Note: we don't remove the note here - it will continue falling and be destroyed by its timer
    }

    public void RemoveNote(TurompoNoteController note)
    {
        if (activeNotes.Contains(note))
        {
            activeNotes.Remove(note);
        }
    }

    // Method to clear all notes when game ends or restarts
    public void ClearAllNotes()
    {
        foreach (var note in new List<TurompoNoteController>(activeNotes))
        {
            if (note != null)
            {
                Destroy(note.gameObject);
            }
        }
        activeNotes.Clear();
    }
}