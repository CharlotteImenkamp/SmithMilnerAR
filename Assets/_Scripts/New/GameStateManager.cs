using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singelton class
/// tasks: change gameStates, which are no data states
/// </summary>
public class GameStateManager: MonoBehaviour
{
    private static GameStateManager _instance = null;

    public static GameStateManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Instance of GameStateManager destroyed."); 
        }
        else
        {
            _instance = this;
        }
    }

    #region private parameters

    static StateMachine gameStateMachine = new StateMachine();

    #endregion private parameters


    #region private Methods
    private void Start()
    {
        gameStateMachine.ChangeState(new Initialization());
        gameStateMachine.ChangeState(new SettingsMenu());
    }

    private void Update()
    {
    }

    #endregion private Methods

    #region public button methods
    /// <summary>
    /// Called in UIManager OnButtonApplyNewDataSettings
    /// </summary>
    /// <param name="startWithPrices"></param>
    public void ApplyNewDataSettings(bool startWithPrices)
    {
        Debug.LogWarning("GameStateManager::ApplyNewDataSettings not implemented");

        if (startWithPrices)
        {
            gameStateMachine.ChangeState(new PriceTest()); 
        }
        else
        {
            gameStateMachine.ChangeState(new LocationTest());
        }
    }

    /// <summary>
    /// Called in UIManager OnButtonApplyOldDataSettings
    /// </summary>
    /// <param name="startWithPrices"></param>
    public void ApplyOldDataSettings(bool startWithPrices)
    {
        Debug.LogWarning("GameStateManager::ApplyOldDataSettings not implemented");

        if (startWithPrices)
        {
            gameStateMachine.ChangeState(new PriceTest());
        }
        else
        {
            gameStateMachine.ChangeState(new LocationTest());
        }
    }

    #region UserButtonListeners

    /// <summary>
    /// Add as Listener to UIManager::OnUserButtonClicked
    /// </summary>
    public void TransitionFromPriceTestToPriceEstimation()
    {
        Debug.LogWarning("GameStateManager::TransitionFromPriceTestToPriceEstimation not implemented");

        gameStateMachine.ChangeState(new PriceEstimation()); 
    }

    /// <summary>
    /// Add as Listener to UIManager::OnUserButtonClicked
    /// </summary>
    public void TransitionFromPriceEstimationToPause()
    {
        Debug.LogWarning("GameStateManager::TransitionFromPriceEstimationToPause not implemented");

        gameStateMachine.ChangeState(new Pause());
    }

    /// <summary>
    /// Add as Listener to UIManager::OnUserButtonClicked
    /// </summary>
    public void TransitionFromLocationTestToLocationEstimation()
    {
        Debug.LogWarning("GameStateManager::TransitionFromLocationTestToLocationEstimation not implemented");

        gameStateMachine.ChangeState(new LocationEstimation());
    }

    /// <summary>
    /// Add as Listener to UIManager::OnUserButtonClicked
    /// </summary>
    public void TransitionFromLocationEstimationToEnd()
    {
        Debug.LogWarning("GameStateManager::TransitionFromLocationEstimationToEnd not implemented");

        gameStateMachine.ChangeState(new End());
    }
    #endregion UserButtonListeners

    #endregion public methods

}
