using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ISubManager
{
    #region inherited Methods
    public void Initialize()
    {
        Debug.Log("UIManager Initialized.");
    }

    public void OnGameStateEntered(string newState)
    {
        switch (newState)
        {
            case "Initialization":
                break;
            case "SettingsMenu":
                OpenSettingMenu(); 
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
        Debug.LogWarning("ObjectManager::OnGameStateChanged not implemented.");
    }

    public void OnGameStateLeft(string oldState)
    {
        switch (oldState)
        {
            case "Initialization":
                break;
            case "SettingsMenu":
                CloseSettingsMenu();
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
        Debug.LogWarning("ObjectManager::OnGameStateChanged not implemented.");
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    
    #endregion inherited Methods

    #region private Methods
    private void CloseSettingsMenu()
    {
        Debug.LogWarning("UIManager::CloseMenu not implemented."); 
    }

    private void OpenSettingMenu()
    {
        Debug.LogWarning("UIManager::OpenMenu not implemented.");
    }

    /// <summary>
    /// Call in Scene on the User button 
    /// Add and remove listeners of GameStateManager depending on gameState
    /// </summary>
    private void OnUserButtonClicked()
    {
        Debug.LogWarning("UIManager::OnUserButtonClicked not implemented"); 
    }

    /// <summary>
    /// Set new bool newDataSet and StartPrices/StartLocations
    /// </summary>
    public void OnButtonApplyGeneralSettings()              //\TODO reset to private!!
    {
        bool newDataSet = true; //\TODO : Add to tick box in menu 
        Debug.LogWarning("TODO: implement newDataSet bool in UIManager Menu");

        if (newDataSet)
        {
            OpenNewDatasetMenu(); 
        }
        else
        {
            OpenOldDatasetMenu(); 
        }
        Debug.LogWarning("UIManager::OnButtonApplyGeneralSettings not implemented");
    }

    private void OpenOldDatasetMenu()
    {
        Debug.LogWarning("UIManager::OpenOldDatasetMenu not implemented");
    }

    private void OpenNewDatasetMenu()
    {
        Debug.LogWarning("UIManager::OpenNewDatasetMenu not implemented");
    }

    private void OnButtonApplyNewDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyNewDataSettings not implemented");
        GameStateManager.Instance.ApplyNewDataSettings(startWithPrices); 
    }

    private void OnButtonApplyOldDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyOldDataSettings not implemented");
        GameStateManager.Instance.ApplyOldDataSettings(startWithPrices);
    }


    #endregion privateMethods
}
