using UnityEngine;
using UnityEngine.UI;

public class PanelButtonController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private bool startClosed = true;

    private void Start()
    {
        // Add click listeners to buttons
        if (openButton != null)
            openButton.onClick.AddListener(OpenPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        // Set initial panel state
        if (panel != null && startClosed)
            panel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // Toggle panel visibility
    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }

    private void OnDestroy()
    {
        // Clean up listeners when object is destroyed
        if (openButton != null)
            openButton.onClick.RemoveListener(OpenPanel);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
    }
}