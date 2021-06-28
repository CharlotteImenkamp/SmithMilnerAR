using System.Collections.Generic;
using UnityEngine;

// https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity 
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

    private List<userSettingsData> userSettings;
    public List<userSettingsData> UserSettings { get => userSettings; set => userSettings = value; }

    void Start()
    {
        // parameters
        UserSettings = new List<userSettingsData>(); 
        // start loading
        dataStateMachine.ChangeState(new LoadSettings()); 
    }

    private void Update()
    {
        dataStateMachine.ExecuteStateUpdate(); 
    }

    #region dataLogging
    public void StartDataLogging()
    {
        dataStateMachine.ChangeState(new LogData()); 
    }

    public void StopDataLogging()
    {
        dataStateMachine.SwitchToIdle(); 
    }
    #endregion dataLogging
}
