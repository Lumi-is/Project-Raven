using Tools;
using UnityEngine;

namespace System.IO.DataHandling{
    public static class GlobalDataHandler
    {
        //_________________________________________________________________________________
        public static void CreateDirectory(string filePath)
        {
            Directory.CreateDirectory(filePath);
        }
        //_________________________________________________________________________________


        //When wanting to save a phase to the level directory
        public static void SaveFile<T>(T storableData, string filePath, bool asJson)
        {
            try {
                if(File.Exists(filePath))
                {
                    OverwriteFile(storableData, filePath, asJson);
                } else {
                    if(asJson)
                    {
                        PhaseDataHandler.CreatePhaseToJson(storableData, filePath);
                    } else {
                        CreateFile(storableData, filePath);
                    }
                    
                }
            } catch (Exception e) {
                Debug.LogError("Error occured when trying to save data to file: '" + filePath + "' - Error at: " + e);
            }
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static void CreateFile<T>(T storableData, string filePath)
        {
            using FileStream stream = new(filePath, FileMode.Create);
            using StreamWriter writer = new(stream);
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static void DeleteFile(string filePath)
        {
            try{
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                } else {
                    Debug.LogWarning("Data from path '"+filePath+"' could not be found. Check the file name and try again.");
                }
            } catch (Exception e){
                Debug.LogError("Failed to delete data '"+filePath+"'. - Error at: "+e);
            }
        }
        //_________________________________________________________________________________

        //_________________________________________________________________________________    
        public static void OverwriteFile<T>(T storableData, string filePath, bool asJson)
        {
            DeleteFile(filePath);
            SaveFile(storableData, filePath, asJson);
        }
        //_________________________________________________________________________________



        //_________________________________________________________________________________
        public static T LoadFile<T>(string filePath)
        {
            return JsonConverter.FromJson<T>(filePath);
        } 
        //_________________________________________________________________________________
    }
}

