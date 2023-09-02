// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using Unity.Plastic.Newtonsoft.Json;
// using Unity.Mathematics;

// public class ObjectTemplateEditorWindow : EditorWindow
// {
//     private ObjectTemplate template;
//     public GameObject gm;
//     private string jsonText = "";
//     public RectTransform panelParent;
//     public Dictionary<string, ObjectTemplate> objDict = new ();

//     public Color color;

//     [MenuItem("Window/Object Template Editor")]
//     public static void ShowWindow()
//     {
//         GetWindow<ObjectTemplateEditorWindow>("Object Template Editor");
//     }
//     bool OnAddButton;
//     bool OnAddImage;
//     bool OnAddText;
//     bool OnGenerate;
//     Vector2 ScrollPos;

//     void OnGUI() 
//     {
//         GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);
//         gm = EditorGUILayout.ObjectField("Template", gm, typeof(GameObject), true) as GameObject;
//         panelParent = EditorGUILayout.ObjectField("Panel Parent", panelParent, typeof(RectTransform), true) as RectTransform;
//         if (gm != null)
//         {
//             ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos,GUILayout.Height(200));
//             jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.ExpandHeight(true));
//             EditorGUILayout.EndScrollView();

//             GUILayout.BeginHorizontal();
//             if (GUILayout.Button("Load JSON"))
//             {
//                 // LoadFromJSON();
//             }
//             if (GUILayout.Button("Save JSON"))
//             {
//                 // SaveToJSON();
//             }
//             GUILayout.EndHorizontal();
//         }
//         GUILayout.BeginHorizontal();
//         if(GUILayout.Button("Add Button"))
//         {
//             OnAddButton = true;
            
//         }
//         if(GUILayout.Button("Add Image"))
//         {
//             OnAddImage = true;
            
//         }
//         if(GUILayout.Button("Add Text"))
//         {
//             OnAddText = true;
            
//         }
//         GUILayout.EndHorizontal();
//         color = EditorGUILayout.ColorField("Color", Color.black);
//         if(GUILayout.Button("Generate Panel"))
//         {
//             OnGenerate = true;
            
//         }
//         if(GUILayout.Button("Clear"))
//         {
//             objDict.Clear(); 
//             jsonText = "";
//         }

//         if(OnAddButton)
//         {
//             if (objDict.ContainsKey("Button"));
//                 SetObjectElement("Button");
//             OnAddButton = false;
//         }
//         if (OnAddImage)
//         {
//             if(objDict.ContainsKey("Image")) return;
//             SetObjectElement("Image");
//             OnAddImage = false;
//         }
//         if(OnAddText)
//         {
//             if(objDict.ContainsKey("Text")) return;
//             SetObjectElement("Text");
//             OnAddText = false;
//         }
        

//         if(OnGenerate)
//         {
//             GenerateUI();
//             OnGenerate = false;
//         }
//     }

//     private void SetObjectElement(string elementName)
//     {
//         SaveToJSON(elementName, Vector3.one, Vector3.one, Quaternion.identity, Color.red);
//         template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
//         objDict.Add(elementName, template);
//         Debug.Log(objDict.Keys);
//         jsonText = JsonConvert.SerializeObject(objDict, Formatting.Indented, new JsonSerializerSettings
//         {
//             ReferenceLoopHandling =  ReferenceLoopHandling.Ignore,
//         });
//     }

//     void GenerateUI()
//     {
//         LoadFromJSON();
//         if(panelParent.Find("UIBase") == null)
//         {
//             GameObject prefab = Resources.Load<GameObject>("UIBase");
//             GameObject uiObject = Instantiate(prefab, panelParent.transform, false);

//         }
//         foreach (var item in objDict)
//         {

            
//             GameObject prefab1 = Resources.Load<GameObject>(item.Key);
//             Instantiate(prefab1, panelParent.GetChild(0).transform, false);

//             prefab1.GetComponent<RectTransform>().localPosition = item.Value.position;
//             prefab1.GetComponent<RectTransform>().localScale = item.Value.scale;
            
//             // prefab1.transform.localRotation = item.Value.rotation;
//         }
//     }
//     // private void OnGUI()
//     // {
//     //     GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);

//     //     // gm = EditorGUILayout.ObjectField("Template", gm, typeof(GameObject), true) as GameObject;

//     //     // if (gm != null)
//     //     // {
//     //     //     jsonText = EditorGUILayout.TextArea(jsonText, GUILayout.Height(500));

//     //     //     GUILayout.BeginHorizontal();
//     //     //     if (GUILayout.Button("Load JSON"))
//     //     //     {
//     //     //         LoadFromJSON();
//     //     //     }
//     //     //     if (GUILayout.Button("Save JSON"))
//     //     //     {
//     //     //         SaveToJSON();
//     //     //     }
//     //     //     GUILayout.EndHorizontal();
//     //     // }
//     // }

//     private void LoadFromJSON()
//     {
//         try
//         {
//             template = JsonUtility.FromJson<ObjectTemplate>(jsonText);
            
//             gm.GetComponent<ObjectTemplateView>().SetObjectTemplate(template);
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("Error loading JSON: " + e.Message);
//         }
//     }

//     private void SaveToJSON(string name, Vector3 scale, Vector3 position,quaternion rotation, Color color)
//     {
//         try
//         {
//             ObjectTemplate data = gm.GetComponent<ObjectTemplateView>().Create(name, scale, position, rotation, color);
//             jsonText = JsonUtility.ToJson(data, true);
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("Error saving JSON: " + e.Message);
//         }
//     }
// }
