using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singelton class
/// tasks: change gameStates, which are no data states
/// </summary>
public class GameStateManager: MonoBehaviour
{
    #region instance
    private static GameStateManager _instance = null;

    public static GameStateManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            Debug.LogError("Instance of GameStateManager destroyed."); 
        }
        else
        {
            _instance = this;
        }
    }
    #endregion 


    private static StateMachine gameStateMachine = new StateMachine();


    private void Start()
    {
        ResetToDefault(); 
    }


    #region button methods

    /// <summary>
    /// Called at Event of menu button press
    /// </summary>
    /// <param name="startWithPrices"></param>
    public void StartTestRun(GameType gameType)
    {
        if (gameType == GameType.Prices)
            gameStateMachine.ChangeState(new PriceTest()); 
        else if(gameType == GameType.Locations)
            gameStateMachine.ChangeState(new LocationTest());
        else
            throw new ArgumentException("GameStateManager::StartTestRun no valid GameType {0}", gameType.ToString()); 
        
        GameManager.Instance.OnUserButtonClicked.RemoveAllListeners();
        GameManager.Instance.OnUserButtonClicked.AddListener(() => StartGame(GameManager.Instance.gameType)); 
    }

    public void StartGame(GameType gameType)
    {
        if (gameType == GameType.Prices)
        {
            gameStateMachine.ChangeState(new PriceEstimation());
            DataManager.Instance.StartDataLogging();
        }
        else if (gameType == GameType.Locations)
        {
            gameStateMachine.ChangeState(new LocationEstimation());
            DataManager.Instance.StartDataLogging(); 
        }
        else
        {
            throw new ArgumentException("GameStateManager::StartTestRun no valid GameType.");
        }

        GameManager.Instance.OnUserButtonClicked.RemoveAllListeners();
        GameManager.Instance.OnUserButtonClicked.AddListener(() => EndGame(GameManager.Instance.gameType));
    }

    public void EndGame(GameType gameType)
    {
        if (gameType == GameType.Prices)
        {
            // stop logging before state change to get the objects in the scene
            DataManager.Instance.StopDataLogging();

            // Change State
            gameStateMachine.ChangeState(new Pause());
        }
        else if (gameType == GameType.Locations)
        {
            // stop logging before state change to get the objects in the scene
            DataManager.Instance.StopDataLogging();

            // Change State
            gameStateMachine.ChangeState(new End());
        }
        else
        {
            throw new ArgumentException("GameStateManager::EndGame no valid GameType.");
        }
        GameManager.Instance.OnUserButtonClicked.RemoveAllListeners(); 
    }
    #endregion


    public void ResetToDefault()
    {
        // Events
        GameManager.Instance.OnUserButtonClicked.RemoveAllListeners(); 

        // game states
        gameStateMachine.ChangeState(new Initialization());
        gameStateMachine.ChangeState(new SettingsMenu());
    }
}


