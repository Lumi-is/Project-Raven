using System.Collections.Generic;

public class PhaseData
{
    public string phaseName;
    //2D List for access to beatData in different parts of the song
    public List<List<BeatData>> beatDataArray;

    public PhaseData(string phaseName, List<List<BeatData>> beatDataArray)
    {
        this.phaseName = phaseName;
        this.beatDataArray = beatDataArray;
    }

    public PhaseData(string phaseName)
    {
        this.phaseName = phaseName;
        beatDataArray = new();
    }

    public PhaseData()
    {
        this.phaseName = "";
        beatDataArray = new();
    }
}
