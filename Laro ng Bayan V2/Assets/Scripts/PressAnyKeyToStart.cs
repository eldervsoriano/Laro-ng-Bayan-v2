using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PressAnyKeyToStart : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string nextSceneName = "MainGame"; // The scene to load when a key is pressed
    [SerializeField] private float blinkInterval = 0.8f; // How fast the text blinks
    [SerializeField] private TextMeshProUGUI startText; // Reference to the text component

    [Header("Optional")]
    [SerializeField] private bool useAnimation = true; // Whether to animate/blink the text
    [SerializeField] private AudioSource startSound; // Optional sound to play when starting

    private float timer = 0f;
    private bool textVisible = true;

    private void Start()
    {
        // If text component wasn't assigned in the inspector, try to find it
        if (startText == null)
        {
            startText = GetComponent<TextMeshProUGUI>();
            if (startText == null)
            {
                Debug.LogWarning("No TextMeshProUGUI component found for the start text!");
            }
        }
    }

    private void Update()
    {
        // Handle blinking text animation
        if (useAnimation && startText != null)
        {
            timer += Time.deltaTime;
            if (timer >= blinkInterval)
            {
                timer = 0f;
                textVisible = !textVisible;
                startText.enabled = textVisible;
            }
        }

        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Play the start sound if assigned
            if (startSound != null)
            {
                startSound.Play();
            }

            // Load the next scene
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Check if the scene exists in the build settings
        if (SceneUtility.GetBuildIndexByScenePath(nextSceneName) >= 0)
        {
            // Load the scene by name
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Scene '" + nextSceneName + "' not found in build settings!");

            // Alternatively, you could load the next scene in the build index
            // int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            // {
            //     SceneManager.LoadScene(nextSceneIndex);
            // }
        }
    }
}