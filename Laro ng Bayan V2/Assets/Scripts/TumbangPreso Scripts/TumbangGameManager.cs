//using UnityEngine;

//public class TumbangGameManager : MonoBehaviour
//{
//    public static TumbangGameManager Instance;

//    public GameObject player1Slipper;
//    public GameObject player2Slipper;

//    private int currentPlayer = 1;
//    private Rigidbody currentRb;
//    private bool isWaitingForStop = false;
//    private bool hasStartedMoving = false;

//    private int player1Score = 0;
//    private int player2Score = 0;
//    public int winningScore = 10;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        SetActivePlayer(1);
//        UIManager.Instance.UpdateScore(player1Score, player2Score);
//        UIManager.Instance.UpdateTurn(currentPlayer);
//    }

//    void Update()
//    {
//        if (isWaitingForStop && currentRb != null)
//        {
//            if (currentRb.velocity.magnitude > 0.1f)
//                hasStartedMoving = true;

//            if (hasStartedMoving && currentRb.velocity.magnitude < 0.05f)
//            {
//                isWaitingForStop = false;
//                hasStartedMoving = false;
//                EndTurn();
//            }
//        }
//    }

//    public void NotifySlipperThrown(Rigidbody rb)
//    {
//        currentRb = rb;
//        isWaitingForStop = true;
//        hasStartedMoving = false;
//    }

//    public void PlayerScored()
//    {
//        int defender = 3 - currentPlayer; // defender is the other player

//        DefenderManager.Instance.StartMiniGame(defender, (bool defenderSucceeded) =>
//        {
//            if (defenderSucceeded)
//            {
//                Debug.Log("❌ Defender succeeded! No point awarded.");
//            }
//            else
//            {
//                if (currentPlayer == 1) player1Score++;
//                else player2Score++;

//                UIManager.Instance.UpdateScore(player1Score, player2Score);

//                if (player1Score >= winningScore)
//                    UIManager.Instance.ShowWinner(1);
//                else if (player2Score >= winningScore)
//                    UIManager.Instance.ShowWinner(2);
//            }
//        });
//    }



//    private void EndTurn()
//    {
//        currentPlayer = 3 - currentPlayer;
//        SetActivePlayer(currentPlayer);
//        UIManager.Instance.UpdateTurn(currentPlayer);
//    }

//    private void SetActivePlayer(int player)
//    {
//        player1Slipper.SetActive(player == 1);
//        player2Slipper.SetActive(player == 2);

//        if (player1Slipper.TryGetComponent<SlipperThrow>(out var s1))
//            s1.ResetTurn();

//        if (player2Slipper.TryGetComponent<SlipperThrow>(out var s2))
//            s2.ResetTurn();

//        currentRb = null;
//        isWaitingForStop = false;
//    }

//    public int GetCurrentPlayer() => currentPlayer;
//}

using UnityEngine;
public class TumbangGameManager : MonoBehaviour
{
    public static TumbangGameManager Instance;
    public GameObject player1Slipper;
    public GameObject player2Slipper;
    private int currentPlayer = 1;
    private Rigidbody currentRb;
    private bool isWaitingForStop = false;
    private bool hasStartedMoving = false;
    private int player1Score = 0;
    private int player2Score = 0;
    public int winningScore = 10;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        SetActivePlayer(1);
        UIManager.Instance.UpdateScore(player1Score, player2Score);
        UIManager.Instance.UpdateTurn(currentPlayer);
    }
    void Update()
    {
        if (isWaitingForStop && currentRb != null)
        {
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
    public void NotifySlipperThrown(Rigidbody rb)
    {
        currentRb = rb;
        isWaitingForStop = true;
        hasStartedMoving = false;
    }
    public void PlayerScored()
    {
        int attackingPlayer = currentPlayer;  // Store who is attacking for clarity
        int defender = 3 - currentPlayer;     // Defender is the other player

        DefenderManager.Instance.StartMiniGame(defender, (bool defenderSucceeded) =>
        {
            if (defenderSucceeded)
            {
                Debug.Log($"❌ Player {defender} defended successfully! No point awarded.");
            }
            else
            {
                // Award point to the attacking player
                if (attackingPlayer == 1)
                {
                    player1Score++;
                    Debug.Log($"✅ Player 1 scored! New score: {player1Score}");
                }
                else
                {
                    player2Score++;
                    Debug.Log($"✅ Player 2 scored! New score: {player2Score}");
                }

                UIManager.Instance.UpdateScore(player1Score, player2Score);

                // Check for winner
                if (player1Score >= winningScore)
                    UIManager.Instance.ShowWinner(1);
                else if (player2Score >= winningScore)
                    UIManager.Instance.ShowWinner(2);
            }
        });
    }
    private void EndTurn()
    {
        currentPlayer = 3 - currentPlayer;
        SetActivePlayer(currentPlayer);
        UIManager.Instance.UpdateTurn(currentPlayer);
    }
    private void SetActivePlayer(int player)
    {
        player1Slipper.SetActive(player == 1);
        player2Slipper.SetActive(player == 2);
        if (player1Slipper.TryGetComponent<SlipperThrow>(out var s1))
            s1.ResetTurn();
        if (player2Slipper.TryGetComponent<SlipperThrow>(out var s2))
            s2.ResetTurn();
        currentRb = null;
        isWaitingForStop = false;
    }
    public int GetCurrentPlayer() => currentPlayer;
}
