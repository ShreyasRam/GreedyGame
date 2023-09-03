using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class GameDataObjectEditorWindow : ExtendedEditorWindow {


    Vector2 ScrollPoss;
    [MenuItem("GreedyGame_Assignment/GameDataObjectWindow")]
    public static void ShowWindow(GameDataObject dataObject) 
    {
        GameDataObjectEditorWindow window = GetWindow<GameDataObjectEditorWindow>("Game Data Editor");
        window.titleContent = new GUIContent("GameDataObjectWindow");
        window.serializedObject = new SerializedObject(dataObject);
        window.Show();
    }

    void OnGUI() 
    {
        if(serializedObject == null) return;
        
        serializedObject.Update();
        currentProperty = serializedObject.FindProperty("objectTemplate");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        
        DrawSidebar(currentProperty);  

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        ScrollPoss = EditorGUILayout.BeginScrollView(ScrollPoss,GUILayout.Height(800));
        if(selectedProperty != null)
        {
            DrawProperties(selectedProperty, true);
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list");
        }
        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
}

