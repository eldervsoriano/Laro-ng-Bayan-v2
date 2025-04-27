using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurompoDifficultyManager : MonoBehaviour
{
    public TurompoRhythmController player1Rhythm;
    public TurompoRhythmController player2Rhythm;

    public void SetEasyDifficulty()
    {
        player1Rhythm.noteSpeed = 3f;
        player1Rhythm.spawnRate = 2f;
        player2Rhythm.noteSpeed = 3f;
        player2Rhythm.spawnRate = 2f;
    }

    public void SetMediumDifficulty()
    {
        player1Rhythm.noteSpeed = 5f;
        player1Rhythm.spawnRate = 1f;
        player2Rhythm.noteSpeed = 5f;
        player2Rhythm.spawnRate = 1f;
    }

    public void SetHardDifficulty()
    {
        player1Rhythm.noteSpeed = 8f;
        player1Rhythm.spawnRate = 0.5f;
        player2Rhythm.noteSpeed = 8f;
        player2Rhythm.spawnRate = 0.5f;
    }
}