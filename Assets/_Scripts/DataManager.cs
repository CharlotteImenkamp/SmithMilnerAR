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
    public Data CurrentSettings { get => currentSet; set => currentSet = value; }

    // Data to Save
    public List<GameObject> ObjectsInScene { get => objectsInScene; set => objectsInScene = value; }
    public List<GameObject> MovingObjects { get => movingObjects; set => movingObjects = value; }
    public HeadData CurrentHeadData { get => currentHeadData; set => currentHeadData = value; }

    // Data from general Settings
    public List<ObjectData> NewSets { get => newSettings; set => newSettings = value; }
    public List<Data> IncompleteUserData { get => incompleteUserData; set => incompleteUserData = value; }
    public List<Data> CompleteUserData { get => completeUserData; set => completeUserData = value; }
    public List<Data> NewUserData { get => newUserData; set => newUserData = value; }

    public struct Data
    {
        ObjectData objData;
        UserSettingsData userData;

        public Data(ObjectData objData, UserSettingsData userData)
        {
            this.objData = objData ?? throw new ArgumentNullException(nameof(objData));
            this.userData = userData ?? throw new ArgumentNullException(nameof(userData));
        }

        public UserSettingsData UserData { get => userData; set => userData = value; }
        public ObjectData ObjData { get => objData; set => objData = value; }
    }
    #endregion public parameters

    #region private paramters
    private List<Data> newUserData; 
    private List<Data> incompleteUserData;
    private List<Data> completeUserData; 

    private List<ObjectData> newSettings;

    private List<GameObject> objectsInScene;
    private List<GameObject> movingObjects;
    private HeadData currentHeadData;

    private Data currentSet;




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
        currentSet = 
            GameManager.Instance.radioButtonCollection
            .GetComponent<CustomToggleListPopulator>()
            .chosenSet[radioButtonIndex];
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

        completeUserData = new List<Data>();
        incompleteUserData = new List<Data>();
        newUserData = new List<Data>();

        movingObjects = new List<GameObject>();
        objectsInScene = new List<GameObject>();
        currentHeadData = new HeadData();

        // start loading
        dataStateMachine.ChangeState(new LoadSettings());
    }


    public void SetAndSaveNewSettings(Data data)
    {
        if (data.UserData != null && data.ObjData.gameObjects.Count != 0)
        {
            // GameManager
            string settingsFolder = GameManager.Instance.generalSettings.objectDataFolder;
            string dataFolder = GameManager.Instance.generalSettings.userDataFolder + "/User" + data.UserData.UserID.ToString();
            string mainFolder = GameManager.Instance.mainFolder;

            // FileNames
            string settingsFileName = "settings" + data.UserData.UserID.ToString();
            string objectFileName = "objectData" + data.UserData.UserID.ToString();
            string userFileName = "user" + data.UserData.UserID.ToString(); 

            // Save into user folder and into settings folder
            DataFile.Save<ObjectData>(data.ObjData, Path.Combine(mainFolder, dataFolder), settingsFileName);
            DataFile.Save<ObjectData>(data.ObjData, Path.Combine(mainFolder, settingsFolder), objectFileName );
            DataFile.Save<UserSettingsData>(data.UserData, Path.Combine(mainFolder, dataFolder), userFileName);

            // Add to general settings
            GameManager.Instance.generalSettings.newObjectData.Add(objectFileName);
            GameManager.Instance.generalSettings.newUserData.Add("User" + data.UserData.UserID.ToString() + "/"+ userFileName);
        }
        else
        {
            GameManager.Instance.debugText.text = "DataManager::SetCurrentSettings no valid data.";
            Debug.LogWarning("DataManager::SetCurrentSettings no valid data.");
        }
    }
}
