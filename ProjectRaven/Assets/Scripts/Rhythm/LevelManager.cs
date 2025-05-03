using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;


public class LevelManager : MonoBehaviour
{   
    #region Boss Variables
    //_________________________________________________________________________________

    // Whether or not a boss level is active
    public bool bossActive;

    // Boss's current HP amount, if one is active
    public float currentBossHP;

    // Boss's maximun HP amount, if one is active
    public float maxBossHP;
    
    //_________________________________________________________________________________
    #endregion



    #region Boss Level Management Variables
    //_________________________________________________________________________________

    [Header("Level Data")]

    // Used to find the current level from levels folder
    [SerializeField] string currentBossLevelName;
    
    // Currently used boss level
    public static BossData currentBossData;

    // List of spawnable items per beat
    public static List<BeatData> currentSpawnableList;

    // BPM of the phase that is currently ongoing. Can be kept private, but uses the SerializeField access modifier for easier debugging
    public static float currentBPM;

    // PersistentDataPath + /Data/Levels/
    public static string bossLevelDirectoryPath;

    //_________________________________________________________________________________
    #endregion



    #region Bullet Management Variables
    //_________________________________________________________________________________

    [Header("Bullet Type List")]
    
    // List of all names of different attack types. If attack type is not in this list, it cannot be used from BeatData files.
    public List<string> bulletTypeList;

    // Dictionary for connecting the name of the attack as stated above, to the prefab saved in the resources/prefabs folder
    private Dictionary<string,GameObject> bulletDictionary = new();

    // Tuple for containing both the active (existing in the scene) bullet GameObject, as well as it's properties as BeatData
    // These are then used when spawning the enemies
    public static Tuple<List<GameObject>,List<BeatData>> activeBulletList = new(new(),new());

    //_________________________________________________________________________________
    #endregion



    #region Beat Management Variables
    //_________________________________________________________________________________

    [Header("Data")]
    [Tooltip("Boolean read by other scripts for checking whether the music is currently on beat and an action can be done.")]
    public static bool OnBeat;

    [Header("Current items")]
    public static int currentBeat;
    public static PhaseData currentPhaseData; 
    //_________________________________________________________________________________
    #endregion



    #region Sound Management Variables
    //_________________________________________________________________________________

    [Header("Sound")]
    [SerializeField] AudioSource MusicPlayer;

    [Header("Music Attributes")]
    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float timerOffsetLimit;

    private float lastBeat;
    private float timeBetweenBeats;
    private float currentFrameMusicTime;

    //_________________________________________________________________________________
    #endregion
    


    #region Player Action Management Variables
    //_________________________________________________________________________________
    
    public static int cooldownUntilNextAction;

    //_________________________________________________________________________________
    #endregion



    #region Initialization
    //_________________________________________________________________________________
    void Start()
    {
        // Initializing levelDirectoryPath to be /Data/Levels/
        bossLevelDirectoryPath = Path.Combine(Application.persistentDataPath,"Data","Levels");
        if(!Directory.Exists(bossLevelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(bossLevelDirectoryPath);
        }

        // For initializing bulletDictionary
        foreach(string str in bulletTypeList)
        {
            bulletDictionary.Add(str,Resources.Load<GameObject>(Path.Combine("Prefabs",str)));
        }

        // Adding audiosource to editor with the object
        if(gameObject.GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        MusicPlayer = gameObject.GetComponent<AudioSource>();
    }
    //_________________________________________________________________________________
    #endregion



    #region On Frame Update
    //_________________________________________________________________________________
    void Update()
    {
        if(bossActive)
        {
            BossLevelActions();
        } else {
            OtherLevelActions();
        }
    }
    //_________________________________________________________________________________    
    #endregion



    #region Boss Level Management
    //_________________________________________________________________________________
    public void StartBoss()
    {
        currentBossData = BossDataHandler.LoadBossLevel(currentBossLevelName);
        currentPhaseData = currentBossData.phaseDataList.Item1[0];
        MusicPlayer.clip = currentBossData.phaseDataList.Item2[0];
        
        bossActive = true;
        OnPhaseReset();
        StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit)); 
    }
    //_________________________________________________________________________________

    public void StopBoss()
    {
        currentBossData = null;
        currentPhaseData = null;
        MusicPlayer.clip = null; // set this to current level ost

        bossActive = false;
        StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit)); 
    }



    //_________________________________________________________________________________
    private void BossLevelActions()
    {
        // Sets the currentFrameMusicTime to be the music time from current clip
        currentFrameMusicTime = MusicPlayer.time;

        // Checks if enough time has gone from last beat for the beat change to occur
        // Additionally, makes sure the music is not at it's end
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

        // Checks if enough time has gone from last beat for the beat change to occur
        // This is for executing the correct actions if the music is at its last beat and should be looped
        else if(currentFrameMusicTime >= lastBeat + timeBetweenBeats && currentBeat == currentPhaseData.beatMatrix.Count)
        {
            if(cooldownUntilNextAction != 0)
            {
                cooldownUntilNextAction--;
            }
            
            OnPhaseReset();
            
            StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit));
        }
    }

        #region PhaseManagement
        //_________________________________________________________________________________
        private void StartConditionTypeActions()
        {
            switch(currentPhaseData.PhaseChangeConditionType)
            {
                case "AREA":
                    //DoStuff WIP
                    break;
                case "TIMER": 
                    //DoStuff WIP
                    break;
                case "BOSSHPPERCENTAGE":
                    //DoStuff WIP
                    break;
                case "BOSSHPPHASEPERCENTAGE":
                    StartCoroutine(BossHPPhasePercentage_PhaseChanger());
                    break;
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        IEnumerator BossHPPhasePercentage_PhaseChanger()
        {
            float currentPhaseIndex = currentBossData.phaseDataList.Item1.IndexOf(currentPhaseData);
            int currentLevelPhaseAmount = currentBossData.phaseDataList.Item1.Count;
            yield return new WaitUntil(() => currentBossHP <= maxBossHP-(currentPhaseIndex+1)*(maxBossHP/currentLevelPhaseAmount));
            ChangePhase();
        }
        //_________________________________________________________________________________    



        //_________________________________________________________________________________
        public void ChangePhase()
        {
            // Gets the index of the next item in the list
            var next = currentBossData.phaseDataList.Item1.IndexOf(currentPhaseData) + 1;
            Debug.Log("test");

            if (next < currentBossData.phaseDataList.Item1.Count) {
                // Sets the currentPhaseData to be the next level
                currentPhaseData = currentBossData.phaseDataList.Item1[next];
                MusicPlayer.clip = currentBossData.phaseDataList.Item2[next];
                OnPhaseReset();
            } 
            else {
                // Boss death, level victory
                Debug.Log("Victory!!");
            } 
        }
        //_________________________________________________________________________________
        #endregion
    //_________________________________________________________________________________
    #endregion



    #region Other Level Management
    //_________________________________________________________________________________
    private void OtherLevelActions()
    {
        // Sets the currentFrameMusicTime to be the music time from current clip
        currentFrameMusicTime = MusicPlayer.time;

        // Checks if enough time has gone from last beat for the beat change to occur
        // Additionally, makes sure the music is not at it's end
        if(currentFrameMusicTime >= lastBeat + timeBetweenBeats)
        {
            lastBeat += timeBetweenBeats;
            if(cooldownUntilNextAction != 0)
            {
                cooldownUntilNextAction--;
            }

            StartCoroutine(BeatEnumerator(currentBPM, timerOffsetLimit));
        } 
    }
    //_________________________________________________________________________________
    #endregion



    #region BeatManagement
    //_________________________________________________________________________________
    private IEnumerator BeatEnumerator(float LocalBPM, float offsetTime)
    {
        yield return new WaitForSeconds(offsetTime/2);
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-offsetTime/2);
        OnBeat = true;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void OnBeatChange()
    {
        currentSpawnableList = currentPhaseData.beatMatrix[currentBeat];   
        SpawnEnemies();
        currentBeat++;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void OnPhaseReset()
    {
        lastBeat = 0;
        currentBeat = 0;
        currentBPM = currentPhaseData.BPM;
        timeBetweenBeats = 60f/currentBPM;
        MusicPlayer.time = (float)GetCorrectAudioStartTime();
        MusicPlayer.Stop();
        MusicPlayer.Play();
        currentSpawnableList = currentPhaseData.beatMatrix[currentBeat];

        StartConditionTypeActions();
        SpawnEnemies();
        currentBeat++;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    private double GetCorrectAudioStartTime()
    {
        double correctAudioStartTime = MusicPlayer.clip.length - Math.Round(MusicPlayer.clip.length);
        return correctAudioStartTime;
    }
    //_________________________________________________________________________________
    #endregion


    #region SpawnManagement
    //_________________________________________________________________________________
    private void SpawnEnemies()
    {
        foreach (BeatData beat in currentSpawnableList)
        {
            //Spawn
            if(beat.behaviorType != "none")
            {
                GameObject instantiatedObject = Instantiate(bulletDictionary[beat.behaviorType], new Vector3(beat.attackPos[0], 0, beat.attackPos[1]), Quaternion.identity);
                activeBulletList.Item1.Add(instantiatedObject);
                activeBulletList.Item2.Add(beat);
            }
        }
    }
    //_________________________________________________________________________________
    #endregion
}