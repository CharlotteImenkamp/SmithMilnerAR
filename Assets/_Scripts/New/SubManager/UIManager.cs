using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ISubManager
{


    // singelton pattern 
    private static UIManager _instance = null;
    public static UIManager Instance
    {

        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }

    }


    private static GameObject generalSettingsMenu;
    private static GameObject newSettingsMenu;
    private static GameObject oldSettingsMenu;
    private static GameObject[] allMenus; 

    #region inherited Methods
    public void Initialize()
    {
        // Get Parameters from GameManager
        generalSettingsMenu = GameManager.Instance.GeneralSettingsMenu;
        newSettingsMenu = GameManager.Instance.NewSettingsMenu;
        oldSettingsMenu = GameManager.Instance.OldSettingsMenu;

        allMenus = new GameObject []{ generalSettingsMenu, newSettingsMenu, oldSettingsMenu };

        //Set Menus inactive
        CloseAllMenus(); 

        Debug.Log("UIManager Initialized.");
    }

    public void OnGameStateEntered(string newState)
    {
        switch (newState)
        {
            case "Initialization":
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
        Debug.LogWarning("ObjectManager::OnGameStateChanged not implemented.");
    }

    public void OnGameStateLeft(string oldState)
    {
        switch (oldState)
        {
            case "Initialization":
                break;
            case "SettingsMenu":
                CloseAllMenus(); 
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


    #region buttons
    /// <summary>
    /// Call in Scene on the User button 
    /// Add and remove listeners of GameStateManager depending on gameState
    /// </summary>
    public void OnUserButtonClicked()
    {
        Debug.LogWarning("UIManager::OnUserButtonClicked not implemented"); 
    }


    /// <summary>
    /// Set new bool newDataSet and StartPrices/StartLocations
    /// </summary>
    public void OnButtonApplyGeneralSettings()             
    {
        bool newDataSet = true; //\TODO : Add to tick box in menu 
        Debug.LogWarning("TODO: implement newDataSet bool in UIManager Menu");

        if (newDataSet)
        {
            OpenMenu(newSettingsMenu); 
        }
        else
        {
            OpenMenu(oldSettingsMenu); 
        }
        Debug.LogWarning("UIManager::OnButtonApplyGeneralSettings not implemented");
    }

    public void OnButtonApplyNewDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyNewDataSettings not implemented");
        GameStateManager.Instance.ApplyNewDataSettings(startWithPrices);
    }

    public void OnButtonApplyOldDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyOldDataSettings not implemented");
        GameStateManager.Instance.ApplyOldDataSettings(startWithPrices);
    }


    #endregion buttons


    #region menu

    private void OpenMenu(GameObject menu)
    {
        foreach (GameObject obj in allMenus)
        {
            obj.SetActive(false);
        }

        menu.SetActive(true); 
    }

    private void CloseAllMenus()
    {
        foreach (GameObject obj in allMenus)
        {
            obj.SetActive(false); 
        }
    }

    #endregion menu

}
