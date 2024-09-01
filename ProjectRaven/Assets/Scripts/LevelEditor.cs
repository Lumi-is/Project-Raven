using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [Header("Music")]
    public AudioClip musicResource;

    public int? BPM = null;

    [Header("Data")]
    [SerializeField] protected LevelList levelListScript;
    [Tooltip("Level that is currently being edited.")]
    public int editedLevel;

    [SerializeField] private LevelDataHandler levelDataHandler;
    [SerializeField] private int maxAttackAmount;

    [Header("UI")]

    [Tooltip("Preset for an empty node in the array.")]
    [SerializeField] GameObject UIArrayNodePrefab; 

    [Tooltip("How much each array node is offset from each other in the editor UI.")]
    [SerializeField] int UIListElementOffset;

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 defaultUIArrayPosition;
    [SerializeField] Transform UIArrayParent;

    [Tooltip("The object which will be enabled when showing users the option to either Create, edit or delete levels upon opening editor.")]
    [SerializeField] GameObject mainLevelEditOptionParent;
    [SerializeField] TMP_InputField BPMInputField; 

    [Tooltip("List of level layouts for users to choose from.")]
    [SerializeField] List<LevelLayout> levelLayoutList = new();

    [SerializeField] int? chosenLayout;

    [SerializeField] GameObject[] AttackerPrefabList;

    [Header("UI Integrated Scene preview")]
    [SerializeField] GameObject ScenePreviewWindow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //If scene is not leveleditor return; // Do not present options if not in leveleditor. //

        /*Give users three options: "Create new level", "Edit a level", "Delete a level"*/
        mainLevelEditOptionParent.SetActive(true);
    }

    private protected int XData;
    private protected int YData;

    private int localTrackBeatAmount;

    private List<Transform> UIArrayNodeList;
    private LevelData editedLevelData;

    //If pressed "Create new level" 
    public void RequestLevelCreation()
    {
        //If request is accepted
        //Add terms
        StartCoroutine(OnCreateRequest());
    }
    private IEnumerator OnCreateRequest()
    {
        //  Set parent for main options as false after choosing a primary function (create, edit or delete)
        mainLevelEditOptionParent.SetActive(false);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //  Ask user for song // Use a default song for testing purposes, but in final build use the WindowsDragAndDrop scripts
        //  musicResource = dragAndDropData;

        //  All 4 lines below are used to ask user for BPM:
        //  Show input field for BPM
        BPMInputField.gameObject.SetActive(true);    
        //  Reset to null in case a new level is being created
        BPM = null;
        //  Wait until user has put in a possible BPM and then continue. 
        //  UpdateBPM updates BPM and checks whether or not what users put into the input field can be used as an integer 
        yield return new WaitUntil(()=> BPM != null);
        //  Turn off UI for BPM input field if user has selected a BPM.
        BPMInputField.gameObject.SetActive(false);

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        //  Present user with options for the level layout (1 of n). 
        //  !! For now no functionality for custom layouts !! 
        foreach (LevelLayout levelLayout in levelLayoutList)
        {
            levelLayout.gameObject.SetActive(true);
        }
        //  ^ TO BE CONTINUED ^

        //  Reset to null in case a new level is being created
        chosenLayout = null;
        // Wait until a layout is chosen by the user. This is done via UI element's void "OnClick()"
        yield return new WaitUntil(()=> chosenLayout != null);

        foreach (LevelLayout levelLayout in levelLayoutList)
        {
            levelLayout.gameObject.SetActive(false);
        }
        //Create level layout to the scene
        LevelLayout currentLevelLayout = levelLayoutList[(int)chosenLayout];

        //Make a new instance of leveldata and add it to levelList, return it for editing purposes
        editedLevelData = CreateLevel();        

        //Create and show user the attack array - return it for editing purposes
        UIArrayNodeList = InstantiateAttackArr(); // Instantiate it under an object

        //Upon user pressing one of the buttons instantiated, get index data from button and get correct data from matching index according to it from LevelData attackArray
        
        //Go to edit mode

        ////

        //For-loop to go through the number list of attacks in the list, instantiate each one as a preview to the leveleditor scene

        //SetActive false the grid parent which would show all the attacks

        //SetActive a popup which has a render texture to a camera 
        //SetActive a UIPanel behind the texture

        //Get a number of different enemy prefabs and instantiate a list of buttons on top of each other - each button should have the following functionality:
        //Make a enemybuttonmanager script which holds a list of button indexes matching to enemy types - use a dictionary

        //Upon clicking, spawn enemy type in a ScreenToWorldPoint and set its transform as ScreenToWorldPoint until mouse button is released
        //If enemy is not touching a valid surface (not inside a hitbox), delete it 
        
        //If enemy is on correct surface: 
        //Save enemy type and position to LevelData 
        
        //Upon pressing an exit button, close the overlay and re-enable the grid parent object

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public LevelData CreateLevel()
    {
        //Calculate how many beats in a song
        levelDataHandler.CalulateBeatAmount();
        localTrackBeatAmount = levelDataHandler.trackBeatAmount;

        //Set variables to be used in creating the array
        XData = localTrackBeatAmount;
        YData = maxAttackAmount;

        //Creating the array
        levelDataHandler.CreateArray();
        //Print the array
        levelDataHandler.PrintArray();

        LevelData levelData = levelDataHandler.SaveData();
        levelListScript.AddLevel(levelData);
        editedLevel = levelListScript.levelListData.Count - 1;

        return levelData;
    }

    public List<Transform> InstantiateAttackArr()
    {
        //Create UI element for editing attackArray
        List<Transform> UIArrayNodeList = new();

        for(int i = 0; i < localTrackBeatAmount; i++)
        {
            Vector2 screenCenter = new(Screen.width/2,Screen.height/2);
            var node = Instantiate(UIArrayNodePrefab, defaultUIArrayPosition + screenCenter + new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
            UIArrayNode nodeData = node.GetComponent<UIArrayNode>();
            nodeData.indexTag = i;

            node.transform.SetParent(UIArrayParent);
            UIArrayNodeList.Add(node.transform);
        }
        UIArrayParent.gameObject.SetActive(true);

        return UIArrayNodeList;
    }

    public void RequestLevelEdit()
    {
        StartCoroutine(OnEditRequest());
    }

    private IEnumerator OnEditRequest()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //If pressed "Edit a level" 
        //Instantiate a list of levels (exclude all levels which are tagged non-custom)
        
        //Upon selecting a level to edit - change "editedlevel" to match index of said level
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

        return null;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    private IEnumerator OnDeleteRequest()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //If pressed "Delete a level" 

        //Instantiate a list of levels (exclude all levels which are tagged non-custom)
        //Upon selecting a level to edit
        //Delete level from levelList

        //Go back to the three main options

        return null;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public void UpdateBPM()
    {
        if(int.TryParse(BPMInputField.text, out int value))
        {
            BPM = value;
        } else {
            BPM = null;
            Debug.LogWarning("Incorrect type detected in BPM input field. Try again.");
        }
    }

    public void UpdateLevelLayout(int layoutIndex)
    {
        chosenLayout = layoutIndex;
    }

    public void OpenPreviewWindow(int BeatToEdit)
    {
        UIArrayParent.gameObject.SetActive(false);
        ScenePreviewWindow.SetActive(true);
        //TO BE CONTINUED
        //for loop to go through the beatDataArr at x point beatToEdit, instantiate all said items to a preview
    }

    public void GoBack()
    {
        if(ScenePreviewWindow.activeSelf)
        {
            ScenePreviewWindow.SetActive(false);
            UIArrayParent.gameObject.SetActive(true);
            return;
        } else if(UIArrayParent.gameObject.activeSelf)
        {
            UIArrayParent.gameObject.SetActive(false);

            foreach(Transform child in UIArrayParent)
            {
                Destroy(child.gameObject);
            }
            mainLevelEditOptionParent.SetActive(true);
        }
    }
}
