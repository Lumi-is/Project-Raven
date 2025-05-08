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

    public string roomType;


    public RoomData(string roomName, List<List<BeatData>> beatList2D, Vector3 roomPos, string roomType)
    {
        this.roomName = roomName;
        beatMatrix = beatList2D;
        
        this.roomPos[0] = roomPos.x; this.roomPos[1] = roomPos.y; this.roomPos[2] = roomPos.z;

        this.roomType = roomType;
    }

    public RoomData(string roomName, List<List<BeatData>> beatList2D, Vector3 roomPos)
    {
        this.roomName = roomName;
        beatMatrix = beatList2D;
        
        this.roomPos[0] = roomPos.x; this.roomPos[1] = roomPos.y; this.roomPos[2] = roomPos.z;

        roomType = "null";
    }

    public RoomData(string roomName, List<List<BeatData>> beatList2D)
    {
        this.roomName = roomName;
        beatMatrix = beatList2D;
        
        roomPos[0] = 0; roomPos[1] = 0; roomPos[2] = 0;

        roomType = "null";
    }

    public RoomData(string roomName)
    {
        this.roomName = roomName;
        beatMatrix = new();
        
        roomPos[0] = 0; roomPos[1] = 0; roomPos[2] = 0;

        roomType = "null";
    }

    public RoomData()
    {
        this.roomName = "null";
        beatMatrix = new();
        
        roomPos[0] = 0; roomPos[1] = 0; roomPos[2] = 0;

        roomType = "null";
    }
}
