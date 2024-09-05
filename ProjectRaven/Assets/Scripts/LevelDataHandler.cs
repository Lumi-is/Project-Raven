using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class LevelDataHandler : LevelEditor 
{   
    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //Dictionary to contain integer types of list with a name constant of type string
    readonly Dictionary<string, BeatData> NamedEnemyDict = new();

    public void CreateArray()
    {
        for(int x = 0; x < XData; x++)
        {
            editedLevelIndex = levelList.levelDataList.Count - 1;
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = editedLevelIndex.ToString()+"yList"+x.ToString();

            //In the two below, put data from users that are creating the level instead of "new". Remember to copy values instead of using a single variable.
            //Creates two items into dictionaries which can both be accessed with the same name
            NamedEnemyDict.Add(Name,new());

            //Add the said items to lists that contain data for the attack spawns
            beatDataArr.Add(NamedEnemyDict[Name]);

            for(int y = 0; y < YData; y++)
            {
                beatDataArr[x].attackTypeList.Add(new());
                beatDataArr[x].attackPosList.Add(new());
            }
        }
    }

    new public LevelData CreateLevel()
    {
        LevelData levelData = new();

        FileDataHandler fileDataHandler = new();

        string name = "Level"+editedLevelIndex.ToString()+"Data.json";
        string assetFilePath = "/Data/Levels/";
        string fullFilePath = Path.Combine(Application.persistentDataPath + assetFilePath + name);


        //Debug.Log(Path.Combine(fullDataPath,"Level"+editedLevelIndex.ToString()+"Data"));
        fileDataHandler.SaveFile(levelData, fullFilePath);
        return levelData;
    }    
    //Goes through the 2D list and prints attack type information on each element

    public void AppendListByY()
    {
        for(int i = 0; i < beatDataArr.Count; i++)
        {
            beatDataArr[i].attackTypeList.Add(new());
            beatDataArr[i].attackPosList.Add(new());
        }
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
        string jsonData = JsonUtility.ToJson(dataToStore, true);

        using FileStream stream = new(fullFilePath, FileMode.Create);
        using StreamWriter writer = new(stream);
        writer.Write(jsonData);
    }

    private void OverwriteFile<T> (T dataToStore, string fullFilePath)
    {
        File.Delete(fullFilePath);
        CreateFile(dataToStore, fullFilePath);
    }

    public T LoadFile<T>(string fullFilePath)
    {
        string jsonString = File.ReadAllText(fullFilePath);
        var dataToLoad = JsonUtility.FromJson<T>(jsonString);

        return dataToLoad;
    }
}

[Serializable]
public class LevelData
{
    public int LevelIndex;
    public List<BeatData> BeatDataArr;
}

[Serializable]
public class BeatData
{
    public List<int> attackTypeList;
    public List<Vector2> attackPosList;
}
