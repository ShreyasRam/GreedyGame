using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameDataObject))]
public class GameDataObjectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Editor"))
        {
            GameDataObjectEditorWindow.ShowWindow((GameDataObject) target);
        }
    }
}