using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;


public class LevelEventManager : MonoBehaviour
{   

    // Variables //



    #region Level Management Variables
    //_________________________________________________________________________________

    // Whether or not a boss level is active
    [Header("Level Attributes")]

    [Tooltip("Accepted values: NONE, AREA, BOSS, ALL")]
    [SerializeField] string activeLevelType = "NONE"; //Accepted values: NONE, AREA, BOSS, ALL
    public bool bossOverridesAreaLevel;

    [Header("Level Data")]

    public bool LevelActive;

    // BPM of the phase that is currently ongoing. Can be kept private, but uses the SerializeField access modifier for easier debugging
    public static float currentBPM;

    // PersistentDataPath + /Data/Bosses/
    public static string bossLevelDirectoryPath;

    // PersistentDataPath + /Data/Areas/
    public static string areaLevelDirectoryPath;


    //_________________________________________________________________________________
    #endregion



    #region Boss Variables
    //_________________________________________________________________________________
    
    // Level Specifics // 
    // Used to find the current level from levels folder
    [Header("Boss Level")]
    [SerializeField] string currentBossLevelName;

    // Currently used boss level
    public static BossData currentBossData;

    // Currently used phase of the boss
    public static List<BossPhaseData> currentPhaseDataList; 

    public static BossPhaseData currentPhaseData;

    private IEnumerator phaseChangerType;

    // List of spawnable items per beat
    public static List<BeatData> currentBossSpawnableList;

    // Entity Specifics //

    // Boss's current HP amount, if one is active
    public float currentBossHP;

    // Boss's maximun HP amount, if one is active
    public float maxBossHP;
    
    //_________________________________________________________________________________
    #endregion



    #region Area Management Variables
    //_________________________________________________________________________________

    // Level // 
    // Used to find the current level from levels folder
    [Header("Area Level")]
    [SerializeField] string currentAreaLevelName;
    [SerializeField] AreaLevelData currentAreaLevelData;

    [SerializeField] AreaData currentAreaData;
    
    [SerializeField] RoomData currentRoomData;

    public static List<BeatData> currentAreaSpawnableList;


    //For room spawning 

    [Header("Room Type List")]
    
    public List<string> roomTypeList;
    private readonly Dictionary<string,GameObject> roomDictionary = new();
    //_________________________________________________________________________________
    #endregion



    #region Bullet Management Variables
    //_________________________________________________________________________________

    [Header("Bullet Type List")]
    
    // List of all names of different attack types. If attack type is not in this list, it cannot be used from BeatData files.
    public List<string> bulletTypeList;

    // Dictionary for connecting the name of the attack as stated above, to the prefab saved in the resources/prefabs folder
    private readonly Dictionary<string,GameObject> bulletDictionary = new();

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
    public int currentBeat;
    //_________________________________________________________________________________
    #endregion



    #region Sound Management Variables
    //_________________________________________________________________________________

    [Header("Sound")]
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] bool guessMusicOffset = true;
    [Range(0,10)]
    [SerializeField] double musicOffset;

    [Header("Music Attributes")]
    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float timerOffsetLimit;

    private float lastBeat;
    private float timeBetweenBeats;
    private float currentFrameTime;

    //_________________________________________________________________________________
    #endregion
    


    #region Player Action Management Variables
    //_________________________________________________________________________________
    
    public static int cooldownUntilNextAction;

    //_________________________________________________________________________________
    #endregion

    #region Debugging
    [Header("Debugging")]
    [SerializeField] bool DebugLevelCreation;
    #endregion


    // Class & Methods // 



    #region Initialization
    //_________________________________________________________________________________
    void Start()
    {
        // Initializing levelDirectoryPath to be /Data/Bosses/ for boss levels
        bossLevelDirectoryPath = Path.Combine(Application.persistentDataPath,"Data","Bosses"); 
        if(!Directory.Exists(bossLevelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(bossLevelDirectoryPath);
        }

        // Initializing levelDirectoryPath to be /Data/Areas/ for area levels
        areaLevelDirectoryPath = Path.Combine(Application.persistentDataPath,"Data","Areas"); 
        if(!Directory.Exists(areaLevelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(areaLevelDirectoryPath);
        }

        // For initializing bulletDictionary
        foreach(string str in bulletTypeList)
        {
            bulletDictionary.Add(str,Resources.Load<GameObject>(Path.Combine("Prefabs","Bullets",str)));
        }

        foreach(string str in roomTypeList)
        {
            roomDictionary.Add(str,Resources.Load<GameObject>(Path.Combine("Prefabs","Rooms",str)));
        }

        // Adding audiosource to editor with the object
        if(gameObject.GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        musicPlayer = gameObject.GetComponent<AudioSource>();

        //DEBUGGING ONLY
        if(DebugLevelCreation)
        {
            gameObject.GetComponent<LevelEditor>().CreateDebugLevels();
        }

        // For setting all items
        InitializeLevel();

        // For spawning in rooms and items
        SpawnRooms();
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void InitializeLevel()
    {
        currentAreaLevelData = AreaLevelHandler.LoadAreaLevel(currentAreaLevelName);
        currentAreaData = currentAreaLevelData.areaDataList.Item1[0];
        currentRoomData = currentAreaData.roomList[0];
        currentBossData = BossDataHandler.LoadBossLevel(currentBossLevelName);
        currentPhaseDataList = currentBossData.phaseDataList.Item1[0];
        currentPhaseData = currentPhaseDataList[UnityEngine.Random.Range(0,currentPhaseDataList.Count)];
    }
    //_________________________________________________________________________________

    //_________________________________________________________________________________
    public void SpawnRooms()
    {
        // Spawn in rooms and items
        foreach(RoomData room in currentAreaData.roomList)
        {
            if(room.roomType != "none")
            {
                GameObject newRoom = Instantiate(roomDictionary[room.roomType],new Vector3(room.roomPos[0],room.roomPos[1],room.roomPos[2]), new Quaternion(0,room.roomRotation,0,0));
                newRoom.name = room.roomName;
            }
        }
    }
    //_________________________________________________________________________________
    #endregion



    #region Level Management
    //_________________________________________________________________________________
    public void LoadLevel()
    {
        if(activeLevelType == "BOSS" || activeLevelType == "ALL")
        {
            musicPlayer.clip = currentBossData.phaseDataList.Item2[0];
        } else if (activeLevelType == "AREA")
        {
            musicPlayer.clip = currentBossData.phaseDataList.Item2[0];
        }

        if(phaseChangerType != null)
        {
            StopCoroutine(phaseChangerType);
        }

        if(activeLevelType == "BOSS" || activeLevelType == "ALL")
        {
            BossConditionTypeActions();
        }
          
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void StartLevel()
    {
        if(activeLevelType == "NONE")
        {
            Debug.Log("Active level type is NONE. Please select a level type and try again.");
            return;
        }
        InitializeLevel();
        LoadLevel();
        ResetBeat();
        LevelActive = true;
    }

    public void StopLevel()
    {
        LevelActive = false;
        ResetBeat();
    }    
    public void SetAllActive()
    {
        activeLevelType = "ALL";
    }
    public void SetBossActive()
    {
        activeLevelType = "BOSS";
    }
    public void SetAreaActive()
    {
        activeLevelType = "AREA";
    }

    public void SetNoneActive()
    {
        activeLevelType = "NONE";
    }
    //_________________________________________________________________________________

        #region On Frame Update
        //_________________________________________________________________________________
        // Every frame
        void Update()
        {
            if(!LevelActive) return;

            // Sets the currentFrameMusicTime to be the music time from current clip
            currentFrameTime = musicPlayer.time;
            
            if(currentBossData != null && (activeLevelType == "BOSS" || activeLevelType == "ALL")) DoBossActions();
            
            if(currentAreaData != null && (activeLevelType == "AREA" || activeLevelType == "ALL")) DoAreaActions();
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        private void DoBossActions()
        {
            // Checks if enough time has gone from last beat for the beat change to occur
            // Additionally, makes sure the music is not at it's end
            if(currentFrameTime > lastBeat + timeBetweenBeats && currentBeat < currentPhaseData.beatMatrix.Count)
            {
                lastBeat += timeBetweenBeats;
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }
                
                Debug.Log("test");
                OnBeatChange();

                StartCoroutine(OnBeatCheck());
            } 

            // Checks if enough time has gone from last beat for the beat change to occur
            // This is for executing the correct actions if the music is at its last beat and should be looped
            else if(currentBeat == currentPhaseData.beatMatrix.Count)
            {
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }
                
                ResetBeat();
                
                StartCoroutine(OnBeatCheck());
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        private void DoAreaActions()
        {
            // Checks if enough time has gone from last beat for the beat change to occur
            // Additionally, makes sure the music is not at it's end
            if(currentFrameTime >= lastBeat + timeBetweenBeats && currentBeat < currentRoomData.beatMatrix.Count)
            {
                lastBeat += timeBetweenBeats;
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }

                OnBeatChange();

                StartCoroutine(OnBeatCheck());
            } 

            // Checks if enough time has gone from last beat for the beat change to occur
            // This is for executing the correct actions if the music is at its last beat and should be looped
            else if(currentFrameTime >= lastBeat + timeBetweenBeats && currentBeat == currentRoomData.beatMatrix.Count)
            {
                if(cooldownUntilNextAction != 0)
                {
                    cooldownUntilNextAction--;
                }
                
                ResetBeat();
                
                StartCoroutine(OnBeatCheck());
            }
        }
        //_________________________________________________________________________________    
        #endregion

    

        #region Phase Management
        
        //_________________________________________________________________________________
        private void BossConditionTypeActions()
        {
            switch(currentPhaseData.phaseChangeConditionType)
            {
                case "TIMER": 
                    phaseChangerType = BossTimer_PhaseChanger();
                    Debug.Log("phaseChangerType recognized as TIMER. Starting coroutine...");
                    StartCoroutine(phaseChangerType);
                    break;
                case "BOSSHPPERCENTAGE":
                    phaseChangerType = BossHPPercentage_PhaseChanger();
                    Debug.Log("phaseChangerType recognized as BOSSHPPERCENTAGE. Starting coroutine...");
                    StartCoroutine(phaseChangerType);
                    break;
                case "BOSSHPPHASEPERCENTAGE":
                    phaseChangerType = BossHPPhasePercentage_PhaseChanger();
                    Debug.Log("phaseChangerType recognized as BOSSHPPHASEPERCENTAGE. Starting coroutine...");
                    StartCoroutine(phaseChangerType);
                    break;
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        IEnumerator BossHPPhasePercentage_PhaseChanger()
        {
            float currentPhaseIndex = currentBossData.phaseDataList.Item1.IndexOf(currentPhaseDataList);
            int currentLevelPhaseAmount = currentBossData.phaseDataList.Item1.Count;
            yield return new WaitUntil(() => currentBossHP <= maxBossHP-(currentPhaseIndex+1)*(maxBossHP/currentLevelPhaseAmount));
            ChangePhase();
        }
        //_________________________________________________________________________________

        
        
        //_________________________________________________________________________________
        IEnumerator BossHPPercentage_PhaseChanger()
        {
            yield return new WaitUntil(() => currentBossHP/maxBossHP < currentPhaseData.percentageAmount);
            ChangePhase();
        }
        //_________________________________________________________________________________ 



        //_________________________________________________________________________________
        IEnumerator BossTimer_PhaseChanger()
        {
            Debug.Log("Timer started. "+currentPhaseData.timerAmount+" seconds until the end of current phase.");
            yield return new WaitForSeconds(currentPhaseData.timerAmount);
            ChangePhase();
        }
        //_________________________________________________________________________________    



        //_________________________________________________________________________________
        public void ChangePhase()
        {
            // Gets the index of the next item in the list
            var next = currentBossData.phaseDataList.Item1.IndexOf(currentPhaseDataList) + 1;

            if (next < currentBossData.phaseDataList.Item1.Count) {
                // Sets the currentPhaseData to be the next level
                currentPhaseDataList = currentBossData.phaseDataList.Item1[next];
                currentPhaseData = currentPhaseDataList[UnityEngine.Random.Range(0,currentPhaseDataList.Count)];
                musicPlayer.clip = currentBossData.phaseDataList.Item2[next];
                Debug.Log("Phase was changed: "+currentPhaseDataList);
                ResetBeat();

                if(phaseChangerType != null)
                {
                    StopCoroutine(phaseChangerType);
                }

                if(activeLevelType == "BOSS" || activeLevelType == "ALL")
                {
                    BossConditionTypeActions();
                }
                
            } 
            else {
                // Boss death, level victory
                Debug.Log("Victory!!");
            } 
        }
        //_________________________________________________________________________________
        #endregion

        #region Room Management
        
        //_________________________________________________________________________________
        // To be referred from player to change room when entering a new one 
        public void ChangeRoomTo(string roomName)
        {
            foreach (RoomData room in currentAreaData.roomList)
            {
                if(room.roomName == roomName)
                {
                    currentRoomData = room;
                    break;
                }
            }
        }
        //_________________________________________________________________________________

        #endregion
    //_________________________________________________________________________________
    #endregion



    #region Beat Management
    //_________________________________________________________________________________
    private IEnumerator OnBeatCheck()
    {
        yield return new WaitForSeconds(timerOffsetLimit/2);
        OnBeat = false;

        yield return new WaitForSeconds(timeBetweenBeats-timerOffsetLimit);
        OnBeat = true;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void OnBeatChange()
    {
        if(bossOverridesAreaLevel) {
            currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];  
            currentAreaSpawnableList = new(); 
        } else {
            if(currentPhaseDataList != null)
            {
                currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            }
    
            if(currentAreaData != null)
            {
                currentAreaSpawnableList = currentRoomData.beatMatrix[currentBeat];
            }
        }
        StartCoroutine(OnBeatCheck());
        SpawnEnemies();
        currentBeat++;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void ResetBeat()
    {
        lastBeat = 0;
        currentBeat = 0;

        if(bossOverridesAreaLevel)
        {
            currentBPM = currentPhaseData.BPM;
            currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];  
            currentAreaSpawnableList = new(); 
        } else {
            if(currentPhaseDataList != null)
            {
                currentBPM = currentPhaseData.BPM;
                currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            }
    
            if(currentAreaData != null)
            {
                currentBPM = currentAreaData.BPM;
                currentAreaSpawnableList = currentRoomData.beatMatrix[currentBeat];
            }
        }

        timeBetweenBeats = 60f/currentBPM;
        musicPlayer.Stop();
        musicPlayer.Play();
        musicPlayer.time = (float)CorrectAudioStartTime();    
    }
    //_________________________________________________________________________________
    #endregion



    #region Music Management
    //_________________________________________________________________________________
    private double CorrectAudioStartTime()
    {
        if(guessMusicOffset)
        {
            double correctAudioStartTime = musicPlayer.clip.length - Math.Round(musicPlayer.clip.length);
            return correctAudioStartTime;
        } else {
            return musicOffset;
        }
        
    }
    //_________________________________________________________________________________
    #endregion



    #region SpawnManagement
    //_________________________________________________________________________________
    private void SpawnEnemies()
    {
        // Skips the execution if currentAreaSpawnableList is null or if activeLevelType is not suitable
        if(activeLevelType == "NONE") return;
        if(currentBossSpawnableList == null || activeLevelType == "AREA") goto areaList;

        foreach (BeatData beat in currentBossSpawnableList)
        {
            // Spawn area bullets, enemies and threats
            if(beat.behaviorType == null) return;
            
            Vector3 beatSpawnPos;
            if(beat.posAnchor == null)
            {
                beatSpawnPos = new(beat.spawnPos[0],0,beat.spawnPos[1]);
            } else {
                beatSpawnPos = new(beat.spawnPos[0],0,beat.spawnPos[1]); 
            }

            GameObject instantiatedObject = Instantiate(bulletDictionary[beat.behaviorType], beatSpawnPos, Quaternion.identity);

            if(beat.lifetime != null)
            {
                StartCoroutine(Destroy(instantiatedObject, (float)beat.lifetime*timeBetweenBeats));
            }

            activeBulletList.Item1.Add(instantiatedObject);
            activeBulletList.Item2.Add(beat);
        }

        areaList:;
        // Skips the execution if currentAreaSpawnableList is null or if activeLevelType is not suitable
        if(currentAreaSpawnableList == null || activeLevelType == "BOSS") return;

        foreach (BeatData beat in currentAreaSpawnableList)
        {
            // Spawn area bullets, enemies and threats
            if(beat.behaviorType == null) return;
            
            Vector3 beatSpawnPos;
            if(beat.posAnchor == null)
            {
                beatSpawnPos = new(beat.spawnPos[0],0,beat.spawnPos[1]);
            } else {
                beatSpawnPos = new(beat.spawnPos[0],0,beat.spawnPos[1]); 
            }

            GameObject instantiatedObject = Instantiate(bulletDictionary[beat.behaviorType], beatSpawnPos, Quaternion.identity);

            if(beat.lifetime != null)
            {
                StartCoroutine(Destroy(instantiatedObject, (float)beat.lifetime*timeBetweenBeats));
            }
            
            activeBulletList.Item1.Add(instantiatedObject);
            activeBulletList.Item2.Add(beat);
        }
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    IEnumerator Destroy(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    //_________________________________________________________________________________
    #endregion
}