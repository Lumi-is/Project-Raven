using System;
using System.Collections;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [Header("Music")]
    public AudioClip musicResource;

    private nullable<int> BPM = new();

    [Header("Data")]
    [SerializeField] LevelList levelList;
    [SerializeField] int maxAttackAmount;

    [Header("UI")]
    [SerializeField] GameObject UIArrayNodePrefab; 

    [Tooltip("How much each array node is offset from each other in the editor UI.")]
    [SerializeField] int UIListElementOffset;

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 defaultUIArrayPosition;
    [SerializeField] Transform UIArrayParent;

    [Tooltip("The object which will be enabled when showing users the option to either Create, edit or delete levels upon opening editor.")]
    [SerializeField] GameObject mainLevelEditOptionParent;
    [SerializeField] InputField BPMInputField; 

    [Tooltip("List of level layouts for users to choose from.")]
    [SerializeField] List<LevelLayout> levelLayoutList = new();

    [SerializeField] int chosenLayout;

    [SerializeField] GameObject AttackerPrefabList;

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

    protected int XData;
    protected int YData;

    private int localTrackBeatAmount;

    private List<Transform> UIArrayNodeList;

    private LevelData currentLevelData;


    //If pressed "Create new level" 
    private IEnumerator OnCreateRequest()
    {
        mainLevelEditOptionParent.SetActive(false);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Ask user for song // Use a default song for testing purposes, but in final build use the WindowsDragAndDrop scripts
        //musicResource = dragAndDropData;
        //Ask user for BPM
        BPMInputField.gameObject.SetActive(true);
        //Wait until user has put in a possible BPM and then continue
        yield return new WaitUntil(()=> BPM != null);
        BPMInputField.gameObject.SetActive(false);

        //Present user with options for the level layout (1 of n)
        for(int i = 0; i<levelLayoutList.Count; i++)
        {
            levelLayoutList[i].gameObject.SetActive(true);
        }
        //Create level layout to the scene
        LevelLayout currentLevelLayout = levelLayoutList[chosenLayout];
        //Create the level from parameters
        XData = musicResource.length/60 * BPM;
        YData = maxAttackAmount; 
        //Make a new instance of leveldata and add it to levelList, return it for editing purposes
        currentLevelData = CreateLevel();        

        //Create and show user the attack array - return it for editing purposes
        UIArrayNodeList = InstantiateAttackArr(); // Instantiate it under an object

        //Upon user pressing one of the buttons instantiated, get index data from button and get correct data from matching index according to it from LevelData attackArray
        
        ////

        //For-loop to go through the number list of attacks in the list, instantiate each one as a preview to the leveleditor scene

        for(int i = 0; i < currentLevelData.beatDataArr[beatToEdit].Count; i++)
        {
            //Instantiate attack previews
        }

        //SetActive false the grid parent which would show all the attacks
        UIArrayParent.SetActive(false);

        //SetActive a popup which has a render texture to a camera 
        //SetActive a UIPanel behind the texture
        ScenePreviewWindow.SetActive(true);

        //Get a number of different enemy prefabs and instantiate a list of buttons on top of each other - each button should have the following functionality:
        //Make a enemybuttonmanager script which holds a list of button indexes matching to enemy types - use a dictionary
        for(int i = 0; i < AttackerPrefabList.Length; i++)
        {
            //Instantiate attacker buttons
        }

        //Upon clicking, spawn enemy type in a ScreenToWorldPoint and set its transform as ScreenToWorldPoint until mouse button is released
        //If enemy is not touching a valid surface (not inside a hitbox), delete it 
        
        //If enemy is on correct surface: 
        //Save enemy type and position to LevelData 
        
        //Upon pressing an exit button, close the overlay and re-enable the grid parent object

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    private IEnumerator OnEditRequest()
    {
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

    }

    private IEnumerator OnDeleteRequest()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //If pressed "Delete a level" 

        //Instantiate a list of levels (exclude all levels which are tagged non-custom)
        //Upon selecting a level to edit
        //Delete level from levelList

        //Go back to the three main options

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public LevelData CreateLevel()
    {
        LevelData levelData = new();
        levelData.CalulateBeatAmount();
        localTrackBeatAmount = levelData.trackBeatAmount;
        levelData.CreateArray();
        //Debug
        levelData.PrintArray();

        levelList.AddLevel(levelData);
        return levelData;
    }

    public List<Transform> InstantiateAttackArr()
    {
        //Create UI element for editing attackArray
        List<Transform> UIArrayNodeList = new();

        for(int i = 0; i < localTrackBeatAmount; i++)
        {
            var UIArrayNode = Instantiate(UIArrayNodePrefab, defaultUIArrayPosition+new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
            UIArrayNode.transform.parent = UIArrayParent;
            UIArrayNodeList.Add(UIArrayNode);
        }

        return UIArrayNodeList;
    }
}
