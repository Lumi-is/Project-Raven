using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    // Name
    public string roomName;

    // List of BeatData objects to be instantiated from LevelEventManager 
    // according to beat by a song determined in an AreaLevelData Tuple
    public List<List<BeatData>> beatMatrix;

    public float[] roomPos = new float[3]{0,0,0};

    public float roomRotation;

    public string roomType;


    public RoomData(string roomName, List<List<BeatData>> beatList2D)
    {
        this.roomName = roomName;
        beatMatrix = beatList2D;
        
        roomPos[0] = 0; roomPos[1] = 0; roomPos[2] = 0;
        roomRotation = 0;

        roomType = "none";
    }

    public RoomData()
    {
        roomName = "";
        beatMatrix = new();
        
        roomPos[0] = 0; roomPos[1] = 0; roomPos[2] = 0;
        roomRotation = 0;

        roomType = "";
    }
}
