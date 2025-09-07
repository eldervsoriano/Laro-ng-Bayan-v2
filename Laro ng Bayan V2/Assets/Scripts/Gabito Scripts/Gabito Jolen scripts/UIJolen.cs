using System.Collections;
using UnityEngine;
using TMPro;

public class UIJolen : MonoBehaviour
{
    public static UIJolen Instance;
    private bool canShowTurn = false; // NEW: only allow turn panel when game starts


    [Header("Score UI")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    [Header("Profiles")]
    public RectTransform player1Profile;
    public RectTransform player2Profile;

    [Header("Turn UI")]
    public TextMeshProUGUI turnText;
    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    [Header("Winner UI")]
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    [Header("Scale Settings")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optional: only keep alive across scenes if really needed
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // remove duplicate if one already exists
        }
    }

    void Start()
    {
        Instance = this;

        // Force hide everything at start
        if (turnText != null) turnText.gameObject.SetActive(false);
        SetProfilesVisible(false);
        if (player1TurnPanel != null) player1TurnPanel.SetActive(false);
        if (player2TurnPanel != null) player2TurnPanel.SetActive(false);
        if (winnerPanel != null) winnerPanel.SetActive(false);
    }

    /// <summary>
    /// Updates both player scores on screen
    /// </summary>
    /// 


    public void EnableUI(bool value)
    {
        this.enabled = value; // now you can enable/disable at runtime
    }

    public void UpdateScore(int p1, int p2)
    {
        player1ScoreText.text = $"Player 1: {p1}";
        player2ScoreText.text = $"Player 2: {p2}";
    }

    /// <summary>
    /// Updates turn UI to highlight active player
    /// </summary>
    /// 
    public void AllowTurnUI() => canShowTurn = true; // call this AFTER countdown

    public void UpdateTurn(int currentPlayer)
    {
        if (turnText != null)
        {
            turnText.gameObject.SetActive(true);
            turnText.text = $"Player {currentPlayer}'s Turn";
        }

        // Highlight active player’s score
        player1ScoreText.fontStyle = (currentPlayer == 1) ? FontStyles.Bold : FontStyles.Normal;
        player2ScoreText.fontStyle = (currentPlayer == 2) ? FontStyles.Bold : FontStyles.Normal;

        // Scale active profile
        if (player1Profile != null && player2Profile != null)
        {
            player1Profile.localScale = (currentPlayer == 1) ? highlightScale : normalScale;
            player2Profile.localScale = (currentPlayer == 2) ? highlightScale : normalScale;
        }

        // Toggle turn panels
        if (player1TurnPanel != null) player1TurnPanel.SetActive(currentPlayer == 1);
        if (player2TurnPanel != null) player2TurnPanel.SetActive(currentPlayer == 2);
    }

    /// <summary>
    /// Shows the winner after a short delay
    /// </summary>
    public void ShowWinner(int player)
    {
        StartCoroutine(ShowWinnerWithDelay(player));
    }

    private IEnumerator ShowWinnerWithDelay(int player)
    {
        yield return new WaitForSeconds(2f);
        if (winnerPanel != null) winnerPanel.SetActive(true);
        if (winnerText != null) winnerText.text = $"Player {player} Wins!";
    }

    /// <summary>
    /// Toggle profile visibility
    /// </summary>
    public void SetProfilesVisible(bool visible)
    {
        if (player1Profile != null) player1Profile.gameObject.SetActive(visible);
        if (player2Profile != null) player2Profile.gameObject.SetActive(visible);
    }
}
