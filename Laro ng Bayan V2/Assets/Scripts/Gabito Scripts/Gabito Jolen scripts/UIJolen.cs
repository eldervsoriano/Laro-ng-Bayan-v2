//using System.Collections;
//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement; // NEW: for scene loading

//public class UIJolen : MonoBehaviour
//{
//    public static UIJolen Instance;
//    private bool canShowTurn = false; // NEW: only allow turn panel when game starts


//    [Header("Score UI")]
//    public TextMeshProUGUI player1ScoreText;
//    public TextMeshProUGUI player2ScoreText;

//    [Header("Profiles")]
//    public RectTransform player1Profile;
//    public RectTransform player2Profile;

//    [Header("Turn UI")]
//    public TextMeshProUGUI turnText;
//    public GameObject player1TurnPanel;
//    public GameObject player2TurnPanel;

//    [Header("Winner UI")]
//    public GameObject winnerPanel;
//    public TextMeshProUGUI winnerText;

//    [Header("Scenes")]
//    [Tooltip("Optional: leave blank if you don’t want to load a defeat scene.")]
//    public string defeatSceneName = " "; // drag your defeat scene name here or edit in Inspector

//    [Header("Scale Settings")]
//    public Vector3 normalScale = Vector3.one;
//    public Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1f);


//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            // DontDestroyOnLoad(gameObject); // optional
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Start()
//    {
//        Instance = this;

//        // Force hide everything at start
//        if (turnText != null) turnText.gameObject.SetActive(false);
//        SetProfilesVisible(false);
//        if (player1TurnPanel != null) player1TurnPanel.SetActive(false);
//        if (player2TurnPanel != null) player2TurnPanel.SetActive(false);
//        if (winnerPanel != null) winnerPanel.SetActive(false);
//    }

//    /// <summary>
//    /// Updates both player scores on screen
//    /// </summary>
//    /// 


//    public void EnableUI(bool value)
//    {
//        this.enabled = value; // now you can enable/disable at runtime
//    }

//    public void UpdateScore(int p1, int p2)
//    {
//        player1ScoreText.text = $"Player 1: {p1}";
//        player2ScoreText.text = $"Player 2: {p2}";
//    }

//    /// <summary>
//    /// Updates turn UI to highlight active player
//    /// </summary>
//    /// 
//    public void AllowTurnUI() => canShowTurn = true; // call this AFTER countdown

//    public void UpdateTurn(int currentPlayer)
//    {
//        if (turnText != null)
//        {
//            turnText.gameObject.SetActive(true);
//            turnText.text = $"Player {currentPlayer}'s Turn";
//        }

//        // Highlight active player’s score
//        player1ScoreText.fontStyle = (currentPlayer == 1) ? FontStyles.Bold : FontStyles.Normal;
//        player2ScoreText.fontStyle = (currentPlayer == 2) ? FontStyles.Bold : FontStyles.Normal;

//        // Scale active profile
//        if (player1Profile != null && player2Profile != null)
//        {
//            player1Profile.localScale = (currentPlayer == 1) ? highlightScale : normalScale;
//            player2Profile.localScale = (currentPlayer == 2) ? highlightScale : normalScale;
//        }

//        // Toggle turn panels
//        if (player1TurnPanel != null) player1TurnPanel.SetActive(currentPlayer == 1);
//        if (player2TurnPanel != null) player2TurnPanel.SetActive(currentPlayer == 2);
//    }

//    /// <summary>
//    /// Shows the winner after a short delay
//    /// </summary>
//    public void ShowWinner(int player)
//    {
//        StartCoroutine(ShowWinnerWithDelay(player));
//    }

//    private IEnumerator ShowWinnerWithDelay(int player)
//    {
//        yield return new WaitForSeconds(1f);

//        // If Player 2 wins AND we actually have a defeat scene name, go there
//        if (player == 2 && !string.IsNullOrEmpty(defeatSceneName))
//        {
//            SceneManager.LoadScene(defeatSceneName);
//        }
//        else
//        {
//            // Show winner panel
//            if (winnerPanel != null) winnerPanel.SetActive(true);
//            if (winnerText != null) winnerText.text = $"Player {player} Wins!";

//            // Pause everything
//            Time.timeScale = 0f;

//            // Disable pause button completely
//            PauseButton.canPause = false;
//            PauseButton.isPaused = true;

//            // Show cursor for winner UI interaction
//            Cursor.visible = true;
//            Cursor.lockState = CursorLockMode.None;
//        }
//    }


//    /// <summary>
//    /// Toggle profile visibility
//    /// </summary>
//    /// 
//        // Called by a Restart or Continue button on the winner panel
//    public void ResumeAfterVictory()
//    {
//        // Resume time and gameplay
//        Time.timeScale = 1f;
//        PauseButton.canPause = true;
//        PauseButton.isPaused = false;

//        // Hide winner panel
//        if (winnerPanel != null)
//            winnerPanel.SetActive(false);

//        // Optionally hide cursor again if returning to gameplay
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    public void SetProfilesVisible(bool visible)
//    {
//        if (player1Profile != null) player1Profile.gameObject.SetActive(visible);
//        if (player2Profile != null) player2Profile.gameObject.SetActive(visible);
//    }
//}


using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIJolen : MonoBehaviour
{
    public static UIJolen Instance;
    private bool canShowTurn = false;

    [Header("Player Names")]
    [Tooltip("Custom names for Player 1 and Player 2 (e.g., Player, AI)")]
    public string player1Name = "Player 1";
    public string player2Name = "Player 2";

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

    [Header("Scenes")]
    [Tooltip("Optional: leave blank if you don’t want to load a defeat scene.")]
    public string defeatSceneName = " ";

    [Header("Scale Settings")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Instance = this;

        if (turnText != null) turnText.gameObject.SetActive(false);
        SetProfilesVisible(false);
        if (player1TurnPanel != null) player1TurnPanel.SetActive(false);
        if (player2TurnPanel != null) player2TurnPanel.SetActive(false);
        if (winnerPanel != null) winnerPanel.SetActive(false);

        // Initialize score text with names
        UpdateScore(0, 0);
    }

    public void EnableUI(bool value)
    {
        this.enabled = value;
    }

    public void UpdateScore(int p1, int p2)
    {
        player1ScoreText.text = $"{player1Name}: {p1}";
        player2ScoreText.text = $"{player2Name}: {p2}";
    }

    public void AllowTurnUI() => canShowTurn = true;

    public void UpdateTurn(int currentPlayer)
    {
        if (!canShowTurn) return; // Prevent showing during tutorial/countdown

        if (turnText != null)
        {
            turnText.gameObject.SetActive(true);
            string currentName = (currentPlayer == 1) ? player1Name : player2Name;
            turnText.text = $"{currentName}'s Turn";
        }

        player1ScoreText.fontStyle = (currentPlayer == 1) ? FontStyles.Bold : FontStyles.Normal;
        player2ScoreText.fontStyle = (currentPlayer == 2) ? FontStyles.Bold : FontStyles.Normal;

        if (player1Profile != null && player2Profile != null)
        {
            player1Profile.localScale = (currentPlayer == 1) ? highlightScale : normalScale;
            player2Profile.localScale = (currentPlayer == 2) ? highlightScale : normalScale;
        }

        if (player1TurnPanel != null) player1TurnPanel.SetActive(currentPlayer == 1);
        if (player2TurnPanel != null) player2TurnPanel.SetActive(currentPlayer == 2);
    }

    public void ShowWinner(int player)
    {
        StartCoroutine(ShowWinnerWithDelay(player));
    }

    private IEnumerator ShowWinnerWithDelay(int player)
    {
        yield return new WaitForSeconds(1f);

        if (player == 2 && !string.IsNullOrEmpty(defeatSceneName))
        {
            SceneManager.LoadScene(defeatSceneName);
        }
        else
        {
            if (winnerPanel != null) winnerPanel.SetActive(true);
            string winnerName = (player == 1) ? player1Name : player2Name;
            if (winnerText != null) winnerText.text = $"{winnerName} Wins!";

            Time.timeScale = 0f;

            PauseButton.canPause = false;
            PauseButton.isPaused = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeAfterVictory()
    {
        Time.timeScale = 1f;
        PauseButton.canPause = true;
        PauseButton.isPaused = false;

        if (winnerPanel != null)
            winnerPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetProfilesVisible(bool visible)
    {
        if (player1Profile != null) player1Profile.gameObject.SetActive(visible);
        if (player2Profile != null) player2Profile.gameObject.SetActive(visible);
    }
}
