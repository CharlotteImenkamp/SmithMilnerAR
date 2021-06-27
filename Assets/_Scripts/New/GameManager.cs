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

    #endregion helper methods

}
