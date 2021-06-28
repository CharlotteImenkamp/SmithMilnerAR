using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SubManager
{
    public void Initialize()
    {
        Debug.Log("ObjectManager Initialized.");
    }

    public override void OnGameStateEntered(string newState)
    {
        switch (newState)
        {
            case "Initialization":
                Initialize(); 
                break; 
            case "SettingsMenu":
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
        Debug.LogWarning("ObjectManager::OnGameStateEntered not implemented.");
    }

    public override void OnGameStateLeft(string oldState)
    {
        switch (oldState)
        {
            case "Initialization":
                break;
            case "SettingsMenu":
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
        Debug.LogWarning("ObjectManager::OnGameStateLeft not implemented.");
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    
}
