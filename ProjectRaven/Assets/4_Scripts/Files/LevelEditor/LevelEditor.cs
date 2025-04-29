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
    [SerializeField] float BPM;

    void Awake()
    {
        //Debugging and testing ONLY
        //LevelDataHandler.DeleteLevel(levelName);
        //LevelDataHandler.CreateLevel(phaseSongList, phaseNameList, levelName, BPM);
    }
}
