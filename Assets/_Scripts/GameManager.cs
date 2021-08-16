using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


// singelton 
// tasks: 
public class GameManager : MonoBehaviour
{
    #region Serialized Parameters
    [Header("Menu Objects")]
    [Tooltip("Add Menu Objects for UIManager")]
    public GameObject GeneralSettingsMenu;
    public GameObject NewSettingsMenu;
    public GameObject OldSettingsMenu;
    public GameObject PauseMenu;
    public GameObject ContinueWithLocationsButton;

    [Header("InteractionObjects")]
    public GameObject parentPlayTable;
    public GameObject parentSideTable;

    [Header("Spawn Points")]
    public Transform spawnPointGame;
    public Transform spawnPointSide;

    [Header("BoundingBox")]
    public Material BoundingBox;
    public Material BoundingBoxGrabbed;
    public Material BoundingBoxHandleWhite;
    public Material BoundingBoxHandleBlueGrabbed;
    public GameObject BoundingBox_RotateHandle;

    [Header("Button")]
    public float ReactivationTimeUserButton = 10.0f;
    public GameObject UserButton; 

    [Header("Debug")]
    public GameObject debugTextObject;
    [NonSerialized]
    public TextMeshPro debugText;

    #endregion Serialized in Editor

    #region FileParameters
    public ApplicationData GeneralSettings { 
        get => generalSettings;
        set 
        { 
            generalSettings = value;
            DataManager.Instance.CompleteUserData = DataFile.LoadUserSets(generalSettings.completeUserData);
            DataManager.Instance.IncompleteUserData = DataFile.LoadUserSets(generalSettings.incompleteUserData);
            DataManager.Instance.NewUserData = DataFile.LoadUserSets(generalSettings.newUserData);
        }
    }

    [NonSerialized]
    private ApplicationData generalSettings;
    [NonSerialized]
    public string mainFolder;
    private float updateRate;
    public float UpdateRate { get => updateRate; set => updateRate = value; }


    #endregion FileParameters

    #region Event Parameters
    // Events
    [NonSerialized]
    public UnityEvent OnUserButtonClicked;
    private bool buttonEnabled; 

    #endregion Event Parameters

    #region Game Flow Parameters
    // game 
    [NonSerialized]
    public GameType gameType; 
    #endregion Game Flow Parameters

    #region Parameters Script Managerment
    // script management
    public List<Type> AttachedManagerScripts { get => attachedManagerScripts; set => attachedManagerScripts = value; }
    public List<SubManager> AttachedSubManagers { get => attachedSubManagers; set => attachedSubManagers = value; }
    #endregion Parameters ScriptManagement

    #region private Parameters
    // Managers
    private List <Type> attachedManagerScripts;
    private List<SubManager> attachedSubManagers;

    // General Settings
    private string applicationFolder;
    private string settingsFolder;
    private ApplicationData settings;

    #endregion private Parameters

    #region instance and awake
    private static GameManager _instance = null;
    public static GameManager Instance { get => _instance; }


    /// <summary>
    /// Manage Instance and Add depending Scripts
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Instance of GameManager destroyed.");
        }
    }
    #endregion 

    void Start()
    {
        buttonEnabled = true; 

        // debug
        debugText = debugTextObject.GetComponent<TextMeshPro>();

        // Check Input parameters
        if (GeneralSettingsMenu == null || NewSettingsMenu == null || OldSettingsMenu == null || PauseMenu == null)
        {
            debugText.text = "Not all Menu Parameters are Set in GameManager."; 
            Debug.LogError("Not all Menu Parameters are Set in GameManager.");
        }
        // Events
        if (OnUserButtonClicked == null)
            OnUserButtonClicked = new UnityEvent();

        mainFolder = "DataFiles";
        updateRate = 1.0f; 

        
        generalSettings = DataFile.SecureLoad<ApplicationData>(Path.Combine(mainFolder, "generalSettings"));

        // Add Managers of Type Monobehaviour
        attachedManagerScripts = new List<Type>();
        AddManagerToScene(typeof(GameStateManager));
        AddManagerToScene(typeof(DataManager));

        //Add SubManager of Type SubManager
        attachedSubManagers = new List<SubManager>();
        AddSubManager(new AudioManager());
        AddSubManager(new UIManager());
        AddSubManager(new ObjectManager());


        ResetToDefault();
    }


    #region buttons

    /// <summary>
    /// Add to Toggle Buttons, to get their input
    /// </summary>
    public void ToggleGameType()
    {
        // toggle game type
        if (gameType == GameType.Locations)
            gameType = GameType.Prices;
        else if (gameType == GameType.Prices)
            gameType = GameType.Locations;
    }

    /// <summary>
    /// Called in Old Settings menu on radio buttons
    /// And on Pause menu at button ContinueWithLocations
    /// Sets Game Type and Assigns Listeners to UserButton
    /// </summary>
    /// <param name="type"></param>
    public void SetGameType(string type)
    {
        if(type == "Locations")
            gameType = GameType.Locations;
        else if (type == "Prices")
            gameType = GameType.Prices; 
        else
            throw new ArgumentException("...GameManager SetGameType to " + type + " not possible."); 
        
       
    }

    /// <summary>
    /// Called after SetGameType to add Listeners to UserButton
    /// The split is necessary to prevent wrong behaviour 
    /// </summary>
    public void StartGame()
    {
        OnUserButtonClicked.RemoveAllListeners();
        OnUserButtonClicked.AddListener(() => GameStateManager.Instance.StartTestRun(GameManager.Instance.gameType));
    }

    /// <summary>
    /// Helper function for the interaction with UserButton
    /// </summary>
    public void UserButtonClicked()
    {
        if (buttonEnabled)
        {
            OnUserButtonClicked.Invoke();
        }
         
         // deactivate user button for some seconds
        UserButton.GetComponent<Interactable>().IsEnabled = false;
        UserButton.GetComponent<PressableButton>().enabled = false;
        UserButton.GetComponent<Interactable>().SetState(InteractableStates.InteractableStateEnum.Pressed, true); 
        buttonEnabled = false;

        StartCoroutine(EnableAfterSeconds()); 
    }

    /// <summary>
    /// Called in user Button Clicked to re enable user button 
    /// </summary>
    IEnumerator EnableAfterSeconds()
    {
        yield return new WaitForSeconds(ReactivationTimeUserButton);

        UserButton.GetComponent<Interactable>().IsEnabled = true;
        UserButton.GetComponent<PressableButton>().enabled = true;
        UserButton.GetComponent<Interactable>().SetState(InteractableStates.InteractableStateEnum.Pressed, false);

        buttonEnabled = true; 
    }

    /// <summary>
    /// Called in Hand Menu to set rotation to zero
    /// </summary>
    public void ResetObjectRotation()
    {
        DataManager.Instance.ResetObjectRotation(); 
    }

    public void ResetMenuValues()
    {
        ResetToDefault(); 
    }
    #endregion buttons

    #region Script Management

    /// <summary>
    /// Managers of are added to the gameObject and collected in the list
    /// </summary>
    /// <param name="classType">Inherited type of Monobehaviour is required. Use with typeof().</param>
    private void AddManagerToScene(Type classType)
    {
        if (this.gameObject.GetComponent(classType) == null)
        {
            this.gameObject.AddComponent(classType);
            attachedManagerScripts.Add(classType);
        }
        else
        {
            debugText.text = "GameManager::AddManagerToScene Manager already exists!";
            Debug.LogWarning("GameManager::AddManagerToScene Manager already exists!"); 
        }
    }
    /// <summary>
    /// Managers of type SubManager are created and collected in the list
    /// </summary>
    /// <param name="newSubManager"></param>
    private void AddSubManager(SubManager newSubManager)
    { 
        if(newSubManager != null)
        {
            attachedSubManagers.Add(newSubManager);
        }
        else
        {
            debugText.text = "GameManager::AddSubManager Failed to load SubManager"; 
            Debug.LogError("GameManager::AddSubManager Failed to load SubManager"); 
        }
    }

    #endregion Script Management

    #region Game Flow

    /// <summary>
    /// Called in Pause UI. Reset attached scripts
    /// </summary>
    public void NewUser()
    {
        ResetToDefault();
    }

    private void ResetToDefault()
    {

        //\TODO in foreach
        gameObject.GetComponent<GameStateManager>().ResetToDefault();
        gameObject.GetComponent<DataManager>().ResetToDefault();

        foreach (SubManager sub in attachedSubManagers)
        {
            sub.Reset();
        }

        OnUserButtonClicked.RemoveAllListeners();

        // game
        gameType = GameType.Prices;
    }

    /// <summary>
    /// Called in General Menu on Quit Game Button 
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); 
    }

    #endregion Game Flow 

    #region filemanagement
    public void UpdateGeneralSettings(string userID, GameType completedType)
    {
        if (completedType == GameType.Locations)
        {
            generalSettings.incompleteUserData.Remove("User" + userID + "/" + "user" + userID);
            generalSettings.completeUserData.Add("User" + userID + "/" + "user" + userID); 
        }
        else if(completedType == GameType.Prices)
        {
            generalSettings.newUserData.Remove("User" + userID + "/" + "user" + userID); 
            generalSettings.incompleteUserData.Add("User" + userID + "/" + "user" + userID); 
        }
        else
        {
            throw new ArgumentException(); 
        }
    }
    #endregion filemanagement
}




