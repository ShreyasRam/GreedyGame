using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GreedyGame/GameDataObject")]
public class GameDataObject : ScriptableObject
{
    public List <ObjectTemplate> objectTemplate = new List<ObjectTemplate>();
}