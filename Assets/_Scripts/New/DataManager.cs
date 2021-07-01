using System.Collections.Generic;
using UnityEngine;
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

    private List<userSettingsData> userSettings;
    public List<userSettingsData> UserSettings { get => userSettings; set => userSettings = value; }
    public userSettingsData CurrentSettings { get => currentSettings; set => currentSettings = value; }

    private userSettingsData currentSettings; 


    void Start()
    {
        // events
        GameManager.Instance.OnNewSettingsButtonClicked.AddListener(StartDataLogging);
        GameManager.Instance.OnOldSettingsButtonClicked.AddListener(StartDataLogging);

       // parameters
       userSettings = new List<userSettingsData>(); 

        // start loading
        dataStateMachine.ChangeState(new LoadSettings()); 
    }

    private void Update()
    {
        dataStateMachine.ExecuteStateUpdate(); 
    }

    #region dataLogging
    /// <summary>
    /// start, if last menu button is pressed   //\TODO: start, if userbutton is pressed first
    /// </summary>
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
