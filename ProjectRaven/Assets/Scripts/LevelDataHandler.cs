using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelDataHandler : LevelEditor 
{   
    public List<List<BeatData>> NewBeatList()
    {
        //Dictionary to contain integer types of list with a name constant of type string
        Dictionary<string, List<BeatData>> NamedEnemyDict = new();
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
        BeatData newBeatData = new()
        {
            attackType = attackType
        };
        newBeatData.attackPos[0] = attackPos.x;
        newBeatData.attackPos[1] = attackPos.y;

        return newBeatData;
    }

    public LevelData CreateLevel()
    {
        trackBeatAmount = (int)(musicResource.length/60 * BPM);
        levelInEdit = levelList.levelDataList.Count;

        LevelData levelData = new()
        {
            beatList = NewBeatList(),
            LevelIndex = levelInEdit
        };

        FileDataHandler fileDataHandler = new();

        string name;
        if(levelName.Equals("default"))
        {
            name = "Level"+levelInEdit.ToString()+"Data.json";
        } else {
            name = levelName + ".json";
        }
        string assetFilePath = "/Data/Levels/";
        string fullFilePath = Path.Combine(Application.persistentDataPath + assetFilePath + name);

        fileDataHandler.SaveFile(levelData, fullFilePath);

        levelList.AddLevel(levelData);

        return levelData;     
    }    

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
        } catch (Exception e) {
            Debug.LogError("Error occured when trying to save data to file: '" + fullFilePath + "' - Error at: " + e);
        }
    }

    private void CreateFile<T> (T dataToStore, string fullFilePath)
    {
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
            string jsonData = JsonConvert.SerializeObject(dataToStore, Formatting.Indented);

            using FileStream stream = new(fullFilePath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(jsonData);
        } catch (Exception e) {
            Debug.LogError("Failed to create file '"+ fullFilePath + "' - Error at: " + e);
        }
        
    }

    private void OverwriteFile<T> (T dataToStore, string fullFilePath)
    {
        try {
            File.Delete(fullFilePath);
            CreateFile(dataToStore, fullFilePath);
        } catch (Exception e) {
            Debug.LogError("Failed to overwrite file '"+ fullFilePath + "' - Error at: " + e);
        }
    }

    public T LoadFile<T>(string fullFilePath)
    {
        try {
            string jsonString = File.ReadAllText(fullFilePath);
            var dataToLoad = JsonConvert.DeserializeObject<T>(jsonString);

            return dataToLoad;
        } catch (Exception e) {
            Debug.LogError("Failed to load data from '"+fullFilePath+"'. - Error at: "+e);

            return default;
        }
        
    }
}

[Serializable]
public class LevelData
{
    public int LevelIndex;
    public List<List<BeatData>> beatList;
}

[Serializable]
public class BeatData
{
    public int attackType;
    public float[] attackPos = new float[2];
}
