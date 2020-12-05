using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevel : MonoBehaviour
{
    [SerializeField] List<MapLevelData> _levels;
    public static ChooseLevel ChooseLevelInstance;

    private void Awake()
    {
        ChooseLevelInstance = this;
    }
    public string CanGoLeft(string levelName)
    {
        foreach(MapLevelData data in _levels)
        {
            if (data.name.Equals(levelName))
                return data.left;
        }
        return "";
    }
    public string CanGoRight(string levelName)
    {
        foreach (MapLevelData data in _levels)
        {
            if (data.name.Equals(levelName))
                return data.right;
        }
        return "";
    }
    public string CanGoUp(string levelName)
    {
        foreach (MapLevelData data in _levels)
        {
            if (data.name.Equals(levelName))
                return data.up;
        }
        return "";
    }
    public string CanGoDown(string levelName)
    {
        foreach (MapLevelData data in _levels)
        {
            if (data.name.Equals(levelName))
                return data.down;
        }
        return "";
    }
    public MapLevelData GetCurrentLevel(string levelName)
    {
        foreach(MapLevelData data in _levels)
        {
            if (data.name.Equals(levelName))
                return data;
        }
        return null;
    }
}
