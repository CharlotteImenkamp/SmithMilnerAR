using Microsoft.MixedReality.Toolkit.UI;
using System;
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

    [Header("Collections")]
    public GameObject radioButtonCollection;
    [Tooltip("Collection to choose between complete/incomplete or new set")]
    public InteractableToggleCollection toggleCollectionSet;

    [Tooltip("Collection to choose between users")]
    public GameObject toggleCollectionUserParent; 

    [Header("InteractionObjects")]
    public GameObject parentInteractionObject;

    [Header("Debug")]
    public GameObject debugTextObject;
    [NonSerialized]
    public TextMeshPro debugText; 

    #endregion Serialized in Editor

    #region FileParameters
    [NonSerialized]
    public ApplicationData generalSettings;
    [NonSerialized]
    public string mainFolder;

    #endregion FileParameters

    #region Event Parameters
    // Events
    [NonSerialized]
    public UnityEvent OnUserButtonClicked;

    #endregion Event Parameters

    #region Game Flow Parameters
    // game 
    [NonSerialized]
    public GameType gameType; 
    private bool enableUserButton;
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

        ResetToDefault();
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

        

    }


    #region buttons



    /// <summary>
    /// Add to Toggle Buttons, to get their input
    /// </summary>
    public void ToggleGameType()
    {
        if (gameType == GameType.Locations)
            gameType = GameType.Prices; 
        else if (gameType == GameType.Prices)
            gameType = GameType.Locations;
    }

    /// <summary>
    /// Called in Old Settings menu on radio buttons 
    /// </summary>
    /// <param name="type"></param>
    public void SetGameType(string type)
    {
        if(type == "Locations")
        {
            gameType = GameType.Locations; 
        }
        else if (type == "Prices")
        {
            gameType = GameType.Prices; 
        }
        else
        {
            throw new ArgumentException("...GameManager SetGameType to " + type + " not possible."); 
        }
    }

    /// <summary>
    /// On Pause Menu button clicked. 
    /// Current Settings stay the same
    /// </summary>
    public void ContinueWithLocations(GameObject button)
    {
        if(gameType == GameType.Prices)
        {
            gameType = GameType.Locations;

            OnUserButtonClicked.RemoveAllListeners();
            OnUserButtonClicked.AddListener(() => GameStateManager.Instance.StartTestRun(gameType));
            button.SetActive(false); 
        }
        else
        {
            button.SetActive(false); 
        }
    }

    /// <summary>
    /// Helper function for the interaction with UserButton
    /// </summary>
    public void UserButtonClicked()
    {
        // Only activate on every second Click  
        // /\ TODO change button color
        // /\ TODO enable after menu
        if (enableUserButton)
        {
            OnUserButtonClicked.Invoke();
            enableUserButton = false;
        }
        else
        {
            enableUserButton = true;
        }

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

        //\TODO in foreach
        gameObject.GetComponent<GameStateManager>().ResetToDefault();
        gameObject.GetComponent<DataManager>().ResetToDefault();

        foreach (SubManager sub in attachedSubManagers)
        {
            sub.Reset(); 
        }
    }

    private void ResetToDefault()
    {
        OnUserButtonClicked.RemoveAllListeners();
        enableUserButton = false;

        // game
        gameType = GameType.None;

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




