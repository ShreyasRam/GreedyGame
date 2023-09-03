using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Ui element property
/// </summary>
[System.Serializable]
public class ObjectTemplate
{
    public string name;
    public PropertyType propertyType;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Color color;
    public float width;
    public float height;

    public Sprite sprite;
    public string description;

    public List<ObjectTemplate> children;
}