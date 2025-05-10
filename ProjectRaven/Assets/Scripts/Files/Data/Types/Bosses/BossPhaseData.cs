using System.Collections.Generic;

public class BossPhaseData 
{
    // Name
    public string phaseName;

    public float BPM;
    
    // 2D List for access to beatData in different beats 
    // one row = one beat
    // one cell = one attack
    public List<List<BeatData>> beatMatrix;

    // For checking if condition for phase transition to next is met
    public string phaseChangeConditionType = "BOSSHPPHASEPERCENTAGE"; //Accepted values: BOSSHPPERCENTAGE, BOSSHPPHASEPERCENTAGE, TIMER
    public int timerAmount; // If phaseChangeConditionType = TIMER, this is the amount of seconds used 
    public float percentageAmount; // IF phaseChangeConditionType = BOSSHPPERCENTAGE, this is the amount where phase is changed to the next


    // Constructors

    public BossPhaseData(string phaseName, List<List<BeatData>> beatMatrix, float BPM)
    {
        this.phaseName = phaseName;
        this.beatMatrix = beatMatrix;
        this.BPM = BPM;
        phaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
        timerAmount = 0;
        percentageAmount = 0;     
    }

    public BossPhaseData()
    {
        phaseName = "";
        beatMatrix = new();
        BPM = 0;
        phaseChangeConditionType = "BOSSHPPHASEPERCENTAGE";
        timerAmount = 0;
        percentageAmount = 0; 
    }
}
