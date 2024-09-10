using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Data")]

    [Tooltip("Which beat is currently being played.")]
    public int currentBeat;

    [Tooltip("List of all gameobjects that attack upon spawning.")]
    public List<GameObject> attackerList;

    [Header("Singleton Scripts")]
    public FileDataHandler dataHandler;
    public BeatManager beatManager;

    //Private
    private LevelData levelData;

    void Awake()
    {
        levelData = beatManager.levelData;
    }

    public void SpawnEnemies()
    {
        //Spawn enemies TO BE CONTINUED

        List<List<BeatData>> beatList = levelData.beatList;

        for(int i = 0; i < beatList[currentBeat].Count; i++)
        {
            BeatData beatNode = beatList[currentBeat][i];
            int enemyTypeToSpawn = beatNode.attackType;

            Instantiate(attackerList[enemyTypeToSpawn], new Vector3(beatNode.attackPos[0], 1, beatNode.attackPos[1]),Quaternion.identity);
        }
    }
}
