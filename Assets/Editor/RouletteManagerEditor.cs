using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RouletteManager))]
public class RouletteManagerEditor : Editor
{
    RouletteManager rouletteManager;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (rouletteManager == null)
            rouletteManager = target as RouletteManager;
        if (GUILayout.Button("Create"))
        {
            rouletteManager.CreateRoulette();
        }
    }
}