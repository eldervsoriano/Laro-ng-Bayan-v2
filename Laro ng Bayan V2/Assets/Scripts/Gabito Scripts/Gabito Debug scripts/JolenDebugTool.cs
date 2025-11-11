using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JolenDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10;
    private bool showDebugPanel = false;
    private Vector2 scrollPos;
    private Rect windowRect = new Rect(20, 20, 350, 300);

    private JolenGameManager gm;

    private void Start()
    {
        gm = JolenGameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    private void OnGUI()
    {
        if (!showDebugPanel || gm == null) return;
        windowRect = GUI.Window(7777, windowRect, DrawWindow, "Jolen Debug Tool");
    }

    private void DrawWindow(int id)
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height - 20));

        GUILayout.BeginVertical("box");

        // ---------- AI Settings ----------
        GUILayout.Label("<b>AI Settings</b>");
        bool newAIState = GUILayout.Toggle(gm.isAIEnabled, "Enable AI for Player 2");
        if (newAIState != gm.isAIEnabled)
        {
            gm.ToggleAI(newAIState);
            Debug.Log("AI Mode set to: " + newAIState);
        }

        GUILayout.Space(10);

        // ---------- Winning Condition ----------
        GUILayout.Label("<b>Winning Condition</b>");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Winning Score:", GUILayout.Width(120));
        string newScoreStr = GUILayout.TextField(gm.winningScore.ToString(), GUILayout.Width(60));
        if (int.TryParse(newScoreStr, out int newScore))
        {
            gm.winningScore = Mathf.Clamp(newScore, 1, 50);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // ---------- Score Simulation ----------
        GUILayout.Label("<b>Player Scores</b>");
        GUILayout.BeginHorizontal();
        GUILayout.Label($"P1: {GetPlayerScore(1)}", GUILayout.Width(80));
        if (GUILayout.Button("+1 P1", GUILayout.Height(25))) SimulateMarbleHit(1);
        GUILayout.Label($"P2: {GetPlayerScore(2)}", GUILayout.Width(80));
        if (GUILayout.Button("+1 P2", GUILayout.Height(25))) SimulateMarbleHit(2);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // ---------- Force Win ----------
        GUILayout.Label("<b>Force Win</b>");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Force P1 Win", GUILayout.Height(25)))
        {
            gm.SendMessage("EndGame", 1, SendMessageOptions.DontRequireReceiver);
        }
        if (GUILayout.Button("Force P2 Win", GUILayout.Height(25)))
        {
            gm.SendMessage("EndGame", 2, SendMessageOptions.DontRequireReceiver);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("<i>Press F10 to toggle</i>");

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private int GetPlayerScore(int player)
    {
        if (player == 1)
        {
            return (int)typeof(JolenGameManager)
                .GetField("player1Score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(gm);
        }
        else
        {
            return (int)typeof(JolenGameManager)
                .GetField("player2Score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(gm);
        }
    }

    private void SimulateMarbleHit(int player)
    {
        if (player == gm.GetCurrentPlayer())
        {
            gm.MarbleKnockedOut(null);
        }
        else
        {
            int prev = gm.GetCurrentPlayer();
            typeof(JolenGameManager)
                .GetField("currentPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(gm, player);
            gm.MarbleKnockedOut(null);
            typeof(JolenGameManager)
                .GetField("currentPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(gm, prev);
        }
    }
}
