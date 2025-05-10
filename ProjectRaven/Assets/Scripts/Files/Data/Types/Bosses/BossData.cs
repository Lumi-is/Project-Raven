using System;
using System.Collections.Generic;
using UnityEngine;

public class BossData 
{
    // Name
    public string bossLevelName;
    
    // List of phases and their songs in current boss 
    public Tuple<List<List<BossPhaseData>>,List<AudioClip>> phaseDataList;


    // Constructor
    public BossData(string bossLevelName)
    {
        this.bossLevelName = bossLevelName;
        phaseDataList = new(new(),new());
    }
}
