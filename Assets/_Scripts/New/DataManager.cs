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

    private PlayerSettings[] userSettings;

    DataManager()
    {
    
    }

    void Start()
    {
        dataStateMachine.ChangeState(new LoadSettings()); 
    }
}
