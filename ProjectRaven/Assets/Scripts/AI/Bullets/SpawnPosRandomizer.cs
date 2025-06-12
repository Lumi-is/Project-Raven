using UnityEngine;

public class SpawnPosRandomizer 
{
    public static Vector3 GetRandomizedPosition(Vector3 self, float x, float z)
    {
        return self + new Vector3(Random.Range(0-x, x), 0, Random.Range(0-z, z));
    }
    public static Vector3 GetRandomizedPosition(Vector3 self, float x, float z, float x2, float z2)
    {
        return self + new Vector3(Random.Range(x, x2), 0, Random.Range(z, z2));
    }
}
