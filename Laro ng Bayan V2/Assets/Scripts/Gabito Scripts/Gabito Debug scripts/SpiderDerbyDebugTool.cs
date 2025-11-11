using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SpiderDerbyDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10;
    private bool showDebugPanel = false;
    private Vector2 scrollPos;
    private SpiderGameManager gameManager;

    private Rect windowRect = new Rect(350, 20, 360, 500);

    private void Start()
    {
        gameManager = SpiderGameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    private void OnGUI()
    {
        if (!showDebugPanel || gameManager == null) return;

        try
        {
            windowRect = GUI.Window(654321, windowRect, DrawWindow, "Spider Derby Debug Tool");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("GUI layout error: " + e.Message);
        }
    }

    private void DrawWindow(int id)
    {
        try
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height - 20));
            GUILayout.BeginVertical("box");

            GUILayout.Label("<b>Player Settings</b>");

            DrawIntField("Player 1 Health:", "player1Health");
            DrawIntField("Player 2 Health:", "player2Health");

            GUILayout.Space(10);
            GUILayout.Label("<b>Damage & Timers</b>");
            DrawIntField("Damage per Attack:", "damagePerAttack");
            DrawFloatField("Winner Delay:", "winnerDelay");
            DrawFloatField("Animation Delay:", "animationDelay");
            DrawFloatField("Damage Animation Delay:", "damageAnimationDelay");
            DrawFloatField("Death Animation Delay:", "deathAnimationDelay");
            DrawFloatField("Draw Delay:", "drawDelay");

            GUILayout.Space(10);
            GUILayout.Label("<b>Game State Controls</b>");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Game", GUILayout.Height(25))) gameManager.ResetGame();
            if (GUILayout.Button("Toggle Mode", GUILayout.Height(25))) gameManager.ToggleGameMode();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.Label("Game Mode: " + (gameManager.isSinglePlayerMode ? "Single Player" : "Two Player"));

            GUILayout.Space(10);
            GUILayout.Label("<b>Reduce Health</b>");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Damage P2", GUILayout.Height(25)))
                ForceWin(1);
            if (GUILayout.Button("Damage P1", GUILayout.Height(25)))
                ForceWin(2);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("<i>Press F10 to toggle</i>");

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SpiderDerbyDebugTool GUI Error: " + e.Message);
            GUILayout.EndScrollView();
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private void DrawIntField(string label, string fieldName)
    {
        FieldInfo field = GetPrivateField(fieldName);
        if (field == null) return;

        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(140));
        string strVal = field.GetValue(gameManager).ToString();
        string newStr = GUILayout.TextField(strVal, GUILayout.Width(60));
        if (int.TryParse(newStr, out int newVal))
            field.SetValue(gameManager, newVal);
        GUILayout.EndHorizontal();
    }

    private void DrawFloatField(string label, string fieldName)
    {
        FieldInfo field = GetPrivateField(fieldName);
        if (field == null) return;

        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(140));
        string strVal = field.GetValue(gameManager).ToString();
        string newStr = GUILayout.TextField(strVal, GUILayout.Width(60));
        if (float.TryParse(newStr, out float newVal))
            field.SetValue(gameManager, newVal);
        GUILayout.EndHorizontal();
    }

    private FieldInfo GetPrivateField(string name)
    {
        FieldInfo f = gameManager.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        if (f == null)
        {
            GUILayout.Label($"Missing field: {name}");
        }
        return f;
    }

    private void ForceWin(int player)
    {
        if (player == 1)
        {
            gameManager.TakeDamage(2); // Reduce P2 to 0 health
            gameManager.CheckGameOver();
        }
        else
        {
            gameManager.TakeDamage(1); // Reduce P1 to 0 health
            gameManager.CheckGameOver();
        }
        Debug.Log($"Forced Player {player} to win!");
    }
}
