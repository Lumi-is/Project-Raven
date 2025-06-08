using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;
using System.IO;
using System;
using Unity.VisualScripting;

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

    [SerializeField] float roomSpacingMultiplier = 60;


    //For room spawning 

    [Header("Room Type List")]
    private readonly Dictionary<string, GameObject> roomDictionary = new();
    //_________________________________________________________________________________
    #endregion



    #region Bullet Management Variables
    //_________________________________________________________________________________
    [Header("Bullet Type List")]

    // Dictionary for connecting the name of the attack as stated above, to the prefab saved in the resources/prefabs folder
    private readonly Dictionary<string, GameObject> bulletDictionary = new();

    // Tuple for containing both the active (existing in the scene) bullet GameObject, as well as it's properties as BeatData
    // These are then used when spawning the enemies
    public static Tuple<List<GameObject>, List<BeatData>> activeBulletList = new(new(), new());

    //_________________________________________________________________________________
    #endregion



    #region Beat Management Variables
    //_________________________________________________________________________________

    [Header("Data")]
    [Tooltip("Boolean read by other scripts for checking whether the music is currently on beat and an action can be done.")]
    public static bool OnBeat;

    [Header("Current items")]
    public int currentBeat;

    [Header("Global")]
    [Range(-1, 1)]
    public float globalOffsetMultiplier;
    //_________________________________________________________________________________
    #endregion



    #region Sound Management Variables
    //_________________________________________________________________________________

    [Header("Sound")]
    [SerializeField] AudioSource musicPlayer;

    [Header("Music Attributes")]
    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    [Range(0.1f, 1)]
    public float maxTimingOffset = 0.75f;

    private float lastBeat;
    public static float timeBetweenBeats;
    private float currentFrameTime;
    private bool cooldownDecremented = false;

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
        bossLevelDirectoryPath = Path.Combine(Application.persistentDataPath, "Data", "Bosses");
        if (!Directory.Exists(bossLevelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(bossLevelDirectoryPath);
        }

        // Initializing levelDirectoryPath to be /Data/Areas/ for area levels
        areaLevelDirectoryPath = Path.Combine(Application.persistentDataPath, "Data", "Areas");
        if (!Directory.Exists(areaLevelDirectoryPath))
        {
            GlobalDataHandler.CreateDirectory(areaLevelDirectoryPath);
        }

        // For initializing bulletDictionary
        foreach (GameObject gameObject in Resources.LoadAll(Path.Combine("Prefabs", "Bullets"), typeof(GameObject)))
        {
            bulletDictionary.Add(gameObject.name, gameObject);
        }

        foreach (GameObject gameObject in Resources.LoadAll(Path.Combine("Prefabs", "Rooms"), typeof(GameObject)))
        {
            roomDictionary.Add(gameObject.name, gameObject);
        }

        // Adding audiosource to editor with the object
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        musicPlayer = gameObject.GetComponent<AudioSource>();

        //DEBUGGING ONLY
        if (DebugLevelCreation)
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
        currentPhaseData = currentPhaseDataList[UnityEngine.Random.Range(0, currentPhaseDataList.Count)];
    }
    //_________________________________________________________________________________

    //_________________________________________________________________________________
    public void SpawnRooms()
    {
        // Spawn in rooms and items
        foreach (RoomData room in currentAreaData.roomList)
        {
            if (room.roomType != "none")
            {
                GameObject newRoom = Instantiate(roomDictionary[room.roomType], new Vector3(room.roomPos[0], room.roomPos[1], room.roomPos[2]) * roomSpacingMultiplier, new Quaternion(0, room.roomRotation, 0, 0));
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
        if (activeLevelType == "BOSS" || activeLevelType == "ALL")
        {
            musicPlayer.clip = currentBossData.phaseDataList.Item2[0];
        }
        else if (activeLevelType == "AREA")
        {
            musicPlayer.clip = currentBossData.phaseDataList.Item2[0];
        }

        if (phaseChangerType != null)
        {
            StopCoroutine(phaseChangerType);
        }

        if (activeLevelType == "BOSS" || activeLevelType == "ALL")
        {
            BossConditionTypeActions();
        }

    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void StartLevel()
    {
        if (activeLevelType == "NONE")
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
    #endregion

    #region On Frame Update
    //_________________________________________________________________________________
    // Every frame
    void Update()
    {
        //Debug.Log(OnBeat);
        if (!LevelActive) return;

        currentFrameTime = musicPlayer.time;

        DoTimingChecks();

        if (currentBossData != null && currentAreaData != null && (activeLevelType == "BOSS" || activeLevelType == "AREA" || activeLevelType == "ALL")) DoBeatChecks();

        Debug.Log("CurrentFrameTime: " + currentFrameTime);
        Debug.Log("lastBeat: " + lastBeat);
        Debug.Log("TimeBetweenBeats: " + timeBetweenBeats);

    }
    //_________________________________________________________________________________

    [SerializeField] GameObject debugVisualsRemoveAfterTesting;

    private void DoTimingChecks()
    {
        float trueOffset = maxTimingOffset * timeBetweenBeats;

        float currentBeatTime;
        //if currentframetime closer to the next beat than the last, check with currentBeat+1. Otherwise check with currentbeat+0
        if (Math.Abs(currentFrameTime - (currentBeat + 1) * timeBetweenBeats) < Math.Abs(currentFrameTime - currentBeat * timeBetweenBeats))
        {
            if (cooldownUntilNextAction != 0 && !cooldownDecremented)
            {
                cooldownUntilNextAction--;
                cooldownDecremented = true;
            }
            currentBeatTime = (currentBeat + 1) * timeBetweenBeats;
        }
        else
        {
            currentBeatTime = currentBeat * timeBetweenBeats;
        }

        if ((currentFrameTime < (currentBeatTime + trueOffset + globalOffsetMultiplier * trueOffset)) && (currentFrameTime > (currentBeatTime - trueOffset + globalOffsetMultiplier * trueOffset)))
        {
            if (OnBeat) return;

            OnBeat = true;
            debugVisualsRemoveAfterTesting.SetActive(true);
        }
        else
        {
            OnBeat = false;
            debugVisualsRemoveAfterTesting.SetActive(false);
        }
    }

    //_________________________________________________________________________________
    private void DoBeatChecks()
    {
        // Checks if enough time has gone from last beat for the beat change to occur
        // Additionally, makes sure the music is not at it's end
        if (currentFrameTime >= lastBeat + timeBetweenBeats)
        {
            IncrementBeat();
        }
        // This is for executing the correct actions if the music is at its last beat and should be looped
        else if (currentFrameTime < lastBeat)
        {
            ResetBeat();
        }
    }
    //_________________________________________________________________________________




    #region Phase Management

    //_________________________________________________________________________________
    private void BossConditionTypeActions()
    {
        switch (currentPhaseData.phaseChangeConditionType)
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
        yield return new WaitUntil(() => currentBossHP <= maxBossHP - (currentPhaseIndex + 1) * (maxBossHP / currentLevelPhaseAmount));
        ChangePhase();
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    IEnumerator BossHPPercentage_PhaseChanger()
    {
        yield return new WaitUntil(() => currentBossHP / maxBossHP < currentPhaseData.percentageAmount);
        ChangePhase();
    }
    //_________________________________________________________________________________ 



    //_________________________________________________________________________________
    IEnumerator BossTimer_PhaseChanger()
    {
        Debug.Log("Timer started. " + currentPhaseData.timerAmount + " seconds until the end of current phase.");
        yield return new WaitForSeconds(currentPhaseData.timerAmount);
        ChangePhase();
    }
    //_________________________________________________________________________________    



    //_________________________________________________________________________________
    public void ChangePhase()
    {
        // Gets the index of the next item in the list
        var next = currentBossData.phaseDataList.Item1.IndexOf(currentPhaseDataList) + 1;

        if (next < currentBossData.phaseDataList.Item1.Count)
        {
            // Sets the currentPhaseData to be the next level
            currentPhaseDataList = currentBossData.phaseDataList.Item1[next];
            currentPhaseData = currentPhaseDataList[UnityEngine.Random.Range(0, currentPhaseDataList.Count)];
            musicPlayer.clip = currentBossData.phaseDataList.Item2[next];
            Debug.Log("Phase was changed: " + currentPhaseDataList);
            ResetBeat();

            if (phaseChangerType != null)
            {
                StopCoroutine(phaseChangerType);
            }

            if (activeLevelType == "BOSS" || activeLevelType == "ALL")
            {
                BossConditionTypeActions();
            }

        }
        else
        {
            // Boss death, level victory
            Debug.Log("Victory!!");
        }
    }
    //_________________________________________________________________________________
    #endregion
    //_________________________________________________________________________________
    #endregion



    #region Beat Management
    //_________________________________________________________________________________
    public void IncrementBeat()
    {
        lastBeat += timeBetweenBeats;

        if (bossOverridesAreaLevel)
        {
            currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            currentAreaSpawnableList = new();
        }
        else
        {
            if (currentPhaseDataList != null)
            {
                currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            }

            if (currentAreaData != null)
            {
                currentAreaSpawnableList = currentRoomData.beatMatrix[currentBeat];
            }
        }
        SpawnEnemies();
        cooldownDecremented = false;
        currentBeat++;
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void ResetBeat()
    {
        lastBeat = 0;
        currentBeat = 0;

        if (bossOverridesAreaLevel)
        {
            currentBPM = currentPhaseData.BPM;
            currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            currentAreaSpawnableList = new();
        }
        else
        {
            if (currentPhaseDataList != null)
            {
                currentBPM = currentPhaseData.BPM;
                currentBossSpawnableList = currentPhaseData.beatMatrix[currentBeat];
            }

            if (currentAreaData != null)
            {
                currentBPM = currentAreaData.BPM;
                currentAreaSpawnableList = currentRoomData.beatMatrix[currentBeat];
            }
        }

        timeBetweenBeats = 60f / currentBPM;
        musicPlayer.Stop();
        musicPlayer.Play();
        musicPlayer.time = 0;
    }
    //_________________________________________________________________________________
    #endregion



    #region SpawnManagement
    //_________________________________________________________________________________
    private void SpawnEnemies()
    {
        // Skips the execution if currentAreaSpawnableList is null or if activeLevelType is not suitable
        List<BeatData> data = new();
        if (activeLevelType == "BOSS" && currentBossSpawnableList != null)
        {
            data = currentBossSpawnableList;
        }
        else if (activeLevelType == "AREA" && currentAreaSpawnableList != null)
        {
            data = currentAreaSpawnableList;
        }
        else if (activeLevelType == "ALL" && currentAreaSpawnableList != null && currentBossSpawnableList != null)
        {
            foreach (BeatData beatData in currentBossSpawnableList)
            {
                data.Add(beatData);
            }
            foreach (BeatData beatData in currentAreaSpawnableList)
            {
                data.Add(beatData);
            }
        }

        foreach (BeatData beat in data)
        {
            // Spawn area bullets, enemies and threats
            if (beat.behaviorType == null) return;

            Vector3 beatSpawnPos;
            if (beat.posAnchor == null)
            {
                beatSpawnPos = new(beat.spawnPos[0], 0, beat.spawnPos[1]);
            }
            else if (beat.posAnchor == "PLAYER")
            {
                beatSpawnPos = MovementController.publicPlayerLocation;
            }
            else if (beat.posAnchor == "ROOM")
            {
                beatSpawnPos = new Vector3(currentRoomData.roomPos[0], currentRoomData.roomPos[1], currentRoomData.roomPos[2]) * roomSpacingMultiplier;
            }
            else
            {
                beatSpawnPos = new(beat.spawnPos[0], 0, beat.spawnPos[1]);
                Debug.LogWarning("Incorrect JSON file found! Check level files and try again.");
            }


            List<GameObject> instantiatedObjectList = new();

            foreach (string str in beat.behaviorType)
            {
                if (beat.isRandomized)
                {
                    beatSpawnPos = SpawnPosRandomizer.GetRandomizedPosition(beatSpawnPos, beat.randomizerAmountX, beat.randomizerAmountZ);
                }
                GameObject instantedObject = Instantiate(bulletDictionary[str], beatSpawnPos, Quaternion.identity);
                instantiatedObjectList.Add(instantedObject);
            }



            foreach (GameObject instantiatedObject in instantiatedObjectList)
            {
                if (beat.lifetime != 0)
                {
                    StartCoroutine(Destroyer(instantiatedObject, beat.lifetime * timeBetweenBeats));
                }
                activeBulletList.Item1.Add(instantiatedObject);
                activeBulletList.Item2.Add(beat);
            }
        }
        //_________________________________________________________________________________

        //_________________________________________________________________________________
        IEnumerator Destroyer(GameObject destroyable, float timeUntil)
        {
            yield return new WaitForSeconds(timeUntil);
            Destroy(destroyable);
        }
        //_________________________________________________________________________________
        #endregion
    }
}