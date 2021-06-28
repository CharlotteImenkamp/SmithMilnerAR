using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SubManager
{
    private static GameObject generalSettingsMenu;
    private static GameObject newSettingsMenu;
    private static GameObject oldSettingsMenu;
    private static GameObject[] allMenus;

    #region inherited Methods
    public UIManager()
    {
        // Get Parameters from GameManager
        generalSettingsMenu = GameManager.Instance.GeneralSettingsMenu;
        newSettingsMenu = GameManager.Instance.NewSettingsMenu;
        oldSettingsMenu = GameManager.Instance.OldSettingsMenu;

        allMenus = new GameObject[] { generalSettingsMenu, newSettingsMenu, oldSettingsMenu };
    }

    public void Initialize()
    {
        CloseAllMenus(); 

        Debug.Log("UIManager Initialized.");
    }

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
        Debug.LogWarning("UIManager::OnGameStateLeft not implemented.");
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    
    #endregion inherited Methods


    #region buttons
    /// <summary>
    /// Call in Scene on the User button 
    /// Add and remove listeners of GameStateManager depending on gameState
    /// </summary>
    public static void OnUserButtonClicked()
    {
        Debug.LogWarning("UIManager::OnUserButtonClicked not implemented"); 
    }


    /// <summary>
    /// Set new bool newDataSet and StartPrices/StartLocations
    /// </summary>
    public static void OnButtonApplyGeneralSettings()             
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

    public static void OnButtonApplyNewDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyNewDataSettings not implemented");
        GameStateManager.Instance.ApplyNewDataSettings(startWithPrices);
    }

    public static void OnButtonApplyOldDataSettings()
    {
        bool startWithPrices = true;
        Debug.LogWarning("TODO: implement startWithPrices bool in UIManager Menu");

        Debug.LogWarning("UIManager::OnButtonApplyOldDataSettings not implemented");
        GameStateManager.Instance.ApplyOldDataSettings(startWithPrices);
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
