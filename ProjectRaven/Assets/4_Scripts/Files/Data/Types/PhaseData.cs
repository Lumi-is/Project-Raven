using System.Collections.Generic;
using UnityEngine;

public class PhaseData 
{
    // Name
    public string phaseName;

    public float BPM;
    
    // 2D List for access to beatData in different beats 
    // one row = one beat
    // one cell = one attack
    public List<List<BeatData>> beatMatrix;

    // For checking if condition for phase transition to next is met
    public bool phaseChangeConditionMet;

    // Constructors
    public PhaseData(string phaseName, List<List<BeatData>> beatDataArray, float BPM)
    {
        this.phaseName = phaseName;
        this.beatMatrix = beatDataArray;
        this.BPM = BPM;
    }

    public PhaseData(string phaseName, List<List<BeatData>> beatDataArray)
    {
        this.phaseName = phaseName;
        beatMatrix = beatDataArray;
    }

    public PhaseData(string phaseName)
    {
        this.phaseName = phaseName;
        beatMatrix = new();
    }

    public PhaseData()
    {
        phaseName = "";
        beatMatrix = new();
    }
}
