using System.Collections;
using UnityEngine;

public class GabitoBlink : MonoBehaviour
{
    [Header("Face Materials")]
    public Material openEyesMat;   // Material for open eyes
    public Material closedEyesMat; // Material for closed eyes

    [Header("Blink Settings")]
    public float blinkInterval = 2f;   // Time between blinks
    public float blinkDuration = 0.2f; // How long eyes stay closed

    [Header("Material Slot Index")]
    [Tooltip("Which material index is the face? (e.g. 0 = first, 1 = second)")]
    public int faceMaterialIndex = 1; // Default to 1 if face is the second material

    private Renderer rend;
    private Material[] mats;
    private float timer;
    private bool isBlinking;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend != null)
        {
            mats = rend.materials; // Copy all materials

            // Set open eyes material on the face slot
            if (openEyesMat != null && faceMaterialIndex < mats.Length)
            {
                mats[faceMaterialIndex] = openEyesMat;
                rend.materials = mats;
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isBlinking && timer >= blinkInterval)
        {
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        isBlinking = true;

        // Closed eyes
        if (closedEyesMat != null && faceMaterialIndex < mats.Length)
        {
            mats[faceMaterialIndex] = closedEyesMat;
            rend.materials = mats;
        }

        yield return new WaitForSeconds(blinkDuration);

        // Open eyes again
        if (openEyesMat != null && faceMaterialIndex < mats.Length)
        {
            mats[faceMaterialIndex] = openEyesMat;
            rend.materials = mats;
        }

        timer = 0f;
        isBlinking = false;
    }
}
