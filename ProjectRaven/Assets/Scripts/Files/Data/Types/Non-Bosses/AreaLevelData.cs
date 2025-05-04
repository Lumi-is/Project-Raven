using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaLevelData
{
    // Name
    public string areaLevelName;

    // List of different "rooms" in the area
    public Tuple<List<AreaData>,List<AudioClip>> areaDataList;

    // Constructor
    public AreaLevelData(string areaLevelName)
    {
        this.areaLevelName = areaLevelName;
        areaDataList = new(new(),new());
    }
}
