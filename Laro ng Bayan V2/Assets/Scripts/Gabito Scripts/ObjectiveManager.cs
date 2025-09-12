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

    // For Tumbang Preso
    public bool tumbangPresoCompleted = false;
    public bool spiderDerbyUnlocked = false;
    public bool spiderDerbyJustUnlocked = false;

    // For Spider Derby
    public bool spiderDerbyCompleted = false;


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

    // === Jolen ===
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

    // === Turumpo ===
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

    // === Tumbang Preso ===
    public void StartTumbangPresoQuest()
    {
        if (tumbangPresoUnlocked && !tumbangPresoCompleted)
        {
            OnQuestUpdated?.Invoke("Find <color=orange>Charles</color>. He can be found near the basketball court.");
        }
    }

    public void CompleteTumbangPreso()
    {
        tumbangPresoCompleted = true;

        if (!spiderDerbyUnlocked) // unlock finale
        {
            spiderDerbyUnlocked = true;
            spiderDerbyJustUnlocked = true;

            Debug.Log("Tumbang Preso complete! Spider Derby is now unlocked.");
            OnQuestUpdated?.Invoke("Find <color=red>Michael</color>. He is waiting for you at the street for the Spider Derby finale!");
        }

    }

    // === Spider Derby ===
    public void StartSpiderDerbyQuest()
    {
        if (spiderDerbyUnlocked && !spiderDerbyCompleted)
        {
            OnQuestUpdated?.Invoke("Find <color=red>Michael</color>. He is waiting for you at the street for the Spider Derby finale!");
        }
    }

    public void CompleteSpiderDerby()
    {
        spiderDerbyCompleted = true;
        Debug.Log("Spider Derby complete! You’ve finished all quests!");

        // Final quest text — can be victory or credits trigger
        OnQuestUpdated?.Invoke("<color=yellow>Congratulations!</color> You’ve completed all Larong Pinoy challenges!");
    }
}
