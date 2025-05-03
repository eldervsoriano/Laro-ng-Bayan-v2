using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class TransparentOnCameraCollision : MonoBehaviour
{
    [Header("Transparency Settings")]
    [SerializeField] private float transparencyAmount = 0.3f;
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private float resetDelay = 0.5f;
    [SerializeField] private string cameraTag = "MainCamera";

    private Renderer[] renderers;
    private Material[] materials;
    private Color[] originalColors;
    private Color[] targetColors;
    private bool isTransparent = false;
    private Coroutine fadeCoroutine;
    private WaitForSeconds resetDelayWait;

    private void Awake()
    {
        // Get all renderers on this object and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Store original materials
        int materialCount = 0;
        for (int i = 0; i < renderers.Length; i++)
        {
            materialCount += renderers[i].materials.Length;
        }

        materials = new Material[materialCount];
        originalColors = new Color[materialCount];
        targetColors = new Color[materialCount];

        int index = 0;
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                materials[index] = renderers[i].materials[j];
                originalColors[index] = materials[index].color;

                // Ensure material supports transparency
                SetupMaterialForTransparency(materials[index]);

                // Calculate target transparent color
                targetColors[index] = new Color(
                    originalColors[index].r,
                    originalColors[index].g,
                    originalColors[index].b,
                    transparencyAmount
                );

                index++;
            }
        }

        resetDelayWait = new WaitForSeconds(resetDelay);
    }

    private void SetupMaterialForTransparency(Material material)
    {
        // Check current rendering mode and change if needed
        if (material.GetFloat("_Mode") != 3) // 3 is Transparent mode
        {
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }

    // Called when something collides with THIS object (the wall)
    private void OnCollisionEnter(Collision collision)
    {
        CheckForCamera(collision.gameObject);
    }

    // Called when a trigger collider enters THIS object
    private void OnTriggerEnter(Collider other)
    {
        CheckForCamera(other.gameObject);
    }

    // Called when a trigger stays inside THIS object
    private void OnTriggerStay(Collider other)
    {
        CheckForCamera(other.gameObject);
    }

    // Called when a trigger exits THIS object
    private void OnTriggerExit(Collider other)
    {
        if (IsCameraObject(other.gameObject))
        {
            StartCoroutine(ResetTransparencyAfterDelay());
        }
    }

    // Called when something stops colliding with THIS object
    private void OnCollisionExit(Collision collision)
    {
        if (IsCameraObject(collision.gameObject))
        {
            StartCoroutine(ResetTransparencyAfterDelay());
        }
    }

    private void CheckForCamera(GameObject obj)
    {
        if (IsCameraObject(obj))
        {
            MakeTransparent();
        }
    }

    private bool IsCameraObject(GameObject obj)
    {
        // Check if the object is the camera or its parent is the camera
        return obj.CompareTag(cameraTag) ||
               (obj.transform.parent != null && obj.transform.parent.CompareTag(cameraTag));
    }

    private void MakeTransparent()
    {
        isTransparent = true;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeTo(targetColors));
    }

    private void MakeOpaque()
    {
        isTransparent = false;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeTo(originalColors));
    }

    private IEnumerator ResetTransparencyAfterDelay()
    {
        yield return resetDelayWait;
        MakeOpaque();
    }

    private IEnumerator FadeTo(Color[] targetColors)
    {
        float elapsedTime = 0f;
        Color[] startColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            startColors[i] = materials[i].color;
        }

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = Color.Lerp(startColors[i], targetColors[i], t);
            }

            yield return null;
        }
    }

    // Optional: Public methods to control transparency from other scripts
    public void SetTransparent(bool transparent)
    {
        if (transparent)
        {
            MakeTransparent();
        }
        else
        {
            MakeOpaque();
        }
    }

    public void SetTransparencyAmount(float amount)
    {
        transparencyAmount = Mathf.Clamp01(amount);
        for (int i = 0; i < targetColors.Length; i++)
        {
            targetColors[i].a = transparencyAmount;
        }

        if (isTransparent)
        {
            MakeTransparent(); // Refresh transparency with new amount
        }
    }
}