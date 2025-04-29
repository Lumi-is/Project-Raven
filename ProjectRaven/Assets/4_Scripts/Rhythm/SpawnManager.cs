using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;
using Unity.Mathematics;

public class SpawnManager : MonoBehaviour
{   
    [Header("Level Data")]
    [SerializeField] string currentLevelName;
    [SerializeField] LevelData currentLevelData;

    [Header("Enemy type list")]
    [Tooltip("Gameobject array in which all attack, bullet and enemy types should be in. These can be spawned into a level on will.")]
    [SerializeField] List<GameObject> enemyTypeList;
    public int currentBeat;


    public PhaseData currentPhaseData; 
    private List<BeatData> currentSpawnableList;
    private string levelDirectoryPath;
    private readonly bool? defeatOrVictory = null;



    #region OnStart
    //_________________________________________________________________________________
    void Awake()
    {
        levelDirectoryPath = Path.Combine(Application.persistentDataPath,"Data","Levels");
        if(!Directory.Exists(levelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(levelDirectoryPath);
        }
    }
    //_________________________________________________________________________________
    #endregion



    #region LevelManagement
    //_________________________________________________________________________________
    public void ChangePhase()
    {
        // Gets the index of the next item in the list
        var next = currentLevelData.phaseList.IndexOf(currentPhaseData) + 1;
        
        // Gets the next item in the list, tests if it exists in order to determine if boss has more phases or not
        var nextItem = currentLevelData.phaseList[next];

        if (nextItem != null) {
            // Sets the currentPhaseData to be the next level
            currentPhaseData = currentLevelData.phaseList[next];
        } else {
            // Boss death, level victory
        }
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void StartLevel()
    {
        try{
            currentLevelData = LevelDataHandler.LoadLevel(currentLevelName);
            //Debug.Log("current level name is"+currentLevelName);
            currentPhaseData = currentLevelData.phaseList[0];
            //Debug.Log("current phase name is "+currentPhaseData.phaseName);
            StartCoroutine(PhaseChanger());
        } catch (Exception e)
        {
            Debug.LogWarning("Level could not be loaded! Error at "+e);
            Debug.LogWarning("This oftentimes occurs when there are <1 phases in the level.");
            Debug.LogWarning("Check level files and try again.");
        }
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    private IEnumerator PhaseChanger(){
        while(defeatOrVictory == null)
        {
            yield return new WaitUntil(() => currentPhaseData.phaseChangeConditionMet);
            ChangePhase();
        }
    }
    //_________________________________________________________________________________
    #endregion



    #region BeatManagement
    //_________________________________________________________________________________
    public void OnBeatChange()
    {
        currentSpawnableList = currentPhaseData.beatMatrix[currentBeat];   
        SpawnEnemies(); 
        currentBeat++;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void OnBeatReset()
    {
        currentBeat = 0;
        currentSpawnableList = currentPhaseData.beatMatrix[currentBeat]; 
    }
    //_________________________________________________________________________________
    #endregion



    #region SpawnManagement
    //_________________________________________________________________________________
    private void SpawnEnemies()
    {
        Debug.Log("Beat was summoned!");
        foreach (BeatData beat in currentSpawnableList)
        {
            //Spawn
            GameObject instantiatedObject = Instantiate(enemyTypeList[beat.behaviorType], new Vector3(beat.attackPos[0], 0, beat.attackPos[1]), Quaternion.identity);
        }
    }
    //_________________________________________________________________________________
    #endregion
}
