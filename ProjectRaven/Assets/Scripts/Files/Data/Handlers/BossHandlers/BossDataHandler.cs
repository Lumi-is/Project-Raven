using System.Collections.Generic;
using UnityEngine;
using OggVorbis;
using Tools;
using System;

namespace System.IO.DataHandling
{
    public class BossDataHandler
    {
        //Creation of a blank level
        public static void CreateBossLevel(List<AudioClip> phaseSongList, List<string> phaseNameList, List<int> phaseVersionAmountList, string levelName, float BPM)
        {
            //Sets the phaseamount variable to be used for slightly less CPU usage every time the script is run
            int phaseAmount = phaseNameList.Count;

            //To make a path for level into directory (Default/Data/Bosses/levelName)
            string path = Path.Combine(LevelEventManager.bossLevelDirectoryPath,levelName);

            //If already exists, stop execution
            if(Directory.Exists(path)) return;

            //Else, continue execution and create a directory
            Directory.CreateDirectory(path);


            //_________________________________________________________________________________

            //Create subfolders for all phases under the level folder itself
            for(int i = 0; i < phaseAmount; i++)
            {
                //Creates a new list the length of how many beats this part of the song has and by default one attack per beat
                List<List<BeatData>> beatList2D = MatrixDataHandler.CreateBeatList2D((int)(phaseSongList[i].length / 60 * BPM), 1);

                //Uses phaseData constructor to create a new phaseData object according to the statistics given
                BossPhaseData phase = new(phaseNameList[i], beatList2D, BPM);

                //Get every phase's paths
                string phasePath = Path.Combine(path, phaseNameList[i]);

                // Create subfolder for each phase
                GlobalDataHandler.CreateDirectory(phasePath);

                //Also creates blank json files as a base for level editing 
                for(int k = 0; k < phaseVersionAmountList[i]; k++)
                {
                    MatrixDataHandler.CreateFileAsJson(phase, Path.Combine(phasePath, phaseNameList[i]+"_ver_"+(k+1)+".json"));                   
                }

                // Saves current phase's song with the same name as the phase to the phase directory
                VorbisPlugin.Save(Path.Combine(phasePath,phaseNameList[i]+".ogg"),phaseSongList[i],1);
            }
            //_________________________________________________________________________________
        }



        //_________________________________________________________________________________
        public static void DeleteBossLevel(string name)
        {
            Debug.Log(Path.Combine(LevelEventManager.bossLevelDirectoryPath,name));
            string path = Path.Combine(LevelEventManager.bossLevelDirectoryPath,name);
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            } else {
                Debug.Log("Level to delete from path \"" + path + "\" not found!");
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static BossData LoadBossLevel(string name)
        {
            string path = Path.Combine(LevelEventManager.bossLevelDirectoryPath,name);

            if(Directory.Exists(path)) // Checks if boss level folder exists with the said name
            {
                BossData levelData = new(name);
                string[] phaseFolderList = Directory.GetDirectories(path); 

                for(int i = 0; i < phaseFolderList.Length; i++) // Goes through every folder under <bossLevelName>
                {
                    levelData.phaseDataList.Item1.Add(new());
                    string phaseFolder = phaseFolderList[i];
                    foreach(string filePath in Directory.GetFiles(phaseFolder)) // Checks all files under folder <bossPhaseName>
                    {
                        string file = Path.GetFileName(filePath);
                        if(file.Contains(".json"))
                        {
                            BossPhaseData phaseData = LoadPhase(filePath);
                            levelData.phaseDataList.Item1[i].Add(phaseData);
                        } else if (file.Contains(".ogg")) {
                            AudioClip audioClip = VorbisPlugin.Load(filePath);
                            levelData.phaseDataList.Item2.Add(audioClip);
                        }
                    }
                }
                Debug.Log("Level "+name+" was loaded!");
                return levelData;
            } else {
                Debug.Log("\""+name+"\" was not found! Check input name and try again.");
                return null;
            }
        }
        //_________________________________________________________________________________

        //_________________________________________________________________________________
        public static BossPhaseData LoadPhase(string filePath)
        {
            BossPhaseData loadedPhase = JsonConverter.FromJson<BossPhaseData>(filePath);
            return loadedPhase;
        }
        //_________________________________________________________________________________
    }
}

