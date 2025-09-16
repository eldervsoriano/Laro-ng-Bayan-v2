// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class DefenderManager : MonoBehaviour
// {
//     public static DefenderManager Instance;
//     public GameObject miniGameUI;
//     public TextMeshProUGUI promptText;
//     private System.Action<bool> onResultCallback;
//     private bool inputAllowed = false;
//     private Coroutine activeRoutine;

//     [Header("Mini-Game Settings")]
//     [Tooltip("How long the slider takes to complete one cycle (in seconds)")]
//     public float sliderCycleTime = 2.0f;
//     [Tooltip("How close to the target line the slider needs to be (0.0 to 1.0)")]
//     public float successThreshold = 0.1f;
//     [Tooltip("Number of cycles before auto-fail")]
//     public int maxCycles = 3;
//     [Tooltip("Minimum position for target line (0.0 to 1.0)")]
//     public float minTargetPosition = 0.1f;
//     [Tooltip("Maximum position for target line (0.0 to 1.0)")]
//     public float maxTargetPosition = 0.9f;

//     [Header("Dynamic Difficulty Settings")]
//     [Tooltip("Minimum slider cycle time (fastest speed)")]
//     public float minCycleTime = 0.5f;
//     [Tooltip("Maximum slider cycle time (slowest speed)")]
//     public float maxCycleTime = 2.0f;
//     [Tooltip("Maximum score that affects difficulty")]
//     public int maxScore = 5;

//     [Header("Slider UI Elements")]
//     [Tooltip("The slider component for the mini-game")]
//     public Slider defenseSlider;
//     [Tooltip("Image that shows the target zone on the slider")]
//     public RectTransform targetLine;
//     [Tooltip("The moving indicator on the slider")]
//     public RectTransform sliderHandle;

//     [Header("Feedback Panel")]
//     public GameObject feedbackPanel;
//     public TextMeshProUGUI feedbackText;
//     public float feedbackDelay = 1.5f;

//     [Header("Player Block Images")]
//     [Tooltip("Image shown when Player 1 is attempting to block")]
//     public RawImage player1BlockAttemptImage;
//     [Tooltip("Image shown when Player 1 successfully blocks")]
//     public RawImage player1BlockSuccessImage;
//     [Tooltip("Image shown when Player 2 is attempting to block")]
//     public RawImage player2BlockAttemptImage;
//     [Tooltip("Image shown when Player 2 successfully blocks")]
//     public RawImage player2BlockSuccessImage;

//     private int currentDefendingPlayer;
//     private float targetPosition;
//     private int currentCycles;
//     private bool sliderMovingRight = true;

//     void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else Destroy(gameObject);

//         HideAllImages();
//     }

//     private void OnEnable()
//     {
//         HideAllImages();
//     }

//     void Start()
//     {
//         HideAllImages();

//         if (miniGameUI != null) miniGameUI.SetActive(false);

//         if (defenseSlider != null)
//         {
//             defenseSlider.minValue = 0f;
//             defenseSlider.maxValue = 1f;
//             defenseSlider.value = 0f;
//         }
//     }

//     private void HideAllImages()
//     {
//         if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//         if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//         if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);
//         if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//     }

//     public void StartMiniGame(int defendingPlayer, System.Action<bool> callback)
//     {
//         onResultCallback = callback;
//         currentDefendingPlayer = defendingPlayer;
//         currentCycles = 0;
//         sliderMovingRight = true;

//         HideAllImages();

//         if (miniGameUI != null) miniGameUI.SetActive(true);

//         if (defendingPlayer == 1 && player1BlockAttemptImage != null)
//             player1BlockAttemptImage.gameObject.SetActive(true);
//         else if (defendingPlayer == 2 && player2BlockAttemptImage != null)
//             player2BlockAttemptImage.gameObject.SetActive(true);

//         targetPosition = Random.Range(minTargetPosition, maxTargetPosition);
//         UpdateTargetLinePosition();

//         if (defenseSlider != null) defenseSlider.value = 0f;

//         if (promptText != null)
//             promptText.text = "PRESS <color=yellow>SPACE</color> when the slider hits the target line!";

//         // 🔥 Adjust difficulty based on the defending player's score
//         int playerScore = (defendingPlayer == 1)
//     ? TumbangGameManager.Instance.GetPlayer1Score()
//     : TumbangGameManager.Instance.GetPlayer2Score();


//         float t = Mathf.Clamp01((float)playerScore / maxScore);
//         sliderCycleTime = Mathf.Lerp(maxCycleTime, minCycleTime, t);

//         Debug.Log($"Player {defendingPlayer} score {playerScore} -> slider cycle time {sliderCycleTime:F2}");

//         activeRoutine = StartCoroutine(SliderMiniGame());
//     }

//     private void UpdateTargetLinePosition()
//     {
//         if (targetLine != null && defenseSlider != null)
//         {
//             RectTransform sliderRect = defenseSlider.GetComponent<RectTransform>();
//             RectTransform fillArea = defenseSlider.fillRect?.parent?.GetComponent<RectTransform>();
//             if (fillArea != null)
//             {
//                 float fillWidth = fillArea.rect.width;
//                 float targetX = (targetPosition - 0.5f) * fillWidth;
//                 targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
//             }
//             else
//             {
//                 float sliderWidth = sliderRect.rect.width;
//                 float targetX = (targetPosition - 0.5f) * sliderWidth;
//                 targetLine.anchoredPosition = new Vector2(targetX, targetLine.anchoredPosition.y);
//             }

//             targetLine.gameObject.SetActive(true);
//         }
//     }

//     private IEnumerator SliderMiniGame()
//     {
//         inputAllowed = true;

//         while (currentCycles < maxCycles)
//         {
//             if (sliderMovingRight)
//             {
//                 float elapsed = 0f;
//                 while (elapsed < sliderCycleTime / 2f)
//                 {
//                     float progress = elapsed / (sliderCycleTime / 2f);
//                     if (defenseSlider != null)
//                         defenseSlider.value = Mathf.Lerp(0f, 1f, progress);

//                     elapsed += Time.deltaTime;
//                     yield return null;
//                 }
//                 if (defenseSlider != null) defenseSlider.value = 1f;
//                 sliderMovingRight = false;
//             }
//             else
//             {
//                 float elapsed = 0f;
//                 while (elapsed < sliderCycleTime / 2f)
//                 {
//                     float progress = elapsed / (sliderCycleTime / 2f);
//                     if (defenseSlider != null)
//                         defenseSlider.value = Mathf.Lerp(1f, 0f, progress);

//                     elapsed += Time.deltaTime;
//                     yield return null;
//                 }
//                 if (defenseSlider != null) defenseSlider.value = 0f;
//                 sliderMovingRight = true;
//                 currentCycles++;
//             }
//         }

//         EndMiniGame(false);
//     }

//     private bool IsSliderInTargetZone()
//     {
//         if (defenseSlider == null) return false;

//         float currentValue = defenseSlider.value;
//         return Mathf.Abs(currentValue - targetPosition) <= successThreshold;
//     }

//     private void EndMiniGame(bool success)
//     {
//         inputAllowed = false;

//         if (activeRoutine != null)
//         {
//             StopCoroutine(activeRoutine);
//             activeRoutine = null;
//         }

//         if (miniGameUI != null) miniGameUI.SetActive(false);
//         if (targetLine != null) targetLine.gameObject.SetActive(false);

//         if (player1BlockAttemptImage != null) player1BlockAttemptImage.gameObject.SetActive(false);
//         if (player2BlockAttemptImage != null) player2BlockAttemptImage.gameObject.SetActive(false);

//         if (success)
//         {
//             if (currentDefendingPlayer == 1 && player1BlockSuccessImage != null)
//                 player1BlockSuccessImage.gameObject.SetActive(true);
//             else if (currentDefendingPlayer == 2 && player2BlockSuccessImage != null)
//                 player2BlockSuccessImage.gameObject.SetActive(true);
//         }
//         else
//         {
//             if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
//             if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);
//         }

//         if (feedbackPanel != null && feedbackText != null)
//         {
//             feedbackText.text = success ? "BLOCK SUCCESSFUL!" : "BLOCK FAILED!";
//             feedbackPanel.SetActive(true);
//         }

//         StartCoroutine(ContinueAfterDelay(success));
//     }

//     private IEnumerator ContinueAfterDelay(bool success)
//     {
//         yield return new WaitForSeconds(feedbackDelay);

//         HideAllImages();

//         if (feedbackPanel != null)
//             feedbackPanel.SetActive(false);

//         if (targetLine != null)
//             targetLine.gameObject.SetActive(false);

//         onResultCallback?.Invoke(success);
//     }

//     void Update()
//     {
//         if (!inputAllowed) return;

//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             bool success = IsSliderInTargetZone();
//             EndMiniGame(success);
//         }
//     }
// }


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

    private int currentDefendingPlayer;
    private float targetPosition;
    private int currentCycles;
    private bool sliderMovingRight = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        HideAllImages();

        // Make sure there’s an AudioSource
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

        if (promptText != null)
            promptText.text = "PRESS <color=yellow>SPACE</color> when the slider hits the target line!";

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

            // 🔊 Play success sound
            if (successClip != null && audioSource != null)
                audioSource.PlayOneShot(successClip);
        }
        else
        {
            if (player1BlockSuccessImage != null) player1BlockSuccessImage.gameObject.SetActive(false);
            if (player2BlockSuccessImage != null) player2BlockSuccessImage.gameObject.SetActive(false);

            // 🔊 Play fail sound
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = IsSliderInTargetZone();
            EndMiniGame(success);
        }
    }
}
