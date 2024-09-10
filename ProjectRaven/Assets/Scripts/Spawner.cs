using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public FileDataHandler dataHandler;
    public BeatManager beatManager;
    public int beatCount;
    public List<GameObject> attackerList; 

    public void SpawnEnemies(string levelName)
    {
        string fullFilePath = dataHandler.defaultFilePath+levelName;
        var levelData = dataHandler.LoadFile<LevelData>(fullFilePath);
        //Spawn enemies TO BE CONTINUED

        
    }

    public void SpawnEditorEnemies()
    {
        
    }
}
