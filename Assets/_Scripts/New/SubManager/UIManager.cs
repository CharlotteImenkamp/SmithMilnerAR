using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class UIManager : SubManager
{
    private static GameObject generalSettingsMenu;
    private static GameObject newSettingsMenu;
    private static GameObject oldSettingsMenu;
    private static GameObject[] allMenus;

    private static InteractableToggleCollection radioButtons; 

    private static bool enableUserButton; 

    public UIManager()
    {
        // Get Parameters from GameManager
        generalSettingsMenu = GameManager.Instance.GeneralSettingsMenu;
        newSettingsMenu = GameManager.Instance.NewSettingsMenu;
        oldSettingsMenu = GameManager.Instance.OldSettingsMenu;
        allMenus = new GameObject[] { generalSettingsMenu, newSettingsMenu, oldSettingsMenu };

        radioButtons = GameManager.Instance.radioButtonCollection.GetComponentInChildren<InteractableToggleCollection>(); 

        // parameters
        enableUserButton = false;

        // events
        GameManager.Instance.OnUserButtonClicked.AddListener(OnUserButtonClicked);
        GameManager.Instance.OnGeneralSettingsButtonClicked.AddListener(()=> OnButtonApplyGeneralSettings(GameManager.Instance.newDataset));
        GameManager.Instance.OnOldSettingsButtonClicked.AddListener(SetCurrentUserSettings); 
    }

    public void Initialize()
    {
        CloseAllMenus(); 

        Debug.Log("UIManager Initialized.");
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    #region gameStates
    public override void OnGameStateEntered(string newState)
    {
        switch (newState)
        {
            case "Initialization":
                Initialize(); 
                break;
            case "SettingsMenu":
                OpenMenu(generalSettingsMenu);  
                break;
            case "LocationTest":
                break;
            case "LocationEstimation":
                break;
            case "PriceTest":
                break;
            case "PriceEstimation":
                break;
            case "Pause":
                break;
            default:
                break;
        }
        Debug.LogWarning("UIManager::OnGameStateEntered not implemented.");
    }

    public override void OnGameStateLeft(string oldState)
    {
        switch (oldState)
        {
            case "Initialization":
                break;

            case "SettingsMenu":
                CloseAllMenus(); 
                break;

            case "LocationTest":
                Debug.LogWarning("UIManager::OnGameStateLeft LocationTest not implemented.");
                break;

            case "LocationEstimation":
                Debug.LogWarning("UIManager::OnGameStateLeft LocationEstimation not implemented.");
                break;

            case "PriceTest":
                Debug.LogWarning("UIManager::OnGameStateLeft PriceTest not implemented.");
                break;

            case "PriceEstimation":
                Debug.LogWarning("UIManager::OnGameStateLeft PriceEstimation not implemented.");
                break;

            case "Pause":
                Debug.LogWarning("UIManager::OnGameStateLeft Pause not implemented.");
                break;

            default:
                Debug.LogError("UIManager::OnGameStateLeft no valid state."); 
                break;
        }
    }

    #endregion gameStates


    #region buttons

    /// <summary>
    /// Call in Scene on the User button 
    /// Add and remove listeners of GameStateManager depending on gameState
    /// </summary>
    public static void OnUserButtonClicked()        //\TODO disable user button, if state is initializing, settingsmenu or end
    {
        if (enableUserButton)
        {
            GameStateManager.gameStateMachine.SwitchToNextState(); 
            enableUserButton = false; 
        }
        else
        {
            enableUserButton = true; 
        }

        Debug.LogWarning("UIManager::OnUserButtonClicked not implemented"); 
    }

    /// <summary>
    /// Set new bool newDataSet and StartPrices/StartLocations
    /// </summary>
    public void OnButtonApplyGeneralSettings(bool newDataSet)             
    {
        if (newDataSet)
        {
            OpenMenu(newSettingsMenu); 
        }
        else
        {
            OpenMenu(oldSettingsMenu); 
        }
        Debug.Log("UIManager::OnButtonApplyGeneralSettings successful");
    }

    private void SetCurrentUserSettings()
    {
        DataManager.Instance.CurrentSettings = DataManager.Instance.UserSettings[radioButtons.CurrentIndex];
        Debug.LogWarning("UIManager::getCurrentUserSettings not implemented."); 
    }


    #endregion buttons


    #region menu

    private static void OpenMenu(GameObject menu)
    {
        foreach (GameObject obj in allMenus)
        {
            obj.SetActive(false);
        }

        menu.SetActive(true); 
    }

    private static void CloseAllMenus()
    {
        foreach (GameObject obj in allMenus)
        {
            obj.SetActive(false); 
        }
    }

    #endregion menu

}
