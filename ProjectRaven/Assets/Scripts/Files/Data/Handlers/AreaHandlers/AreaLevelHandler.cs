
using System.Collections.Generic;
using UnityEngine;
using OggVorbis;
using Tools;

namespace System.IO.DataHandling
{
    public class AreaLevelHandler
    {
        //_________________________________________________________________________________
        public static void CreateAreaLevel(List<AudioClip> areaSongList, List<string> areaNameList, string levelName, float BPM, List<string> roomNameList)
        {
            //To make a path for level into directory (Default/Data/Areas/levelName)
            string path = Path.Combine(LevelEventManager.areaLevelDirectoryPath,levelName);

            //If already exists, stop execution
            if(Directory.Exists(path)) return;

            //Else, continue execution and create a directory
            Directory.CreateDirectory(path);

            //_________________________________________________________________________________

            //Create subfolders for all phases under the level folder itself
            for(int i = 0; i < areaNameList.Count; i++)
            {
                //Creates a new list the length of how many beats this part of the song has and by default one attack per beat
                List<List<BeatData>> beatList2D = MatrixDataHandler.CreateBeatList2D((int)(areaSongList[i].length / 60 * BPM), 1);

                //Uses phaseData constructor to create a new phaseData object according to the statistics given
                // Currently Creates one room as the default
                AreaData area = new(areaNameList[i], beatList2D, BPM, roomNameList); 
                //Get every phase's paths
                string phasePath = Path.Combine(path, areaNameList[i]);

                // Create subfolder for each phase
                GlobalDataHandler.CreateDirectory(phasePath);

                //Also creates blank json files as a base for level editing 
                MatrixDataHandler.CreateFileAsJson(area, Path.Combine(phasePath, areaNameList[i]+".json"));

                // Saves current phase's song with the same name as the phase to the phase directory
                VorbisPlugin.Save(Path.Combine(phasePath,areaNameList[i]+".ogg"),areaSongList[i],1);
            }
            
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static void DeleteAreaLevel(string name)
        {
            Debug.Log(Path.Combine(LevelEventManager.areaLevelDirectoryPath,name));
            string path = Path.Combine(LevelEventManager.areaLevelDirectoryPath,name);
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            } else {
                Debug.Log("Level to delete from path \"" + path + "\" not found!");
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static AreaLevelData LoadAreaLevel(string name)
        {
            string path = Path.Combine(LevelEventManager.areaLevelDirectoryPath,name);

            if(Directory.Exists(path))
            {
                AreaLevelData areaLevelData = new(name);
                string[] files = Directory.GetDirectories(path); 

                foreach (string area in files)
                {
                    string areaPath = Path.Combine(area,Directory.GetFiles(area)[0]);
                    AreaData areaData = LoadArea(areaPath);
                    areaLevelData.areaDataList.Item1.Add(areaData);

                    string areaSongPath = Path.Combine(area,Directory.GetFiles(area)[1]);
                    AudioClip audioClip = VorbisPlugin.Load(areaSongPath);
                    areaLevelData.areaDataList.Item2.Add(audioClip);
                }
                Debug.Log("Level "+name+" was loaded!");
                return areaLevelData;
            } else {
                Debug.Log("\""+name+"\" was not found! Check input name and try again.");
                return null;
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static AreaData LoadArea(string filePath)
        {
            AreaData loadedArea = JsonConverter.FromJson<AreaData>(filePath);
            return loadedArea;
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public void AddRoom(AreaData areaData, string roomName, AudioClip song, float BPM)
        {
            List<List<BeatData>> beatList2D = MatrixDataHandler.CreateBeatList2D((int)(song.length / 60 * BPM), 1);
            areaData.roomList.Add(new(roomName, beatList2D));
        }
        //_________________________________________________________________________________
    }
}


