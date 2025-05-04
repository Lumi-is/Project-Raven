using System.Collections.Generic;
using System.IO.DataHandling;
using UnityEngine.LightTransport.PostProcessing;

public class AreaData
{
    // Name
    public string areaName;

    // BPM
    public float BPM;

    public List<RoomData> roomList = new();


    public AreaData(string areaName,List<List<BeatData>> beatList2D, float BPM, List<string> roomNameList)
    {
        this.areaName = areaName;
        roomList = new();
        int roomAmount = roomNameList.Count;
        this.BPM = BPM;

        for(int i = 0; i < roomAmount; i++)
        {
            roomList.Add(new RoomData(roomNameList[i],beatList2D));
        }
    }

    public AreaData(string areaName)
    {
        this.areaName = areaName;
        roomList = new();
        BPM = 0;
    }

    public AreaData()
    {
        areaName = "";
        roomList = new();
        BPM = 0;
    }
}
