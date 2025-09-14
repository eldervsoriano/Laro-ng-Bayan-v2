using System;
using UnityEditor;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    // Track which games/objectives are unlocked
    [Header("Jolen")]
    public bool jolenCompleted = false;
    public bool turumpoUnlocked = false;
    public bool turumpoJustUnlocked = false;

    // For Turumpo
    [Header("Turumpo")]
    public bool turumpoCompleted = false;
    public bool tumbangPresoUnlocked = false;
    public bool tumbangPresoJustUnlocked = false;

    // For Tumbang Preso
    [Header("Tumbang Preso")]
    public bool tumbangPresoCompleted = false;
    public bool spiderDerbyUnlocked = false;
    public bool spiderDerbyJustUnlocked = false;

    // For Spider Derby
    [Header("Spider Derby")]
    public bool spiderDerbyCompleted;
    public bool showFinalPanel; // new flag

    // Event for quest updates
    public event Action<string> OnQuestUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // stays alive between scenes
            LoadProgress(); // load when game starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveProgress();
    }

    // === SAVE ===
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("JolenCompleted", jolenCompleted ? 1 : 0);
        PlayerPrefs.SetInt("TurumpoUnlocked", turumpoUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("TurumpoCompleted", turumpoCompleted ? 1 : 0);
        PlayerPrefs.SetInt("TumbangPresoUnlocked", tumbangPresoUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("TumbangPresoCompleted", tumbangPresoCompleted ? 1 : 0);
        PlayerPrefs.SetInt("SpiderDerbyUnlocked", spiderDerbyUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("SpiderDerbyCompleted", spiderDerbyCompleted ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Progress Saved!");
    }

    // === LOAD ===
    public void LoadProgress()
    {
        jolenCompleted = PlayerPrefs.GetInt("JolenCompleted", 0) == 1;
        turumpoUnlocked = PlayerPrefs.GetInt("TurumpoUnlocked", 0) == 1;
        turumpoCompleted = PlayerPrefs.GetInt("TurumpoCompleted", 0) == 1;
        tumbangPresoUnlocked = PlayerPrefs.GetInt("TumbangPresoUnlocked", 0) == 1;
        tumbangPresoCompleted = PlayerPrefs.GetInt("TumbangPresoCompleted", 0) == 1;
        spiderDerbyUnlocked = PlayerPrefs.GetInt("SpiderDerbyUnlocked", 0) == 1;
        spiderDerbyCompleted = PlayerPrefs.GetInt("SpiderDerbyCompleted", 0) == 1;

        Debug.Log("Progress Loaded!");
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

        SaveProgress();
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

        SaveProgress();
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

        SaveProgress();
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
        showFinalPanel = true; // set the flag
        Debug.Log("Spider Derby complete! You’ve finished all quests!");

        OnQuestUpdated?.Invoke("<color=yellow>Congratulations!</color> You’ve completed all Larong Pinoy challenges!");

        SaveProgress();

    }


    // == DELETE ALL PROGRESS ==

    public void ResetProgress()
    {
        // Reset progress flags
        jolenCompleted = false;
        turumpoUnlocked = false;
        turumpoJustUnlocked = false;

        turumpoCompleted = false;
        tumbangPresoUnlocked = false;
        tumbangPresoJustUnlocked = false;

        tumbangPresoCompleted = false;
        spiderDerbyUnlocked = false;
        spiderDerbyJustUnlocked = false;

        spiderDerbyCompleted = false;
        showFinalPanel = false;

        // Clear saves
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Progress has been reset.");

        OnQuestUpdated?.Invoke("Progress reset. Start again from <color=green>Nina</color>!");
    }



}
