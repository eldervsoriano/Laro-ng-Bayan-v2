using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PreviewOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    [Header("Preview Settings")]
    [Tooltip("The GameObject that appears when hovering (e.g., a text or preview image).")]
    public GameObject previewObject;

    [Tooltip("Optional: The GameObject to hide while hovering (e.g., the original icon).")]
    public GameObject objectToHide;

    private Button button; // to check if it's interactable

    private void Start()
    {
        // Cache button component if it exists
        button = GetComponent<Button>();

        // Start hidden by default
        if (previewObject != null)
            previewObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only show preview if the button is interactable or if there’s no button attached
        if (button != null && !button.interactable)
            return;

        Debug.Log($"Hover ENTER detected on {gameObject.name}");

        // shows previews
        if (previewObject != null)
        {
            previewObject.SetActive(true);
            Debug.Log($"Showing preview: {previewObject.name}");
        }
        
        // hides previews
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
            Debug.Log($"Hiding object: {objectToHide.name}");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only hide preview if the button is interactable (or no button attached)
        if (button != null && !button.interactable)
            return;

        Debug.Log($"Hover EXIT detected on {gameObject.name}");

        // Hide preview
        if (previewObject != null)
        {
            previewObject.SetActive(false);
            Debug.Log($"Hiding preview: {previewObject.name}");
        }

        // Show the hidden object again
        if (objectToHide != null)
        {
            objectToHide.SetActive(true);
            Debug.Log($"Showing object: {objectToHide.name}");
        }
    }
    
}