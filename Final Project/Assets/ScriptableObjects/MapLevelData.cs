using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New LevelData")]
public class MapLevelData : ScriptableObject
{
    public string left, right, up, down;
}
