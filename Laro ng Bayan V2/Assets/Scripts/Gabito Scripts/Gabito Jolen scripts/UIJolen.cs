using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIJolen : MonoBehaviour
{
    // EDITED BY GABITO

    public static UIJolen Instance;


    // Separate text components for each player's score
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    // Player profile icons (drag your UI Image/Panel here in Inspector)
    public RectTransform player1Profile;
    public RectTransform player2Profile;

    public TextMeshProUGUI turnText;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    // Panels for turn indicators
    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    // Scale settings
    public Vector3 normalScale = Vector3.one;
    public Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1f); // make 20% bigger

    void Awake()
    {
        Instance = this;
        if (turnText != null)
            turnText.gameObject.SetActive(false); // hide at start

        SetProfilesVisible(false); // hide profiles at start

        // Hide turn panels at start
        if (player1TurnPanel != null) 
            player1TurnPanel.SetActive(false);

        if (player2TurnPanel != null) 
            player2TurnPanel.SetActive(false);
    }


    public void UpdateScore(int p1, int p2)
    {
        // Update each player's score separately
        player1ScoreText.text = $"Player 1: {p1}";
        player2ScoreText.text = $"Player 2: {p2}";
    }

    public void UpdateTurn(int currentPlayer)
    {
        turnText.text = $"Player {currentPlayer}'s Turn";

        // Highlight active player's score text
        player1ScoreText.fontStyle = (currentPlayer == 1) ? FontStyles.Bold : FontStyles.Normal;
        player2ScoreText.fontStyle = (currentPlayer == 2) ? FontStyles.Bold : FontStyles.Normal;

        // Scale profiles (make active one bigger)
        if (player1Profile != null && player2Profile != null)
        {
            player1Profile.localScale = (currentPlayer == 1) ? highlightScale : normalScale;
            player2Profile.localScale = (currentPlayer == 2) ? highlightScale : normalScale;
        }

        // Toggle turn panels
        if (player1TurnPanel != null)
            player1TurnPanel.SetActive(currentPlayer == 1);
        if (player2TurnPanel != null)
            player2TurnPanel.SetActive(currentPlayer == 2);
    }

    public void ShowWinner(int player)
    {
        StartCoroutine(ShowWinnerWithDelay(player));
    }

    private System.Collections.IEnumerator ShowWinnerWithDelay(int player)
    {
        yield return new WaitForSeconds(2f); // delay duration
        winnerPanel.SetActive(true);
        winnerText.text = $"Player {player} Wins!";
    }

    public void SetTurnTextVisible(bool visible)
    {
        if (turnText != null)
            turnText.gameObject.SetActive(visible);
    }

    public void SetProfilesVisible(bool visible)
    {
        if (player1Profile != null)
            player1Profile.gameObject.SetActive(visible);
        if (player2Profile != null)
            player2Profile.gameObject.SetActive(visible);
    }

}
