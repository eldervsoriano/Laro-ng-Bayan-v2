using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurumpoQuestManager : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to quest updates if needed
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnQuestUpdated += HandleQuestUpdate;
        }
    }

    private void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnQuestUpdated -= HandleQuestUpdate;
        }
    }

    private void HandleQuestUpdate(string questText)
    {
        // Optional: if you want to handle displaying Turumpo quest locally
        Debug.Log("Turumpo Quest Manager received quest update: " + questText);
    }

    // Call this when player finishes the Turumpo game
    public void FinishTurumpoQuest()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteTurumpo();
        }
    }

    // If you want to start Turumpo quest explicitly
    public void StartTurumpoQuest()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.StartTurumpoQuest();
        }
    }

}