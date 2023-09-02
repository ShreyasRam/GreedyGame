using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// public class ObjectTemplateView : MonoBehaviour
// {
//     public ObjectTemplate objectTemplate;

//     public ObjectTemplate Create(string name, Vector3 scale, Vector3 position,Quaternion rotation, Color color)
//     {
//         // return new ObjectTemplate(name, scale, position,rotation, color);
//     }

//     public ObjectTemplate GetObjectTemplate()
//     {
//         return objectTemplate; 
//     }

//     public void SetObjectTemplate(ObjectTemplate objectTemplate_)
//     {
//         objectTemplate = objectTemplate_;
//     }
// }

[System.Serializable]
public class ObjectTemplate
{
    public Data data;

    // public bool isNested;
    // public string name;
    // public PropertyType propertyType;
    // public Vector3 scale;
    // public Vector3 position;
    // // public Quaternion rotation;
    // public Color color;
    // public float width;
    // public float height;

    // public Sprite sprite;
    // public List<string> description;
}

[System.Serializable]
public struct Data
{
    public bool isNested;
    public string name;
    public PropertyType propertyType;
    public Vector3 scale;
    public Vector3 position;
    // public Quaternion rotation;
    public Color color;
    public float width;
    public float height;

    public Sprite sprite;
    public string description;

    public List<Data> children;
}

public enum PropertyType
{
    Button,
    Image,
    Text
}