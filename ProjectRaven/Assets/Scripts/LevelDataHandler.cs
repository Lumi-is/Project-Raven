using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelDataHandler : LevelEditor 
{   
    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //Dictionary to contain integer types of list with a name constant of type string
    readonly Dictionary<string, List<BeatData>> NamedEnemyDict = new();

    public List<List<BeatData>> NewBeatList()
    {
        List<List<BeatData>> beatList = new();
        levelInEdit = levelList.levelDataList.Count;

        for(int x = 0; x < trackBeatAmount; x++)
        {
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = levelInEdit.ToString()+"yList"+x.ToString();

            //In the two below, put data from users that are creating the level instead of "new". Remember to copy values instead of using a single variable.
            //Creates two items into dictionaries which can both be accessed with the same name
            NamedEnemyDict.Add(Name,new());

            //Add the said items to lists that contain data for the attack spawns
            beatList.Add(NamedEnemyDict[Name]);

            for(int y = 0; y < maxAttackAmount; y++)
            {
                beatList[x].Add(NewBeatData(0, new Vector2(0,0)));
            }
        }
        return beatList;
    }

    BeatData NewBeatData(int attackType, Vector2 attackPos)
    {
        BeatData newBeatData = new();
        newBeatData.attackType = attackType;
        newBeatData.attackPos[0] = attackPos.x;
        newBeatData.attackPos[1] = attackPos.y;

        return newBeatData;
    }

    new public LevelData CreateLevel()
    {
        levelInEdit = levelList.levelDataList.Count;

        LevelData levelData = new();
        levelData.beatList = NewBeatList();
        levelData.LevelIndex = levelInEdit;

        FileDataHandler fileDataHandler = new();

        string name = "Level"+levelInEdit.ToString()+"Data.json";
        string assetFilePath = "/Data/Levels/";
        string fullFilePath = Path.Combine(Application.persistentDataPath + assetFilePath + name);


        //Debug.Log(Path.Combine(fullDataPath,"Level"+editedLevelIndex.ToString()+"Data"));
        fileDataHandler.SaveFile(levelData, fullFilePath);

        levelList.AddLevel(levelData);

        return levelData;     
    }    
    //Goes through the 2D list and prints attack type information on each element

    public List<List<BeatData>> AppendListByY(List<List<BeatData>> beatList)
    {
        for(int i = 0; i < beatList.Count; i++)
        {
            beatList[i].Add(NewBeatData(0,new Vector2(0,0)));
        }

        return beatList;
    }
}

public class FileDataHandler
{
    public void SaveFile<T>(T dataToStore, string fullFilePath)
    {
        try {
            if(File.Exists(fullFilePath))
            {
                OverwriteFile(dataToStore, fullFilePath);
            } else {
                CreateFile(dataToStore, fullFilePath);
            }
        } catch (Exception exception)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullFilePath + " : " + exception);
        }
    }

    private void CreateFile<T> (T dataToStore, string fullFilePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
        string jsonData = JsonConvert.SerializeObject(dataToStore);

        using FileStream stream = new(fullFilePath, FileMode.Create);
        using StreamWriter writer = new(stream);
        writer.Write(jsonData);
    }

    private void OverwriteFile<T> (T dataToStore, string fullFilePath)
    {
        try {
            File.Delete(fullFilePath);
            CreateFile(dataToStore, fullFilePath);
        } catch (Exception exception) {
            Debug.LogError("Error overwriting the data - "+exception);
        }
    }

    public T LoadFile<T>(string fullFilePath)
    {
        string jsonString = File.ReadAllText(fullFilePath);
        var dataToLoad = JsonConvert.DeserializeObject<T>(jsonString);

        return dataToLoad;
    }
}

[System.Serializable]
public class LevelData
{
    public int LevelIndex;
    public List<List<BeatData>> beatList;
}

[System.Serializable]
public class BeatData
{
    public int attackType;
    public float[] attackPos = new float[2];
}
