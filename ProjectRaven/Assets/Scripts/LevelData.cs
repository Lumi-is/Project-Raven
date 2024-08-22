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
    }

    //Updates the X and YData variables with new information from the editor. 
    void UpdateData()
    {
        XData = trackBeatAmount;
        YData = maxConcurrentAttacks;
    }


    //Create a few lists and dictionaries to be used in spawner and level editor scripts.

    //enemyTypeArr contains integer field lists which can then be used for attack type information in spawner scripts. This should be set in LevelEditor.
    public List<List<int>> enemyTypeArr = new();

    //attackPosArr contains positions in the same indexes as the enemyTypeArr. This is used to check where to spawn the above said attacks in the play area. 
    public List<List<Vector2>> attackPosArr = new();

    //Dictionary to contain integer types of list with a name constant of type string
    readonly Dictionary<string, List<int>> NamedEnemyTypesDict = new();

    //Dictionary to contain Vector2 types of list with a name constant of type string
    readonly Dictionary<string, List<Vector2>> NamedEnemyAttacksDict = new();


    public void CreateArray()
    {
        for(int x = 0; x < XData; x++)
        {
            //Makes a temporary name for the following script to use in creating items. This name is never used later. 
            string Name = "yList"+x.ToString();

            //In the two below, put data from users that are creating the level instead of "new". Remember to copy values instead of using a single variable.
            //Creates two items into dictionaries which can both be accessed with the same name
            NamedEnemyTypesDict.Add(Name,new());
            NamedEnemyAttacksDict.Add(Name,new());

            //Add the said items to lists that contain data for the attack spawns
            enemyTypeArr.Add(NamedEnemyTypesDict[Name]);
            attackPosArr.Add(NamedEnemyAttacksDict[Name]);

            for(int y = 0; y < YData; y++)
            {
                //Both refer to ArrayY
                enemyTypeArr[x].Add(0); 
                attackPosArr[x].Add(new Vector2(0,0)); 
            }
        }
    }

    //Goes through the 2D list and prints attack type information on each element

    private void PrintArray()
    {
        int xVar = 0;
        int yVar = 0;

        Debug.Log(enemyTypeArr[0][0]);

        Debug.Log(enemyTypeArr[0]);

        string print = "";

        //Change this to change value :3 FOR TESTING ONLY!!!!
        enemyTypeArr[30][0] = 1;
        attackPosArr[30][0] = new Vector2(5,5);

        for(int i = 0; i < XData*YData; i++)
        {
            if(xVar < enemyTypeArr.Count-1)
            {
                print = print + " " + enemyTypeArr[xVar][yVar];
                xVar++;
            } else {
                print = print + " " + enemyTypeArr[xVar][yVar];
                xVar = 0;
                yVar++;
                Debug.Log(print);
                print = "";
            }
        }
    }

    public void AppendListByY()
    {
        for(int i = 0; i < enemyTypeArr.Count; i++)
        {
            enemyTypeArr[i].Add(new());
        }
    }

    public void RemoveFromListByY()
    {
        for(int i = 0; i < enemyTypeArr.Count; i++)
        {
            enemyTypeArr[i].RemoveAt(enemyTypeArr[i].Count-1);
        }
    }
}
