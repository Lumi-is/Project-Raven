using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelData : ScriptableObject
{
    public int levelIndex;
    public List<List<BeatData>> beatDataArr;

    public void SaveFile(List<List<BeatData>> transferredData, int transferredIndex)
    {
        beatDataArr = transferredData;
        levelIndex = transferredIndex;
    }

    public List<List<BeatData>> LoadData()
    {
        return beatDataArr;
    }
}
