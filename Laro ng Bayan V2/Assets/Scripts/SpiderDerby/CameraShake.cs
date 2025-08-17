using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeReductionSpeed = 1.0f;
    [SerializeField] private float maxShakeDistance = 1.0f;
    [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private Vector3 originalPosition;
    private bool isShaking = false;
    private Coroutine shakeCoroutine;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Main shake method that can be called from other scripts
    public void Shake(float duration, float intensity)
    {
        // Stop any existing shake
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        // Start new shake
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, intensity));
    }

    // Coroutine that handles the shake effect
    private IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate shake progress (0 to 1)
            float progress = elapsedTime / duration;

            // Use animation curve to control shake intensity over time
            float currentIntensity = intensity * shakeCurve.Evaluate(progress);

            // Generate random offset
            Vector3 randomOffset = Random.insideUnitSphere * currentIntensity;
            randomOffset = Vector3.ClampMagnitude(randomOffset, maxShakeDistance);

            // Apply shake to camera position
            transform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to original position
        transform.localPosition = originalPosition;
        isShaking = false;
        shakeCoroutine = null;
    }

    // Method to shake with custom curve
    public void ShakeWithCurve(float duration, float intensity, AnimationCurve customCurve)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutineWithCurve(duration, intensity, customCurve));
    }

    private IEnumerator ShakeCoroutineWithCurve(float duration, float intensity, AnimationCurve curve)
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float currentIntensity = intensity * curve.Evaluate(progress);

            Vector3 randomOffset = Random.insideUnitSphere * currentIntensity;
            randomOffset = Vector3.ClampMagnitude(randomOffset, maxShakeDistance);

            transform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
        shakeCoroutine = null;
    }

    // Method for quick shake effects
    public void QuickShake(float intensity = 1.0f)
    {
        Shake(0.2f, intensity);
    }

    // Method for long shake effects
    public void LongShake(float intensity = 1.0f)
    {
        Shake(1.0f, intensity);
    }

    // Method to stop shaking immediately
    public void StopShaking()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }

    // Check if camera is currently shaking
    public bool IsShaking()
    {
        return isShaking;
    }

    // Method to update original position (useful if camera moves)
    public void UpdateOriginalPosition()
    {
        if (!isShaking)
        {
            originalPosition = transform.localPosition;
        }
    }
}