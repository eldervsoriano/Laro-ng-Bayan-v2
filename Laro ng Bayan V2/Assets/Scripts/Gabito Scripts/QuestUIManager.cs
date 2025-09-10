using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text questText;

    private void Start()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnQuestUpdated += UpdateQuest;

            // Initialize: if player already finished Jolen, show Turumpo quest
            if (ObjectiveManager.Instance.jolenCompleted && ObjectiveManager.Instance.turumpoUnlocked)
            {
                UpdateQuest("Find <color=purple>Andrea</color>. She can be found in the end of Martinez Street");
            }
            else
            {
                UpdateQuest("Find <color=green>Nina</color>. She can be found besides a fishball cart in Gen street");
            }
        }
    }

    private void OnDestroy()
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.OnQuestUpdated -= UpdateQuest;
        }
    }

    private void UpdateQuest(string message)
    {
        if (questText != null)
            questText.text = message;
    }
}
