using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;



public class PlayerManager : MonoBehaviour
{   
    #region SpawnManagementVariables

    [Header("Level Data")]
    [SerializeField] string currentLevelName;
    [SerializeField] LevelData currentLevelData;

    [Header("Enemy type list")]
    [Tooltip("Gameobject array in which all attack, bullet and enemy types should be in. These can be spawned into a level on will.")]
    [SerializeField] List<GameObject> enemyTypeList;
    [SerializeField] Tuple<List<GameObject>,List<BeatData>> ActiveEnemyList = new(new(),new());

    [Header("Current items")]
    public int currentBeat;
    public PhaseData currentPhaseData; 

    // Private
    private List<BeatData> currentSpawnableList;
    private string levelDirectoryPath;
    private readonly bool? defeatOrVictory = null;
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
    private float BPM;
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
            //Debug.Log("current phase name is "+currentPhaseData.phaseName);
            currentPhaseMusicPlayer.clip = currentLevelData.phaseDataList.Item2[0];
            StartCoroutine(PhaseChanger());
        } catch (Exception e) {
            Debug.LogWarning("Level could not be loaded! Error at "+e);
            Debug.LogWarning("This oftentimes occurs when there are <1 phases in the level.");
            Debug.LogWarning("Check level files and try again.");
        }

        // For Beat Management
        try {
            BPM = currentPhaseData.BPM;
            LevelActive = true;
            lastBeat = 0;
            timeBetweenBeats = 60f/BPM;

            currentPhaseMusicPlayer.Play();

            OnBeatReset();
            StartCoroutine(BeatEnumerator(BPM, timerOffsetLimit));
        } catch (Exception) {throw;}

        
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



    //_________________________________________________________________________________
    public void ChangePhase()
    {
        // Gets the index of the next item in the list
        var next = currentLevelData.phaseDataList.Item1.IndexOf(currentPhaseData) + 1;
        
        // Gets the next item in the list, tests if it exists in order to determine if boss has more phases or not
        var nextPhase = currentLevelData.phaseDataList.Item1[next];
        var nextSong = currentLevelData.phaseDataList.Item2[next];

        if (nextPhase != null) {
            // Sets the currentPhaseData to be the next level
            currentPhaseData = nextPhase;
            currentPhaseMusicPlayer.clip = nextSong;
        } 
        else {
            // Boss death, level victory
        } 
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
                    SFXPlayer.Play();
                    cooldownUntilNextAction--;
                }
                OnBeatChange();
                StartCoroutine(BeatEnumerator(BPM, timerOffsetLimit));
            } 
            
            if(currentFrameMusicTime >= lastBeat + timeBetweenBeats && currentBeat == currentPhaseData.beatMatrix.Count)
            {
                lastBeat = 0;
                if(cooldownUntilNextAction != 0)
                {
                    SFXPlayer.Play();
                    cooldownUntilNextAction--;
                }
                OnBeatReset();
                StartCoroutine(BeatEnumerator(BPM, timerOffsetLimit));
            }
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
        currentPhaseMusicPlayer.Stop();
        currentPhaseMusicPlayer.Play();
        currentPhaseMusicPlayer.time = (float)GetCorrectAudioStartTime();
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
            GameObject instantiatedObject = Instantiate(enemyTypeList[beat.behaviorType], new Vector3(beat.attackPos[0], 0, beat.attackPos[1]), Quaternion.identity);
            ActiveEnemyList.Item1.Add(instantiatedObject);
            ActiveEnemyList.Item2.Add(beat);
        }
    }
    //_________________________________________________________________________________
    #endregion
}


