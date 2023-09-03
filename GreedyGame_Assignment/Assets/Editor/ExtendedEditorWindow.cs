using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using Codice.CM.Client.Differences;
using Unity.Mathematics;

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

    TextAsset loadFile;
    
    /// <summary>
    /// Display all serialized data
    /// </summary>
    /// <param name="property"></param>
    /// <param name="drawChildren"></param>
    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;

        foreach (SerializedProperty property1 in property)
        {
            if (!string.IsNullOrEmpty(lastPropPath) && property1.propertyPath.Contains(lastPropPath)) { continue; }
            lastPropPath = property1.propertyPath;
            EditorGUILayout.PropertyField(property1, drawChildren);
        }

        EditorGUILayout.BeginHorizontal();

        ConvertSerializedObjectToJson();
        InstantiateUiElements(property);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save json"))
        {
            if(string.IsNullOrEmpty(jsonText))
            {
                throw new Exception("Json string Empty. Please Convert The Json First To Save.");
            }

            string inputString = "templateData" + UnityEngine.Random.Range(0, 1000);
            string savePath = Application.dataPath + string.Format("/Saves/{0}.txt", inputString);
            System.IO.File.WriteAllText(savePath, jsonText);

            Debug.Log("Json Saved as " + savePath );
            
        }
        EditorGUILayout.EndHorizontal();

        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Height(600));
        jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.ExpandHeight(true));
        
        EditorGUILayout.EndScrollView();

    }

    private void InstantiateUiElements(SerializedProperty property)
    {
        if (GUILayout.Button("Instantiate"))
        {
            ClearScreen();

            prefab = Resources.Load<GameObject>("Canvas");
            uiObject = Instantiate(prefab);
            GameDataObject s = (GameDataObject)property.serializedObject.targetObjects[0];

            for (int i = 0; i < s.objectTemplate.Count; i++)
            {
                ObjectTemplate objectTemplate = s.objectTemplate[i];
                GenerateUIHierarchy(objectTemplate, uiObject.transform);
            }
        }
    }

    private void ConvertSerializedObjectToJson()
    {
        if (GUILayout.Button("Convert to json"))
        {
            jsonText = JsonUtility.ToJson(serializedObject.targetObjects[0], prettyPrint: true);
        }
    }

    /// <summary>
    /// Instantiate UI hierarchy as per json structure
    /// </summary>
    /// <param name="data"></param>
    /// <param name="parentTransform"></param>
    void GenerateUIHierarchy(ObjectTemplate data, Transform parentTransform)
    {
        // Instantiate a GameObject from the prefab
        GameObject prefabss = Resources.Load<GameObject>(data.propertyType.ToString());
        GameObject obj = Instantiate(prefabss, parentTransform, true);
        // Set the name of the GameObject
        obj.name = data.name;

        SetElement(data, obj);
        // Recursively create children objects
        foreach (ObjectTemplate childData in data.children)
        {
            GenerateUIHierarchy(childData, obj.transform);
        }
    }

    /// <summary>
    /// Clears Any Existing Canvas Elements
    /// </summary>
    private static void ClearScreen()
    {
        Canvas[] foundCanvasObjects = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in foundCanvasObjects)
        {
            DestroyImmediate(canvas.gameObject);
        }
    }

    /// <summary>
    /// Apply property values to their corresponding UI elements
    /// </summary>
    /// <param name="data"></param>
    /// <param name="uiTransform"></param>
    private static void SetElement(ObjectTemplate data, GameObject uiTransform)
    {
        RectTransform uiRect = uiTransform.GetComponent<RectTransform>();
        uiRect.anchoredPosition = data.position;
        uiRect.sizeDelta = new Vector2(data.width, data.height);
        uiRect.localRotation = Quaternion.Euler( data.rotation );
        if (data.propertyType == PropertyType.Button)
        {
            if(uiTransform.TryGetComponent<Button>(out Button button))
            {
                button.image.color = data.color;
                button.image.sprite = data.sprite;
                button.GetComponentInChildren<Text>().text = data.description;             
            }
            else
            {
                Debug.LogError("Button Element Does Not Exist");
            }

        }
        if (data.propertyType == PropertyType.Image)
        {
            Image image = uiTransform.GetComponent<Image>();
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
            Text text = uiTransform.GetComponent<Text>();
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

    // public TextAsset loadFile;
    /// <summary>
    /// Draw Sidebar on editor window to populate elements
    /// </summary>
    /// <param name="property"></param>
    protected void DrawSidebar(SerializedProperty property)
    {
        loadFile = EditorGUILayout.ObjectField("Load text file", loadFile, typeof(TextAsset), false) as TextAsset;
        if (GUILayout.Button("Load Saved Data"))
        {
            jsonText = loadFile.ToString();
        }
        EditorGUILayout.Separator();
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