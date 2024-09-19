using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System;

public class LevelEditor : MonoBehaviour
{
    [Header("Music")]
    public AudioClip musicResource;

    public int? BPM = null;

    [Header("Data")]
    [Tooltip("List of level layouts for users to choose from.")]
    [SerializeField] protected LevelList levelList;

    [Tooltip("Input field for users to put the desired level name to when creating a level.")]
    [SerializeField] protected TMP_InputField creationNameInput;

    [Tooltip("Input field for users to put the desired level name to when deleting a level.")]
    [SerializeField] protected TMP_InputField deletionNameInput;

    //Name of the currently edited level. This is referenced in scripts, used in saving files and **should not be changed directly via files**.
    [Tooltip("Index of a level in [levelList] that is currently being edited.")]
    public int levelInEdit;
    [Tooltip("Number of beats in the song which is being edited")]
    public int trackBeatAmount;
    [Tooltip("Maximum number of concurrent instantiated attacks per beat.")]
    public int maxAttackAmount = 1;
    //Name of the level that is being edited (:O)
    protected string EditedLevelName;
    //levelData used to store data and then save it
    protected LevelData levelData;


    //Make another method in spawner script to spawn enemies in a stationary mode.

    [Header("UI (General)")]
    [Range(0,1000)]
    [Tooltip("How much each array node is offset from each other in the editor UI (pixels)")]
    [SerializeField] int UIListElementOffset;

    [Header("UI (BeatList)")]
    [Tooltip("Prefab for an empty node in the UI list")]
    [SerializeField] GameObject UIListNode; 

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 UIListPos;

    [Tooltip("Parent object under which [UIBeatList] will be instantiated.")]
    [SerializeField] Transform UIListParent;

    [Header("UI (Scene preview)")]
    [Tooltip("Preview window for scene editing.")]
    [SerializeField] PreviewWindow previewWindow;


    [Tooltip("List of all the different possible bullets.")]
    [SerializeField] List<GameObject> bulletList = new();

    [Tooltip("Node prefab which will be instantiated in editor.")]
    [SerializeField] GameObject bulletListNode;

    [Tooltip("Where the bullet list will start on screen. Additive to screenCenter.")]
    [SerializeField] Vector2 bulletListPos;

    [Tooltip("Parent under which the bulletListNodes should be instantiated")]
    [SerializeField] Transform bulletListParent;



    [Header("UI (other)")]
    [Tooltip("User input field which will ask users for the song BPM")]
    [SerializeField] TMP_InputField BPMInputField; 

    [Tooltip("The object which will be enabled when showing users the option to either Create, edit or delete levels upon opening editor.")]
    [SerializeField] Transform optionRootParent;

    [Header("Singleton scripts")]
    [SerializeField] private FileDataHandler dataHandler;

    [HideInInspector]
    public string defaultFilePath; 

    // Awake is called once before the first execution of Update after the MonoBehaviour is created regardless of whether or not the instance is active
    void Awake()
    {
        //Give users three options: "Create new level", "Edit a level", "Delete a level"
        optionRootParent.gameObject.SetActive(true);

        defaultFilePath = Path.Combine("Data","Levels");
    }

    void Start()
    {
        dataHandler.CheckForDirectoryChanges();
    }

//Level creation 
    public void RequestLevelCreation()
    {
        StartCoroutine(OnCreateRequest());
    }

    private IEnumerator OnCreateRequest()
    {   
        // Wait until user has put in a level name
        // UpdateName updates editedLevelName value
        EditedLevelName = null; 
        yield return new WaitUntil(()=> EditedLevelName != null);
        creationNameInput.gameObject.SetActive(false);

        BPMInputField.gameObject.SetActive(true);

        BPM = null;
        //  Wait until user has put in a possible BPM and then continue. 
        //  UpdateBPM updates BPM value and checks whether or not what users put into the input field can be used as an integer. Decimal BPM values are not supported. 
        yield return new WaitUntil(()=> BPM != null);
        BPMInputField.gameObject.SetActive(false);

        levelData = dataHandler.CreateLevel();

        RequestLevelEdit(levelData);
    }


//Level editing
    public void RequestLevelEdit(LevelData levelData)
    {
        StartCoroutine(OnEditRequest(levelData));
    }

    private IEnumerator OnEditRequest(LevelData levelData)
    {
        if(levelData == null)
        {
            //Instantiate level options if editor not opened via creation of a new level, yield WaitUntil one is chosen
            //TO BE CONTINUED
        } 

        InstantiateAttackArr();
        UIListParent.gameObject.SetActive(true);

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

//Level deletion
    public void RequestLevelDeletion()
    {
        StartCoroutine(OnDeleteRequest());
    }

    private IEnumerator OnDeleteRequest()
    {
        // Wait until user has put in a level name
        // UpdateName updates editedLevelName value
        EditedLevelName = null;
        yield return new WaitUntil(()=> EditedLevelName != null);
        deletionNameInput.gameObject.SetActive(false);

        dataHandler.DeleteFile(EditedLevelName);
    }

    public void RequestReload()
    {
        dataHandler.CheckForDirectoryChanges();
    }

    public void InstantiateAttackArr()
    {
        //Delete possible previous lists
        DestroyUIList(UIListParent);

        //Create UI element for editing beatMatrix
        List<Transform> UINodePosList = new();

        for(int i = 0; i < trackBeatAmount; i++)
        {
            Vector2 screenCenter = new(Screen.width/2,Screen.height/2);
            var node = Instantiate(UIListNode, UIListPos + screenCenter + new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
            UIListNode nodeData = node.GetComponent<UIListNode>();
            nodeData.levelEditor = this;
            nodeData.indexTag = i;

            node.transform.SetParent(UIListParent);
            UINodePosList.Add(node.transform);
        }
    }

    public void InstantiateBulletTypes()
    {
        DestroyUIList(bulletListParent);

        List<Transform> UINodePosList = new();

        for(int i = 0; i < bulletList.Count; i++)
        {
            Vector2 screenCenter = new(Screen.width/2,Screen.height/2);
            var node = Instantiate(bulletListNode, bulletListPos + screenCenter + new Vector2(0,i*UIListElementOffset),Quaternion.identity);
            UIBulletNode nodeData = node.GetComponent<UIBulletNode>();
            nodeData.levelEditor = this;
            nodeData.indexTag = i;

            node.transform.SetParent(bulletListParent);
            UINodePosList.Add(node.transform);
        }
    }

    public void OpenPreviewWindow()
    {
        UIListParent.gameObject.SetActive(false);

        previewWindow.Open();
        InstantiateBulletTypes();

        bulletListParent.gameObject.SetActive(true);
        previewWindow.gameObject.SetActive(true);
    }

    public void ClosePreviewWindow()
    {
        previewWindow.Close();

        previewWindow.gameObject.SetActive(false);
        UIListParent.gameObject.SetActive(true);
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

    public void UpdateName()
    {
        if(creationNameInput.gameObject.activeSelf)
        {
            EditedLevelName = creationNameInput.text;
        } else if (deletionNameInput.gameObject.activeSelf) {
            EditedLevelName = deletionNameInput.text;
        } else {
            GoBack();
        }
        
    }

    public void DestroyUIList(Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void GoBack()
    {
        StopAllCoroutines();

        if(previewWindow.gameObject.activeSelf)
        {
            previewWindow.Close();
            previewWindow.gameObject.SetActive(false);

            UIListParent.gameObject.SetActive(true);
        } else {
            UIListParent.gameObject.SetActive(false);
            creationNameInput.gameObject.SetActive(false);
            deletionNameInput.gameObject.SetActive(false);
            BPMInputField.gameObject.SetActive(false);

            optionRootParent.gameObject.SetActive(true);
        }
    }
}
