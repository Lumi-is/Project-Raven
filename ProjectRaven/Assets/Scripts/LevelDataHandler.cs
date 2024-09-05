using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelDataHandler : LevelEditor 
{   
    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //Dictionary to contain integer types of list with a name constant of type string
    readonly Dictionary<string, List<BeatData>> NamedEnemyTypesDict = new();

    public void CalulateBeatAmount()
    {
        trackBeatAmount = (int)(musicResource.length/60 * BPM);
    }

    public void CreateArray()
    {
        for(int x = 0; x < XData; x++)
        {
            editedLevelIndex = levelList.levelDataList.Count - 1;
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = editedLevelIndex.ToString()+"yList"+x.ToString();

            //In the two below, put data from users that are creating the level instead of "new". Remember to copy values instead of using a single variable.
            //Creates two items into dictionaries which can both be accessed with the same name
            NamedEnemyTypesDict.Add(Name,new());

            //Add the said items to lists that contain data for the attack spawns
            beatDataArr.Add(NamedEnemyTypesDict[Name]);;

            for(int y = 0; y < YData; y++)
            {
                beatDataArr[x].Add(new());
            }
        }
    }

    new public LevelData CreateLevel()
    {
        LevelData levelData = new();

        FileDataHandler fileDataHandler = new();

        string fullDataPath = Application.persistentDataPath + "/Data/Levels";
        //Debug.Log(Path.Combine(fullDataPath,"Level"+editedLevelIndex.ToString()+"Data"));
        fileDataHandler.SaveFile(levelData, fullDataPath, "Level"+editedLevelIndex.ToString()+"Data");
        return levelData;
    }    
    public LevelData SetVariables(LevelData levelData, List<List<BeatData>> transferredData, int transferredIndex)
    {
        levelData.beatDataArr = transferredData;
        levelData.levelIndex = transferredIndex;

        return levelData;
    }

    //Goes through the 2D list and prints attack type information on each element
    public void PrintArray()
    {
        int x = 0;
        int y = 0;

        string print = "";

        for(int i = 0; i < XData*YData; i++)
        {
            if(x < beatDataArr.Count-1)
            {
                print = print + " " + beatDataArr[x][y].attackType;
                x++;
            } else {
                print = print + " " + beatDataArr[x][y].attackType;
                x = 0;
                y++;
                Debug.Log(print);
                print = "";
            }
        }
    }

    public void AppendListByY()
    {
        for(int i = 0; i < beatDataArr.Count; i++)
        {
            beatDataArr[i].Add(new());
        }
    }

    public void RemoveFromListByY()
    {
        for(int i = 0; i < beatDataArr.Count; i++)
        {
            beatDataArr[i].RemoveAt(beatDataArr[i].Count-1);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public int levelIndex {get; set;}
    public List<List<BeatData>> beatDataArr {get; set;}
}

public class BeatData
{
    public int attackType;
    public Vector2 attackPos;
}

public class FileDataHandler
{
    public void SaveFile<T>(T dataToStore, string filePath, string fileName)
    {
        try {
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if(File.Exists(filePath))
            {
                OverwriteFile(dataToStore, filePath, fileName);
            } else {
                CreateFile(dataToStore, filePath, fileName);
            }
        } catch (Exception exception)
        {
            Debug.LogError("Error occured when trying to save data to file: " + filePath + " : " + exception);
        }
    }

    private void CreateFile<T> (T dataToStore, string filePath, string fileName)
    {
        string jsonString = JsonConvert.SerializeObject(dataToStore);
        string file = string.Format(filePath, fileName);
        File.WriteAllText(filePath, file);
    }

    private void OverwriteFile<T> (T dataToStore, string filePath, string fileName)
    {
        File.Delete(Path.Combine(filePath,fileName));
        CreateFile(dataToStore, filePath, fileName);
    }

    public T LoadFile<T>(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        var dataToLoad = JsonConvert.DeserializeObject<T>(jsonString);

        return dataToLoad;
    }
}
