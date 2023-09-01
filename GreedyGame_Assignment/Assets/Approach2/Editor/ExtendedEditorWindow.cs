using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

class ExtendedEditorWindow : EditorWindow 
{

    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;
    Vector2 ScrollPos;
    private string jsonText = "";
    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        // EditorGUILayout.BeginHorizontal();
        foreach (SerializedProperty property1 in property)
        {
            if(property1.isArray && property1.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                property1.isExpanded = EditorGUILayout.Foldout(property1.isExpanded, property1.displayName);
                EditorGUILayout.EndHorizontal();

                if(property1.isExpanded)
                {
                    EditorGUI.indentLevel ++;
                    DrawProperties(property1, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && property1.propertyPath.Contains(lastPropPath)) {continue;}
                lastPropPath = property1.propertyPath;
                EditorGUILayout.PropertyField(property1, drawChildren);
            } 

        }
        // EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Convert to json"))
        {
            jsonText = JsonUtility.ToJson(serializedObject.targetObjects[0], prettyPrint: true);
            Debug.Log(jsonText);
            
        }
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos,GUILayout.Height(200));
        jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("instantiate"))
        {
            GameObject prefab = Resources.Load<GameObject>("Canvas");
            GameObject uiObject = Instantiate(prefab);
            GameDataObject s = (GameDataObject) property.serializedObject.targetObjects[0];
            Debug.Log(s.objectTemplate[0].name);
            // foreach (SerializedProperty property1 in property.serializedObject.GetProperties())
            // {
            //     var s =  property1.serializedObject.FindProperty("propertyType");
            // }
        }
    }

    [MenuItem("GreedyGame_Assignment/ExtendedWindow")]
    static void ShowWindow() {
        var window = GetWindow<ExtendedEditorWindow>();
        window.titleContent = new GUIContent("ExtendedWindow");
        window.Show();
    }

    protected void DrawSidebar(SerializedProperty property)
    {
        if (GUILayout.Button("Add Item"))
        {
            property.InsertArrayElementAtIndex(property.arraySize);
        }
        if (GUILayout.Button("Remove Item") && property.arraySize > 0)
        {
            property.DeleteArrayElementAtIndex(property.arraySize - 1);
        }
        foreach (SerializedProperty p in property)
        {
            if(GUILayout.Button(p.displayName))
            {
                selectedPropertyPath = p.propertyPath;
            }
        }

        if(!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            
        }
    }

    void OnGUI() {
        
    }
}