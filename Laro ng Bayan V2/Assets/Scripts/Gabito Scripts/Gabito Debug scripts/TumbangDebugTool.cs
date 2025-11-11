using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbangDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10;
    private bool showDebugPanel = false;
    private Rect windowRect = new Rect(20, 260, 360, 250); // initial size
    private Vector2 scrollPos;

    private TumbangGameManager gm;

    void Start()
    {
        gm = TumbangGameManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    void OnGUI()
    {
        if (!showDebugPanel || gm == null) return;
        windowRect = GUI.Window(7788, windowRect, DrawWindow, "Tumbang Preso Debug Tool");
    }

    private void DrawWindow(int id)
    {
        // Scrollable content area (subtract drag bar height)
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height - 20));

        GUILayout.BeginVertical();

        GUILayout.Label("<b>Game Controls</b>");
        GUILayout.Space(4);

        GUILayout.Label("Winning Score:");
        string scoreInput = GUILayout.TextField(gm.winningScore.ToString(), GUILayout.Width(60));
        if (int.TryParse(scoreInput, out int newScore))
            gm.winningScore = Mathf.Clamp(newScore, 1, 50);

        GUILayout.Space(10);
        GUILayout.Label("<b>Score Simulation</b>");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+1 Player 1")) SimulateScore(1);
        if (GUILayout.Button("+1 Player 2")) SimulateScore(2);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("<b>End Game Tests</b>");
        if (GUILayout.Button("Force P1 Victory")) ForceGameOver(1);
        if (GUILayout.Button("Force P2 Victory")) ForceGameOver(2);

        GUILayout.Space(10);
        GUILayout.Label("<i>Press F10 to toggle</i>");

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        // Drag bar (top 20px)
        GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
    }

    private void SimulateScore(int player)
    {
        if (player == 1)
        {
            typeof(TumbangGameManager).GetField("player1Score",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(gm, gm.GetPlayer1Score() + 1);
        }
        else
        {
            typeof(TumbangGameManager).GetField("player2Score",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(gm, gm.GetPlayer2Score() + 1);
        }

        UIManager.Instance.UpdateScore(gm.GetPlayer1Score(), gm.GetPlayer2Score());
        Debug.Log($"Added +1 to Player {player}");
    }

    private void ForceGameOver(int winner)
    {
        UIManager.Instance.ShowWinner(winner);
        ObjectiveManager.Instance?.CompleteTumbangPreso();
        Debug.Log($"Forced game over. Winner: Player {winner}");
    }
}
