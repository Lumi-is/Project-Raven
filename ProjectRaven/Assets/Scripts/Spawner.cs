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

    public void SpawnEnemies()
    {
        
    }

    public void SpawnEditorEnemies(int indexTag)
    {
        
    }
    public Enemy GetEnemyType(int enemyTypeIndex)
    {
        return enemyTypeList[enemyTypeIndex];
    }
}
