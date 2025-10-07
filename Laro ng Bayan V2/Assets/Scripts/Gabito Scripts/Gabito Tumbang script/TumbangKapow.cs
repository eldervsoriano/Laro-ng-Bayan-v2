using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbangKapow : MonoBehaviour
{
    [Header("Kapow Effect Settings")]
    public GameObject kapowImage;     // Assign your "Kapow!" sprite or image here
    public float showDuration = 1f;   // How long it stays visible
    public string slipperTag = "Slipper"; // Tag used by slippers

    private bool hasShown = false;

    private void Start()
    {
        if (kapowImage != null)
            kapowImage.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detect hit from slipper without touching CanTarget.cs
        if (!hasShown && collision.gameObject.CompareTag(slipperTag))
        {
            hasShown = true;
            StartCoroutine(ShowKapow());
        }
    }

    private IEnumerator ShowKapow()
    {
        if (kapowImage != null)
        {
            kapowImage.SetActive(true);
            yield return new WaitForSeconds(showDuration);
            kapowImage.SetActive(false);
        }

        // Reset flag so it can trigger again if reused (like respawned can)
        hasShown = false;
    }
}
