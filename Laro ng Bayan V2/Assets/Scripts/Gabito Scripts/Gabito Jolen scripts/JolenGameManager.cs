//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class JolenGameManager : MonoBehaviour
//{
//    // Gabito's jolen

//    public static JolenGameManager Instance;

//    public GameObject player1Pamato;
//    public GameObject player2Pamato;

//    private Rigidbody currentRb;
//    private bool isWaitingForStop = false;

//    private int player1Score = 0;
//    private int player2Score = 0;
//    private int currentPlayer = 1;
//    private bool hasStartedMoving = false;
//    public int winningScore = 5;

//    // NEW: Stall detection
//    private float lowSpeedTimer = 0f;
//    public float lowSpeedThreshold = 0.1f; // below this is "rolling slowly"
//    public float stallTimeLimit = 2f;      // if rolling slowly for 2s -> end turn

//    void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Update()
//    {
//        if (isWaitingForStop && currentRb != null)
//        {
//            // Did the pamato actually move?
//            if (currentRb.velocity.magnitude > 0.1f)
//                hasStartedMoving = true;

//            if (hasStartedMoving)
//            {
//                // Check if it's nearly stopped
//                if (currentRb.velocity.magnitude < lowSpeedThreshold)
//                {
//                    lowSpeedTimer += Time.deltaTime;

//                    if (lowSpeedTimer >= stallTimeLimit)
//                    {
//                        Debug.Log("Pamato stalled too long, ending turn...");
//                        ForceStopPamato();
//                        EndTurn();
//                    }
//                }
//                else
//                {
//                    // Reset if it picks up speed again
//                    lowSpeedTimer = 0f;
//                }
//            }

//            // Old rule: fully stopped
//            if (hasStartedMoving && currentRb.velocity.magnitude < 0.05f)
//            {
//                Debug.Log("Pamato stopped naturally, ending turn...");
//                isWaitingForStop = false;
//                hasStartedMoving = false;
//                lowSpeedTimer = 0f;
//                EndTurn();
//            }
//        }
//    }

//    public void NotifyShot(Rigidbody rb)
//    {
//        currentRb = rb;
//        isWaitingForStop = true;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//        Debug.Log("Player " + currentPlayer + " shot. Waiting for stop...");
//    }

//    private void ForceStopPamato()
//    {
//        if (currentRb != null)
//        {
//            currentRb.velocity = Vector3.zero;
//            currentRb.angularVelocity = Vector3.zero;
//        }
//        isWaitingForStop = false;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//    }

//    private void EndTurn()
//    {
//        currentPlayer = 3 - currentPlayer;
//        SetActivePlayer(currentPlayer);
//    }

//    private void SetActivePlayer(int player)
//    {
//        player1Pamato.SetActive(player == 1);
//        player2Pamato.SetActive(player == 2);

//        if (player == 1 && player1Pamato.TryGetComponent<PamatoShooter>(out var shooter1))
//            shooter1.ResetTurn();

//        if (player == 2 && player2Pamato.TryGetComponent<PamatoShooter>(out var shooter2))
//            shooter2.ResetTurn();

//        UIJolen.Instance.UpdateTurn(player);

//        currentRb = null;
//        isWaitingForStop = false;
//        lowSpeedTimer = 0f;
//    }

//    public void MarbleKnockedOut(GameObject marble)
//    {
//        if (currentPlayer == 1) player1Score++;
//        else player2Score++;

//        UIJolen.Instance.UpdateScore(player1Score, player2Score);

//        if (player1Score >= winningScore) EndGame(1);
//        else if (player2Score >= winningScore) EndGame(2);
//    }

//    public int GetCurrentPlayer()
//    {
//        return currentPlayer;
//    }

//    private void EndGame(int winningPlayer)
//    {
//        UIJolen.Instance.ShowWinner(winningPlayer);

//        player1Pamato.SetActive(false);
//        player2Pamato.SetActive(false);

//        // Unlock Turumpo after Jolen finishes
//        if (ObjectiveManager.Instance != null)
//        {
//            ObjectiveManager.Instance.CompleteJolen();
//        }
//    }


//    public void NotifyPamatoFell()
//    {
//        isWaitingForStop = false;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//        EndTurn();
//    }
//}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class JolenGameManager : MonoBehaviour
//{
//    // Gabito's jolen

//    public static JolenGameManager Instance;

//    public GameObject player1Pamato;
//    public GameObject player2Pamato;

//    [Header("AI Settings")]
//    public bool isAIEnabled = false; // Toggle AI on/off in Inspector

//    private Rigidbody currentRb;
//    private bool isWaitingForStop = false;

//    private int player1Score = 0;
//    private int player2Score = 0;
//    private int currentPlayer = 1;
//    private bool hasStartedMoving = false;
//    public int winningScore = 5;

//    // NEW: Stall detection
//    private float lowSpeedTimer = 0f;
//    public float lowSpeedThreshold = 0.1f; // below this is "rolling slowly"
//    public float stallTimeLimit = 2f;      // if rolling slowly for 2s -> end turn

//    void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Update()
//    {
//        if (isWaitingForStop && currentRb != null)
//        {
//            // Did the pamato actually move?
//            if (currentRb.velocity.magnitude > 0.1f)
//                hasStartedMoving = true;

//            if (hasStartedMoving)
//            {
//                // Check if it's nearly stopped
//                if (currentRb.velocity.magnitude < lowSpeedThreshold)
//                {
//                    lowSpeedTimer += Time.deltaTime;

//                    if (lowSpeedTimer >= stallTimeLimit)
//                    {
//                        Debug.Log("Pamato stalled too long, ending turn...");
//                        ForceStopPamato();
//                        EndTurn();
//                    }
//                }
//                else
//                {
//                    // Reset if it picks up speed again
//                    lowSpeedTimer = 0f;
//                }
//            }

//            // Old rule: fully stopped
//            if (hasStartedMoving && currentRb.velocity.magnitude < 0.05f)
//            {
//                Debug.Log("Pamato stopped naturally, ending turn...");
//                isWaitingForStop = false;
//                hasStartedMoving = false;
//                lowSpeedTimer = 0f;
//                EndTurn();
//            }
//        }
//    }

//    public void NotifyShot(Rigidbody rb)
//    {
//        currentRb = rb;
//        isWaitingForStop = true;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//        Debug.Log("Player " + currentPlayer + " shot. Waiting for stop...");
//    }

//    private void ForceStopPamato()
//    {
//        if (currentRb != null)
//        {
//            currentRb.velocity = Vector3.zero;
//            currentRb.angularVelocity = Vector3.zero;
//        }
//        isWaitingForStop = false;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//    }

//    private void EndTurn()
//    {
//        currentPlayer = 3 - currentPlayer;
//        SetActivePlayer(currentPlayer);
//    }

//    private void SetActivePlayer(int player)
//    {
//        player1Pamato.SetActive(player == 1);
//        player2Pamato.SetActive(player == 2);

//        if (player == 1 && player1Pamato.TryGetComponent<PamatoShooter>(out var shooter1))
//            shooter1.ResetTurn();

//        if (player == 2)
//        {
//            if (isAIEnabled && player2Pamato.TryGetComponent<PamatoAI>(out var aiShooter))
//            {
//                aiShooter.ResetTurn();
//                // Trigger AI turn after a short delay
//                Invoke(nameof(TriggerAITurn), 0.5f);
//            }
//            else if (player2Pamato.TryGetComponent<PamatoShooter>(out var shooter2))
//            {
//                shooter2.ResetTurn();
//            }
//        }

//        UIJolen.Instance.UpdateTurn(player);

//        currentRb = null;
//        isWaitingForStop = false;
//        lowSpeedTimer = 0f;
//    }

//    private void TriggerAITurn()
//    {
//        if (player2Pamato.TryGetComponent<PamatoAI>(out var aiShooter))
//        {
//            aiShooter.TakeAITurn();
//        }
//    }

//    public void MarbleKnockedOut(GameObject marble)
//    {
//        if (currentPlayer == 1) player1Score++;
//        else player2Score++;

//        UIJolen.Instance.UpdateScore(player1Score, player2Score);

//        if (player1Score >= winningScore) EndGame(1);
//        else if (player2Score >= winningScore) EndGame(2);
//    }

//    public int GetCurrentPlayer()
//    {
//        return currentPlayer;
//    }

//    private void EndGame(int winningPlayer)
//    {
//        UIJolen.Instance.ShowWinner(winningPlayer);

//        player1Pamato.SetActive(false);
//        player2Pamato.SetActive(false);

//        // Unlock Turumpo after Jolen finishes
//        if (ObjectiveManager.Instance != null)
//        {
//            ObjectiveManager.Instance.CompleteJolen();
//        }
//    }


//    public void NotifyPamatoFell()
//    {
//        isWaitingForStop = false;
//        hasStartedMoving = false;
//        lowSpeedTimer = 0f;
//        EndTurn();
//    }

//    // Optional: Toggle AI during gameplay
//    public void ToggleAI(bool enabled)
//    {
//        isAIEnabled = enabled;
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JolenGameManager : MonoBehaviour
{
    // Gabito's jolen

    public static JolenGameManager Instance;

    public GameObject player1Pamato;
    public GameObject player2Pamato;

    [Header("AI Settings")]
    public bool isAIEnabled = false; // Toggle AI on/off in Inspector

    private Rigidbody currentRb;
    private bool isWaitingForStop = false;

    private int player1Score = 0;
    private int player2Score = 0;
    private int currentPlayer = 1;
    private bool hasStartedMoving = false;
    public int winningScore = 5;

    // NEW: Stall detection
    private float lowSpeedTimer = 0f;
    public float lowSpeedThreshold = 0.1f; // below this is "rolling slowly"
    public float stallTimeLimit = 2f;      // if rolling slowly for 2s -> end turn

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Initialize player 2 controls based on AI setting
        UpdatePlayer2Controls();
    }

    void Update()
    {
        if (isWaitingForStop && currentRb != null)
        {
            // Did the pamato actually move?
            if (currentRb.velocity.magnitude > 0.1f)
                hasStartedMoving = true;

            if (hasStartedMoving)
            {
                // Check if it's nearly stopped
                if (currentRb.velocity.magnitude < lowSpeedThreshold)
                {
                    lowSpeedTimer += Time.deltaTime;

                    if (lowSpeedTimer >= stallTimeLimit)
                    {
                        Debug.Log("Pamato stalled too long, ending turn...");
                        ForceStopPamato();
                        EndTurn();
                    }
                }
                else
                {
                    // Reset if it picks up speed again
                    lowSpeedTimer = 0f;
                }
            }

            // Old rule: fully stopped
            if (hasStartedMoving && currentRb.velocity.magnitude < 0.05f)
            {
                Debug.Log("Pamato stopped naturally, ending turn...");
                isWaitingForStop = false;
                hasStartedMoving = false;
                lowSpeedTimer = 0f;
                EndTurn();
            }
        }
    }

    public void NotifyShot(Rigidbody rb)
    {
        currentRb = rb;
        isWaitingForStop = true;
        hasStartedMoving = false;
        lowSpeedTimer = 0f;
        Debug.Log("Player " + currentPlayer + " shot. Waiting for stop...");
    }

    private void ForceStopPamato()
    {
        if (currentRb != null)
        {
            currentRb.velocity = Vector3.zero;
            currentRb.angularVelocity = Vector3.zero;
        }
        isWaitingForStop = false;
        hasStartedMoving = false;
        lowSpeedTimer = 0f;
    }

    private void EndTurn()
    {
        currentPlayer = 3 - currentPlayer;
        SetActivePlayer(currentPlayer);
    }

    private void SetActivePlayer(int player)
    {
        // Stop both pamatos completely before switching
        StopPamato(player1Pamato);
        StopPamato(player2Pamato);

        player1Pamato.SetActive(player == 1);
        player2Pamato.SetActive(player == 2);

        if (player == 1 && player1Pamato.TryGetComponent<PamatoShooter>(out var shooter1))
            shooter1.ResetTurn();

        if (player == 2)
        {
            // Always update controls when setting player 2 active
            UpdatePlayer2Controls();

            if (isAIEnabled && player2Pamato.TryGetComponent<PamatoAI>(out var aiShooter))
            {
                aiShooter.ResetTurn();
                Invoke(nameof(TriggerAITurn), 0.5f);
            }
            else if (player2Pamato.TryGetComponent<PamatoShooter>(out var shooter2))
            {
                shooter2.ResetTurn();
            }
        }

        UIJolen.Instance.UpdateTurn(player);

        currentRb = null;
        isWaitingForStop = false;
        lowSpeedTimer = 0f;
    }


    private void StopPamato(GameObject pamato)
    {
        if (pamato != null && pamato.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Optional: re-freeze rotation to prevent sliding on slopes
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
        }
    }



    private void TriggerAITurn()
    {
        if (player2Pamato.TryGetComponent<PamatoAI>(out var aiShooter))
        {
            aiShooter.TakeAITurn();
        }
    }

    public void MarbleKnockedOut(GameObject marble)
    {
        if (currentPlayer == 1) player1Score++;
        else player2Score++;

        UIJolen.Instance.UpdateScore(player1Score, player2Score);

        if (player1Score >= winningScore) EndGame(1);
        else if (player2Score >= winningScore) EndGame(2);
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    private void EndGame(int winningPlayer)
    {
        UIJolen.Instance.ShowWinner(winningPlayer);

        player1Pamato.SetActive(false);
        player2Pamato.SetActive(false);

        // Unlock Turumpo after Jolen finishes
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteJolen();
        }
    }

    public void NotifyPamatoFell()
    {
        isWaitingForStop = false;
        hasStartedMoving = false;
        lowSpeedTimer = 0f;
        EndTurn();
    }

    // Toggle AI during gameplay and update controls
    public void ToggleAI(bool enabled)
    {
        isAIEnabled = enabled;
        UpdatePlayer2Controls();
    }

    // Enable/disable appropriate components based on AI setting
    private void UpdatePlayer2Controls()
    {
        if (player2Pamato == null) return;

        PamatoShooter shooter = player2Pamato.GetComponent<PamatoShooter>();
        PamatoAI ai = player2Pamato.GetComponent<PamatoAI>();

        if (isAIEnabled)
        {
            // Disable manual shooter, enable AI
            if (shooter != null) shooter.enabled = false;
            if (ai != null) ai.enabled = true;
            Debug.Log("AI mode enabled for Player 2");
        }
        else
        {
            // Enable manual shooter, disable AI
            if (shooter != null) shooter.enabled = true;
            if (ai != null) ai.enabled = false;
            Debug.Log("Manual mode enabled for Player 2");
        }
    }
}