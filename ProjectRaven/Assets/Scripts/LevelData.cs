using UnityEngine;
using System.Collections.Generic;

public class LevelData : MonoBehaviour
{
    public int trackBeatAmount;
    public int maxConcurrentAttacks;

    void Start()
    {
        UpdateData();
        CreateArray();
        PrintArray();
    }

    private int XData; 
    private int YData;
    
    void Update()
    {
        //Updates data each second. This is about 60 times more performance effective than doing so every frame which is completely unnecessary
        if(Time.frameCount % 60 == 0)
        {
            UpdateData();
        }

        if(maxConcurrentAttacks > beatDataArr[0].Count-1)
        {
            AppendListByY();
        }
    }

    //Updates the X and YData variables with new information from the editor. 
    void UpdateData()
    {
        XData = trackBeatAmount;
        YData = maxConcurrentAttacks;
    }


    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //beatDataArr contains integer field lists which can then be used for attack type information in spawner scripts. This should be set in LevelEditor.
    //Also contains positions for the said indexes
    public List<List<BeatData>> beatDataArr = new();

    //Dictionary to contain integer types of list with a name constant of type string
    readonly Dictionary<string, List<BeatData>> NamedEnemyTypesDict = new();

    //Dictionary to contain Vector2 types of list with a name constant of type string
    //readonly Dictionary<string, List<Vector2>> NamedEnemyAttacksDict = new();


    public void CreateArray()
    {
        for(int x = 0; x < XData; x++)
        {
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = "yList"+x.ToString();

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

    //Goes through the 2D list and prints attack type information on each element

    private void PrintArray()
    {
        int xVar = 0;
        int yVar = 0;

        string print = "";

        for(int i = 0; i < XData*YData; i++)
        {
            if(xVar < beatDataArr.Count-1)
            {
                print = print + " " + beatDataArr[xVar][yVar].attackType;
                xVar++;
            } else {
                print = print + " " + beatDataArr[xVar][yVar].attackType;
                xVar = 0;
                yVar++;
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
