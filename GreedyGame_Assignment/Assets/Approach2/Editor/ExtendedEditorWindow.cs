using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Codice.CM.Client.Differences;
using System.IO;

class ExtendedEditorWindow : EditorWindow 
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;
    Vector2 ScrollPos;
    private string jsonText = "";
    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    GameObject prefab;
    GameObject uiObject;
    GameObject gm;
    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;

        foreach (SerializedProperty property1 in property)
        {
            if(!string.IsNullOrEmpty(lastPropPath) && property1.propertyPath.Contains(lastPropPath)) {continue;}
            lastPropPath = property1.propertyPath;
            EditorGUILayout.PropertyField(property1, drawChildren);
        }
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Convert to json"))
        {
            jsonText = JsonUtility.ToJson(serializedObject.targetObjects[0], prettyPrint: true);
            // System.IO.File.WriteAllText(Application.dataPath + "/templateData.txt", jsonText);
        }
        
        if (GUILayout.Button("instantiate"))
        {
            // Undo.RecordObject()
            ClearScreen();

            prefab = Resources.Load<GameObject>("Canvas");
            uiObject = Instantiate(prefab);
            GameDataObject s = (GameDataObject)property.serializedObject.targetObjects[0];

            for (int i = 0; i < s.objectTemplate.Count; i++)
            {
                var p = s.objectTemplate[i];
                CreateHierarchy(p.data, uiObject.transform);
                // SetElement(p.data[i], uiObjects);
            }
        }
        EditorGUILayout.EndHorizontal();
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos,GUILayout.Height(600));
        jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

    }
    void CreateHierarchy(Data node, Transform parentTransform)
    {
        // Instantiate a GameObject from the prefab
        // GameObject obj = Instantiate(prefab, parentTransform);
        GameObject prefabss = Resources.Load<GameObject>(node.propertyType.ToString());
        GameObject obj = Instantiate(prefabss, parentTransform, true);
        // Set the name of the GameObject
        obj.name = node.name;
        obj.GetComponent<RectTransform>().anchoredPosition = node.position;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(node.width, node.height);

        SetElement(node, obj);
        // Recursively create children objects
        foreach (var childNode in node.children)
        {
            CreateHierarchy(childNode, obj.transform);
        }
    }

    private static void ClearScreen()
    {
        Canvas[] foundCanvasObjects = FindObjectsOfType<Canvas>();
        foreach (var canvas in foundCanvasObjects)
        {
            DestroyImmediate(canvas.gameObject);
        }
    }

    private static void SetElement(Data data, GameObject uiObjects)
    {
        if (data.propertyType == PropertyType.Button)
        {
            Button button = uiObjects.GetComponent<Button>();
            try
            {
                button.image.color = data.color;
                button.image.sprite = data.sprite;
                button.GetComponentInChildren<Text>().text = data.description;             
            }
            catch (System.Exception)
            {
                //null exception
            }

        }
        if (data.propertyType == PropertyType.Image)
        {
            Image image = uiObjects.GetComponent<Image>();
            try
            {
                image.color = data.color;
                image.sprite = data.sprite;       
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
        if (data.propertyType == PropertyType.Text)
        {
            Text text = uiObjects.GetComponent<Text>();
            try
            {
                text.color = data.color;
                text.text = data.description;  
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
    }

    [MenuItem("GreedyGame_Assignment/ExtendedWindow")]
    static void ShowWindow() {
        var window = GetWindow<ExtendedEditorWindow>();
        window.titleContent = new GUIContent("ExtendedWindow");
        window.Show();
    }


    // public TextAsset loadFile;
    protected void DrawSidebar(SerializedProperty property)
    {
        // loadFile = EditorGUILayout.ObjectField("Load", loadFile, typeof(TextAsset), false) as TextAsset;
        // if (GUILayout.Button("Load Saved Data"))
        // {
        //     jsonText = loadFile.ToString();
        // }
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
}