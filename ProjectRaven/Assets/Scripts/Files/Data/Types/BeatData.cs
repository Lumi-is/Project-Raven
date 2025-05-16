using System;

public class BeatData 
{
    //For spawner script to determine the type of attack to spawn
    public string behaviorType;

    //If null, means position is determined in behavior type script. Otherwise spawns object in position [attackPos]
    public float[] spawnPos;

    //
    public string posAnchor = null;

    public float? lifetime;

    public BeatData(string behaviorType, float[] spawnPos, string posAnchor, float? lifetime)
    {
        this.behaviorType = behaviorType;
        this.spawnPos = spawnPos;
        this.posAnchor = posAnchor;
        this.lifetime = lifetime;
    }

    public BeatData(string behaviorType, float[] spawnPos, string posAnchor)
    {
        this.behaviorType = behaviorType;
        this.spawnPos = spawnPos;
        this.posAnchor = posAnchor;
        lifetime = 10;
    }

    public BeatData(string behaviorType, float[] spawnPos)
    {
        this.behaviorType = behaviorType;
        this.spawnPos = spawnPos;
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData(string behaviorType)
    {
        this.behaviorType = behaviorType;
        spawnPos = new float[2];
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData(float[] spawnPos)
    {
        behaviorType = null;
        this.spawnPos = spawnPos;
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData()
    {
        //should be set to null by default. Is "linearBullet" for testing reasons
        behaviorType = "LinearBullet";
        spawnPos = new float[2];
        posAnchor = null;
        lifetime = 10;
    }
}
