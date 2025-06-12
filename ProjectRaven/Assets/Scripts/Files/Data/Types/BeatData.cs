using System;

public class BeatData 
{
    // For spawner script to determine the type of attack to spawn
    public string[] behaviorTypeList;

    // If null, means position is determined in behavior type script. Otherwise spawns object in position [attackPos]
    public float[] spawnPos;

    // If null, position determined purely by coordinates. Accepted values: PLAYER, ROOM, 
    // If PLAYER - Set position base to Player's location
    // If ROOM - Set position base to Current Room Location
    public string posAnchor = null;

    // Whether beat should inject it's behavior type into boss AI or exist as a seperate entity
    // If true, injects the component to Boss GameObject. NOTE: If BossObject has not yet been assigned, takes the behaviorType of current beat and instantiates it as a base for future injections
    // If false, proceeds as normal
    public bool isAIInjectable;

    // Whether or not to use Randomizer in determining spawn location - Amounts determine the variance of positions calculated in Randomizer
    public bool isRandomized;
    public float randomizerAmountX;
    public float randomizerAmountZ;

    // For how many beats the attack is alive
    public float lifetime;

    public BeatData(string[] behaviorType, float[] spawnPos, string posAnchor, float lifetime)
    {
        this.behaviorTypeList = behaviorType;
        this.spawnPos = spawnPos;
        this.posAnchor = posAnchor;
        this.lifetime = lifetime;
    }

    public BeatData(string[] behaviorType, float[] spawnPos, string posAnchor)
    {
        this.behaviorTypeList = behaviorType;
        this.spawnPos = spawnPos;
        this.posAnchor = posAnchor;
        lifetime = 10;
    }

    public BeatData(string[] behaviorType, float[] spawnPos)
    {
        this.behaviorTypeList = behaviorType;
        this.spawnPos = spawnPos;
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData(string[] behaviorType)
    {
        this.behaviorTypeList = behaviorType;
        spawnPos = new float[2];
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData(float[] spawnPos)
    {
        behaviorTypeList = null;
        this.spawnPos = spawnPos;
        posAnchor = null;
        lifetime = 10;
    }

    public BeatData()
    {
        //should be set to null by default. Is "linearBullet" for testing reasons
        behaviorTypeList = new string[] {"LinearBullet"};
        spawnPos = new float[2];
        posAnchor = null;
        lifetime = 10;
    }
}
