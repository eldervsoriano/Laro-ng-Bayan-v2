using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurumpoDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10; // Secret key for this specific debug tool
    private bool showDebugPanel = false;
    private Vector2 scrollPosition;

    private TurompoGameManager gm;

    private Rect windowRect = new Rect(360, 20, 340, 500); // separate from the Objective Debug Tool

    private void Start()
    {
        gm = TurompoGameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    private void OnGUI()
    {
        if (!showDebugPanel || gm == null) return;

        windowRect = GUI.Window(4444, windowRect, DrawWindow, "Turumpo Debug Tool");
    }

    private void DrawWindow(int id)
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(320), GUILayout.Height(420));
        GUILayout.BeginVertical("box");

        GUILayout.Label("<b>AI SETTINGS</b>");
        gm.enableSinglePlayerMode = GUILayout.Toggle(gm.enableSinglePlayerMode, "Enable Single Player Mode");

        GUILayout.BeginHorizontal();
        GUILayout.Label("AI Difficulty:", GUILayout.Width(100));
        gm.aiDifficultyLevel = GUILayout.HorizontalSlider(gm.aiDifficultyLevel, 0f, 1f);
        GUILayout.Label(gm.aiDifficultyLevel.ToString("F2"), GUILayout.Width(35));
        GUILayout.EndHorizontal();

        gm.adaptiveAIDifficulty = GUILayout.Toggle(gm.adaptiveAIDifficulty, "Adaptive AI Difficulty");

        GUILayout.Space(10);
        GUILayout.Label("<b>GAME SETTINGS</b>");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Duration:", GUILayout.Width(100));
        string durationStr = GUILayout.TextField(gm.gameDuration.ToString("F0"), GUILayout.Width(60));
        if (float.TryParse(durationStr, out float newDuration)) gm.gameDuration = Mathf.Max(10f, newDuration);
        GUILayout.Label("sec");
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("<b>PROGRESSION SETTINGS</b>");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Interval:", GUILayout.Width(100));
        string progInterval = GUILayout.TextField(gm.progressionInterval.ToString("F1"), GUILayout.Width(60));
        if (float.TryParse(progInterval, out float newInterval)) gm.progressionInterval = Mathf.Max(1f, newInterval);
        GUILayout.Label("sec");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spin Decay +", GUILayout.Width(100));
        string decayStr = GUILayout.TextField(gm.spinDecayIncrease.ToString("F1"), GUILayout.Width(60));
        if (float.TryParse(decayStr, out float newDecay)) gm.spinDecayIncrease = Mathf.Max(0f, newDecay);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Note Speed +", GUILayout.Width(100));
        string speedStr = GUILayout.TextField(gm.noteSpeedIncrease.ToString("F1"), GUILayout.Width(60));
        if (float.TryParse(speedStr, out float newSpeed)) gm.noteSpeedIncrease = Mathf.Max(0f, newSpeed);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn Rate -", GUILayout.Width(100));
        string spawnStr = GUILayout.TextField(gm.spawnRateDecrease.ToString("F2"), GUILayout.Width(60));
        if (float.TryParse(spawnStr, out float newSpawn)) gm.spawnRateDecrease = Mathf.Max(0f, newSpawn);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Restart Game", GUILayout.Height(25)))
        {
            gm.RestartGame();
        }
        if (GUILayout.Button("Reset Difficulty", GUILayout.Height(25)))
        {
            // quick reflection to call private ResetDifficulty()
            var method = typeof(TurompoGameManager).GetMethod("ResetDifficulty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(gm, null);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        if (GUILayout.Button("Force Game Over", GUILayout.Height(25)))
        {
            gm.GameTimeOver();
        }

        GUILayout.EndScrollView();
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
