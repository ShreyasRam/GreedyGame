using UnityEngine;

public class ObjectTemplateView : MonoBehaviour
{
    public ObjectTemplate objectTemplate;

    public ObjectTemplate Create(string name, Vector3 scale, Vector3 position,Vector3 rotation, Color color)
    {
        return new ObjectTemplate(name, scale, position,rotation, color);
    }

    public ObjectTemplate GetObjectTemplate()
    {
        return objectTemplate; 
    }

    public void SetObjectTemplate(ObjectTemplate objectTemplate_)
    {
        objectTemplate = objectTemplate_;
    }
}

[System.Serializable]
public class ObjectTemplate
{
    public string name;
    public PropertyType propertyType;
    public Vector3 scale;
    public Vector3 position, rotation;
    public Color color;

    public Sprite sprite;
    public string description;

    // Add more properties as needed

    public ObjectTemplate(string name, Vector2 scale, Vector3 position,Vector3 rotation, Color color)
    {
        this.name = name;
        this.scale = scale;
        this.position = position;   
        this.rotation = rotation;   
        this.color = color; 
    }
}

public enum PropertyType
{
    Button,
    Image,
    Text
}