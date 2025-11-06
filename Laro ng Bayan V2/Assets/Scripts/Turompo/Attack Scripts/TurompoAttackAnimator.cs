using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurompoAttackAnimator : MonoBehaviour
{
    [Header("Attack Animation Settings")]
    public float attackDuration = 0.3f;
    public float attackScaleMultiplier = 1.3f;
    public float attackMoveDistance = 0.5f;
    public Color attackColor = Color.yellow;
    public float attackRotationMultiplier = 2.0f;

    [Header("Visual Effects")]
    public ParticleSystem attackParticles;
    public AudioClip attackSound;
    public float screenShakeIntensity = 0.1f;

    [Header("References")]
    public Transform turompoModel;
    public Renderer turompoRenderer;
    public AudioSource audioSource;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isAttacking = false;
    private TurompoController turompoController;

    private Vector3 trueOriginalPosition;
    private Vector3 trueOriginalScale;
    private Color trueOriginalColor;
    private bool originalValuesStored = false;

    private Material originalMaterial;
    private Material runtimeMaterial;

    void Awake()
    {
        InitializeReferences();
    }

    void Start()
    {
        StoreOriginalValues();
        EnsureModelVisibility();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Reinitialize();
    }

    void InitializeReferences()
    {
        turompoController = GetComponent<TurompoController>();

        if (turompoModel == null)
        {
            Debug.LogError($"TurompoAttackAnimator on {gameObject.name}: turompoModel is not assigned!");
            Transform modelChild = transform.Find("TurompoModel");
            if (modelChild != null)
            {
                turompoModel = modelChild;
                Debug.Log($"Auto-found turompo model: {modelChild.name}");
            }
            else
            {
                MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                if (renderers.Length > 0)
                {
                    turompoModel = renderers[0].transform;
                    Debug.Log($"Auto-assigned turompo model to first MeshRenderer found: {turompoModel.name}");
                }
            }
        }

        if (turompoRenderer == null && turompoModel != null)
        {
            turompoRenderer = turompoModel.GetComponent<Renderer>() ?? turompoModel.GetComponentInChildren<Renderer>();
        }

        if (turompoRenderer == null)
        {
            Debug.LogWarning($"No Renderer found for turompo model on {gameObject.name}. Color animations will not work.");
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (turompoRenderer != null && turompoRenderer.sharedMaterial != null)
        {
            originalMaterial = turompoRenderer.sharedMaterial;
        }
    }

    void EnsureModelVisibility()
    {
        if (turompoModel != null)
        {
            if (!turompoModel.gameObject.activeInHierarchy)
            {
                turompoModel.gameObject.SetActive(true);
                Debug.Log($"Activated turompo model: {turompoModel.name}");
            }

            if (turompoRenderer != null && !turompoRenderer.enabled)
            {
                turompoRenderer.enabled = true;
                Debug.Log($"Enabled turompo renderer: {turompoRenderer.name}");
            }

            if (originalValuesStored)
            {
                turompoModel.localPosition = trueOriginalPosition;
                turompoModel.localScale = trueOriginalScale;
            }
        }
        else
        {
            Debug.LogError($"TurompoAttackAnimator on {gameObject.name}: Cannot ensure model visibility - turompoModel is null!");
        }
    }

    void StoreOriginalValues()
    {
        if (turompoModel == null)
        {
            Debug.LogError($"Cannot store original values - turompoModel is null on {gameObject.name}");
            return;
        }

        if (!originalValuesStored)
        {
            trueOriginalPosition = turompoModel.localPosition;
            trueOriginalScale = turompoModel.localScale;

            if (turompoRenderer != null && turompoRenderer.material != null)
            {
                trueOriginalColor = turompoRenderer.material.color;
                trueOriginalColor.a = 1f;
            }
            else
            {
                trueOriginalColor = Color.white;
            }

            originalValuesStored = true;
        }

        originalPosition = turompoModel.localPosition;
        originalScale = turompoModel.localScale;

        if (turompoRenderer != null && turompoRenderer.material != null)
        {
            originalColor = turompoRenderer.material.color;
            originalColor.a = 1f;
        }
        else
        {
            originalColor = trueOriginalColor;
        }
    }

    public void TriggerAttack()
    {
        if (turompoModel == null)
        {
            Debug.LogError($"Cannot trigger attack - turompoModel is null on {gameObject.name}");
            return;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackAnimation());
        }
    }

    public void TriggerAttackWithCollision(Vector3 collisionPoint)
    {
        if (turompoModel == null)
        {
            Debug.LogError($"Cannot trigger attack with collision - turompoModel is null on {gameObject.name}");
            return;
        }

        if (!isAttacking)
        {
            StartCoroutine(AttackAnimationWithCollision(collisionPoint));
        }
    }

    private IEnumerator AttackAnimation()
    {
        isAttacking = true;

        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        if (attackParticles != null)
            attackParticles.Play();

        if (screenShakeIntensity > 0)
            StartCoroutine(ScreenShake());

        float halfDuration = attackDuration * 0.5f;

        yield return StartCoroutine(AnimateToValues(
            originalScale * attackScaleMultiplier,
            originalPosition + Vector3.forward * attackMoveDistance,
            attackColor,
            halfDuration
        ));

        yield return StartCoroutine(AnimateToValues(
            originalScale,
            originalPosition,
            originalColor,
            halfDuration
        ));

        isAttacking = false;
    }

    private IEnumerator AttackAnimationWithCollision(Vector3 collisionPoint)
    {
        isAttacking = true;

        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        CreateCollisionEffect(collisionPoint);

        if (screenShakeIntensity > 0)
            StartCoroutine(ScreenShake());

        Vector3 directionToCollision = (collisionPoint - transform.position).normalized;
        Vector3 attackPosition = originalPosition + directionToCollision * attackMoveDistance;

        float halfDuration = attackDuration * 0.5f;

        yield return StartCoroutine(AnimateToValues(
            originalScale * attackScaleMultiplier,
            attackPosition,
            attackColor,
            halfDuration
        ));

        yield return StartCoroutine(AnimateToValues(
            originalScale,
            originalPosition,
            originalColor,
            halfDuration
        ));

        isAttacking = false;
    }

    private IEnumerator AnimateToValues(Vector3 targetScale, Vector3 targetPosition, Color targetColor, float duration)
    {
        if (turompoModel == null)
        {
            Debug.LogError("Cannot animate - turompoModel is null");
            yield break;
        }

        Vector3 startScale = turompoModel.localScale;
        Vector3 startPosition = turompoModel.localPosition;
        Color startColor = turompoRenderer != null ? turompoRenderer.material.color : Color.white;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutBack(t);

            turompoModel.localScale = Vector3.Lerp(startScale, targetScale, easedT);
            turompoModel.localPosition = Vector3.Lerp(startPosition, targetPosition, easedT);

            if (turompoRenderer != null && turompoRenderer.material != null)
                turompoRenderer.material.color = Color.Lerp(startColor, targetColor, easedT);

            if (turompoController != null && isAttacking)
            {
                turompoController.currentSpinSpeed = Mathf.Min(
                    turompoController.currentSpinSpeed * (1f + attackRotationMultiplier * easedT * 0.1f),
                    turompoController.maxSpinSpeed
                );
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        turompoModel.localScale = targetScale;
        turompoModel.localPosition = targetPosition;

        if (turompoRenderer != null && turompoRenderer.material != null)
            turompoRenderer.material.color = targetColor;
    }

    private void CreateCollisionEffect(Vector3 collisionPoint)
    {
        GameObject collisionEffect = new GameObject("CollisionEffect");
        collisionEffect.transform.position = collisionPoint;
        StartCoroutine(CollisionEffectAnimation(collisionEffect));

        if (attackParticles != null)
        {
            GameObject tempParticles = Instantiate(attackParticles.gameObject, collisionPoint, Quaternion.identity);
            ParticleSystem ps = tempParticles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(tempParticles, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
    }

    private IEnumerator CollisionEffectAnimation(GameObject effect)
    {
        float duration = 0.2f;
        float maxScale = 0.3f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            if (effect == null) yield break;
            effect.transform.localScale = Vector3.one * Mathf.Lerp(0, maxScale, t / duration);
            yield return null;
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            if (effect == null) yield break;
            effect.transform.localScale = Vector3.one * Mathf.Lerp(maxScale, 0, t / duration);
            yield return null;
        }

        if (effect != null)
            Destroy(effect);
    }

    private IEnumerator ScreenShake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        Vector3 originalCameraPos = mainCamera.transform.position;
        float duration = 0.1f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * screenShakeIntensity;
            shakeOffset.z = 0;
            mainCamera.transform.position = originalCameraPos + shakeOffset;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPos;
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    public void ResetToOriginal()
    {
        if (turompoModel == null)
        {
            Debug.LogError($"Cannot reset to original - turompoModel is null on {gameObject.name}");
            return;
        }

        StopAllCoroutines();

        turompoModel.localPosition = trueOriginalPosition;
        turompoModel.localScale = trueOriginalScale;

        EnsureModelVisibility();

        if (turompoRenderer != null && turompoRenderer.material != null)
        {
            Color resetColor = trueOriginalColor;
            resetColor.a = 1f;
            turompoRenderer.material.color = resetColor;
        }

        isAttacking = false;
        StoreOriginalValues();
    }

    public void Reinitialize()
    {
        Debug.Log($"Reinitializing TurompoAttackAnimator on {gameObject.name}");

        StopAllCoroutines();
        isAttacking = false;
        originalValuesStored = false;

        InitializeReferences();
        StoreOriginalValues();
        EnsureModelVisibility();
    }

    public void OnGameRestart()
    {
        Reinitialize();
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
            DestroyImmediate(runtimeMaterial);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void DebugCurrentState()
    {
        Debug.Log($"=== TurompoAttackAnimator Debug Info for {gameObject.name} ===");
        Debug.Log($"turompoModel: {(turompoModel != null ? turompoModel.name : "NULL")}");
        Debug.Log($"turompoModel active: {(turompoModel != null ? turompoModel.gameObject.activeInHierarchy.ToString() : "N/A")}");
        Debug.Log($"turompoRenderer: {(turompoRenderer != null ? turompoRenderer.name : "NULL")}");
        Debug.Log($"turompoRenderer enabled: {(turompoRenderer != null ? turompoRenderer.enabled.ToString() : "N/A")}");
        Debug.Log($"Original values stored: {originalValuesStored}");
        Debug.Log($"Is attacking: {isAttacking}");
        if (originalValuesStored)
        {
            Debug.Log($"True original position: {trueOriginalPosition}");
            Debug.Log($"True original scale: {trueOriginalScale}");
        }
    }
}
