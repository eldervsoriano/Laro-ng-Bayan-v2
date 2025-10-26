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
//    public float sliderCycleTime = 2.0f;
//    public float successThreshold = 0.1f;
//    public int maxCycles = 3;
//    public float minTargetPosition = 0.1f;
//    public float maxTargetPosition = 0.9f;

//    [Header("Dynamic Difficulty Settings")]
//    public float minCycleTime = 0.5f;
//    public float maxCycleTime = 2.0f;
//    public int maxScore = 5;

//    [Header("Slider UI Elements")]
//    public Slider defenseSlider;
//    public RectTransform targetLine;
//    public RectTransform sliderHandle;

//    [Header("Feedback Panel")]
//    public GameObject feedbackPanel;
//    public TextMeshProUGUI feedbackText;
//    public float feedbackDelay = 1.5f;

//    [Header("Player Block Images")]
//    public RawImage player1BlockAttemptImage;
//    public RawImage player1BlockSuccessImage;
//    public RawImage player2BlockAttemptImage;
//    public RawImage player2BlockSuccessImage;

//    [Header("Sound Effects")]
//    public AudioClip successClip;
//    public AudioClip failClip;
//    private AudioSource audioSource;

//    private int currentDefendingPlayer;
//    private float targetPosition;
//    private int currentCycles;
//    private bool sliderMovingRight = true;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        HideAllImages();

//        // Make sure there’s an AudioSource
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//            audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    private void OnEnable()
//    {
//        HideAllImages();
//    }

//    void Start()
//    {
//        HideAllImages();

//        if (miniGameUI != null) miniGameUI.SetActive(false);

//        if (defenseSlider != null)
//        {
//            defenseSlider.minValue = 0f;
//            defenseSlider.maxValue = 1f;
//            defenseSlider.value = 0f;
//        }
//    }

//    private void HideAllImages()
//    {
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

//        HideAllImages();

//        if (miniGameUI != null) miniGameUI.SetActive(true);

//        if (defendingPlayer == 1 && player1BlockAttemptImage != null)
//            player1BlockAttemptImage.gameObject.SetActive(true);
//        else if (defendingPlayer == 2 && player2BlockAttemptImage != null)
//            player2BlockAttemptImage.gameObject.SetActive(true);

//        targetPosition = Random.Range(minTargetPosition, maxTargetPosition);
//        UpdateTargetLinePosition();

//        if (defenseSlider != null) defenseSlider.value = 0f;

//        if (promptText != null)
//            promptText.text = "PRESS <color=yellow>SPACE</color> when the slider hits the target line!";

//        int playerScore = (defendingPlayer == 1)
//            ? TumbangGameManager.Instance.GetPlayer1Score()
//            : TumbangGameManager.Instance.GetPlayer2Score();

//        float t = Mathf.Clamp01((float)playerScore / maxScore);
//        sliderCycleTime = Mathf.Lerp(maxCycleTime, minCycleTime, t);

//        Debug.Log($"Player {defendingPlayer} score {playerScore} -> slider cycle time {sliderCycleTime:F2}");

//        activeRoutine = StartCoroutine(SliderMiniGame());
//    }

//    private void UpdateTargetLinePosition()
//    {
//        if (targetLine != null && defenseSlider != null)
//        {
//            RectTransform sliderRect = defenseSlider.GetComponent<RectTransform>();
//            RectTransform fillArea = defenseSlider.fillRect?.parent?.GetComponent<RectTransform>();
//            if (fillArea != null)
//            {
//                float fillWidth = fillArea.rect.width;
//                float targetX = (targetPosition - 0.5f) * fillWidth;
//                targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
//            }
//            else
//            {
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

//        if (activeRoutine != null)
//        {
//            StopCoroutine(activeRoutine);
//            activeRoutine = null;
//        }

//        if (miniGameUI != null) miniGameUI.SetActive(false);
//        if (targetLine != null) targetLine.gameObject.SetActive(false);

//        if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//        if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);

//        if (success)
//        {
//            if (currentDefendingPlayer == 1 && player1BlockSuccessImage != null)
//                player1BlockSuccessImage.gameObject.SetActive(true);
//            else if (currentDefendingPlayer == 2 && player2BlockSuccessImage != null)
//                player2BlockSuccessImage.gameObject.SetActive(true);

//            // 🔊 Play success sound
//            if (successClip != null && audioSource != null)
//                audioSource.PlayOneShot(successClip);
//        }
//        else
//        {
//            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);

//            // 🔊 Play fail sound
//            if (failClip != null && audioSource != null)
//                audioSource.PlayOneShot(failClip);
//        }

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

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            bool success = IsSliderInTargetZone();
//            EndMiniGame(success);
//        }
//    }
//}


//AI

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
    public float sliderCycleTime = 2.0f;
    public float successThreshold = 0.1f;
    public int maxCycles = 3;
    public float minTargetPosition = 0.1f;
    public float maxTargetPosition = 0.9f;

    [Header("Dynamic Difficulty Settings")]
    public float minCycleTime = 0.5f;
    public float maxCycleTime = 2.0f;
    public int maxScore = 5;

    [Header("Slider UI Elements")]
    public Slider defenseSlider;
    public RectTransform targetLine;
    public RectTransform sliderHandle;

    [Header("Feedback Panel")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public float feedbackDelay = 1.5f;

    [Header("Player Block Images")]
    public RawImage player1BlockAttemptImage;
    public RawImage player1BlockSuccessImage;
    public RawImage player2BlockAttemptImage;
    public RawImage player2BlockSuccessImage;

    [Header("Sound Effects")]
    public AudioClip successClip;
    public AudioClip failClip;
    private AudioSource audioSource;

    [Header("AI References")]
    public AIPlayer player1AI;
    public AIPlayer player2AI;

    private int currentDefendingPlayer;
    private float targetPosition;
    private int currentCycles;
    private bool sliderMovingRight = true;
    private Coroutine aiDefenseCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HideAllImages();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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

        // Check if defending player is AI
        bool isDefenderAI = IsPlayerAI(defendingPlayer);

        if (promptText != null)
        {
            if (isDefenderAI)
                promptText.text = $"<color=orange>AI Player {defendingPlayer} is defending...</color>";
            else
                promptText.text = "PRESS <color=yellow>SPACE</color> when the yellow bar hits the <color=red>red bar!</color>";
        }

        int playerScore = (defendingPlayer == 1)
            ? TumbangGameManager.Instance.GetPlayer1Score()
            : TumbangGameManager.Instance.GetPlayer2Score();

        float t = Mathf.Clamp01((float)playerScore / maxScore);
        sliderCycleTime = Mathf.Lerp(maxCycleTime, minCycleTime, t);

        Debug.Log($"Player {defendingPlayer} score {playerScore} -> slider cycle time {sliderCycleTime:F2}");

        // Start AI defense BEFORE starting slider
        if (isDefenderAI)
        {
            AIPlayer aiPlayer = GetAIPlayer(defendingPlayer);
            if (aiPlayer != null)
            {
                Debug.Log($"🤖 Starting AI defense for Player {defendingPlayer}");
                aiDefenseCoroutine = StartCoroutine(AIDefenseMonitor(aiPlayer));
            }
            else
            {
                Debug.LogError($"❌ AI Player {defendingPlayer} reference is missing!");
            }
        }

        activeRoutine = StartCoroutine(SliderMiniGame());
    }

    private AIPlayer GetAIPlayer(int playerNumber)
    {
        if (playerNumber == 1) return player1AI;
        if (playerNumber == 2) return player2AI;
        return null;
    }

    private IEnumerator AIDefenseMonitor(AIPlayer aiPlayer)
    {
        // Wait for initial reaction
        yield return new WaitForSeconds(aiPlayer.defenseReactionTime * Random.Range(0.9f, 1.5f));

        Debug.Log($"🤖 AI DEFENSE: Monitoring started for Player {aiPlayer.playerNumber}");

        // Base defense accuracy
        float currentDefenseAccuracy = aiPlayer.defenseAccuracy;
        if (aiPlayer.scaleDifficulty)
        {
            int myScore = (aiPlayer.playerNumber == 1)
                ? TumbangGameManager.Instance.GetPlayer1Score()
                : TumbangGameManager.Instance.GetPlayer2Score();

            float difficultyProgress = Mathf.Clamp01((float)myScore / aiPlayer.scoreThresholdForMax);
            currentDefenseAccuracy = Mathf.Lerp(aiPlayer.minAccuracy, aiPlayer.maxAccuracy, difficultyProgress);
        }

        // 🎲 1. Random fail chance — sometimes just too slow or off-timed
        if (Random.value > currentDefenseAccuracy)
        {
            float failDelay = Random.Range(0.2f, 0.5f);
            Debug.Log($"💤 AI failed to react properly! Delaying press by {failDelay:F2}s (Acc: {currentDefenseAccuracy:F2})");
            yield return new WaitForSeconds(failDelay);
        }

        // 🎯 2. Increase timing error range
        float baseError = Mathf.Lerp(0.15f, 0.02f, currentDefenseAccuracy); // 0.15 (bad) to 0.02 (good)
        float aiTargetTiming = targetPosition + Random.Range(-baseError, baseError);
        aiTargetTiming = Mathf.Clamp01(aiTargetTiming);

        Debug.Log($"🎯 AI target timing set to {aiTargetTiming:F2} (def acc: {currentDefenseAccuracy:F2})");

        // 🎛️ 3. AI tries to press only once per defense window
        bool hasPressed = false;
        float elapsed = 0f;
        float timeout = maxCycles * sliderCycleTime + 1.0f;

        while (!hasPressed && elapsed < timeout && inputAllowed)
        {
            if (defenseSlider == null)
            {
                Debug.LogError("❌ Defense slider is null!");
                yield break;
            }

            float currentValue = defenseSlider.value;
            float distance = Mathf.Abs(currentValue - aiTargetTiming);

            // Add some delay to simulate "thinking"
            if (distance <= successThreshold)
            {
                hasPressed = true;

                // 💥 Add random reaction jitter
                float jitterDelay = Random.Range(0f, 0.1f * (1f - currentDefenseAccuracy));
                yield return new WaitForSeconds(jitterDelay);

                bool success = IsSliderInTargetZone();

                // Add small chance of missing even when inside target zone
                if (success && Random.value > currentDefenseAccuracy)
                    success = false;

                Debug.Log($"🤖 AI pressed (Acc: {currentDefenseAccuracy:F2}) | Target: {targetPosition:F2}, Actual: {currentValue:F2}, Result: {(success ? "SUCCESS" : "FAIL")}");
                EndMiniGame(success);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!hasPressed)
            Debug.Log($"⏱️ AI failed to react in time (Acc: {currentDefenseAccuracy:F2})");
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
                    if (!inputAllowed) yield break; // Stop if game ended

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
                    if (!inputAllowed) yield break; // Stop if game ended

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

        // Only end if still active (AI might have already ended it)
        if (inputAllowed)
        {
            EndMiniGame(false);
        }
    }

    private bool IsSliderInTargetZone()
    {
        if (defenseSlider == null) return false;

        float currentValue = defenseSlider.value;
        float distance = Mathf.Abs(currentValue - targetPosition);
        Debug.Log($"Checking target zone: current={currentValue:F2}, target={targetPosition:F2}, distance={distance:F3}, threshold={successThreshold:F2}");
        return distance <= successThreshold;
    }

    private bool IsPlayerAI(int playerNumber)
    {
        if (playerNumber == 1 && player1AI != null)
            return player1AI.isAI;
        if (playerNumber == 2 && player2AI != null)
            return player2AI.isAI;
        return false;
    }

    private void EndMiniGame(bool success)
    {
        inputAllowed = false;

        // Stop all coroutines
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            activeRoutine = null;
        }

        if (aiDefenseCoroutine != null)
        {
            StopCoroutine(aiDefenseCoroutine);
            aiDefenseCoroutine = null;
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

            if (successClip != null && audioSource != null)
                audioSource.PlayOneShot(successClip);
        }
        else
        {
            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);

            if (failClip != null && audioSource != null)
                audioSource.PlayOneShot(failClip);
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

        // Only allow manual input if current defender is not AI
        bool isDefenderAI = IsPlayerAI(currentDefendingPlayer);
        if (!isDefenderAI && Input.GetKeyDown(KeyCode.Space))
        {
            bool success = IsSliderInTargetZone();
            EndMiniGame(success);
        }
    }
}