using Tools;
using System.Collections.Generic;
using UnityEngine;
using OggVorbis;

namespace System.IO.DataHandling{
    public  class PhaseDataHandler
    {  
        //Exclusive for creation of .json based level files
        public static void CreatePhaseToJson<T>(T storableData, string filePath)
        {
            using FileStream stream = new(filePath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(JsonConverter.ToJson(storableData));
        }
        //_________________________________________________________________________________


        //_________________________________________________________________________________
        public static PhaseData LoadPhase(string filePath)
        {
            PhaseData loadedPhase = JsonConverter.FromJson<PhaseData>(filePath);
            return loadedPhase;
        }
        //_________________________________________________________________________________

        public static AudioClip LoadPhaseAudio(string filePath)
        {
            AudioClip loadedAudio = VorbisPlugin.Load(filePath);
            return loadedAudio;
        }



        //_________________________________________________________________________________
        public static List<List<BeatData>> CreateBeatList2D(int amountOfCellsX, int amountOfCellsY) 
        {
            // Make a new 2D list with only one object using beatData constructor
            List<BeatData> beatList = new(){
                new()
            };
            List<List<BeatData>> beatList2D = new(){
                beatList
            };
            //_________________________________________________________________________________
            
            beatList2D = AppendListByX(beatList2D, amountOfCellsX-1);
            beatList2D = AppendListByY(beatList2D, amountOfCellsY-1);

            return beatList2D;
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static List<List<BeatData>> AppendListByX(List<List<BeatData>> beatList2D, int amountOfTimes)
        {
            // Creates a one dimensional column which is then added to the 2D list

            // Creates a new empty list
            List<BeatData> beatList = new();

            // Creates a one dimensional column beatList and extends it to the same length as other columns in beatList2D
            for(int i = 0; i < beatList2D[0].Count; i++) //beatList2D is empty??
            {
                beatList.Add(new());
            }

            // Adds number of the instantiated beatLists to beatList2D according to amountOfTimes 
            for(int i = 0; i < amountOfTimes; i++)
            {
                beatList2D.Add(beatList);
            }

            return beatList2D;
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________

        public static List<List<BeatData>> AppendListByY(List<List<BeatData>> beatList2D, int amountOfTimes)
        {
            // Creates a one dimensional row which is then added to the 2D list
            /*  Aka enumerates through the list of items by X, 
                and adds an item to every column according to 
                variable amountOfTimes*/

            for(int i = 0; i < beatList2D.Count; i++)
            {
                for(int j = 0; j < amountOfTimes; j++)
                {
                    beatList2D[i].Add(new());
                }
            }

            return beatList2D;
        }
        //_________________________________________________________________________________
    }
}

