using System;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    // Track which games/objectives are unlocked
    public bool jolenCompleted = false;
    public bool turumpoUnlocked = false;
    public bool turumpoJustUnlocked = false;

    // For Turumpo
    public bool turumpoCompleted = false;
    public bool tumbangPresoUnlocked = false;
    public bool tumbangPresoJustUnlocked = false;

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
        OnQuestUpdated?.Invoke("Find <color=green>Nina</color>. She can be found beside a fishball cart in Gen street.");
    }

    public void CompleteJolen()
    {
        jolenCompleted = true;

        if (!turumpoUnlocked) // only set the first time
        {
            turumpoUnlocked = true;
            turumpoJustUnlocked = true;

            Debug.Log("Jolen complete! Turumpo is now unlocked.");
            OnQuestUpdated?.Invoke("Find <color=purple>Andrea</color>. She can be found at the end of Martinez Street.");
        }
    }

    public void StartTurumpoQuest()
    {
        if (turumpoUnlocked && !turumpoCompleted)
        {
            OnQuestUpdated?.Invoke("Find <color=purple>Andrea</color>. She can be found at the end of Martinez Street.");
        }
    }

    public void CompleteTurumpo()
    {
        turumpoCompleted = true;

        if (!tumbangPresoUnlocked) // only unlock first time
        {
            tumbangPresoUnlocked = true;
            tumbangPresoJustUnlocked = true;

            Debug.Log("Turumpo complete! Tumbang Preso is now unlocked.");
            OnQuestUpdated?.Invoke("Find <color=orange>Charles</color>. He can be found near the basketball court.");
        }
    }

    public void StartTumbangPresoQuest()
    {
        if (tumbangPresoUnlocked && !turumpoCompleted)
        {
            OnQuestUpdated?.Invoke("Find <color=orange>Charles</color>. He can be found near the basketball court.");
        }
    }
}
