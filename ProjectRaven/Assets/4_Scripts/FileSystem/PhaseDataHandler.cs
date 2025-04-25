using UnityEngine;
using System;
using System.IO;

public class PhaseDataHandler : JsonConverter, IDataHandler
{
    //When wanting to create a directory for a level in which the phases will be stored
    public void CreateDirectory(string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
    }
    //_________________________________________________________________________________



    //When wanting to save a phase to the level directory
    public void SaveFile<T>(T storableData, string filePath)
    {
        try {
            if(File.Exists(filePath))
            {
                OverwriteFile(storableData, filePath);
            } else {
                CreateFile(storableData, filePath);
            }
        } catch (Exception e) {
            Debug.LogError("Error occured when trying to save data to file: '" + filePath + "' - Error at: " + e);
        }
    }
    //_________________________________________________________________________________


    
    //_________________________________________________________________________________    
    public void OverwriteFile<T>(T storableData, string filePath)
    {
        DeleteFile(filePath);
        SaveFile(storableData, filePath);
    }
    //_________________________________________________________________________________



    //_________________________________________________________________________________
    public void DeleteFile(string filePath)
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


    
    //Exclusive for creation of .json based level files
    public void CreateFile<T>(T storableData, string filePath)
    {
        using FileStream stream = new(filePath, FileMode.Create);
        using StreamWriter writer = new(stream);
        writer.Write(ToJson(storableData));
    }
    //_________________________________________________________________________________



    //Exclusive for loading of .json based level files
    public T LoadFile<T>(string filePath)
    {
        return FromJson<T>(filePath);
    } 
    //_________________________________________________________________________________
}
