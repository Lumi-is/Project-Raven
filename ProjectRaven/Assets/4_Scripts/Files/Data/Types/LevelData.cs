using System.Collections.Generic;
using UnityEngine;

public class LevelData 
{
    // Name
    public string levelName;
    // List of phases in current level
    public List<PhaseData> phaseList;


    // Constructor
    public LevelData(string levelName)
    {
        this.levelName = levelName;
        phaseList = new();
    }
}
