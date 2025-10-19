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

            // Check if all quests are completed
            if (ObjectiveManager.Instance.jolenCompleted &&
                ObjectiveManager.Instance.turumpoCompleted &&
                ObjectiveManager.Instance.tumbangPresoCompleted &&
                ObjectiveManager.Instance.spiderDerbyCompleted)
            {
                UpdateQuest("<color=yellow>You finished all the tasks!</color>");
                return; // Stop here since it’s the final message
            }


            // Initialize quest text depending on progress
            if (ObjectiveManager.Instance.tumbangPresoCompleted && ObjectiveManager.Instance.spiderDerbyUnlocked)
            {
                UpdateQuest("Find <color=grey>Michael</color>. He is waiting for you besides a sorbetes cart in Gonzales Street");
            }
            else if (ObjectiveManager.Instance.turumpoCompleted && ObjectiveManager.Instance.tumbangPresoUnlocked)
            {
                UpdateQuest("Find <color=orange>Charles</color>. He can be found near the basketball court.");
            }
            else if (ObjectiveManager.Instance.jolenCompleted && ObjectiveManager.Instance.turumpoUnlocked)
            {
                UpdateQuest("Find <color=purple>Andrea</color>. She can be found at the end of Martinez Street.");
            }
            else
            {
                UpdateQuest("Find <color=green>Nina</color>. She can be found beside a fishball cart in Gen street.");
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
