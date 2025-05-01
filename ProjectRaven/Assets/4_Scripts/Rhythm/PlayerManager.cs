using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;
using UnityEditor.Rendering;



public class PlayerManager : MonoBehaviour
{   
    #region SpawnManagementVariables

    [Header("Level Data")]
    [SerializeField] string currentLevelName;
    [SerializeField] LevelData currentLevelData;
    [SerializeField] float currentBPM;

    [Header("Bullet type list")]
    [Tooltip("Gameobject array in which all attack, bullet and Bullet types should be in. These can be spawned into a level on will.")]
    [SerializeField] List<GameObject> bulletTypeList;
    [SerializeField] Tuple<List<GameObject>,List<BeatData>> activeBulletList = new(new(),new());

    [Header("Boss HP")]
    [SerializeField] float currentBossHP;
    [SerializeField] float maxBossHP;
    
    [Header("Current items")]
    public int currentBeat;
    public PhaseData currentPhaseData;    


    // Private
    private List<BeatData> currentSpawnableList;
    private string levelDirectoryPath;
    #endregion



    #region BeatManagementVariables

    [Header("Sound")]
    [SerializeField] AudioSource currentPhaseMusicPlayer;
    [SerializeField] AudioSource SFXPlayer;
    
    [Header("Attributes")]
    [Tooltip("Whether or not player has done any action on the current beat.")]
    public int cooldownUntilNextAction;

    [Header("Music Attributes")]
    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float timerOffsetLimit;

    [Header("Data")]
    [Tooltip("Boolean read by other scripts for checking whether the music is currently on beat and an action can be done.")]
    public bool OnBeat;

    //Private
    private float lastBeat;
    private float timeBetweenBeats;
    private float currentFrameMusicTime;
    private bool LevelActive;

    #endregion



    #region Initialization
    //_________________________________________________________________________________
    void Start()
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
    public void StartLevel()
    {
        // For Spawn Management
        try {
            currentLevelData = LevelDataHandler.LoadLevel(currentLevelName);
            //Debug.Log("current level name is"+currentLevelName);
            currentPhaseData = currentLevelData.phaseDataList.Item1[0];
            Debug.Log("Count "+currentLevelData.phaseDataList.Item1.Count);
            currentPhaseMusicPlayer.clip = currentLevelData.phaseDataList.Item2[0];
        } catch (Exception e) {
            Debug.LogWarning("Level could not be loaded! Error at "+e);
            Debug.LogWarning("This usually means there are zero phases in the level, and thus it cannot be loaded.");
            Debug.LogWarning("Check level files and try again.");
        }

        // For Beat Management
        try {
            LevelActive = true;
            OnBeatReset();
            StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit));
        } catch (Exception) {throw;}  
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    void Update()
    {
        if(LevelActive)
        {
            // Sets the lastFrameMusicTime to be that of currentFrameMusicTime
            // This works because currentFrameMusicTime is in this case set after the lastFrameMusicTime
            // This means the lastFrameMusicTime always gets updated one frame late
            currentFrameMusicTime = currentPhaseMusicPlayer.time;

            // Checks if enough time has gone from last beat for the beat change to occur
            // Additionally, makes sure the music has not looped
            if(currentFrameMusicTime >= lastBeat + timeBetweenBeats && currentBeat < currentPhaseData.beatMatrix.Count)
            {
                lastBeat += timeBetweenBeats;
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }
                OnBeatChange();
                StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit));
            } 
            
            if(currentFrameMusicTime >= lastBeat + timeBetweenBeats && currentBeat == currentPhaseData.beatMatrix.Count)
            {
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }
                OnBeatReset();
                StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit));
            }
        }
    }
    //_________________________________________________________________________________
    #endregion



    #region PhaseManagement
    //_________________________________________________________________________________
    private void PhaseChanger()
    {
        switch(currentPhaseData.PhaseChangeConditionType)
        {
            case "Timer": 
                //DoStuff WIP
                break;
            case "BossHPPercentage":
                //DoStuff WIP
                break;
            case "BossHPPhasePercentage":

                float currentPhaseIndex = currentLevelData.phaseDataList.Item1.IndexOf(currentPhaseData);
                int currentLevelPhaseAmount = currentLevelData.phaseDataList.Item1.Count;

                if(currentBossHP <= maxBossHP-(currentPhaseIndex+1)*(maxBossHP/currentLevelPhaseAmount))
                {
                    ChangePhase();
                }
                break;
        }
    }
    //_________________________________________________________________________________
    



    //_________________________________________________________________________________
    public void ChangePhase()
    {
        // Gets the index of the next item in the list
        var next = currentLevelData.phaseDataList.Item1.IndexOf(currentPhaseData) + 1;
        Debug.Log("test");

        if (currentLevelData.phaseDataList.Item1[next] != null) {
            // Sets the currentPhaseData to be the next level
            currentPhaseData = currentLevelData.phaseDataList.Item1[next];
            currentPhaseMusicPlayer.clip = currentLevelData.phaseDataList.Item2[next];
            OnBeatReset();
        } 
        else {
            // Boss death, level victory
            Debug.Log("Victory!!");
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
        PhaseChanger();
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void OnBeatReset()
    {
        lastBeat = 0;
        currentBeat = 0;
        currentBPM = currentPhaseData.BPM;
        timeBetweenBeats = 60f/currentBPM;
        currentPhaseMusicPlayer.time = (float)GetCorrectAudioStartTime();
        currentPhaseMusicPlayer.Stop();
        currentPhaseMusicPlayer.Play();
        currentSpawnableList = currentPhaseData.beatMatrix[currentBeat]; 
        SpawnEnemies();
        currentBeat++;
    }
    //_________________________________________________________________________________


    private double GetCorrectAudioStartTime()
    {
        double correctAudioStartTime = currentPhaseMusicPlayer.clip.length - Math.Round(currentPhaseMusicPlayer.clip.length);
        return correctAudioStartTime;
    }



    //_________________________________________________________________________________
    private IEnumerator BeatEnumerator(float LocalBPM, float offsetTime)
    {
        yield return new WaitForSeconds(offsetTime/2);
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-offsetTime/2);
        OnBeat = true;
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
            GameObject instantiatedObject = Instantiate(bulletTypeList[beat.behaviorType], new Vector3(beat.attackPos[0], 0, beat.attackPos[1]), Quaternion.identity);
            activeBulletList.Item1.Add(instantiatedObject);
            activeBulletList.Item2.Add(beat);
        }
    }
    //_________________________________________________________________________________
    #endregion
}


