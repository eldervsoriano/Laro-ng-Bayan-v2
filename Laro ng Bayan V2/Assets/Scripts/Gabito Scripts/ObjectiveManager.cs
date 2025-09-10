using System;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    // Track which games/objectives are unlocked
    public bool jolenCompleted = false;
    public bool turumpoUnlocked = false;
    public bool turumpoJustUnlocked = false;

    // Event for quest updates
    public event Action<string> OnQuestUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // stays alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartJolenQuest()
    {
        OnQuestUpdated?.Invoke("Find <color=green>Nina</color>. She can be found besides a fishball cart in Gen street");
    }

    public void CompleteJolen()
    {
        jolenCompleted = true;

        if (!turumpoUnlocked) // only set the first time
        {
            turumpoUnlocked = true;
            turumpoJustUnlocked = true;

            Debug.Log("Jolen complete! Turumpo is now unlocked.");
            OnQuestUpdated?.Invoke("Find <color=purple>Andrea</color>. She can be found in the end of Martinez Street"); // push new quest
        }
    }
}
