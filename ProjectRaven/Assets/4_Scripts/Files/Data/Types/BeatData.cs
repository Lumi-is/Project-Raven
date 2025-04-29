using UnityEngine;

public class BeatData 
{
    //For spawner script to determine the type of attack to spawn
    public int behaviorType;

    //If null, means position is determined in behavior type script. Otherwise spawns object in position [attackPos]
    public float[] attackPos;


    public BeatData(int behaviorType, float[] attackPos)
    {
        this.behaviorType = behaviorType;
        this.attackPos = attackPos;
    }

    public BeatData(int behaviorType)
    {
        this.behaviorType = behaviorType;
        attackPos = new float[2];
    }

    public BeatData(float[] attackPos)
    {
        behaviorType = 0;
        this.attackPos = attackPos;
    }
    public BeatData()
    {
        behaviorType = 0;
        attackPos = new float[2];
    }
}
