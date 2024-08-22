using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public LevelData levelData;
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

        List<int> enemyTypeArrY = levelData.enemyTypeArr[beatCount];
        List<Vector2> attackPosArrY = levelData.attackPosArr[beatCount];

        for(int i = 0; i < enemyTypeArrY.Count; i++)
        {
            if(enemyTypeArrY[i] != 0)
            {
                enemyToSpawn = GetEnemyType(enemyTypeArrY[i]);

                Vector3 instantiatePos = new(attackPosArrY[i].x, transform.position.y, attackPosArrY[i].y); 
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
