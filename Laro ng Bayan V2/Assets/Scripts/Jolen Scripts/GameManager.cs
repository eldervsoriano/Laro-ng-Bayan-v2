using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1Pamato;
    public GameObject player2Pamato;

    private Rigidbody currentRb;
    private bool isWaitingForStop = false;

    private int player1Score = 0;
    private int player2Score = 0;
    private int currentPlayer = 1;
    private bool hasStartedMoving = false;
    public int winningScore = 5;



    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetActivePlayer(1);
    }

    void Update()
    {
        if (isWaitingForStop && currentRb != null)
        {
            // If the pamato NEVER moved (didn't get a valid force), skip ending turn
            if (currentRb.velocity.magnitude > 0.1f)
                hasStartedMoving = true;

            if (hasStartedMoving && currentRb.velocity.magnitude < 0.05f)
            {
                isWaitingForStop = false;
                hasStartedMoving = false;
                EndTurn();
            }
        }
    }


    public void MarbleKnockedOut(GameObject marble)
    {
        if (currentPlayer == 1) player1Score++;
        else player2Score++;

        UIManager.Instance.UpdateScore(player1Score, player2Score);

        // Check for winner
        if (player1Score >= winningScore)
        {
            EndGame(1);
        }
        else if (player2Score >= winningScore)
        {
            EndGame(2);
        }
    }



    public void NotifyShot(Rigidbody rb)
    {
        currentRb = rb;
        isWaitingForStop = true;
        hasStartedMoving = false;
        Debug.Log("Player " + currentPlayer + " shot. Waiting for stop...");
    }


    private void EndTurn()
    {
        currentPlayer = 3 - currentPlayer;
        SetActivePlayer(currentPlayer);
    }

    private void SetActivePlayer(int player)
    {
        player1Pamato.SetActive(player == 1);
        player2Pamato.SetActive(player == 2);

        if (player == 1 && player1Pamato.TryGetComponent<PamatoShooter>(out var shooter1))
            shooter1.ResetTurn();

        if (player == 2 && player2Pamato.TryGetComponent<PamatoShooter>(out var shooter2))
            shooter2.ResetTurn();

        UIManager.Instance.UpdateTurn(player); // 👈 NEW LINE

        currentRb = null;
        isWaitingForStop = false;
    }



    public int GetCurrentPlayer() => currentPlayer;

    private void EndGame(int winningPlayer)
    {
        UIManager.Instance.ShowWinner(winningPlayer);

        // Disable both pamatos
        player1Pamato.SetActive(false);
        player2Pamato.SetActive(false);
    }

}
