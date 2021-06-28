using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// singelton 
// tasks: 
public class GameManager : MonoBehaviour
{
    #region create instance
    private static GameManager _instance = null;
    public static GameManager Instance
    {

        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("GameManager created from script.");

                _instance = new GameManager();
            }
            return _instance;
        }

    }
    #endregion

    [Tooltip("Add Menu Objects for UIManager")]
    public GameObject GeneralSettingsMenu;
    public GameObject NewSettingsMenu;
    public GameObject OldSettingsMenu;

    [Tooltip("General Settings file path")]
    public string settingsFile;  

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

        // Check Input parameters
        if (GeneralSettingsMenu == null || NewSettingsMenu == null || OldSettingsMenu == null)
        {
            Debug.LogError("Not all Parameters are Set in GameManager."); 
        }


    }

    void Start()
    {
        // Load General Settings
        LoadGeneralSettings(); 

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

    private void LoadGeneralSettings()
    {
        string filepath = Application.persistentDataPath + "/generalSettings.json";
        if (File.Exists(filepath))
        {
            applicationData set = JsonUtility.FromJson<applicationData>(filepath); 
        }


        // 
        //settings = new applicationData();
        //settings.settingFiles = new string[] { "s1", "x", "c" };
        //settings.dataFolder = "//data";
        //settings.dataFiles = new string[] { "d1", "d2" }; 

        //string jsonString = JsonUtility.ToJson(settings, true);
        //jsonString += System.Environment.NewLine;
        //System.IO.File.AppendAllText(filepath, jsonString); 
        //File.WriteAllText(filepath, jsonString);
        //}


        Debug.Log(filepath); 
        Debug.Log("GameManager::LoadGeneralSettings successful."); 
    }

    void Update()
    {

        if (Input.GetKeyDown("y"))
        {
            OnMenuButtonPressed(1); 
        }
        if (Input.GetKeyDown("x"))
        {
            OnMenuButtonPressed(3);
        }
        if (Input.GetKeyDown("z"))
        {
            OnMenuButtonPressed(2);
        }
    }

    #region helper methods
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
    /// <param name="type"> Define, which button type is pressed. 
    /// 0 "Userbutton", 
    /// 1 "ApplyGeneralSettings", 
    /// 2 "ApplyNewDataSettings"
    /// 3 ApplyOldDataSettings"
    /// </param>
    public void OnMenuButtonPressed(int type)
    {
        switch (type)
        {
            case (int)ButtonType.UserButton:
                UIManager.OnUserButtonClicked(); 
                break;
            case (int)ButtonType.ApplyGeneralSettings:
                UIManager.OnButtonApplyGeneralSettings(); 
                break;
            case (int)ButtonType.ApplyNewDataSettings:
                UIManager.OnButtonApplyNewDataSettings(); 
                break;
            case (int)ButtonType.ApplyOldDataSettings:
                UIManager.OnButtonApplyOldDataSettings(); 
                break;
            default:
                break;
        }
    }

    #endregion helper methods

}




