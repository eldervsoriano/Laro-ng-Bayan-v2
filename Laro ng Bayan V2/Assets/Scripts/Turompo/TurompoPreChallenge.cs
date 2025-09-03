//using System.Collections;
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class TurompoPreChallenge : MonoBehaviour
//{
//    [Header("Pre-Challenge Settings")]
//    public float challengeDuration = 30f;
//    public int scoreAdvantage = 100;
//    public float tugStrength = 20f;
//    public float centerDrift = 2f;
//    public float sliderSmoothSpeed = 0.15f; // Lower = smoother, higher = snappier

//    [Header("UI References")]
//    public GameObject preChallengePanel;
//    public TextMeshProUGUI challengeTimerText;
//    public TextMeshProUGUI player1TapsText;
//    public TextMeshProUGUI player2TapsText;
//    public TextMeshProUGUI instructionText;
//    public TextMeshProUGUI countdownText;
//    public Slider tugOfWarSlider;
//    public RectTransform player1DangerLine;
//    public RectTransform player2DangerLine;
//    public TextMeshProUGUI resultText;

//    [Header("Visual Effects")]
//    public GameObject player1TapEffect;
//    public GameObject player2TapEffect;

//    // State
//    private bool isChallengeActive = false;
//    private bool isCountingDown = false;
//    private int player1Taps = 0;
//    private int player2Taps = 0;
//    private float remainingTime;
//    private int winnerPlayer = 0;

//    // Slider
//    private float sliderPosition = 0f;
//    private float targetSliderPosition = 0f;
//    private float sliderVelocity = 0f; // used for SmoothDamp
//    private float dangerZone = 80f;

//    // Keys
//    private KeyCode player1Key = KeyCode.W;
//    private KeyCode player2Key = KeyCode.UpArrow;

//    void Start()
//    {
//        if (preChallengePanel != null)
//            preChallengePanel.SetActive(false);

//        if (tugOfWarSlider != null)
//        {
//            tugOfWarSlider.minValue = -100;
//            tugOfWarSlider.maxValue = 100;
//            tugOfWarSlider.value = 0;
//        }

//        PositionDangerLines();
//    }

//    void Update()
//    {
//        if (isChallengeActive)
//        {
//            bool player1Tapped = false;
//            bool player2Tapped = false;

//            if (Input.GetKeyDown(player1Key))
//            {
//                RegisterTap(1);
//                player1Tapped = true;
//            }

//            if (Input.GetKeyDown(player2Key))
//            {
//                RegisterTap(2);
//                player2Tapped = true;
//            }

//            UpdateSliderPosition(player1Tapped, player2Tapped);

//            // SmoothDamp instead of Lerp for more fluid tugging
//            sliderPosition = Mathf.SmoothDamp(sliderPosition, targetSliderPosition, ref sliderVelocity, sliderSmoothSpeed);
//            tugOfWarSlider.value = sliderPosition;

//            // Danger checks
//            if (sliderPosition <= -dangerZone)
//            {
//                EndChallenge(2, "Player 1's line reached!");
//                return;
//            }
//            else if (sliderPosition >= dangerZone)
//            {
//                EndChallenge(1, "Player 2's line reached!");
//                return;
//            }

//            // Timer
//            remainingTime -= Time.deltaTime;
//            UpdateTimerUI();

//            if (remainingTime <= 0)
//            {
//                EndChallenge(0, "Time's up!");
//            }
//        }
//    }

//    public void StartPreChallenge()
//    {
//        StartCoroutine(StartChallengeSequence());
//    }

//    private IEnumerator StartChallengeSequence()
//    {
//        if (preChallengePanel != null)
//            preChallengePanel.SetActive(true);

//        player1Taps = 0;
//        player2Taps = 0;
//        remainingTime = challengeDuration;
//        winnerPlayer = 0;
//        sliderPosition = 0f;
//        targetSliderPosition = 0f;

//        if (resultText != null)
//            resultText.gameObject.SetActive(false);

//        if (instructionText != null)
//            instructionText.text = $"Tug of War!\nDon't let the slider reach your danger line!\nPlayer 1: Press '{player1Key}' rapidly\nPlayer 2: Press '{player2Key}' rapidly";

//        // Countdown
//        isCountingDown = true;
//        for (int i = 3; i > 0; i--)
//        {
//            if (countdownText != null)
//                countdownText.text = i.ToString();
//            yield return new WaitForSeconds(1f);
//        }

//        if (countdownText != null)
//            countdownText.text = "FIGHT!";

//        isCountingDown = false;
//        isChallengeActive = true;

//        UpdateTapUI();
//        UpdateTimerUI();

//        yield return new WaitForSeconds(0.5f);

//        if (countdownText != null)
//            countdownText.text = "";
//    }

//    private void RegisterTap(int playerIndex)
//    {
//        if (!isChallengeActive) return;

//        if (playerIndex == 1)
//        {
//            player1Taps++;
//            if (player1TapEffect != null)
//                StartCoroutine(ShowTapEffect(player1TapEffect));
//        }
//        else if (playerIndex == 2)
//        {
//            player2Taps++;
//            if (player2TapEffect != null)
//                StartCoroutine(ShowTapEffect(player2TapEffect));
//        }

//        UpdateTapUI();
//    }

//    private IEnumerator ShowTapEffect(GameObject effect)
//    {
//        effect.SetActive(true);
//        yield return new WaitForSeconds(0.1f);
//        effect.SetActive(false);
//    }

//    private void UpdateSliderPosition(bool player1Tapped, bool player2Tapped)
//    {
//        if (player1Tapped)
//            targetSliderPosition += tugStrength;
//        if (player2Tapped)
//            targetSliderPosition -= tugStrength;

//        if (!player1Tapped && !player2Tapped)
//            targetSliderPosition = Mathf.MoveTowards(targetSliderPosition, 0, centerDrift * Time.deltaTime);

//        targetSliderPosition = Mathf.Clamp(targetSliderPosition, -100f, 100f);
//    }

//    private void UpdateTapUI()
//    {
//        if (player1TapsText != null)
//            player1TapsText.text = $"P1: {player1Taps}";
//        if (player2TapsText != null)
//            player2TapsText.text = $"P2: {player2Taps}";
//    }

//    private void UpdateTimerUI()
//    {
//        if (challengeTimerText != null)
//            challengeTimerText.text = $"Time: {remainingTime:F1}s";
//    }

//    private void PositionDangerLines()
//    {
//        if (tugOfWarSlider == null) return;

//        RectTransform sliderRect = tugOfWarSlider.GetComponent<RectTransform>();
//        if (sliderRect == null) return;

//        float sliderWidth = sliderRect.rect.width;
//        float dangerLineOffset = (dangerZone / 100f) * (sliderWidth / 2f);

//        if (player1DangerLine != null)
//        {
//            Vector3 pos = player1DangerLine.localPosition;
//            pos.x = -dangerLineOffset;
//            player1DangerLine.localPosition = pos;
//        }

//        if (player2DangerLine != null)
//        {
//            Vector3 pos = player2DangerLine.localPosition;
//            pos.x = dangerLineOffset;
//            player2DangerLine.localPosition = pos;
//        }
//    }

//    private void EndChallenge(int winner, string reason)
//    {
//        isChallengeActive = false;
//        winnerPlayer = winner;

//        if (resultText != null)
//        {
//            resultText.gameObject.SetActive(true);
//            string msg = (winnerPlayer == 0) ? $"{reason}\nIt's a Draw!" : $"{reason}\nPlayer {winnerPlayer} Wins!\n+{scoreAdvantage} bonus points!";
//            resultText.text = msg;
//        }

//        // Proceed after short delay
//        StartCoroutine(ProceedToMainGame());
//    }

//    private IEnumerator ProceedToMainGame()
//    {
//        yield return new WaitForSeconds(2f);

//        // Play your transition video or animation here
//        yield return PlayTransitionAnimation();

//        if (TurompoGameManager.Instance != null)
//        {
//            TurompoGameManager.Instance.StartGameFromPreChallenge();

//            if (winnerPlayer != 0)
//                TurompoGameManager.Instance.AddScore(winnerPlayer, scoreAdvantage);
//        }

//        if (preChallengePanel != null)
//            preChallengePanel.SetActive(false);
//    }

//    private IEnumerator PlayTransitionAnimation()
//    {
//        // Placeholder – replace with your actual VideoPlayer/Animator call
//        Debug.Log("Playing transition video/animation...");
//        yield return new WaitForSeconds(3f);
//    }

//    public bool IsChallengeActive()
//    {
//        return isChallengeActive || isCountingDown;
//    }
//}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurompoPreChallenge : MonoBehaviour
{
    [Header("Pre-Challenge Settings")]
    public float challengeDuration = 30f;
    public int scoreAdvantage = 100;
    public float centerDrift = 2f;
    public float sliderSmoothSpeed = 0.15f; // Lower = smoother, higher = snappier

    [Header("Momentum Settings")]
    public float momentumPerTap = 5f;       // How much momentum is gained per tap
    public float momentumDecay = 8f;        // How fast momentum decays per second
    public float momentumMultiplier = 1.2f; // Scales how much momentum affects tug strength

    [Header("UI References")]
    public GameObject preChallengePanel;
    public TextMeshProUGUI challengeTimerText;
    public TextMeshProUGUI player1TapsText;
    public TextMeshProUGUI player2TapsText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI countdownText;
    public Slider tugOfWarSlider;
    public RectTransform player1DangerLine;
    public RectTransform player2DangerLine;
    public TextMeshProUGUI resultText;

    [Header("Visual Effects")]
    public GameObject player1TapEffect;
    public GameObject player2TapEffect;

    // State
    private bool isChallengeActive = false;
    private bool isCountingDown = false;
    private int player1Taps = 0;
    private int player2Taps = 0;
    private float remainingTime;
    private int winnerPlayer = 0;

    // Momentum
    private float player1Momentum = 0f;
    private float player2Momentum = 0f;

    // Slider
    private float sliderPosition = 0f;
    private float targetSliderPosition = 0f;
    private float sliderVelocity = 0f; // used for SmoothDamp
    private float dangerZone = 80f;

    // Keys
    private KeyCode player1Key = KeyCode.W;
    private KeyCode player2Key = KeyCode.UpArrow;

    void Start()
    {
        if (preChallengePanel != null)
            preChallengePanel.SetActive(false);

        if (tugOfWarSlider != null)
        {
            tugOfWarSlider.minValue = -100;
            tugOfWarSlider.maxValue = 100;
            tugOfWarSlider.value = 0;
        }

        PositionDangerLines();
    }

    void Update()
    {
        if (isChallengeActive)
        {
            bool player1Tapped = false;
            bool player2Tapped = false;

            if (Input.GetKeyDown(player1Key))
            {
                RegisterTap(1);
                player1Tapped = true;
            }

            if (Input.GetKeyDown(player2Key))
            {
                RegisterTap(2);
                player2Tapped = true;
            }

            UpdateSliderPosition(player1Tapped, player2Tapped);

            // SmoothDamp instead of Lerp for more fluid tugging
            sliderPosition = Mathf.SmoothDamp(sliderPosition, targetSliderPosition, ref sliderVelocity, sliderSmoothSpeed);
            tugOfWarSlider.value = sliderPosition;

            // Danger checks
            if (sliderPosition <= -dangerZone)
            {
                EndChallenge(2, "Player 1's line reached!");
                return;
            }
            else if (sliderPosition >= dangerZone)
            {
                EndChallenge(1, "Player 2's line reached!");
                return;
            }

            // Timer
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                EndChallenge(0, "Time's up!");
            }
        }
    }

    public void StartPreChallenge()
    {
        StartCoroutine(StartChallengeSequence());
    }

    private IEnumerator StartChallengeSequence()
    {
        if (preChallengePanel != null)
            preChallengePanel.SetActive(true);

        player1Taps = 0;
        player2Taps = 0;
        player1Momentum = 0f;
        player2Momentum = 0f;
        remainingTime = challengeDuration;
        winnerPlayer = 0;
        sliderPosition = 0f;
        targetSliderPosition = 0f;

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (instructionText != null)
            instructionText.text = $"Tug of War!\nDon't let the slider reach your danger line!\nPlayer 1: Press '{player1Key}' rapidly\nPlayer 2: Press '{player2Key}' rapidly";

        // Countdown
        isCountingDown = true;
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
            countdownText.text = "FIGHT!";

        isCountingDown = false;
        isChallengeActive = true;

        UpdateTapUI();
        UpdateTimerUI();

        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
            countdownText.text = "";
    }

    private void RegisterTap(int playerIndex)
    {
        if (!isChallengeActive) return;

        if (playerIndex == 1)
        {
            player1Taps++;
            player1Momentum += momentumPerTap; // add momentum
            if (player1TapEffect != null)
                StartCoroutine(ShowTapEffect(player1TapEffect));
        }
        else if (playerIndex == 2)
        {
            player2Taps++;
            player2Momentum += momentumPerTap; // add momentum
            if (player2TapEffect != null)
                StartCoroutine(ShowTapEffect(player2TapEffect));
        }

        UpdateTapUI();
    }

    private IEnumerator ShowTapEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        effect.SetActive(false);
    }

    private void UpdateSliderPosition(bool player1Tapped, bool player2Tapped)
    {
        // Apply momentum difference as tugging force
        targetSliderPosition += (player1Momentum - player2Momentum) * momentumMultiplier * Time.deltaTime;

        // Natural drift toward center if no one taps
        if (!player1Tapped && !player2Tapped)
            targetSliderPosition = Mathf.MoveTowards(targetSliderPosition, 0, centerDrift * Time.deltaTime);

        // Momentum decay
        player1Momentum = Mathf.Max(0, player1Momentum - momentumDecay * Time.deltaTime);
        player2Momentum = Mathf.Max(0, player2Momentum - momentumDecay * Time.deltaTime);

        // Clamp slider
        targetSliderPosition = Mathf.Clamp(targetSliderPosition, -100f, 100f);
    }

    private void UpdateTapUI()
    {
        if (player1TapsText != null)
            player1TapsText.text = $"P1: {player1Taps}";
        if (player2TapsText != null)
            player2TapsText.text = $"P2: {player2Taps}";
    }

    private void UpdateTimerUI()
    {
        if (challengeTimerText != null)
            challengeTimerText.text = $"Time: {remainingTime:F1}s";
    }

    private void PositionDangerLines()
    {
        if (tugOfWarSlider == null) return;

        RectTransform sliderRect = tugOfWarSlider.GetComponent<RectTransform>();
        if (sliderRect == null) return;

        float sliderWidth = sliderRect.rect.width;
        float dangerLineOffset = (dangerZone / 100f) * (sliderWidth / 2f);

        if (player1DangerLine != null)
        {
            Vector3 pos = player1DangerLine.localPosition;
            pos.x = -dangerLineOffset;
            player1DangerLine.localPosition = pos;
        }

        if (player2DangerLine != null)
        {
            Vector3 pos = player2DangerLine.localPosition;
            pos.x = dangerLineOffset;
            player2DangerLine.localPosition = pos;
        }
    }

    private void EndChallenge(int winner, string reason)
    {
        isChallengeActive = false;
        winnerPlayer = winner;

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            string msg = (winnerPlayer == 0) ? $"{reason}\nIt's a Draw!" : $"{reason}\nPlayer {winnerPlayer} Wins!\n+{scoreAdvantage} bonus points!";
            resultText.text = msg;
        }

        // Proceed after short delay
        StartCoroutine(ProceedToMainGame());
    }

    private IEnumerator ProceedToMainGame()
    {
        yield return new WaitForSeconds(2f);

        // Play your transition video or animation here
        yield return PlayTransitionAnimation();

        if (TurompoGameManager.Instance != null)
        {
            TurompoGameManager.Instance.StartGameFromPreChallenge();

            if (winnerPlayer != 0)
                TurompoGameManager.Instance.AddScore(winnerPlayer, scoreAdvantage);
        }

        if (preChallengePanel != null)
            preChallengePanel.SetActive(false);
    }

    private IEnumerator PlayTransitionAnimation()
    {
        // Placeholder – replace with your actual VideoPlayer/Animator call
        Debug.Log("Playing transition video/animation...");
        yield return new WaitForSeconds(3f);
    }

    public bool IsChallengeActive()
    {
        return isChallengeActive || isCountingDown;
    }
}
