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


    public static StateMachine gameStateMachine = new StateMachine();




    private void Start()
    {
        // Events
        GameManager.Instance.OnNewSettingsButtonClicked.AddListener(() => StartGame(GameManager.Instance.startWithPrices));
        GameManager.Instance.OnOldSettingsButtonClicked.AddListener(() => StartGame(GameManager.Instance.startWithPrices)); 

        // game states
        gameStateMachine.ChangeState(new Initialization());
        gameStateMachine.ChangeState(new SettingsMenu());
    }


    private void ChangeStateToNextGameState()
    {
        throw new NotImplementedException();
    }


    #region button methods

    /// <summary>
    /// Called at Event of menu button press
    /// </summary>
    /// <param name="startWithPrices"></param>
    public void StartGame(bool startWithPrices)
    {
        if (startWithPrices)
        {
            gameStateMachine.ChangeState(new PriceTest()); 
        }
        else
        {
            gameStateMachine.ChangeState(new LocationTest());
        }
    }


    #endregion  

}


