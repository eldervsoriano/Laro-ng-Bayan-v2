//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections;
//public class DefenderManager : MonoBehaviour
//{
//    public static DefenderManager Instance;
//    public GameObject miniGameUI;
//    public TextMeshProUGUI promptText;
//    private System.Action<bool> onResultCallback;
//    private KeyCode expectedKey;
//    private bool inputAllowed = false;
//    private Coroutine activeRoutine;

//    [Header("Mini-Game Settings")]
//    [Tooltip("How long the defender has to react (in seconds)")]
//    public float minTimeLimit = 0.5f;
//    public float maxTimeLimit = 1.0f;

//    [Header("Feedback Panel")]
//    public GameObject feedbackPanel;
//    public TextMeshProUGUI feedbackText;
//    public float feedbackDelay = 1.5f;

//    [Header("Player Block Images")]
//    [Tooltip("Image shown when Player 1 is attempting to block")]
//    public RawImage player1BlockAttemptImage;
//    [Tooltip("Image shown when Player 1 successfully blocks")]
//    public RawImage player1BlockSuccessImage;
//    [Tooltip("Image shown when Player 2 is attempting to block")]
//    public RawImage player2BlockAttemptImage;
//    [Tooltip("Image shown when Player 2 successfully blocks")]
//    public RawImage player2BlockSuccessImage;

//    private readonly KeyCode[] player1Keys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.D };
//    private readonly KeyCode[] player2Keys = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };

//    private int currentDefendingPlayer;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        // Make sure all images are initially disabled
//        HideAllImages();
//    }

//    private void OnEnable()
//    {
//        // Ensure images are hidden when the component is enabled
//        HideAllImages();
//    }

//    void Start()
//    {
//        // Make sure images are hidden at start as well
//        HideAllImages();

//        // Make sure the miniGameUI is hidden initially
//        if (miniGameUI != null) miniGameUI.SetActive(false);
//    }

//    private void HideAllImages()
//    {
//        // Make sure all block images are disabled
//        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//        if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);
//        if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//    }

//    public void StartMiniGame(int defendingPlayer, System.Action<bool> callback)
//    {
//        onResultCallback = callback;
//        currentDefendingPlayer = defendingPlayer;

//        // Ensure all images are hidden before starting
//        HideAllImages();

//        // Activate the mini-game UI
//        if (miniGameUI != null) miniGameUI.SetActive(true);

//        // Only now that the mini-game UI is active, show the appropriate attempting block image
//        if (defendingPlayer == 1 && player1BlockAttemptImage != null)
//            player1BlockAttemptImage.gameObject.SetActive(true);
//        else if (defendingPlayer == 2 && player2BlockAttemptImage != null)
//            player2BlockAttemptImage.gameObject.SetActive(true);

//        // Start the reaction game
//        activeRoutine = StartCoroutine(KeyReactionMiniGame(defendingPlayer));
//    }

//    private IEnumerator KeyReactionMiniGame(int player)
//    {
//        KeyCode[] keyPool = player == 1 ? player1Keys : player2Keys;
//        expectedKey = keyPool[Random.Range(0, keyPool.Length)];

//        if (promptText != null)
//            promptText.text = $"PRESS: {GetKeySymbol(expectedKey)}";

//        inputAllowed = true;
//        float timeLimit = Random.Range(minTimeLimit, maxTimeLimit);
//        float elapsed = 0f;

//        while (elapsed < timeLimit)
//        {
//            if (Input.GetKeyDown(expectedKey))
//            {
//                EndMiniGame(true);
//                yield break;
//            }

//            elapsed += Time.deltaTime;
//            yield return null;
//        }

//        EndMiniGame(false);
//    }

//    private void EndMiniGame(bool success)
//    {
//        inputAllowed = false;

//        // First hide the mini-game UI
//        if (miniGameUI != null)
//            miniGameUI.SetActive(false);

//        // Hide all attempt images first
//        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);

//        // Show the appropriate success image if block was successful, otherwise hide all
//        if (success)
//        {
//            if (currentDefendingPlayer == 1 && player1BlockSuccessImage != null)
//                player1BlockSuccessImage.gameObject.SetActive(true);
//            else if (currentDefendingPlayer == 2 && player2BlockSuccessImage != null)
//                player2BlockSuccessImage.gameObject.SetActive(true);
//        }
//        else
//        {
//            // Ensure success images are also hidden on failure
//            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//        }

//        // Show feedback
//        if (feedbackPanel != null && feedbackText != null)
//        {
//            feedbackText.text = success ? "BLOCK SUCCESSFUL!" : "BLOCK FAILED!";
//            feedbackPanel.SetActive(true);
//        }

//        StartCoroutine(ContinueAfterDelay(success));
//    }

//    private IEnumerator ContinueAfterDelay(bool success)
//    {
//        yield return new WaitForSeconds(feedbackDelay);

//        // Hide any active images and panels
//        HideAllImages();

//        if (feedbackPanel != null)
//            feedbackPanel.SetActive(false);

//        onResultCallback?.Invoke(success);
//    }

//    void Update()
//    {
//        if (!inputAllowed) return;

//        // Instant fail on wrong key
//        if (Input.anyKeyDown && !Input.GetKeyDown(expectedKey))
//        {
//            EndMiniGame(false);
//        }
//    }

//    private string GetKeySymbol(KeyCode key)
//    {
//        switch (key)
//        {
//            case KeyCode.W: return "W";
//            case KeyCode.A: return "A";
//            case KeyCode.S: return "S";
//            case KeyCode.D: return "D";
//            case KeyCode.UpArrow: return "↑";
//            case KeyCode.DownArrow: return "↓";
//            case KeyCode.LeftArrow: return "←";
//            case KeyCode.RightArrow: return "→";
//            default: return key.ToString();
//        }
//    }
//}

//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections;

//public class DefenderManager : MonoBehaviour
//{
//    public static DefenderManager Instance;
//    public GameObject miniGameUI;
//    public TextMeshProUGUI promptText;
//    private System.Action<bool> onResultCallback;
//    private bool inputAllowed = false;
//    private Coroutine activeRoutine;

//    [Header("Mini-Game Settings")]
//    [Tooltip("How long the slider takes to complete one cycle (in seconds)")]
//    public float sliderCycleTime = 2.0f;
//    [Tooltip("How close to the target line the slider needs to be (0.0 to 1.0)")]
//    public float successThreshold = 0.1f;
//    [Tooltip("Number of cycles before auto-fail")]
//    public int maxCycles = 3;
//    [Tooltip("Minimum position for target line (0.0 to 1.0)")]
//    public float minTargetPosition = 0.1f;
//    [Tooltip("Maximum position for target line (0.0 to 1.0)")]
//    public float maxTargetPosition = 0.9f;

//    [Header("Slider UI Elements")]
//    [Tooltip("The slider component for the mini-game")]
//    public Slider defenseSlider;
//    [Tooltip("Image that shows the target zone on the slider")]
//    public RectTransform targetLine;
//    [Tooltip("The moving indicator on the slider")]
//    public RectTransform sliderHandle;

//    [Header("Feedback Panel")]
//    public GameObject feedbackPanel;
//    public TextMeshProUGUI feedbackText;
//    public float feedbackDelay = 1.5f;

//    [Header("Player Block Images")]
//    [Tooltip("Image shown when Player 1 is attempting to block")]
//    public RawImage player1BlockAttemptImage;
//    [Tooltip("Image shown when Player 1 successfully blocks")]
//    public RawImage player1BlockSuccessImage;
//    [Tooltip("Image shown when Player 2 is attempting to block")]
//    public RawImage player2BlockAttemptImage;
//    [Tooltip("Image shown when Player 2 successfully blocks")]
//    public RawImage player2BlockSuccessImage;

//    private int currentDefendingPlayer;
//    private float targetPosition;
//    private int currentCycles;
//    private bool sliderMovingRight = true;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        // Make sure all images are initially disabled
//        HideAllImages();
//    }

//    private void OnEnable()
//    {
//        // Ensure images are hidden when the component is enabled
//        HideAllImages();
//    }

//    void Start()
//    {
//        // Make sure images are hidden at start as well
//        HideAllImages();

//        // Make sure the miniGameUI is hidden initially
//        if (miniGameUI != null) miniGameUI.SetActive(false);

//        // Initialize slider
//        if (defenseSlider != null)
//        {
//            defenseSlider.minValue = 0f;
//            defenseSlider.maxValue = 1f;
//            defenseSlider.value = 0f;
//        }
//    }

//    private void HideAllImages()
//    {
//        // Make sure all block images are disabled
//        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//        if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);
//        if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//    }

//    public void StartMiniGame(int defendingPlayer, System.Action<bool> callback)
//    {
//        onResultCallback = callback;
//        currentDefendingPlayer = defendingPlayer;
//        currentCycles = 0;
//        sliderMovingRight = true;

//        // Ensure all images are hidden before starting
//        HideAllImages();

//        // Activate the mini-game UI
//        if (miniGameUI != null) miniGameUI.SetActive(true);

//        // Show the appropriate attempting block image
//        if (defendingPlayer == 1 && player1BlockAttemptImage != null)
//            player1BlockAttemptImage.gameObject.SetActive(true);
//        else if (defendingPlayer == 2 && player2BlockAttemptImage != null)
//            player2BlockAttemptImage.gameObject.SetActive(true);

//        // Set random target position within the specified range
//        targetPosition = Random.Range(minTargetPosition, maxTargetPosition);
//        UpdateTargetLinePosition();

//        Debug.Log($"New target position set to: {targetPosition:F2}"); // Optional debug info

//        // Reset slider position
//        if (defenseSlider != null)
//            defenseSlider.value = 0f;

//        // Update prompt text
//        if (promptText != null)
//            promptText.text = "PRESS SPACE when the slider hits the target line!";

//        // Start the slider mini-game
//        activeRoutine = StartCoroutine(SliderMiniGame());
//    }

//    private void UpdateTargetLinePosition()
//    {
//        if (targetLine != null && defenseSlider != null)
//        {
//            // Position the target line based on the target position (0.0 = left, 1.0 = right)
//            RectTransform sliderRect = defenseSlider.GetComponent<RectTransform>();

//            // Get the slider's fill area to position the target line correctly
//            RectTransform fillArea = defenseSlider.fillRect?.parent?.GetComponent<RectTransform>();
//            if (fillArea != null)
//            {
//                float fillWidth = fillArea.rect.width;
//                float targetX = (targetPosition - 0.5f) * fillWidth;
//                targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
//            }
//            else
//            {
//                // Fallback positioning method
//                float sliderWidth = sliderRect.rect.width;
//                float targetX = (targetPosition - 0.5f) * sliderWidth;
//                targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
//            }

//            targetLine.gameObject.SetActive(true);
//        }
//    }

//    private IEnumerator SliderMiniGame()
//    {
//        inputAllowed = true;

//        while (currentCycles < maxCycles)
//        {
//            // Move slider from 0 to 1
//            if (sliderMovingRight)
//            {
//                float elapsed = 0f;
//                while (elapsed < sliderCycleTime / 2f)
//                {
//                    float progress = elapsed / (sliderCycleTime / 2f);
//                    if (defenseSlider != null)
//                        defenseSlider.value = Mathf.Lerp(0f, 1f, progress);

//                    elapsed += Time.deltaTime;
//                    yield return null;
//                }
//                if (defenseSlider != null) defenseSlider.value = 1f;
//                sliderMovingRight = false;
//            }
//            // Move slider from 1 to 0
//            else
//            {
//                float elapsed = 0f;
//                while (elapsed < sliderCycleTime / 2f)
//                {
//                    float progress = elapsed / (sliderCycleTime / 2f);
//                    if (defenseSlider != null)
//                        defenseSlider.value = Mathf.Lerp(1f, 0f, progress);

//                    elapsed += Time.deltaTime;
//                    yield return null;
//                }
//                if (defenseSlider != null) defenseSlider.value = 0f;
//                sliderMovingRight = true;
//                currentCycles++;
//            }
//        }

//        // Failed - ran out of cycles
//        EndMiniGame(false);
//    }

//    private bool IsSliderInTargetZone()
//    {
//        if (defenseSlider == null) return false;

//        float currentValue = defenseSlider.value;
//        return Mathf.Abs(currentValue - targetPosition) <= successThreshold;
//    }

//    private void EndMiniGame(bool success)
//    {
//        inputAllowed = false;

//        // Stop the active routine
//        if (activeRoutine != null)
//        {
//            StopCoroutine(activeRoutine);
//            activeRoutine = null;
//        }

//        // First hide the mini-game UI
//        if (miniGameUI != null)
//            miniGameUI.SetActive(false);

//        // Hide the target line
//        if (targetLine != null)
//            targetLine.gameObject.SetActive(false);

//        // Hide all attempt images first
//        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);

//        // Show the appropriate success image if block was successful, otherwise hide all
//        if (success)
//        {
//            if (currentDefendingPlayer == 1 && player1BlockSuccessImage != null)
//                player1BlockSuccessImage.gameObject.SetActive(true);
//            else if (currentDefendingPlayer == 2 && player2BlockSuccessImage != null)
//                player2BlockSuccessImage.gameObject.SetActive(true);
//        }
//        else
//        {
//            // Ensure success images are also hidden on failure
//            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//        }

//        // Show feedback
//        if (feedbackPanel != null && feedbackText != null)
//        {
//            feedbackText.text = success ? "BLOCK SUCCESSFUL!" : "BLOCK FAILED!";
//            feedbackPanel.SetActive(true);
//        }

//        StartCoroutine(ContinueAfterDelay(success));
//    }

//    private IEnumerator ContinueAfterDelay(bool success)
//    {
//        yield return new WaitForSeconds(feedbackDelay);

//        // Hide any active images and panels
//        HideAllImages();

//        if (feedbackPanel != null)
//            feedbackPanel.SetActive(false);

//        if (targetLine != null)
//            targetLine.gameObject.SetActive(false);

//        onResultCallback?.Invoke(success);
//    }

//    void Update()
//    {
//        if (!inputAllowed) return;

//        // Check for space key press
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            bool success = IsSliderInTargetZone();
//            EndMiniGame(success);
//        }
//    }
//}

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
    private bool inputAllowed = false;
    private Coroutine activeRoutine;

    [Header("Mini-Game Settings")]
    [Tooltip("How long the slider takes to complete one cycle (in seconds)")]
    public float sliderCycleTime = 2.0f;
    [Tooltip("How close to the target line the slider needs to be (0.0 to 1.0)")]
    public float successThreshold = 0.1f;
    [Tooltip("Number of cycles before auto-fail")]
    public int maxCycles = 3;
    [Tooltip("Minimum position for target line (0.0 to 1.0)")]
    public float minTargetPosition = 0.1f;
    [Tooltip("Maximum position for target line (0.0 to 1.0)")]
    public float maxTargetPosition = 0.9f;

    [Header("Dynamic Difficulty Settings")]
    [Tooltip("Minimum slider cycle time (fastest speed)")]
    public float minCycleTime = 0.5f;
    [Tooltip("Maximum slider cycle time (slowest speed)")]
    public float maxCycleTime = 2.0f;
    [Tooltip("Maximum score that affects difficulty")]
    public int maxScore = 5;

    [Header("Slider UI Elements")]
    [Tooltip("The slider component for the mini-game")]
    public Slider defenseSlider;
    [Tooltip("Image that shows the target zone on the slider")]
    public RectTransform targetLine;
    [Tooltip("The moving indicator on the slider")]
    public RectTransform sliderHandle;

    [Header("Feedback Panel")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public float feedbackDelay = 1.5f;

    [Header("Player Block Images")]
    [Tooltip("Image shown when Player 1 is attempting to block")]
    public RawImage player1BlockAttemptImage;
    [Tooltip("Image shown when Player 1 successfully blocks")]
    public RawImage player1BlockSuccessImage;
    [Tooltip("Image shown when Player 2 is attempting to block")]
    public RawImage player2BlockAttemptImage;
    [Tooltip("Image shown when Player 2 successfully blocks")]
    public RawImage player2BlockSuccessImage;

    private int currentDefendingPlayer;
    private float targetPosition;
    private int currentCycles;
    private bool sliderMovingRight = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HideAllImages();
    }

    private void OnEnable()
    {
        HideAllImages();
    }

    void Start()
    {
        HideAllImages();

        if (miniGameUI != null) miniGameUI.SetActive(false);

        if (defenseSlider != null)
        {
            defenseSlider.minValue = 0f;
            defenseSlider.maxValue = 1f;
            defenseSlider.value = 0f;
        }
    }

    private void HideAllImages()
    {
        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
        if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);
        if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
    }

    public void StartMiniGame(int defendingPlayer, System.Action<bool> callback)
    {
        onResultCallback = callback;
        currentDefendingPlayer = defendingPlayer;
        currentCycles = 0;
        sliderMovingRight = true;

        HideAllImages();

        if (miniGameUI != null) miniGameUI.SetActive(true);

        if (defendingPlayer == 1 && player1BlockAttemptImage != null)
            player1BlockAttemptImage.gameObject.SetActive(true);
        else if (defendingPlayer == 2 && player2BlockAttemptImage != null)
            player2BlockAttemptImage.gameObject.SetActive(true);

        targetPosition = Random.Range(minTargetPosition, maxTargetPosition);
        UpdateTargetLinePosition();

        if (defenseSlider != null) defenseSlider.value = 0f;

        if (promptText != null)
            promptText.text = "PRESS SPACE when the slider hits the target line!";

        // 🔥 Adjust difficulty based on the defending player's score
        int playerScore = (defendingPlayer == 1)
    ? TumbangGameManager.Instance.GetPlayer1Score()
    : TumbangGameManager.Instance.GetPlayer2Score();

        float t = Mathf.Clamp01((float)playerScore / maxScore);
        sliderCycleTime = Mathf.Lerp(maxCycleTime, minCycleTime, t);

        Debug.Log($"Player {defendingPlayer} score {playerScore} -> slider cycle time {sliderCycleTime:F2}");

        activeRoutine = StartCoroutine(SliderMiniGame());
    }

    private void UpdateTargetLinePosition()
    {
        if (targetLine != null && defenseSlider != null)
        {
            RectTransform sliderRect = defenseSlider.GetComponent<RectTransform>();
            RectTransform fillArea = defenseSlider.fillRect?.parent?.GetComponent<RectTransform>();
            if (fillArea != null)
            {
                float fillWidth = fillArea.rect.width;
                float targetX = (targetPosition - 0.5f) * fillWidth;
                targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
            }
            else
            {
                float sliderWidth = sliderRect.rect.width;
                float targetX = (targetPosition - 0.5f) * sliderWidth;
                targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
            }

            targetLine.gameObject.SetActive(true);
        }
    }

    private IEnumerator SliderMiniGame()
    {
        inputAllowed = true;

        while (currentCycles < maxCycles)
        {
            if (sliderMovingRight)
            {
                float elapsed = 0f;
                while (elapsed < sliderCycleTime / 2f)
                {
                    float progress = elapsed / (sliderCycleTime / 2f);
                    if (defenseSlider != null)
                        defenseSlider.value = Mathf.Lerp(0f, 1f, progress);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
                if (defenseSlider != null) defenseSlider.value = 1f;
                sliderMovingRight = false;
            }
            else
            {
                float elapsed = 0f;
                while (elapsed < sliderCycleTime / 2f)
                {
                    float progress = elapsed / (sliderCycleTime / 2f);
                    if (defenseSlider != null)
                        defenseSlider.value = Mathf.Lerp(1f, 0f, progress);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
                if (defenseSlider != null) defenseSlider.value = 0f;
                sliderMovingRight = true;
                currentCycles++;
            }
        }

        EndMiniGame(false);
    }

    private bool IsSliderInTargetZone()
    {
        if (defenseSlider == null) return false;

        float currentValue = defenseSlider.value;
        return Mathf.Abs(currentValue - targetPosition) <= successThreshold;
    }

    private void EndMiniGame(bool success)
    {
        inputAllowed = false;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (miniGameUI != null) miniGameUI.SetActive(false);
        if (targetLine != null) targetLine.gameObject.SetActive(false);

        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);

        if (success)
        {
            if (currentDefendingPlayer == 1 && player1BlockSuccessImage != null)
                player1BlockSuccessImage.gameObject.SetActive(true);
            else if (currentDefendingPlayer == 2 && player2BlockSuccessImage != null)
                player2BlockSuccessImage.gameObject.SetActive(true);
        }
        else
        {
            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
        }

        if (feedbackPanel != null && feedbackText != null)
        {
            feedbackText.text = success ? "BLOCK SUCCESSFUL!" : "BLOCK FAILED!";
            feedbackPanel.SetActive(true);
        }

        StartCoroutine(ContinueAfterDelay(success));
    }

    private IEnumerator ContinueAfterDelay(bool success)
    {
        yield return new WaitForSeconds(feedbackDelay);

        HideAllImages();

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        if (targetLine != null)
            targetLine.gameObject.SetActive(false);

        onResultCallback?.Invoke(success);
    }

    void Update()
    {
        if (!inputAllowed) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = IsSliderInTargetZone();
            EndMiniGame(success);
        }
    }
}
