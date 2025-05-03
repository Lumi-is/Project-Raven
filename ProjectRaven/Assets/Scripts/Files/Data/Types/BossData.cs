using System;
using System.Collections.Generic;
using UnityEngine;

public class BossData 
{
    // Name
    public string levelName;
    // List of phases in current level

    public Tuple<List<PhaseData>,List<AudioClip>> phaseDataList;


    // Constructor
    public BossData(string levelName)
    {
        this.levelName = levelName;
        phaseDataList = new(new(),new());
    }
}
