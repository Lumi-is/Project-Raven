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

    [Tooltip("List of level layouts for users to choose from.")]
    [SerializeField] protected LevelList levelList;

    [Tooltip("Input field for users to put the desired level name to.")]
    [SerializeField] protected TMP_InputField nameInputField;

    //Name of the currently edited level. This is referenced in scripts, used in saving files and **should not be changed directly via files**.
    protected string levelName;

    private LevelData levelData;

    [Tooltip("Index of a level in [levelList] that is currently being edited.")]
    public int levelInEdit;

    [Tooltip("Number of beats in the song which is being edited")]
    public int trackBeatAmount;

    [Tooltip("Maximum number of concurrent instantiated attacks per beat.")]
    public int maxAttackAmount = 1;

    //Make another method in spawner script to spawn enemies in a stationary mode.

    [Header("UI (BeatList)")]

    [Tooltip("List of UI elements that represent [beatList]")]
    [SerializeField] private List<Transform> UIBeatList;

    [Tooltip("Prefab for an empty node in the UI list")]
    [SerializeField] GameObject UIListNode; 

    [Tooltip("How much each array node is offset from each other in the editor UI.")]
    [SerializeField] int UIListElementOffset;

    [Tooltip("Position where the UI array starts from.")]
    [SerializeField] Vector2 UIListPos;

    [Tooltip("Parent object under which [UIBeatList] will be instantiated.")]
    [SerializeField] Transform UIListParent;

    [Header("UI (Scene preview)")]

    [Tooltip("Preview window for scene editing.")]
    [SerializeField] GameObject ScenePreviewWindow;

    [Header("UI (other)")]

    [Tooltip("User input field which will ask users for the song BPM")]
    [SerializeField] TMP_InputField BPMInputField; 

    [Tooltip("The object which will be enabled when showing users the option to either Create, edit or delete levels upon opening editor.")]
    [SerializeField] GameObject optionRootParent;


    [Header("Singleton scripts")]
    [SerializeField] private Spawner spawnerScript;
    [SerializeField] private LevelDataHandler levelDataHandler;



    // Awake is called once before the first execution of Update after the MonoBehaviour is created regardless of whether or not the instance is active
    void Awake()
    {
        //Give users three options: "Create new level", "Edit a level", "Delete a level"
        optionRootParent.SetActive(true);
    }



    //Upon clicking "Create a new level" 
    public void RequestLevelCreation()
    {
        StartCoroutine(OnCreateRequest());
    }

    private IEnumerator OnCreateRequest()
    {   
        levelName = null; 

        yield return new WaitUntil(()=> levelName != null);

        nameInputField.gameObject.SetActive(false);
        BPMInputField.gameObject.SetActive(true);

        BPM = null;
        //  Wait until user has put in a possible BPM and then continue. 
        //  UpdateBPM updates BPM value and checks whether or not what users put into the input field can be used as an integer. Decimal BPM values are not supported. 
        yield return new WaitUntil(()=> BPM != null);

        BPMInputField.gameObject.SetActive(false);
        levelData = levelDataHandler.CreateLevel();

        RequestLevelEdit(levelData);
    }



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

        UIBeatList = InstantiateAttackArr();

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
            var node = Instantiate(UIListNode, UIListPos + screenCenter + new Vector2(i*UIListElementOffset, 0), Quaternion.identity);
            UIArrayNode nodeData = node.GetComponent<UIArrayNode>();
            nodeData.levelEditor = this;
            nodeData.indexTag = i;

            node.transform.SetParent(UIListParent);
            UIArrayNodeList.Add(node.transform);
        }
        UIListParent.gameObject.SetActive(true);

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

    public void UpdateName()
    {
        levelName = nameInputField.text;
    }

    public void ClosePreviewWindow()
    {
        ScenePreviewWindow.SetActive(false);
        UIListParent.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        if(ScenePreviewWindow.activeSelf)
        {
            ScenePreviewWindow.SetActive(false);
            UIListParent.gameObject.SetActive(true);
            
        } else if(UIListParent.gameObject.activeSelf) {

            UIListParent.gameObject.SetActive(false);
            DestroyUIArray();
            optionRootParent.SetActive(true);
        } else {
            optionRootParent.SetActive(true);
            ScenePreviewWindow.SetActive(false);
            UIListParent.gameObject.SetActive(false);
            DestroyUIArray();
            BPMInputField.gameObject.SetActive(false);
        }
    }

    public void DestroyUIArray()
    {
        foreach(Transform child in UIListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
