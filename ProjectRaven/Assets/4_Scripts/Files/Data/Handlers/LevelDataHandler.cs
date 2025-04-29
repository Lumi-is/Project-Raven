using System.Collections.Generic;
using UnityEngine;

namespace System.IO.DataHandling
{
    public class LevelDataHandler
    {
        //Creation of a blank level
        public static void CreateLevel(List<AudioClip> phaseSongList, List<string> phaseNameList, string levelName, float BPM)
        {
            //Sets the phaseamount variable to be used for slightly less cpu usage every time the script is run
            int phaseAmount = phaseNameList.Count;

            //To make a path for level into directory (Default/Data/Levels/levelFilePath)
            string path = Path.Combine(Application.persistentDataPath,"Data","Levels",levelName);

            //If already exists, stop execution
            if(Directory.Exists(path)) return;

            //Else, continue execution and create a directory
            Directory.CreateDirectory(path);


            //_________________________________________________________________________________

            //Create subfolders for all phases under the level folder itself
            for(int i = 0; i < phaseAmount; i++)
            {
                //Creates a new list the length of how many beats this part of the song has and by default one attack per beat
                List<List<BeatData>> beatList2D = PhaseDataHandler.CreateBeatList2D((int)(phaseSongList[i].length / 60 * BPM), 1);

                //Uses phaseData constructor to create a new phaseData object according to the statistics given
                PhaseData phase = new(phaseNameList[i], beatList2D, BPM);

                //Get every phase's paths
                string phasePath = Path.Combine(path, phaseNameList[i]);

                //Also creates blank json files as a base for level editing 
                PhaseDataHandler.CreatePhaseToJson(phase, Path.Combine(phasePath+".json"));
            }
            //_________________________________________________________________________________
        }



        //_________________________________________________________________________________
        public static void DeleteLevel(string levelName)
        {
            string path = Path.Combine(Application.persistentDataPath,"Data","Levels",levelName);
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            } else {
                Debug.Log("Level to delete from" + path + " not found!");
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static LevelData LoadLevel(string levelName)
        {
            string levelPath = Path.Combine(Application.persistentDataPath,"Data","Levels",levelName);

            if(Directory.Exists(levelPath))
            {
                Debug.Log("Level "+levelName+" was loaded!");
                LevelData levelData = new(levelName);
                string[] phaseFiles = Directory.GetFiles(levelPath); 

                foreach (string phase in phaseFiles)
                {
                    PhaseData phaseData = PhaseDataHandler.LoadPhase(phase);
                    levelData.phaseList.Add(phaseData);
                }
                return levelData;
            } else {
                Debug.Log("Level was not found!");
                return null;
            }
        }
        //_________________________________________________________________________________
    }
}

