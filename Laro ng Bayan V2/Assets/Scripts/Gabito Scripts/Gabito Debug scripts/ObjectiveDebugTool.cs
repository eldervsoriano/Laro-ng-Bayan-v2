using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDebugTool : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F9; // secret key
    private bool showDebugPanel = false;
    private Vector2 scrollPosition;
    private ObjectiveManager manager;

    private Rect windowRect = new Rect(20, 20, 320, 450); // better size

    private void Start()
    {
        manager = ObjectiveManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showDebugPanel = !showDebugPanel;
    }

    private void OnGUI()
    {
        if (!showDebugPanel || manager == null) return;

        // draw the window properly aligned
        windowRect = GUI.Window(123456, windowRect, DrawWindow, "Objective Debug Tool");
    }

    private void DrawWindow(int windowID)
    {
        // scroll area
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(300), GUILayout.Height(370));
        GUILayout.BeginVertical("box");

        GUILayout.Label("<b>Jolen</b>");
        manager.jolenCompleted = GUILayout.Toggle(manager.jolenCompleted, "Jolen Completed");
        manager.turumpoUnlocked = GUILayout.Toggle(manager.turumpoUnlocked, "Turumpo Unlocked");
        manager.turumpoJustUnlocked = GUILayout.Toggle(manager.turumpoJustUnlocked, "Turumpo Just Unlocked");

        GUILayout.Space(5);
        GUILayout.Label("<b>Turumpo</b>");
        manager.turumpoCompleted = GUILayout.Toggle(manager.turumpoCompleted, "Turumpo Completed");
        manager.tumbangPresoUnlocked = GUILayout.Toggle(manager.tumbangPresoUnlocked, "Tumbang Preso Unlocked");
        manager.tumbangPresoJustUnlocked = GUILayout.Toggle(manager.tumbangPresoJustUnlocked, "Tumbang Preso Just Unlocked");

        GUILayout.Space(5);
        GUILayout.Label("<b>Tumbang Preso</b>");
        manager.tumbangPresoCompleted = GUILayout.Toggle(manager.tumbangPresoCompleted, "Tumbang Preso Completed");
        manager.spiderDerbyUnlocked = GUILayout.Toggle(manager.spiderDerbyUnlocked, "Spider Derby Unlocked");
        manager.spiderDerbyJustUnlocked = GUILayout.Toggle(manager.spiderDerbyJustUnlocked, "Spider Derby Just Unlocked");

        GUILayout.Space(5);
        GUILayout.Label("<b>Spider Derby</b>");
        manager.spiderDerbyCompleted = GUILayout.Toggle(manager.spiderDerbyCompleted, "Spider Derby Completed");
        manager.showFinalPanel = GUILayout.Toggle(manager.showFinalPanel, "Show Final Panel");

        GUILayout.EndVertical(); // inner box
        GUILayout.Space(5);

        // control buttons
        GUILayout.BeginHorizontal();
        //if (GUILayout.Button("Save", GUILayout.Height(25))) manager.SaveProgress();
        //if (GUILayout.Button("Load", GUILayout.Height(25))) manager.LoadProgress();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (GUILayout.Button("Reset All Progress", GUILayout.Height(25)))
            manager.ResetProgress();

        GUILayout.EndScrollView();

        // make window draggable
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
