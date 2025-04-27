
//using UnityEngine;
//using TMPro;

//public class UIManager : MonoBehaviour
//{
//    public static UIManager Instance;

//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI turnText;
//    public GameObject winnerPanel;
//    public TextMeshProUGUI winnerText;

//    void Awake()
//    {
//        Instance = this;
//    }

//    public void UpdateScore(int p1, int p2)
//    {
//        scoreText.text = $"Player 1: {p1}   |   Player 2: {p2}";
//    }

//    public void UpdateTurn(int currentPlayer)
//    {
//        turnText.text = $"Player {currentPlayer}'s Turn";
//    }

//    public void ShowWinner(int player)
//    {
//        StartCoroutine(ShowWinnerWithDelay(player));
//    }

//    private System.Collections.IEnumerator ShowWinnerWithDelay(int player)
//    {
//        yield return new WaitForSeconds(2f); // ⏳ delay duration

//        winnerPanel.SetActive(true);
//        winnerText.text = $"🎉 Player {player} Wins!";
//    }

//}

using UnityEngine;
using TMPro;
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

    private System.Collections.IEnumerator ShowWinnerWithDelay(int player)
    {
        yield return new WaitForSeconds(2f); // ⏳ delay duration
        winnerPanel.SetActive(true);
        winnerText.text = $"🎉 Player {player} Wins!";
    }
}
