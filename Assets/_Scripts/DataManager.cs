using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit; 
using UnityEngine.Events;
using System;
using System.IO;

public class DataManager : MonoBehaviour
{
    #region create instance
    private static DataManager _instance = null;

    public static DataManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            Debug.LogError("Instance of DataManager destroyed.");
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private static StateMachine dataStateMachine = new StateMachine();

    #region public parameters
    public List<ObjectData> NewSets { get => newSettings; set => newSettings = value; }
    public userSettingsData CurrentSettings { get => currentSettings; set => currentSettings = value; }
    public List<GameObject> ObjectsInScene { get => objectsInScene; set => objectsInScene = value; }
    public List<GameObject> MovingObjects { get => movingObjects; set => movingObjects = value; }
    public HeadData CurrentHeadData { get => currentHeadData; set => currentHeadData = value; }
    public List<ObjectData> IncompleteUserData { get => incompleteSettings; set => incompleteSettings = value; }
    public List<ObjectData> CompleteUserData { get => completeSettings; set => completeSettings = value; }
    public ObjectData CurrentObjectData { get => currentObjectData; set => currentObjectData = value; }

    #endregion public parameters

    #region private paramters
    private List<ObjectData> newSettings;
    private List<ObjectData> incompleteSettings;
    private List<ObjectData> completeSettings;

    private userSettingsData currentSettings;
    private ObjectData currentObjectData; 

    private List<GameObject> objectsInScene;
    private List<GameObject> movingObjects;
    private HeadData currentHeadData;
    #endregion private parameters


    void Start()
    {
        ResetToDefault();  
    }

    private void Update()
    {
        currentHeadData.SetCameraParameters(Camera.main.transform);
        currentHeadData.SetGazeParameters(CoreServices.InputSystem.EyeGazeProvider.GazeOrigin, CoreServices.InputSystem.EyeGazeProvider.GazeDirection); 

        dataStateMachine.ExecuteStateUpdate(); 
    }

    #region dataLogging

    /// <summary>
    /// Called by UIManager from button event
    /// </summary>
    /// <param name="radioButtonIndex"></param>
    public void SetCurrentUserSettings(int radioButtonIndex)
    {
        currentSettings = GameManager.Instance.radioButtonCollection.GetComponent<CustomToggleListPopulator>().chosenSet[radioButtonIndex];
        // currentObjectData = ...

        throw new System.NotImplementedException(); 
    }

    /// <summary>
    /// start, if GameState changed to Location Estimation or PriceEstimation
    /// </summary>
    public void StartDataLogging()
    {
        dataStateMachine.ChangeState(new LogData(GameManager.Instance.gameType)); 
    }

    public void StopDataLogging()
    {
        dataStateMachine.SwitchToIdle(); 
    }

    #endregion dataLogging

    public void ResetToDefault()
    {
        // parameters
        newSettings = new List<ObjectData>();
        completeSettings = new List<ObjectData>();
        incompleteSettings = new List<ObjectData>();

        movingObjects = new List<GameObject>();
        objectsInScene = new List<GameObject>();
        currentHeadData = new HeadData();

        // start loading
        dataStateMachine.ChangeState(new LoadSettings());
    }


    public void SetAndSaveNewSettings(userSettingsData data, ObjectData objectData)
    {
        if (data != null && objectData.gameObjects.Count != 0)
        {
            currentSettings = data;
            string settingsFolder = GameManager.Instance.generalSettings.objectDataFolder;
            string dataFolder = GameManager.Instance.generalSettings.userDataFolder + "/User_" + data.UserID.ToString();
            string mainFolder = GameManager.Instance.mainFolder;
            
            // Save into user folder and into settings folder
            DataFile.Save<ObjectData>(objectData, Path.Combine(mainFolder, dataFolder), "userSettings" + data.UserID.ToString());
            DataFile.Save<ObjectData>(objectData, Path.Combine(mainFolder, settingsFolder), "objectData" + data.UserID.ToString());

            DataFile.Save<userSettingsData>(data, Path.Combine(mainFolder, dataFolder), "user" + data.UserID.ToString()); 
        }
        else
        {
            GameManager.Instance.debugText.text = "DataManager::SetCurrentSettings no valid data.";
            Debug.LogWarning("DataManager::SetCurrentSettings no valid data.");
        }
    }
}
