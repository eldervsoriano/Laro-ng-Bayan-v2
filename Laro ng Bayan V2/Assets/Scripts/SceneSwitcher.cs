using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple scene switcher that changes to another scene when a specified key is pressed.
/// Can be configured in the inspector.
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name of the scene to switch to")]
    [SerializeField] private string targetSceneName = "";

    [Header("Input Settings")]
    [Tooltip("The key that triggers the scene change")]
    [SerializeField] private KeyCode sceneChangeKey = KeyCode.M;

    [Header("Cursor Settings")]
    [Tooltip("Show cursor in the target scene")]
    [SerializeField] private bool showCursorInTargetScene = true;

    [Tooltip("Lock cursor in the target scene")]
    [SerializeField] private bool lockCursorInTargetScene = false;

    [Header("Optional Settings")]
    [Tooltip("Should this object persist between scenes?")]
    [SerializeField] private bool dontDestroyOnLoad = false;

    [Tooltip("Optional transition delay in seconds")]
    [SerializeField] private float transitionDelay = 0f;

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        // Check if the designated key was pressed
        if (Input.GetKeyDown(sceneChangeKey))
        {
            SwitchScene();
        }
    }

    /// <summary>
    /// Switch to the target scene with optional delay
    /// </summary>
    private void SwitchScene()
    {
        // Validate scene name
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is empty! Please set it in the inspector.");
            return;
        }

        // Check if the scene exists in the build settings
        if (SceneUtility.GetBuildIndexByScenePath("Scenes/" + targetSceneName) < 0)
        {
            Debug.LogWarning("Scene '" + targetSceneName + "' is not included in build settings. Make sure to add it.");
        }

        // Apply cursor settings before scene change
        Cursor.visible = showCursorInTargetScene;
        Cursor.lockState = lockCursorInTargetScene ? CursorLockMode.Locked : CursorLockMode.None;

        if (transitionDelay > 0)
        {
            // Use coroutine for delayed loading
            StartCoroutine(LoadSceneAfterDelay());
        }
        else
        {
            // Load immediately
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(targetSceneName);
    }

    /// <summary>
    /// Public method to switch scenes from external scripts or UI events
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void SwitchToScene(string sceneName)
    {
        targetSceneName = sceneName;
        SwitchScene();
    }

    /// <summary>
    /// Public method to switch scenes with specific cursor settings
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <param name="showCursor">Whether to show the cursor</param>
    /// <param name="lockCursor">Whether to lock the cursor</param>
    public void SwitchToScene(string sceneName, bool showCursor, bool lockCursor)
    {
        targetSceneName = sceneName;
        showCursorInTargetScene = showCursor;
        lockCursorInTargetScene = lockCursor;
        SwitchScene();
    }
}