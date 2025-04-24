using UnityEngine;
using TMPro;
using System.Collections;  // For IEnumerator and Coroutines
using System.Collections.Generic;  // For List<>

public class SpiderUIManager : MonoBehaviour
{
    public static SpiderUIManager Instance;

    [Header("Health UI")]
    public TextMeshProUGUI player1HealthText;
    public TextMeshProUGUI player2HealthText;
    public GameObject player1HealthBar;
    public GameObject player2HealthBar;

    [Header("Round UI")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI sequenceText;

    [Header("Feedback UI")]
    public GameObject attackFeedbackPanel;
    public TextMeshProUGUI attackFeedbackText;

    [Header("Winner UI")]
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Show current round number
    public void ShowRound(int roundNumber)
    {
        roundText.text = "Round: " + roundNumber;
    }

    // Display the key sequence that players need to react to
    public void DisplaySequence(List<KeyCode> sequence, int playerNumber)
    {
        string sequenceDisplay = "";
        foreach (KeyCode key in sequence)
        {
            sequenceDisplay += GetKeySymbol(key) + " ";
        }

        sequenceText.text = "Player " + playerNumber + " - " + sequenceDisplay;
    }

    // Update Health UI for both players
    public void UpdateHealth(int player1Health, int player2Health)
    {
        player1HealthText.text = "Player 1: " + player1Health;
        player2HealthText.text = "Player 2: " + player2Health;

        float player1HealthPercentage = (float)player1Health / 5f;
        float player2HealthPercentage = (float)player2Health / 5f;

        player1HealthBar.transform.localScale = new Vector3(player1HealthPercentage, 1f, 1f);
        player2HealthBar.transform.localScale = new Vector3(player2HealthPercentage, 1f, 1f);
    }

    // Display feedback after attack (Success or Fail)
    public void ShowAttack(int playerNumber)
    {
        attackFeedbackPanel.SetActive(true);
        attackFeedbackText.text = "Player " + playerNumber + " Attacked!";
        StartCoroutine(HideAttackFeedback());
    }

    private IEnumerator HideAttackFeedback()
    {
        yield return new WaitForSeconds(1.5f);
        attackFeedbackPanel.SetActive(false);
    }

    // Show a draw message when both players tie in a round
    public void ShowDraw()
    {
        attackFeedbackPanel.SetActive(true);
        attackFeedbackText.text = "Draw!";
        StartCoroutine(HideAttackFeedback());
    }

    // Show winner at the end of the game
    public void ShowWinner(int playerNumber)
    {
        winnerPanel.SetActive(true);
        winnerText.text = "Player " + playerNumber + " Wins!";
    }

    private string GetKeySymbol(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W: return "W";
            case KeyCode.A: return "A";
            case KeyCode.S: return "S";
            case KeyCode.D: return "D";
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            default: return key.ToString();
        }
    }
}
