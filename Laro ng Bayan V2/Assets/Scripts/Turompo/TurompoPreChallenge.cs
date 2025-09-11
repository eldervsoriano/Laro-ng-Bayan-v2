// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class TurompoPreChallenge : MonoBehaviour
// {
//     [Header("Pre-Challenge Settings")]
//     public float challengeDuration = 30f;
//     public int scoreAdvantage = 100;
//     public float centerDrift = 2f;
//     public float sliderSmoothSpeed = 0.15f; // Lower = smoother, higher = snappier

//     [Header("Momentum Settings")]
//     public float momentumPerTap = 5f;       // How much momentum is gained per tap
//     public float momentumDecay = 8f;        // How fast momentum decays per second
//     public float momentumMultiplier = 1.2f; // Scales how much momentum affects tug strength

//     [Header("UI References")]
//     public GameObject preChallengePanel;
//     public TextMeshProUGUI challengeTimerText;
//     public TextMeshProUGUI player1TapsText;
//     public TextMeshProUGUI player2TapsText;
//     public TextMeshProUGUI instructionText;
//     public TextMeshProUGUI countdownText;
//     public Slider tugOfWarSlider;
//     public RectTransform player1DangerLine;
//     public RectTransform player2DangerLine;
//     public TextMeshProUGUI resultText;

//     [Header("Visual Effects")]
//     public GameObject player1TapEffect;
//     public GameObject player2TapEffect;

//     // State
//     private bool isChallengeActive = false;
//     private bool isCountingDown = false;
//     private int player1Taps = 0;
//     private int player2Taps = 0;
//     private float remainingTime;
//     private int winnerPlayer = 0;

//     // Momentum
//     private float player1Momentum = 0f;
//     private float player2Momentum = 0f;

//     // Slider
//     private float sliderPosition = 0f;
//     private float targetSliderPosition = 0f;
//     private float sliderVelocity = 0f; // used for SmoothDamp
//     private float dangerZone = 80f;

//     // Keys
//     private KeyCode player1Key = KeyCode.W;
//     private KeyCode player2Key = KeyCode.UpArrow;

//     void Start()
//     {
//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(false);

//         if (tugOfWarSlider != null)
//         {
//             tugOfWarSlider.minValue = -100;
//             tugOfWarSlider.maxValue = 100;
//             tugOfWarSlider.value = 0;
//         }

//         PositionDangerLines();
//     }

//     void Update()
//     {
//         if (isChallengeActive)
//         {
//             bool player1Tapped = false;
//             bool player2Tapped = false;

//             if (Input.GetKeyDown(player1Key))
//             {
//                 RegisterTap(1);
//                 player1Tapped = true;
//             }

//             if (Input.GetKeyDown(player2Key))
//             {
//                 RegisterTap(2);
//                 player2Tapped = true;
//             }

//             UpdateSliderPosition(player1Tapped, player2Tapped);

//             // SmoothDamp instead of Lerp for more fluid tugging
//             sliderPosition = Mathf.SmoothDamp(sliderPosition, targetSliderPosition, ref sliderVelocity, sliderSmoothSpeed);
//             tugOfWarSlider.value = sliderPosition;

//             // Danger checks
//             if (sliderPosition <= -dangerZone)
//             {
//                 EndChallenge(2, "Player 1's line reached!");
//                 return;
//             }
//             else if (sliderPosition >= dangerZone)
//             {
//                 EndChallenge(1, "Player 2's line reached!");
//                 return;
//             }

//             // Timer
//             remainingTime -= Time.deltaTime;
//             UpdateTimerUI();

//             if (remainingTime <= 0)
//             {
//                 EndChallenge(0, "Time's up!");
//             }
//         }
//     }

//     public void StartPreChallenge()
//     {
//         StartCoroutine(StartChallengeSequence());
//     }

//     private IEnumerator StartChallengeSequence()
//     {
//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(true);

//         player1Taps = 0;
//         player2Taps = 0;
//         player1Momentum = 0f;
//         player2Momentum = 0f;
//         remainingTime = challengeDuration;
//         winnerPlayer = 0;
//         sliderPosition = 0f;
//         targetSliderPosition = 0f;

//         if (resultText != null)
//             resultText.gameObject.SetActive(false);

//         if (instructionText != null)
//             instructionText.text = $"Tug of War!\nDon't let the slider reach your danger line!\nPlayer 1: Press '{player1Key}' rapidly\nPlayer 2: Press '{player2Key}' rapidly";

//         // Countdown
//         isCountingDown = true;
//         for (int i = 3; i > 0; i--)
//         {
//             if (countdownText != null)
//                 countdownText.text = i.ToString();
//             yield return new WaitForSeconds(1f);
//         }

//         if (countdownText != null)
//             countdownText.text = "FIGHT!";

//         isCountingDown = false;
//         isChallengeActive = true;

//         UpdateTapUI();
//         UpdateTimerUI();

//         yield return new WaitForSeconds(0.5f);

//         if (countdownText != null)
//             countdownText.text = "";
//     }

//     private void RegisterTap(int playerIndex)
//     {
//         if (!isChallengeActive) return;

//         if (playerIndex == 1)
//         {
//             player1Taps++;
//             player1Momentum += momentumPerTap; // add momentum
//             if (player1TapEffect != null)
//                 StartCoroutine(ShowTapEffect(player1TapEffect));
//         }
//         else if (playerIndex == 2)
//         {
//             player2Taps++;
//             player2Momentum += momentumPerTap; // add momentum
//             if (player2TapEffect != null)
//                 StartCoroutine(ShowTapEffect(player2TapEffect));
//         }

//         UpdateTapUI();
//     }

//     private IEnumerator ShowTapEffect(GameObject effect)
//     {
//         effect.SetActive(true);
//         yield return new WaitForSeconds(0.1f);
//         effect.SetActive(false);
//     }

//     private void UpdateSliderPosition(bool player1Tapped, bool player2Tapped)
//     {
//         // Apply momentum difference as tugging force
//         targetSliderPosition += (player1Momentum - player2Momentum) * momentumMultiplier * Time.deltaTime;

//         // Natural drift toward center if no one taps
//         if (!player1Tapped && !player2Tapped)
//             targetSliderPosition = Mathf.MoveTowards(targetSliderPosition, 0, centerDrift * Time.deltaTime);

//         // Momentum decay
//         player1Momentum = Mathf.Max(0, player1Momentum - momentumDecay * Time.deltaTime);
//         player2Momentum = Mathf.Max(0, player2Momentum - momentumDecay * Time.deltaTime);

//         // Clamp slider
//         targetSliderPosition = Mathf.Clamp(targetSliderPosition, -100f, 100f);
//     }

//     private void UpdateTapUI()
//     {
//         if (player1TapsText != null)
//             player1TapsText.text = $"P1: {player1Taps}";
//         if (player2TapsText != null)
//             player2TapsText.text = $"P2: {player2Taps}";
//     }

//     private void UpdateTimerUI()
//     {
//         if (challengeTimerText != null)
//             challengeTimerText.text = $"Time: {remainingTime:F1}s";
//     }

//     private void PositionDangerLines()
//     {
//         if (tugOfWarSlider == null) return;

//         RectTransform sliderRect = tugOfWarSlider.GetComponent<RectTransform>();
//         if (sliderRect == null) return;

//         float sliderWidth = sliderRect.rect.width;
//         float dangerLineOffset = (dangerZone / 100f) * (sliderWidth / 2f);

//         if (player1DangerLine != null)
//         {
//             Vector3 pos = player1DangerLine.localPosition;
//             pos.x = -dangerLineOffset;
//             player1DangerLine.localPosition = pos;
//         }

//         if (player2DangerLine != null)
//         {
//             Vector3 pos = player2DangerLine.localPosition;
//             pos.x = dangerLineOffset;
//             player2DangerLine.localPosition = pos;
//         }
//     }

//     private void EndChallenge(int winner, string reason)
//     {
//         isChallengeActive = false;
//         winnerPlayer = winner;

//         if (resultText != null)
//         {
//             resultText.gameObject.SetActive(true);
//             string msg = (winnerPlayer == 0) ? $"{reason}\nIt's a Draw!" : $"{reason}\nPlayer {winnerPlayer} Wins!\n+{scoreAdvantage} bonus points!";
//             resultText.text = msg;
//         }

//         // Proceed after short delay
//         StartCoroutine(ProceedToMainGame());
//     }

//     private IEnumerator ProceedToMainGame()
//     {
//         yield return new WaitForSeconds(2f);

//         // Play your transition video or animation here
//         yield return PlayTransitionAnimation();

//         if (TurompoGameManager.Instance != null)
//         {
//             TurompoGameManager.Instance.StartGameFromPreChallenge();

//             if (winnerPlayer != 0)
//                 TurompoGameManager.Instance.AddScore(winnerPlayer, scoreAdvantage);
//         }

//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(false);
//     }

//     private IEnumerator PlayTransitionAnimation()
//     {
//         // Placeholder � replace with your actual VideoPlayer/Animator call
//         Debug.Log("Playing transition video/animation...");
//         yield return new WaitForSeconds(3f);
//     }

//     public bool IsChallengeActive()
//     {
//         return isChallengeActive || isCountingDown;
//     }
// }



// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class TurompoPreChallenge : MonoBehaviour
// {
//     [Header("Pre-Challenge Settings")]
//     public float challengeDuration = 20f;
//     public int scoreAdvantage = 100;
//     public float centerDrift = 2f;
//     public float sliderSmoothSpeed = 0.15f;

//     [Header("Momentum Settings")]
//     public float momentumPerTap = 8f;
//     public float momentumDecay = 5f;
//     public float momentumMultiplier = 1.5f;

//     [Header("UI References")]
//     public GameObject preChallengePanel;
//     public TextMeshProUGUI challengeTimerText;
//     public TextMeshProUGUI player1TapsText;
//     public TextMeshProUGUI player2TapsText;
//     public TextMeshProUGUI instructionText;
//     public TextMeshProUGUI countdownText;
//     public TextMeshProUGUI player1KeyIndicator;
//     public TextMeshProUGUI player2KeyIndicator;
//     public Slider tugOfWarSlider;
//     public RectTransform player1DangerLine;
//     public RectTransform player2DangerLine;
//     public TextMeshProUGUI resultText;

//     [Header("Visual Effects")]
//     public GameObject player1TapEffect;
//     public GameObject player2TapEffect;

//     // State
//     private bool isChallengeActive = false;
//     private bool isCountingDown = false;
//     private int player1Taps = 0;
//     private int player2Taps = 0;
//     private float remainingTime;
//     private int winnerPlayer = 0;

//     // Momentum
//     private float player1Momentum = 0f;
//     private float player2Momentum = 0f;

//     // Slider
//     private float sliderPosition = 0f;
//     private float targetSliderPosition = 0f;
//     private float sliderVelocity = 0f;
//     private float dangerZone = 80f;

//     // Key pools
//     private KeyCode[] player1Keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
//     private KeyCode[] player2Keys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.UpArrow };
//     private KeyCode currentPlayer1Key;
//     private KeyCode currentPlayer2Key;

//     // Track last used keys
//     private KeyCode lastPlayer1Key;
//     private KeyCode lastPlayer2Key;

//     [Header("Key Switching Settings")]
//     public float keySwitchInterval = 5f; // seconds before switching
//     private float keySwitchTimer = 0f;

//     void Start()
//     {
//         Cursor.visible = false;
//         Cursor.lockState = CursorLockMode.Locked;
//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(false);

//         if (tugOfWarSlider != null)
//         {
//             tugOfWarSlider.minValue = -100;
//             tugOfWarSlider.maxValue = 100;
//             tugOfWarSlider.value = 0;
//         }

//         PositionDangerLines();
//     }

//     void Update()
//     {
//         if (isChallengeActive)
//         {
//             // Handle key switching
//             keySwitchTimer -= Time.deltaTime;
//             if (keySwitchTimer <= 0)
//             {
//                 SwitchKeys();
//                 keySwitchTimer = keySwitchInterval;
//             }

//             bool player1Tapped = false;
//             bool player2Tapped = false;

//             if (Input.GetKeyDown(currentPlayer1Key))
//             {
//                 RegisterTap(1);
//                 player1Tapped = true;
//             }

//             if (Input.GetKeyDown(currentPlayer2Key))
//             {
//                 RegisterTap(2);
//                 player2Tapped = true;
//             }

//             UpdateSliderPosition(player1Tapped, player2Tapped);

//             // SmoothDamp
//             sliderPosition = Mathf.SmoothDamp(sliderPosition, targetSliderPosition, ref sliderVelocity, sliderSmoothSpeed);
//             tugOfWarSlider.value = sliderPosition;

//             // Danger checks
//             if (sliderPosition <= -dangerZone)
//             {
//                 EndChallenge(2, "Player 1's line reached!");
//                 return;
//             }
//             else if (sliderPosition >= dangerZone)
//             {
//                 EndChallenge(1, "Player 2's line reached!");
//                 return;
//             }

//             // Timer
//             remainingTime -= Time.deltaTime;
//             UpdateTimerUI();

//             if (remainingTime <= 0)
//             {
//                 EndChallenge(0, "Time's up!");
//             }
//         }
//     }

//     public void StartPreChallenge()
//     {
//         StartCoroutine(StartChallengeSequence());
//     }

//     private IEnumerator StartChallengeSequence()
//     {
//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(true);

//         player1Taps = 0;
//         player2Taps = 0;
//         player1Momentum = 0f;
//         player2Momentum = 0f;
//         remainingTime = challengeDuration;
//         winnerPlayer = 0;
//         sliderPosition = 0f;
//         targetSliderPosition = 0f;

//         if (resultText != null)
//             resultText.gameObject.SetActive(false);

//         if (instructionText != null)
//             instructionText.text = $"Tug of War!\nPress the key shown on your screen rapidly!";

//         // Countdown
//         isCountingDown = true;
//         for (int i = 3; i > 0; i--)
//         {
//             if (countdownText != null)
//                 countdownText.text = i.ToString();
//             yield return new WaitForSeconds(1f);
//         }

//         if (countdownText != null)
//             countdownText.text = "FIGHT!";

//         isCountingDown = false;
//         isChallengeActive = true;

//         // Pick initial keys
//         SwitchKeys();
//         keySwitchTimer = keySwitchInterval;

//         UpdateTapUI();
//         UpdateTimerUI();

//         yield return new WaitForSeconds(0.5f);

//         if (countdownText != null)
//             countdownText.text = "";
//     }

//     private void SwitchKeys()
//     {
//         // Player 1
//         KeyCode newKey1;
//         do
//         {
//             newKey1 = player1Keys[Random.Range(0, player1Keys.Length)];
//         } while (newKey1 == lastPlayer1Key); // prevent same as last
//         currentPlayer1Key = newKey1;
//         lastPlayer1Key = newKey1;

//         // Player 2
//         KeyCode newKey2;
//         do
//         {
//             newKey2 = player2Keys[Random.Range(0, player2Keys.Length)];
//         } while (newKey2 == lastPlayer2Key); // prevent same as last
//         currentPlayer2Key = newKey2;
//         lastPlayer2Key = newKey2;

//         // Update UI
//         if (player1KeyIndicator != null)
//             player1KeyIndicator.text = $"P1 Press: {currentPlayer1Key}";
//         if (player2KeyIndicator != null)
//             player2KeyIndicator.text = $"P2 Press: {currentPlayer2Key}";
//     }

//     private void RegisterTap(int playerIndex)
//     {
//         if (!isChallengeActive) return;

//         if (playerIndex == 1)
//         {
//             player1Taps++;
//             player1Momentum += momentumPerTap;
//             if (player1TapEffect != null)
//                 StartCoroutine(ShowTapEffect(player1TapEffect));
//         }
//         else if (playerIndex == 2)
//         {
//             player2Taps++;
//             player2Momentum += momentumPerTap;
//             if (player2TapEffect != null)
//                 StartCoroutine(ShowTapEffect(player2TapEffect));
//         }

//         UpdateTapUI();
//     }

//     private IEnumerator ShowTapEffect(GameObject effect)
//     {
//         effect.SetActive(true);
//         yield return new WaitForSeconds(0.1f);
//         effect.SetActive(false);
//     }

//     private void UpdateSliderPosition(bool player1Tapped, bool player2Tapped)
//     {
//         targetSliderPosition += (player1Momentum - player2Momentum) * momentumMultiplier * Time.deltaTime;

//         if (!player1Tapped && !player2Tapped)
//             targetSliderPosition = Mathf.MoveTowards(targetSliderPosition, 0, centerDrift * Time.deltaTime);

//         player1Momentum = Mathf.Max(0, player1Momentum - momentumDecay * Time.deltaTime);
//         player2Momentum = Mathf.Max(0, player2Momentum - momentumDecay * Time.deltaTime);

//         targetSliderPosition = Mathf.Clamp(targetSliderPosition, -100f, 100f);
//     }

//     private void UpdateTapUI()
//     {
//         if (player1TapsText != null)
//             player1TapsText.text = $"P1: {player1Taps}";
//         if (player2TapsText != null)
//             player2TapsText.text = $"P2: {player2Taps}";
//     }

//     private void UpdateTimerUI()
//     {
//         if (challengeTimerText != null)
//             challengeTimerText.text = $"Time: {remainingTime:F1}s";
//     }

//     private void PositionDangerLines()
//     {
//         if (tugOfWarSlider == null) return;

//         RectTransform sliderRect = tugOfWarSlider.GetComponent<RectTransform>();
//         if (sliderRect == null) return;

//         float sliderWidth = sliderRect.rect.width;
//         float dangerLineOffset = (dangerZone / 100f) * (sliderWidth / 2f);

//         if (player1DangerLine != null)
//         {
//             Vector3 pos = player1DangerLine.localPosition;
//             pos.x = -dangerLineOffset;
//             player1DangerLine.localPosition = pos;
//         }

//         if (player2DangerLine != null)
//         {
//             Vector3 pos = player2DangerLine.localPosition;
//             pos.x = dangerLineOffset;
//             player2DangerLine.localPosition = pos;
//         }
//     }

//     private void EndChallenge(int winner, string reason)
//     {
//         isChallengeActive = false;
//         winnerPlayer = winner;

//         if (resultText != null)
//         {
//             resultText.gameObject.SetActive(true);
//             string msg = (winnerPlayer == 0) ? $"{reason}\nIt's a Draw!" : $"{reason}\nPlayer {winnerPlayer} Wins!\n+{scoreAdvantage} bonus points!";
//             resultText.text = msg;
//         }

//         StartCoroutine(ProceedToMainGame());
//     }

//     private IEnumerator ProceedToMainGame()
//     {
//         yield return new WaitForSeconds(2f);
//         yield return PlayTransitionAnimation();

//         if (TurompoGameManager.Instance != null)
//         {
//             TurompoGameManager.Instance.StartGameFromPreChallenge();
//             if (winnerPlayer != 0)
//                 TurompoGameManager.Instance.AddScore(winnerPlayer, scoreAdvantage);
//         }

//         if (preChallengePanel != null)
//             preChallengePanel.SetActive(false);
//     }

//     private IEnumerator PlayTransitionAnimation()
//     {
//         Debug.Log("Playing transition video/animation...");
//         yield return new WaitForSeconds(3f);
//     }

//     public bool IsChallengeActive()
//     {
//         return isChallengeActive || isCountingDown;
//     }
// }


using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurompoPreChallenge : MonoBehaviour
{
    [Header("Pre-Challenge Settings")]
    public float challengeDuration = 20f;
    public int scoreAdvantage = 100;
    public float centerDrift = 2f;
    public float sliderSmoothSpeed = 0.15f;

    [Header("Momentum Settings")]
    public float momentumPerTap = 8f;
    public float momentumDecay = 5f;
    public float momentumMultiplier = 1.5f;

    [Header("UI References")]
    public GameObject preChallengePanel;
    public TextMeshProUGUI challengeTimerText;
    public TextMeshProUGUI player1TapsText;
    public TextMeshProUGUI player2TapsText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI player1KeyIndicator;
    public Image player2KeyIndicator; // image for P2
    public Slider tugOfWarSlider;
    public RectTransform player1DangerLine;
    public RectTransform player2DangerLine;
    public TextMeshProUGUI resultText;

    [Header("Visual Effects")]
    public GameObject player1TapEffect;
    public GameObject player2TapEffect;

    [Header("Arrow Sprites (Player 2)")]
    public Sprite upArrowSprite;
    public Sprite downArrowSprite;
    public Sprite leftArrowSprite;
    public Sprite rightArrowSprite;

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
    private float sliderVelocity = 0f;
    private float dangerZone = 80f;

    // Key pools
    private KeyCode[] player1Keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode[] player2Keys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.UpArrow };
    private KeyCode currentPlayer1Key;
    private KeyCode currentPlayer2Key;

    // Track last used keys
    private KeyCode lastPlayer1Key;
    private KeyCode lastPlayer2Key;

    [Header("Key Switching Settings")]
    public float keySwitchInterval = 5f; // seconds before switching
    private float keySwitchTimer = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
            // Handle key switching
            keySwitchTimer -= Time.deltaTime;
            if (keySwitchTimer <= 0)
            {
                SwitchKeys();
                keySwitchTimer = keySwitchInterval;
            }

            bool player1Tapped = false;
            bool player2Tapped = false;

            if (Input.GetKeyDown(currentPlayer1Key))
            {
                RegisterTap(1);
                player1Tapped = true;
            }

            if (Input.GetKeyDown(currentPlayer2Key))
            {
                RegisterTap(2);
                player2Tapped = true;
            }

            UpdateSliderPosition(player1Tapped, player2Tapped);

            // SmoothDamp
            sliderPosition = Mathf.SmoothDamp(sliderPosition, targetSliderPosition, ref sliderVelocity, sliderSmoothSpeed);
            tugOfWarSlider.value = sliderPosition;

            // Danger checks
            if (sliderPosition <= -dangerZone)
            {
                EndChallenge(1, "Trophy reached Player 1's side!");
                return;
            }
            else if (sliderPosition >= dangerZone)
            {
                EndChallenge(2, "Trophy reached Player 2's side!");
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
            instructionText.text = $"Tug of War!\nPull the trophy toward your side by pressing the shown key!";

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

        // Pick initial keys
        SwitchKeys();
        keySwitchTimer = keySwitchInterval;

        UpdateTapUI();
        UpdateTimerUI();

        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
            countdownText.text = "";
    }

    private void SwitchKeys()
    {
        // Player 1
        KeyCode newKey1;
        do
        {
            newKey1 = player1Keys[Random.Range(0, player1Keys.Length)];
        } while (newKey1 == lastPlayer1Key);
        currentPlayer1Key = newKey1;
        lastPlayer1Key = newKey1;

        // Player 2
        KeyCode newKey2;
        do
        {
            newKey2 = player2Keys[Random.Range(0, player2Keys.Length)];
        } while (newKey2 == lastPlayer2Key);
        currentPlayer2Key = newKey2;
        lastPlayer2Key = newKey2;

        // Update UI
        if (player1KeyIndicator != null)
            player1KeyIndicator.text = $"P1 Press: {currentPlayer1Key}";

        if (player2KeyIndicator != null)
        {
            switch (currentPlayer2Key)
            {
                case KeyCode.UpArrow: player2KeyIndicator.sprite = upArrowSprite; break;
                case KeyCode.DownArrow: player2KeyIndicator.sprite = downArrowSprite; break;
                case KeyCode.LeftArrow: player2KeyIndicator.sprite = leftArrowSprite; break;
                case KeyCode.RightArrow: player2KeyIndicator.sprite = rightArrowSprite; break;
            }
        }
    }

    private void RegisterTap(int playerIndex)
    {
        if (!isChallengeActive) return;

        if (playerIndex == 1)
        {
            player1Taps++;
            player1Momentum += momentumPerTap;
            if (player1TapEffect != null)
                StartCoroutine(ShowTapEffect(player1TapEffect));
        }
        else if (playerIndex == 2)
        {
            player2Taps++;
            player2Momentum += momentumPerTap;
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
        // Player 1 pulls slider left (-), Player 2 pulls right (+)
        targetSliderPosition += ((-player1Momentum) + (player2Momentum)) * momentumMultiplier * Time.deltaTime;

        // Center pull if no one taps
        if (!player1Tapped && !player2Tapped)
            targetSliderPosition = Mathf.MoveTowards(targetSliderPosition, 0, centerDrift * Time.deltaTime);

        player1Momentum = Mathf.Max(0, player1Momentum - momentumDecay * Time.deltaTime);
        player2Momentum = Mathf.Max(0, player2Momentum - momentumDecay * Time.deltaTime);

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

        StartCoroutine(ProceedToMainGame());
    }

    private IEnumerator ProceedToMainGame()
    {
        yield return new WaitForSeconds(2f);
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
        Debug.Log("Playing transition video/animation...");
        yield return new WaitForSeconds(3f);
    }

    public bool IsChallengeActive()
    {
        return isChallengeActive || isCountingDown;
    }
}
