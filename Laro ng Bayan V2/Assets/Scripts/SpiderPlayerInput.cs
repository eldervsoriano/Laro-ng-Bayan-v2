using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderPlayerInput : MonoBehaviour
{
    public int playerNumber = 1;  // 1 for Player 1, 2 for Player 2
    public int correctCount = 0;  // Number of correct key presses this round

    // UI Prefabs for the falling keys (UI Image prefabs)
    public GameObject wKeyPrefab;
    public GameObject aKeyPrefab;
    public GameObject sKeyPrefab;
    public GameObject dKeyPrefab;
    public GameObject leftArrowPrefab;
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject rightArrowPrefab;

    public Transform keyDropArea; // Where the keys will fall from (in UI space)
    public float fallSpeed = 2.0f; // Speed at which the keys fall down

    private bool inputActive = false;
    private List<KeyCode> keySequence;

    void Start()
    {
        correctCount = 0;
    }

    public IEnumerator BeginInput(List<KeyCode> sequence, int playerNum)
    {
        playerNumber = playerNum;
        keySequence = sequence;
        correctCount = 0;

        // Show the sequence of keys falling down
        foreach (KeyCode key in keySequence)
        {
            GameObject fallingKey = GetKeyPrefabForKey(key);  // Get the correct prefab for each key
            if (fallingKey != null)
            {
                // Instantiate the key prefab as a UI object inside the Canvas
                GameObject spawnedKey = Instantiate(fallingKey, keyDropArea.position, Quaternion.identity);

                // Ensure it is in the Canvas (RectTransform instead of Transform)
                RectTransform rectTransform = spawnedKey.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.SetParent(keyDropArea); // Set the parent to keyDropArea (Canvas space)
                    rectTransform.localScale = Vector3.one;  // Ensure scale is uniform (optional)
                }

                KeyFalling keyFallingScript = spawnedKey.GetComponent<KeyFalling>();
                keyFallingScript.Init(key, fallSpeed, this);  // Initialize falling key with proper key

                yield return new WaitForSeconds(0.5f); // delay between spawning keys
            }
        }

        // Wait for input to finish
        inputActive = true;
        yield return new WaitForSeconds(keySequence.Count * 1.0f); // Adjust based on sequence length
        inputActive = false;
    }

    // Get the corresponding key prefab based on the KeyCode
    GameObject GetKeyPrefabForKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W:
                return wKeyPrefab;
            case KeyCode.A:
                return aKeyPrefab;
            case KeyCode.S:
                return sKeyPrefab;
            case KeyCode.D:
                return dKeyPrefab;

            case KeyCode.LeftArrow:
                return leftArrowPrefab;
            case KeyCode.UpArrow:
                return upArrowPrefab;
            case KeyCode.DownArrow:
                return downArrowPrefab;
            case KeyCode.RightArrow:
                return rightArrowPrefab;

            default:
                return null;  // No prefab for this key
        }
    }

    // Called when the player presses the correct key
    public void CorrectKeyPressed(KeyCode key)
    {
        correctCount++;
    }

    // Called when the player presses the wrong key or misses
    public void InvalidKeyPressed()
    {
        // Optionally, show some feedback for an invalid key press
    }
}
