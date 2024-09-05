using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour
{
    [Header("Music")]
    public AudioClip musicResource;

    public int? BPM = null;

    [Header("Data")]
    [SerializeField] protected LevelList levelList;
    [Tooltip("Level that is currently being edited.")]
    public int editedLevelIndex;

    [SerializeField] private LevelDataHandler levelDataHandler;
    [SerializeField] private int maxAttackAmount;
    [SerializeField] private Spawner spawnerScript;
    //Make another method in spawner script to spawn enemies in a stationary mode.

    [Header("UI")]

    [Tooltip("Preset for an empty node in the array.")]
    [SerializeField] GameObject UIArrayNodePrefab; 

    [Tooltip("How much each array node is offset from each other in the editor UI.")]
    [SerializeField] int UIListElementOffset;

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 defaultUIArrayPosition;
    [SerializeField] Transform UIArrayParent;
    public List<BeatData> beatDataArr = new();

    [Tooltip("The object which will be enabled when showing users the option to either Create, edit or delete levels upon opening editor.")]
    [SerializeField] GameObject mainLevelEditOptionParent;
    [SerializeField] TMP_InputField BPMInputField; 

    [Tooltip("List of level layouts for users to choose from.")]

    [SerializeField] GameObject[] AttackerPrefabList;

    [Header("UI Integrated Scene preview")]
    [SerializeField] GameObject ScenePreviewWindow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //If scene is not leveleditor return; // Do not present options if not in leveleditor. //

        /*Give users three options: "Create new level", "Edit a level", "Delete a level"*/
        mainLevelEditOptionParent.SetActive(true);
    }

    private protected int XData;
    private protected int YData;

    public int trackBeatAmount;

    private List<Transform> UIArrayNodeList;
    private LevelData levelData;

    public void RequestLevelCreation()
    {
        StartCoroutine(OnCreateRequest());
    }
    private IEnumerator OnCreateRequest()
    {
        mainLevelEditOptionParent.SetActive(false);
        BPMInputField.gameObject.SetActive(true);    
        BPM = null;
        //  Wait until user has put in a possible BPM and then continue. 
        //  UpdateBPM updates BPM and checks whether or not what users put into the input field can be used as an integer 
        yield return new WaitUntil(()=> BPM != null);

        BPMInputField.gameObject.SetActive(false);

        levelData = CreateLevel();

        RequestLevelEdit(levelData);        

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    public LevelData CreateLevel()
    {
        //Calculate how many beats in a song
        CalculateBeatAmount();

        //Set variables to be used in creating the array
        XData = trackBeatAmount;
        YData = maxAttackAmount;

        //Creating the array
        levelDataHandler.CreateArray();

        LevelData levelData = levelDataHandler.CreateLevel();
        levelList.AddLevel(levelData);

        return levelData;
    }

    public void CalculateBeatAmount()
    {
        trackBeatAmount = (int)(musicResource.length/60 * BPM);
    }

    public void RequestLevelEdit(LevelData levelData)
    {
        StartCoroutine(OnEditRequest(levelData));
    }

    private IEnumerator OnEditRequest(LevelData levelData)
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if(levelData == null)
        {
            //Instantiate level options if editor not opened via creation of a new level, yield WaitUntil one is chosen
            //TO BE CONTINUED
        } 

        UIArrayNodeList = InstantiateAttackArr();

        //Goes to OpenPreviewWindow method via UI element on click:
        //Upon user pressing one of the buttons instantiated, get index data from button and get correct data from matching index according to it from LevelData attackArray
        //SetActive a popup which has a render texture to a camera 
        //SetActive false the grid parent which would show all the attacks
        //SetActive a UIPanel behind the texture
        //For-loop to go through the number list of attacks in the list, instantiate each one as a preview to the leveleditor scene

        //Get a number of different enemy prefabs and instantiate a list of buttons on top of each other - each button should have the following functionality:
        //Make a enemybuttonmanager script which holds a list of button indexes matching to enemy types - use a dictionary
        //Upon clicking, spawn enemy type in a ScreenToWorldPoint and set its transform as ScreenToWorldPoint until mouse button is released
        //If enemy is not touching a valid surface (not inside a hitbox), delete it 
        
        //If enemy is on correct surface: 
        //Save enemy type and position to LevelData 
        
        //Upon pressing an exit button, close the overlay and re-enable the grid parent object

        yield return null;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    public List<Transform> InstantiateAttackArr()
    {
        //Create UI element for editing attackArray
        List<Transform> UIArrayNodeList = new();

        for(int i = 0; i < trackBeatAmount; i++)
        {
            Vector2 screenCenter = new(Screen.width/2,Screen.height/2);
            var node = Instantiate(UIArrayNodePrefab, defaultUIArrayPosition + screenCenter + new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
            UIArrayNode nodeData = node.GetComponent<UIArrayNode>();
            nodeData.levelEditor = this;
            nodeData.indexTag = i;

            node.transform.SetParent(UIArrayParent);
            UIArrayNodeList.Add(node.transform);
        }
        UIArrayParent.gameObject.SetActive(true);

        return UIArrayNodeList;
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

    public void ClosePreviewWindow()
    {
        ScenePreviewWindow.SetActive(false);
        UIArrayParent.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        if(ScenePreviewWindow.activeSelf)
        {
            ScenePreviewWindow.SetActive(false);
            UIArrayParent.gameObject.SetActive(true);
            
        } else if(UIArrayParent.gameObject.activeSelf) {

            UIArrayParent.gameObject.SetActive(false);
            DestroyUIArray();
            mainLevelEditOptionParent.SetActive(true);
        } else {
            mainLevelEditOptionParent.SetActive(true);
            ScenePreviewWindow.SetActive(false);
            UIArrayParent.gameObject.SetActive(false);
            DestroyUIArray();
            BPMInputField.gameObject.SetActive(false);
        }
    }

    public void DestroyUIArray()
    {
        foreach(Transform child in UIArrayParent)
        {
            Destroy(child.gameObject);
        }
    }
}
