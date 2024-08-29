using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList",menuName = "New Playlist/Create New Playlist")]
public class LevelList : ScriptableObject
{
    readonly List<LevelData> levelList = new();

    public void AddLevel(LevelData newLevelData)
    {
        levelList.Add(newLevelData);
    }
}
