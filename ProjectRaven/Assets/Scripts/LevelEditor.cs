using System;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [Header("Music")]
    public AudioClip musicResource;

    public int BPM;

    [Header("Data")]
    [SerializeField] LevelList levelList;

    [SerializeField] GameObject UIArrayNodePrefab; 

    [Tooltip("How much each array node is offset from each other in the editor UI.")]
    [SerializeField] int UIListElementOffset;

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 defaultUISpawnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //If scene is not leveleditor return; // Do not present options if not in leveleditor. //

        /*Give users three options: "Create new level", "Edit a level", "Delete a level"*/

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //If pressed "Create new level" 

        //Ask user for song // Use a default song for testing purposes, but in final build use the WindowsDragAndDrop scripts
        //Ask user for BPM
        //Present user with options for the level layout (1 of n)

        //Create level layout to the scene
        //CreateLevel();
        //InstantiateAttackArr(); - Instantiate it under an object

        //Upon user pressing one of the buttons instantiated, get index data from button and get correct data from matching index according to it from LevelData attackArray
        //For-loop to go through the number list of attacks in the list, instantiate each one as a preview to the leveleditor scene

        //SetActive a popup which has a render texture to a camera 
        //SetActive false the grid parent which would show all the attacks
        //SetActive a UIPanel behind the texture
        //Get a number of different enemy prefabs and instantiate a list of buttons on top of each other - each button should have the following functionality:
        //Make a enemybuttonmanager script which holds a list of button indexes matching to enemy types - use a dictionary
        //Upon clicking, spawn enemy type in a ScreenToWorldPoint and set its transform as ScreenToWorldPoint until mouse button is released
        //If enemy is not touching a valid surface (not inside a hitbox), delete it 
        
        //If enemy is on correct surface: 
        //Save enemy type and position to LevelData 
        
        //Upon pressing an exit button, close the overlay and re-enable the grid parent object

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //If pressed "Edit a level" 
        //Instantiate a list of levels (exclude all levels which are tagged non-custom)
        
        //Upon selecting a level to edit
        //InstantiateAttackArr(); - Instantiate it under an object

        //Upon user pressing one of the buttons instantiated, get index data from button and get correct data from matching index according to it from LevelData attackArray
        //For-loop to go through the number list of attacks in the list, instantiate each one as a preview to the leveleditor scene

        //SetActive a popup which has a render texture to a camera 
        //SetActive false the grid parent which would show all the attacks
        //SetActive a UIPanel behind the texture
        //Get a number of different enemy prefabs and instantiate a list of buttons on top of each other - each button should have the following functionality:
        //Make a enemybuttonmanager script which holds a list of button indexes matching to enemy types - use a dictionary
        //Upon clicking, spawn enemy type in a ScreenToWorldPoint and set its transform as ScreenToWorldPoint until mouse button is released
        //If enemy is not touching a valid surface (not inside a hitbox), delete it 
        
        //If enemy is on correct surface: 
        //Save enemy type and position to LevelData 
        
        //Upon pressing an exit button, close the overlay and re-enable the grid parent object

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //If pressed "Delete a level" 

        //Instantiate a list of levels (exclude all levels which are tagged non-custom)
        //Upon selecting a level to edit
        //Delete level from levelList

        //Go back to the three main options

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    protected int XData;
    protected int YData;

    private int localTrackBeatAmount;

    public void CreateLevel()
    {
        LevelData levelData = new();
        levelData.CalulateBeatAmount();
        localTrackBeatAmount = levelData.trackBeatAmount;
        levelData.CreateArray();
        //Debug
        levelData.PrintArray();

        levelList.AddLevel(levelData);
    }

    public void InstantiateAttackArr()
    {
        //Create UI element for editing attackArray

        for(int i = 0; i < localTrackBeatAmount; i++)
        {
            Instantiate(UIArrayNodePrefab, defaultUISpawnPosition+new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
        }
    }
}
