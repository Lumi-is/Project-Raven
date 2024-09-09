using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public LevelDataHandler dataHandler;
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

    public void SpawnEditorEnemies()
    {
        
    }
    public Enemy GetAttackType(int attackIndex)
    {
        return enemyTypeList[attackIndex];
    }
}
