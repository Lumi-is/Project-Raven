using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    // Name
    public string roomName;

    // List of BeatData objects to be instantiated from LevelEventManager 
    // according to beat by a song determined in an AreaLevelData Tuple
    public List<List<BeatData>> beatMatrix;


    public RoomData(string roomName, List<List<BeatData>> beatList2D)
    {
        this.roomName = roomName;
        beatMatrix = beatList2D;
    }
}
