using UnityEngine;
using UnityEngine.UI;

public class UITemplateGenerator : MonoBehaviour
{
    public GameObject panel;
    public TextAsset templateJson;

    private void Start()
    {
        GenerateTemplate();
    }

    private void GenerateTemplate()
    {
        string json = templateJson.text;
        TemplateData templateData = JsonUtility.FromJson<TemplateData>(json);

        foreach (var objData in templateData.objects)
        {
            GameObject prefab = Resources.Load<GameObject>(objData.type);
            GameObject uiObject = Instantiate(prefab, panel.transform);
            
            uiObject.GetComponent<RectTransform>().anchoredPosition = objData.position;
            uiObject.GetComponent<RectTransform>().sizeDelta = objData.size;
            // objData.
            if (objData.type == "Button")
            {
                Button button = uiObject.GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = objData.text;
            }
            // else if (objData.type == "Text")
            // {
            //     Text text = uiObject.GetComponent<Text>();
            //     text.text = objData.text;
            // }
        }
    }
    [System.Serializable]
    public class TemplateObjectData
    {
        public string type;
        public Vector2 position;
        public Vector2 size;
        public string text;
    }

    [System.Serializable]
    public class TemplateData
    {
        public string templateName;
        public TemplateObjectData[] objects;
    }
}
