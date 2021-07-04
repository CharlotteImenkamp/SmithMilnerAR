using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit; 
using UnityEngine.Events;

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
    public List<userSettingsData> UserSettings { get => userSettings; set => userSettings = value; }
    public userSettingsData CurrentSettings { get => currentSettings; set => currentSettings = value; }
    public List<GameObject> ObjectsInScene { get => objectsInScene; set => objectsInScene = value; }
    public List<GameObject> MovingObjects { get => movingObjects; set => movingObjects = value; }
    public HeadData CurrentHeadData { get => currentHeadData; set => currentHeadData = value; }

    #endregion public parameters

    #region private paramters
    private List<userSettingsData> userSettings;
    private userSettingsData currentSettings;
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
        currentSettings = userSettings[radioButtonIndex];
    }

    public void GenerateNewUserSettings()
    {
        var set = new userSettingsData();
        set.updateRate = 1.0f;
        set.UserID = 2;

        var t = new CustomObject("CoffeeCupGravity", new Vector3(-0.162803515791893f, -0.14000004529953004f, 1.7623728513717652f), new Quaternion(8.940696716308594e-8f, 0.005584994331002235f, -4.656612873077393e-10f, 0.9999845027923584f));

        var f = new CustomObject("Pear_MPI01_fbx", new Vector3(0f, 0.005f, 1.759580373764038f), new Quaternion(8.940696716308594e-8f, 0.005584994331002235f,-4.656612873077393e-10f, 0.9999845027923584f));
        set.gameObjects = new List<CustomObject> { t, f };

        currentSettings = set; 
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
        userSettings = new List<userSettingsData>();
        movingObjects = new List<GameObject>();
        objectsInScene = new List<GameObject>();
        currentHeadData = new HeadData();

        // start loading
        dataStateMachine.ChangeState(new LoadSettings());
    }
}
