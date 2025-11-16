using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cutscenePanel;   // The main panel for the cutscene
    [SerializeField] private GameObject nextButton;      // Next button
    [SerializeField] private GameObject doneButton;      // Done button

    [Header("Cutscene Images")]
    [SerializeField] private List<GameObject> cutsceneImages;  // All image panels in order

    private int currentIndex = 0;

    void Start()
    {
        if (cutscenePanel != null)
        {
            cutscenePanel.SetActive(true);
            ShowImage(currentIndex);
        }
    }

    private void ShowImage(int index)
    {
        for (int i = 0; i < cutsceneImages.Count; i++)
        {
            cutsceneImages[i].SetActive(i == index); // Only show the current image
        }

        // Buttons
        if (nextButton != null) nextButton.SetActive(index < cutsceneImages.Count - 1);
        if (doneButton != null) doneButton.SetActive(index == cutsceneImages.Count - 1);
    }

    public void OnNextClicked()
    {
        currentIndex++;
        if (currentIndex < cutsceneImages.Count)
            ShowImage(currentIndex);
    }

    public void OnDoneClicked()
    {
        cutscenePanel.SetActive(false); // Hide cutscene
    }
}
