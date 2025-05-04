using UnityEngine;

public class BeatData 
{
    //For spawner script to determine the type of attack to spawn
    public string behaviorType;

    //If null, means position is determined in behavior type script. Otherwise spawns object in position [attackPos]
    public float[] attackPos;


    public BeatData(string behaviorType, float[] attackPos)
    {
        this.behaviorType = behaviorType;
        this.attackPos = attackPos;
    }

    public BeatData(string behaviorType)
    {
        this.behaviorType = behaviorType;
        attackPos = new float[2];
    }

    public BeatData(float[] attackPos)
    {
        behaviorType = "none";
        this.attackPos = attackPos;
    }
    public BeatData()
    {
        //should be set to "none" by default
        behaviorType = "LinearBullet";
        attackPos = new float[2];
    }
}
