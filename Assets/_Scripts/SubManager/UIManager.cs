using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class UIManager : SubManager
{
    private GameObject generalSettingsMenu;
    private GameObject newSettingsMenu;
    private GameObject oldSettingsMenu;
    private GameObject pauseMenu; 
    private GameObject[] allMenus;

    public void Initialize()
    {
        // Menus from GameManager
        generalSettingsMenu = GameManager.Instance.GeneralSettingsMenu;
        newSettingsMenu = GameManager.Instance.NewSettingsMenu;
        oldSettingsMenu = GameManager.Instance.OldSettingsMenu;
        pauseMenu = GameManager.Instance.PauseMenu; 

        allMenus = new GameObject[] { generalSettingsMenu, newSettingsMenu, oldSettingsMenu, pauseMenu };

        CloseAllMenus(); 
    }

    public override void Reset()
    {
        GameManager.Instance.debugText.text = "UIManager::Reset"; 
        Debug.Log("UIManager::Reset"); 
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
                CloseAllMenus();
                OpenMenu(pauseMenu);
                break;

            case "End":
                CloseAllMenus(); 
                OpenMenu(pauseMenu);
                SetChildActive(false, pauseMenu, "ContinueWithLocations"); //\TODO was besseres als string
                break;

            default:
                break;
        }
    }

    public override void OnGameStateLeft(string oldState)
    {
        switch (oldState)
        {
            case "Initialization":
                break;

            case "SettingsMenu":
                CloseAllMenus();

                // Apply user Settings, if old ones were used
                if (GameManager.Instance.radioButtonCollection.GetComponentInChildren<InteractableToggleCollection>() != null)
                {
                    DataManager.Instance.SetCurrentUserSettings(
                                        GameManager.Instance.radioButtonCollection
                                        .transform.Find("ScrollParent")
                                        .GetComponentInChildren<InteractableToggleCollection>().CurrentIndex);
                }              
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
                CloseAllMenus();
                break;

            case "End":
                break;

            default:
                Debug.LogError("UIManager::OnGameStateLeft no valid state."); 
                break;
        }
    }

    #endregion gameStates



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

    private void SetChildActive(bool setActive, GameObject menu, string objectName)
    {
        var child = menu.transform.Find(objectName);
        if(child != null)
        {
            child.gameObject.SetActive(setActive);
        }
        else
        {
            Debug.LogWarning("UIManager::SetChildActive no child found"); 
        }
    }

    #endregion menu

}
