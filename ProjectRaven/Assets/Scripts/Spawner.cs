using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public LevelDataHandler levelData;
    public BeatManager beatManager;
    public List<Enemy> enemyTypeList = new();

    public int beatCount;

    public void Awake()
    {
        beatCount = 0;
    }
    
    Enemy enemyToSpawn;

    public void SpawnEnemies()
    {
        if(beatCount >= levelData.trackBeatAmount) return;

        List<BeatData> beatDataArr = levelData.beatDataArr[beatCount];

        for(int i = 0; i < beatDataArr.Count; i++)
        {
            if(beatDataArr[i].attackType != 0)
            {
                enemyToSpawn = GetEnemyType(beatDataArr[i].attackType);

                Vector3 instantiatePos = new(beatDataArr[i].attackPos.x, transform.position.y, beatDataArr[i].attackPos.y); 
                Instantiate(enemyToSpawn.enemyPrefab, instantiatePos, Quaternion.identity);
            }
        }
        beatCount++;
    }

    public Enemy GetEnemyType(int enemyTypeIndex)
    {
        return enemyTypeList[enemyTypeIndex];
    }
}
