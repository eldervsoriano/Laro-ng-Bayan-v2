using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    // Track which games/objectives are unlocked
    public bool jolenCompleted = false;
    public bool turumpoUnlocked = false;

    public bool turumpoJustUnlocked = false;


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

    public void CompleteJolen()
    {
        jolenCompleted = true;

        if (!turumpoUnlocked) // only set the first time
        {
            turumpoUnlocked = true;
            turumpoJustUnlocked = true; // mark as freshly unlocked
            Debug.Log("Jolen complete! Turumpo is now unlocked.");
        }
    }
}
