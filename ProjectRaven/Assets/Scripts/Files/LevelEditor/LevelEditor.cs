using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;

public class LevelEditor : MonoBehaviour
{
    [Header("Level information")]
    [SerializeField] string levelName;

    [Header("Phases information")]
    [SerializeField] List<string> phaseNameList;

    [Header("Song information")]

    [SerializeField] List<AudioClip> phaseSongList;
    [SerializeField] float defaultBPM;

    void Awake()
    {
        //Debugging and testing ONLY
        BossDataHandler.DeleteBossLevel(levelName);
        BossDataHandler.CreateBossLevel(phaseSongList, phaseNameList, levelName, defaultBPM);
    }
}
