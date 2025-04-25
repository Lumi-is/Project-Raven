public class BeatData
{
    //For spawner script to determine the type of attack to spawn
    public int behaviorType;

    //If null, means position is determined in behavior type script. Otherwise spawns object in position [attackPos]
    public float[] attackPos;
}
