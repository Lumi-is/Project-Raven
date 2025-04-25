using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

public class LevelEditor : LevelDataHandler
{
    [Header("Level information")]
    [SerializeField] string levelName;

    [Header("Phases information")]
    [SerializeField] List<string> phaseNameList;

    [Header("Song information")]

    [SerializeField] List<AudioClip> phaseSongList;
    [SerializeField] float BPM;

    [Header("Files")]
    [SerializeField] PhaseDataHandler phaseDH;
    private string defaultFilePath;
    private delegate void fileHandler();
    
    
    void Awake()
    {
        defaultFilePath = Path.Combine(Application.persistentDataPath,"Data","Levels","");
        if(!Directory.Exists(defaultFilePath))
        {
            CreateDirectory(defaultFilePath);
        }
        DeleteLevel(levelName, defaultFilePath);
        CreateLevel(levelName, phaseSongList, phaseNameList, defaultFilePath, BPM);
    }
}
