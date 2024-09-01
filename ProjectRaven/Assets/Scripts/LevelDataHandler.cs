using UnityEngine;
using System.Collections.Generic;

public class LevelDataHandler : LevelEditor 
{   
    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //beatDataArr contains integer field lists which can then be used for attack type information in spawner scripts. This should be set in LevelEditor and positions for the said indexes
    public List<List<BeatData>> beatDataArr = new();
    public int trackBeatAmount;

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
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = editedLevel.ToString()+"yList"+x.ToString();

            //In the two below, put data from users that are creating the level instead of "new". Remember to copy values instead of using a single variable.
            //Creates two items into dictionaries which can both be accessed with the same name
            NamedEnemyTypesDict.Add(Name,new());

            //Add the said items to lists that contain data for the attack spawns
            beatDataArr.Add(NamedEnemyTypesDict[Name]);;

            for(int y = 0; y < YData; y++)
            {
                beatDataArr[x].Add(ScriptableObject.CreateInstance<BeatData>());
            }
        }
    }

    public LevelData SaveData()
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.SaveData(beatDataArr, levelListScript.levelListData.Count);
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
            beatDataArr[i].Add(ScriptableObject.CreateInstance<BeatData>());
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
