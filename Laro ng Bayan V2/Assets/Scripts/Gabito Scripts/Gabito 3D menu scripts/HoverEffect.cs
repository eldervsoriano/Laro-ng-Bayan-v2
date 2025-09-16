using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [Header("Target models to scale/outline")]
    public GameObject[] targetModels;

    public Outline[] outlines;
    private Vector3[] originalScales;

    public float scaleFactor = 1.1f; // How much bigger when hovered

    void Start()
    {
        if (targetModels == null || targetModels.Length == 0)
        {
            Debug.LogWarning("HoverEffect: No target models assigned!", this);
            return;
        }

        outlines = new Outline[targetModels.Length];
        originalScales = new Vector3[targetModels.Length];

        for (int i = 0; i < targetModels.Length; i++)
        {
            GameObject model = targetModels[i];

            if (model == null) continue;

            // Grab Outline or add one if missing
            Outline o = model.GetComponent<Outline>();
            if (o == null)
            {
                o = model.AddComponent<Outline>();
            }

            o.enabled = false;
            outlines[i] = o;

            // Store original scale
            originalScales[i] = model.transform.localScale;
        }
    }

    void OnMouseEnter()
    {
        for (int i = 0; i < targetModels.Length; i++)
        {
            if (targetModels[i] == null) continue;

            outlines[i].enabled = true;
            targetModels[i].transform.localScale = originalScales[i] * scaleFactor;
        }
    }

    void OnMouseExit()
    {
        for (int i = 0; i < targetModels.Length; i++)
        {
            if (targetModels[i] == null) continue;

            outlines[i].enabled = false;
            targetModels[i].transform.localScale = originalScales[i];
        }
    }
}
