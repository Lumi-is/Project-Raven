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
    public string PhaseChangeConditionType = "BOSSHPPHASEPERCENTAGE"; //Accepted values: BOSSHPPERCENTAGE, BOSSHPPHASEPERCENTAGE, TIMER, AREA

    // Constructors

    public PhaseData(string phaseName, List<List<BeatData>> beatMatrix, float BPM, string PhaseChangeConditionType)
    {
        this.phaseName = phaseName;
        this.beatMatrix = beatMatrix;
        this.BPM = BPM;
        this.PhaseChangeConditionType = PhaseChangeConditionType;        
    }

    public PhaseData(string phaseName, List<List<BeatData>> beatMatrix, float BPM)
    {
        this.phaseName = phaseName;
        this.beatMatrix = beatMatrix;
        this.BPM = BPM;
        PhaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
    }

    public PhaseData(string phaseName, List<List<BeatData>> beatDataArray)
    {
        this.phaseName = phaseName;
        beatMatrix = beatDataArray;
        BPM = 0;
        PhaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
    }

    public PhaseData(string phaseName)
    {
        this.phaseName = phaseName;
        beatMatrix = new();
        BPM = 0;
        PhaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
    }

    public PhaseData()
    {
        phaseName = "default";
        beatMatrix = new();
        BPM = 0;
        PhaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
    }
}
