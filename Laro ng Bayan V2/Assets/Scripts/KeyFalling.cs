using UnityEngine;

public class KeyFalling : MonoBehaviour
{
    public KeyCode keyToPress;  // The key this falling object represents (e.g., W, A, S, D, Arrow Keys)
    public float fallSpeed = 2.0f;  // Speed at which the key falls down
    public SpiderPlayerInput playerInputScript;  // Reference to the player's input script

    private RectTransform rectTransform;  // RectTransform for UI elements (falling keys)
    private Vector2 targetPosition;  // Target position for the falling key (the "hit line")
    private bool keyPressedCorrectly = false;

    void Start()
    {
        // Get RectTransform component for UI Image (falling key)
        rectTransform = GetComponent<RectTransform>();

        // Set the target position for the falling key (where it should land in UI space)
        targetPosition = new Vector2(rectTransform.position.x, -5f); // Modify -5f to your "hit line" y-position
    }

    void Update()
    {
        // Fall down the screen (in UI space, we move the RectTransform)
        rectTransform.position = Vector2.Lerp(rectTransform.position, targetPosition, fallSpeed * Time.deltaTime);

        // When the key reaches or passes the hit line (y-position), check if the player presses the correct key
        if (rectTransform.position.y <= -5f)  // This is your "hit line" (adjust as needed)
        {
            // Check if the player presses the correct key
            if (keyPressedCorrectly)
            {
                // Correct key press, increment the score
                playerInputScript.CorrectKeyPressed(keyToPress);
            }
            else
            {
                // Player missed or pressed the wrong key
                playerInputScript.InvalidKeyPressed();
            }

            // Destroy the falling key object after the interaction
            Destroy(gameObject);
        }
    }

    // Called to initialize the falling key with the correct key and fall speed
    public void Init(KeyCode key, float fallSpeed, SpiderPlayerInput playerInput)
    {
        this.keyToPress = key;
        this.fallSpeed = fallSpeed;
        this.playerInputScript = playerInput;
    }

    // Check for key press when the player presses a key
    void OnMouseDown()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            keyPressedCorrectly = true;
        }
        else
        {
            keyPressedCorrectly = false;
        }
    }
}
