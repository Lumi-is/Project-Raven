using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList",menuName = "New Playlist/Create New Playlist")]
public class LevelList : ScriptableObject
{
    public List<LevelData> levelListData = new();

    public void AddLevel(LevelData newLevelData)
    {
        levelListData.Add(newLevelData);
    }
}
