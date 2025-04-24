using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance;

    public GameObject miniGameUI;
    public TextMeshProUGUI promptText;

    private System.Action<bool> onResultCallback;
    private KeyCode expectedKey;
    private bool inputAllowed = false;
    private Coroutine activeRoutine;

    [Header("Mini-Game Settings")]
    [Tooltip("How long the defender has to react (in seconds)")]
    public float minTimeLimit = 0.5f;
    public float maxTimeLimit = 1.0f;

    [Header("Feedback Panel")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public float feedbackDelay = 1.5f;


    private readonly KeyCode[] player1Keys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.D };
    private readonly KeyCode[] player2Keys = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMiniGame(int defendingPlayer, System.Action<bool> callback)
    {
        onResultCallback = callback;
        if (miniGameUI != null) miniGameUI.SetActive(true);

        // Start the reaction game
        activeRoutine = StartCoroutine(KeyReactionMiniGame(defendingPlayer));
    }

    private IEnumerator KeyReactionMiniGame(int player)
    {
        KeyCode[] keyPool = player == 1 ? player1Keys : player2Keys;
        expectedKey = keyPool[Random.Range(0, keyPool.Length)];

        if (promptText != null)
            promptText.text = $"PRESS: {GetKeySymbol(expectedKey)}";

        inputAllowed = true;

        float timeLimit = Random.Range(minTimeLimit, maxTimeLimit);
        float elapsed = 0f;

        while (elapsed < timeLimit)
        {
            if (Input.GetKeyDown(expectedKey))
            {
                EndMiniGame(true);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        EndMiniGame(false);
    }

    private void EndMiniGame(bool success)
    {
        inputAllowed = false;
        if (miniGameUI != null)
            miniGameUI.SetActive(false);

        // Show feedback
        if (feedbackPanel != null && feedbackText != null)
        {
            feedbackText.text = success ? "✅ BLOCK SUCCESSFUL!" : "❌ BLOCK FAILED!";
            feedbackPanel.SetActive(true);
        }

        StartCoroutine(ContinueAfterDelay(success));
    }

    private IEnumerator ContinueAfterDelay(bool success)
    {
        yield return new WaitForSeconds(feedbackDelay);

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        onResultCallback?.Invoke(success);
    }


    void Update()
    {
        if (!inputAllowed) return;

        // Instant fail on wrong key
        if (Input.anyKeyDown && !Input.GetKeyDown(expectedKey))
        {
            EndMiniGame(false);
        }
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
