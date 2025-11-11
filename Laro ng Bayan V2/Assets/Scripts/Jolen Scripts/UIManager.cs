using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // Separate text components for each player's score
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public TextMeshProUGUI turnText;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    void Awake()
    {
        Instance = this;
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

        // Optional: Highlight the active player's score text
        player1ScoreText.fontStyle = (currentPlayer == 1) ? FontStyles.Bold : FontStyles.Normal;
        player2ScoreText.fontStyle = (currentPlayer == 2) ? FontStyles.Bold : FontStyles.Normal;
    }

    public void ShowWinner(int player)
    {
        StartCoroutine(ShowWinnerWithDelay(player));
    }

    private IEnumerator ShowWinnerWithDelay(int player)
    {
        yield return new WaitForSeconds(1f); // suspense before victory screen

        winnerPanel.SetActive(true);
        winnerText.text = $"Player {player} Wins!";

        // Freeze time
        Time.timeScale = 0f;

        // Disable pause system so players can't pause the victory screen
        PauseButton.canPause = false;
        PauseButton.isPaused = true; // ensure pause logic doesn't try to resume

        // Show the cursor for UI interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // This can be called by a "Restart" or "Back to Menu" button
    public void ResumeAfterVictory()
    {
        // Resume normal gameplay or menu state
        Time.timeScale = 1f;
        PauseButton.canPause = true;
        PauseButton.isPaused = false;

        // Hide the winner panel
        if (winnerPanel != null)
            winnerPanel.SetActive(false);

        // Optionally hide the cursor if you’re returning to gameplay
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

}
