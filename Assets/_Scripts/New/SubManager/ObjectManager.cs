using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : ISubManager
{
    public void Initialize()
    {
        Debug.Log("ObjectManager Initialized.");
    }

    public void OnGameStateEntered(string newState)
    {
        switch (newState)
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
        Debug.LogWarning("ObjectManager::OnGameStateChanged not implemented.");
    }

    public void OnGameStateLeft(string oldState)
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
        Debug.LogWarning("ObjectManager::OnGameStateChanged not implemented.");
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    
}
