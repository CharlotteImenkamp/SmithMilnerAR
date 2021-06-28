using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// singelton 
// tasks: 
public class GameManager : MonoBehaviour
{

    [Header("Menu Objects")]
    [Tooltip("Add Menu Objects for UIManager")]
    public GameObject GeneralSettingsMenu;
    public GameObject NewSettingsMenu;
    public GameObject OldSettingsMenu;

    [Header("ButtonObjects")]
    public GameObject GeneralSettingsToggleButton;
    public GameObject NewSettingsToggleButton;
    public GameObject OldSettingsToggleButton;

    [NonSerialized]
    public string settingsFile;
    [NonSerialized]
    public string persistentPath;
    [NonSerialized]
    public applicationData generalSettings; 

    private string generalSettingsPath; 

    #region public parameters
    public List<Type> AttachedManagerScripts { get => attachedManagerScripts; set => attachedManagerScripts = value; }
    public List<SubManager> AttachedSubManagers { get => attachedSubManagers; set => attachedSubManagers = value; }

    #endregion public parameters

    #region private parameters
    // Managers
    private List <Type> attachedManagerScripts;
    private List<SubManager> attachedSubManagers;

    // General Settings
    private string applicationFolder;
    private string settingsFolder;
    private applicationData settings;



    #endregion private parameters

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
        // Check Input parameters
        if (GeneralSettingsMenu == null || NewSettingsMenu == null || OldSettingsMenu == null)
        {
            Debug.LogError("Not all Menu Parameters are Set in GameManager.");
        }
        if (GeneralSettingsToggleButton == null || NewSettingsToggleButton == null || OldSettingsToggleButton == null)
        {
            Debug.LogError("Not all Menu Parameters are Set in GameManager.");
        }

        // Load General Settings
        persistentPath = Application.persistentDataPath;
        generalSettingsPath = persistentPath + "/generalSettings.json";
        generalSettings = LoadGeneralSettings(generalSettingsPath); 

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

    #region helper methods


    /// <summary>
    /// General Settings include the datapath for the userSetting files and the generated files and the same for the userData.
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private applicationData LoadGeneralSettings(string filepath)
    {
        applicationData newGeneralSettings = new applicationData();

        if (File.Exists(filepath))
        {
            string jsonString = File.ReadAllText(filepath);
            newGeneralSettings = JsonUtility.FromJson<applicationData>(jsonString);
            if (newGeneralSettings != null)
            {
                Debug.Log("GameManager::LoadGeneralSettings successful.");
            }
        }
        else
        {
            Debug.LogError("GameManager::LoadGeneralSettings no file found.");
        }
        Debug.Log(filepath);

        return newGeneralSettings;
    }

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
            Debug.LogError("GameManager::AddSubManager Failed to load SubManager"); 
        }
        
    }

    /// <summary>
    /// Helper function for the interaction between Button Scene comnponents and UIManager
    /// </summary>
    /// <param name="type"> Define, which button type is pressed.  \TODO better solution
    /// </param>
    public void OnMenuButtonClicked(string type)
    {
        switch (type)
        {
            case "UserButton":
                UIManager.OnUserButtonClicked(); 
                break;

            case "ApplyGeneralSettings":
                bool useOldSettings = GeneralSettingsToggleButton.GetComponent<Interactable>().IsToggled; 
                UIManager.OnButtonApplyGeneralSettings(useOldSettings);
                break;

            case "ApplyNewDataSettings":
                bool startWithPricesNew = NewSettingsToggleButton.GetComponent<Interactable>().IsToggled; 
                UIManager.OnButtonApplyNewDataSettings(startWithPricesNew); 
                break;

            case "ApplyOldDataSettings":
                bool startWithPricesOld = OldSettingsToggleButton.GetComponent<Interactable>().IsToggled; 
                UIManager.OnButtonApplyOldDataSettings(startWithPricesOld); 
                break;

            default:
                Debug.LogError("GameManager::OnMenuButtonClicked no valid input string"); 
                break;
        }
    }



    #endregion helper methods

}




