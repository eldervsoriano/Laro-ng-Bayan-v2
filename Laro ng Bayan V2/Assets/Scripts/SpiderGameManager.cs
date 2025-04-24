using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGameManager : MonoBehaviour
{
    public static SpiderGameManager Instance;

    public int maxHealth = 5;  // Maximum health per player
    private int player1Health;
    private int player2Health;

    private int currentRound = 1; // Starting round number
    private int sequenceLength = 1; // Starting number of keys in sequence

    public SpiderPlayerInput player1Input; // Reference to Player 1 input script
    public SpiderPlayerInput player2Input; // Reference to Player 2 input script

    public SpiderUIManager uiManager; // Reference to UI manager for displaying data

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartNewGame();
    }

    // Initialize game with players' health and starting round
    void StartNewGame()
    {
        player1Health = maxHealth;
        player2Health = maxHealth;
        currentRound = 1;
        sequenceLength = 1;

        uiManager.UpdateHealth(player1Health, player2Health);
        StartCoroutine(RunRound());
    }

    // Handle each round of the game
    IEnumerator RunRound()
    {
        // Display round information
        uiManager.ShowRound(currentRound);

        yield return new WaitForSeconds(1.5f);

        // Generate the sequence of keys players must press
        List<KeyCode> sequence = GenerateRandomSequence(sequenceLength);

        // Start input for both players (asynchronous)
        yield return StartCoroutine(player1Input.BeginInput(sequence, 1)); // Player 1's input phase
        yield return StartCoroutine(player2Input.BeginInput(sequence, 2)); // Player 2's input phase

        // Compare scores (number of correct keys pressed)
        int p1Score = player1Input.correctCount;
        int p2Score = player2Input.correctCount;

        yield return new WaitForSeconds(0.5f); // Delay before showing result

        // Determine which player wins the round and apply damage
        if (p1Score > p2Score)
        {
            player2Health--; // Player 1 attacks Player 2
            uiManager.ShowAttack(1); // Show Player 1's attack feedback
        }
        else if (p2Score > p1Score)
        {
            player1Health--; // Player 2 attacks Player 1
            uiManager.ShowAttack(2); // Show Player 2's attack feedback
        }
        else
        {
            uiManager.ShowDraw(); // Show draw feedback if no player wins
        }

        // Update health UI
        uiManager.UpdateHealth(player1Health, player2Health);

        yield return new WaitForSeconds(1.5f); // Delay before the next round or end

        // Check if any player has won the game
        if (player1Health <= 0)
        {
            uiManager.ShowWinner(2); // Player 2 wins
        }
        else if (player2Health <= 0)
        {
            uiManager.ShowWinner(1); // Player 1 wins
        }
        else
        {
            // Proceed to the next round with increased difficulty
            currentRound++;
            sequenceLength++; // Increase the number of keys in the sequence
            StartCoroutine(RunRound()); // Start the next round
        }
    }

    // Generate a random sequence of key presses
    List<KeyCode> GenerateRandomSequence(int length)
    {
        KeyCode[] possibleKeys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.D }; // Keys for Player 1
        if (player2Input != null) // Player 2 uses arrow keys
            possibleKeys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow };

        List<KeyCode> sequence = new List<KeyCode>();

        for (int i = 0; i < length; i++)
        {
            sequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]); // Randomly select a key
        }

        return sequence;
    }
}
