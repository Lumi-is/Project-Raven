using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelDataHandler : PhaseDataHandler
{
    //Creation of a blank level
    public void CreateLevel(string levelName, List<AudioClip> phaseSongList, List<string> phaseNameList, string defaultFilePath, float BPM)
    {
        //Sets the phaseamount variable to be used for slightly less cpu usage every time the script is run
        int phaseAmount = phaseNameList.Count;

        //To make a path for level into directory (Default/Data/Levels/levelFilePath)
        string levelFilePath = Path.Combine(defaultFilePath, levelName);

        //If already exists, stop execution
        if(Directory.Exists(levelFilePath)) return;

        //Else, continue execution and create a directory
        Directory.CreateDirectory(levelFilePath);

        //_________________________________________________________________________________

        //Create subfolders for all phases under the level folder itself
        for(int i = 0; i < phaseAmount; i++)
        {
            //Creates a new list the length of how many beats this part of the song has
            List<List<BeatData>> beatList = new List<List<BeatData>>(new List<BeatData>[(int)(phaseSongList[i].length / 60 * BPM)]);

            //Uses phaseData constructor to create a new phaseData object according to the statistics given
            PhaseData phaseData = new(phaseNameList[i], beatList);

            //Get every phase's paths
            string phaseFilePath = Path.Combine(levelFilePath, phaseNameList[i]);

            //Subfolders are to be named after phaseName
            Directory.CreateDirectory(phaseFilePath);

            //Also creates blank json files as a base for level editing 
            CreateFile(phaseData, Path.Combine(phaseFilePath,phaseNameList[i]+".json"));
        }
        //_________________________________________________________________________________
    }
}
