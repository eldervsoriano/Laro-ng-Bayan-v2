using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbangDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F10;
    private bool showDebugPanel = false;
    private Rect windowRect = new Rect(20, 20, 400, 400);
    private Vector2 scrollPos;

    private TumbangGameManager gm;
    private DefenderManager def;

    void Start()
    {
        gm = TumbangGameManager.Instance;
        def = DefenderManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    void OnGUI()
    {
        if (!showDebugPanel || gm == null || def == null) return;
        windowRect = GUI.Window(8800, windowRect, DrawWindow, "Tumbang Preso Debug Tool");
    }

    private void DrawWindow(int id)
    {
        // Scrollable content area (leave room for drag bar)
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height - 20));

        GUILayout.BeginVertical();

        // ---------- Game Controls ----------
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

        // ---------- Mini-Game Settings ----------
        GUILayout.Label("<b>Mini-Game Settings</b>");
        GUILayout.Space(4);

        //GUILayout.Label($"Slider Cycle Time: {def.sliderCycleTime:F2}");
        //def.sliderCycleTime = GUILayout.HorizontalSlider(def.sliderCycleTime, 0.5f, 3f);

        GUILayout.Label($"Success Threshold: {def.successThreshold:F2}");
        def.successThreshold = GUILayout.HorizontalSlider(def.successThreshold, 0.02f, 0.3f);

        GUILayout.Label($"Max Cycles: {def.maxCycles}");
        def.maxCycles = Mathf.RoundToInt(GUILayout.HorizontalSlider(def.maxCycles, 1, 6));

        GUILayout.Space(6);

        GUILayout.Label("<b>AI Player Toggles</b>");
        //bool ai1 = def.player1AI != null && def.player1AI.isAI;
        bool ai2 = def.player2AI != null && def.player2AI.isAI;

        //bool newAI1 = GUILayout.Toggle(ai1, "Enable AI Player 1");
        bool newAI2 = GUILayout.Toggle(ai2, "Enable AI Player 2");

        //if (def.player1AI != null) def.player1AI.isAI = newAI1;
        if (def.player2AI != null) def.player2AI.isAI = newAI2;

        GUILayout.Space(8);

        //GUILayout.Label("<b>Quick Mini-Game Tests</b>");
        GUILayout.BeginHorizontal();
        //if (GUILayout.Button("Start MiniGame (P1 Defender)"))
        //    def.StartMiniGame(1, result => Debug.Log($"P1 result: {result}"));
        //if (GUILayout.Button("Start MiniGame (P2 Defender)"))
        //    def.StartMiniGame(2, result => Debug.Log($"P2 result: {result}"));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("<i>Press F10 to toggle</i>");

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        // Drag bar at the top
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