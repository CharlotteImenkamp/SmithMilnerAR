using System;
using System.Collections.Generic;
using UnityEngine;

// singelton 
// tasks: 
public class GameManager : MonoBehaviour
{
    // singelton pattern 
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

    [Tooltip("Add Menu Objects for UIManager")]
    public GameObject GeneralSettingsMenu;
    public GameObject NewSettingsMenu;
    public GameObject OldSettingsMenu; 


    #region public parameters
    public List<Type> AttachedManagerScripts { get => attachedManagerScripts; set => attachedManagerScripts = value; }
    public List<ISubManager> AttachedSubManagers { get => attachedSubManagers; set => attachedSubManagers = value; }

    #endregion public parameters

    #region private parameters
    private List <Type> attachedManagerScripts;
    private List<ISubManager> attachedSubManagers; 
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
        }
        // Check Input parameters
        if(GeneralSettingsMenu == null || NewSettingsMenu == null || OldSettingsMenu == null)
        {
            Debug.LogError("Not all Parameters are Set in GameManager."); 
        }

        // Add Managers of Type Monobehaviour
        attachedManagerScripts = new List<Type>(); 
        AddManagerToScene(typeof(GameStateManager));
        AddManagerToScene(typeof(DataManager)); 

        //Add SubManager of Type ISubManager
        attachedSubManagers = new List<ISubManager>(); 
        AddSubManager(new AudioManager());
        AddSubManager(new UIManager());
        AddSubManager(new ObjectManager());
    }

    void Start()
    {
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
    /// Managers of type Monobehaviour are added to the gameObject and collected in the list
    /// </summary>
    /// <param name="classType"></param>
    private void AddManagerToScene(Type classType)
    {
        if (this.gameObject.GetComponent(classType) == null)
        {
            this.gameObject.AddComponent(classType);
            attachedManagerScripts.Add(classType);
        }
    }
    /// <summary>
    /// Managers of type ISubManager are created and collected in the list
    /// </summary>
    /// <param name="newSubManager"></param>
    private void AddSubManager(ISubManager newSubManager)
    { 
        if(newSubManager != null)
        {
            attachedSubManagers.Add(newSubManager);
        }
        else
        {
            Debug.LogError("GameManager Failed to load SubManager"); 
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
                UIManager.Instance.OnUserButtonClicked(); 
                break;
            case (int)ButtonType.ApplyGeneralSettings:
                UIManager.Instance.OnButtonApplyGeneralSettings(); 
                break;
            case (int)ButtonType.ApplyNewDataSettings:
                UIManager.Instance.OnButtonApplyNewDataSettings(); 
                break;
            case (int)ButtonType.ApplyOldDataSettings:
                UIManager.Instance.OnButtonApplyOldDataSettings(); 
                break;
            default:
                break;
        }
    }

    #endregion helper methods

}


