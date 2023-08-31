using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

public class ObjectTemplateEditorWindow : EditorWindow
{
    private ObjectTemplate template;
    public GameObject gm;
    private string jsonText = "";
    public RectTransform panelParent;
    public Dictionary<string, ObjectTemplate> objDict = new ();

    [MenuItem("Window/Object Template Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObjectTemplateEditorWindow>("Object Template Editor");
    }
    bool OnAddButton;
    bool OnAddImage;
    bool OnAddText;
    bool OnGenerate;
    void OnGUI() 
    {
        GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);
        gm = EditorGUILayout.ObjectField("Template", gm, typeof(GameObject), true) as GameObject;
        panelParent = EditorGUILayout.ObjectField("Panel Parent", panelParent, typeof(RectTransform), true) as RectTransform;
        if (gm != null)
        {
            jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.Height(500));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load JSON"))
            {
                // LoadFromJSON();
            }
            if (GUILayout.Button("Save JSON"))
            {
                // SaveToJSON();
            }
            GUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Add Button"))
        {
            OnAddButton = true;
            
        }
        if(GUILayout.Button("Add Image"))
        {
            OnAddImage = true;
            
        }
        if(GUILayout.Button("Add Text"))
        {
            OnAddText = true;
            
        }
        if(GUILayout.Button("Generate Panel"))
        {
            OnGenerate = true;
            
        }

        if(OnAddButton)
        {
            if(objDict.ContainsKey("Button")) return;
            SaveToJSON("Button", Vector3.one, Vector3.one,Vector3.zero, Color.red);
            template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
            objDict.Add("Button", template);
            Debug.Log(objDict.Keys);
            jsonText = JsonConvert.SerializeObject(objDict, Formatting.Indented);
            // jsonText = JsonConvert.SerializeObject(objDict);
            // var s =  JsonConvert.SerializeObject(objDict);
            OnAddButton = false;
        }
        if(OnAddImage)
        {
            if(objDict.ContainsKey("Image")) return;
            SaveToJSON("Image", Vector3.one, Vector3.one,Vector3.zero, Color.green);
            template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
            objDict.Add("Image", template);
            Debug.Log(objDict.Keys);
            jsonText = JsonConvert.SerializeObject(objDict, Formatting.Indented);
            // jsonText = JsonConvert.SerializeObject(objDict);
            // var s =  JsonConvert.SerializeObject(objDict);
            OnAddImage = false;
        }
        if(OnAddText)
        {
            if(objDict.ContainsKey("Text")) return;
            SaveToJSON("Text", Vector3.one, Vector3.one,Vector3.zero, Color.yellow);
            template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
            objDict.Add("Text", template);
            Debug.Log(objDict.Keys);
            jsonText = JsonConvert.SerializeObject(objDict, Formatting.Indented);
            // jsonText = JsonConvert.SerializeObject(objDict);
            // var s =  JsonConvert.SerializeObject(objDict);
            OnAddText = false;
        }
        

        if(OnGenerate)
        {
            GenerateUI();
            OnGenerate = false;
        }

        
    }

    void GenerateUI()
    {
        GameObject prefab = Resources.Load<GameObject>("UIBase");
        GameObject uiObject = Instantiate(prefab, panelParent.transform, false);

        foreach (var item in objDict)
        {
            GameObject prefab1 = Resources.Load<GameObject>(item.Key);
            Instantiate(prefab1, panelParent.GetChild(0).transform, false);
        }
    }
    // private void OnGUI()
    // {
    //     GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);

    //     // gm = EditorGUILayout.ObjectField("Template", gm, typeof(GameObject), true) as GameObject;

    //     // if (gm != null)
    //     // {
    //     //     jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.Height(500));

    //     //     GUILayout.BeginHorizontal();
    //     //     if (GUILayout.Button("Load JSON"))
    //     //     {
    //     //         LoadFromJSON();
    //     //     }
    //     //     if (GUILayout.Button("Save JSON"))
    //     //     {
    //     //         SaveToJSON();
    //     //     }
    //     //     GUILayout.EndHorizontal();
    //     // }
    // }

    private void LoadFromJSON()
    {
        try
        {
            template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
            
            gm.GetComponent<ObjectTemplateView>().SetObjectTemplate(template);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading JSON: " + e.Message);
        }
    }

    private void SaveToJSON(string name, Vector3 scale, Vector3 position,Vector3 rotation, Color color)
    {
        try
        {
            ObjectTemplate data = gm.GetComponent<ObjectTemplateView>().Create(name, scale, position, rotation, color);
            jsonText = JsonUtility.ToJson(data, true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving JSON: " + e.Message);
        }
    }
}
