using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SubManager
{
    private GameObject[] interactionObjects;
    private GameObject testObject; 
    private Vector3 testPosition;
    private Quaternion testRotation;

    private userSettingsData currentSettings; 

    private ObjectCreator objectCreator; 

    public void Initialize()
    {
        Debug.Log("ObjectManager Initialized.");

        objectCreator = new ObjectCreator();
        objectCreator.PrefabFolderName = "Objects";
        objectCreator.BoundingBoxFolderName = "BoundingBox";
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
                break;

            case "LocationTest":
                CheckDefaultParameters();
                objectCreator.SpawnObject(testObject,testPosition,testRotation, ConfigType.MovementEnabled); 
                break;

            case "LocationEstimation":
                objectCreator.SpawnObjects(interactionObjects,
                    currentSettings.GetObjectPositions(),
                    currentSettings.GetObjectRotations(), 
                    ConfigType.MovementEnabled);
                break;

            case "PriceTest":
                CheckDefaultParameters();
                objectCreator.SpawnObject(testObject, testPosition, testRotation, ConfigType.MovementDisabled); 
                break;

            case "PriceEstimation":
                objectCreator.SpawnObjects(interactionObjects,
                    currentSettings.GetObjectPositions(),
                    currentSettings.GetObjectRotations(), 
                    ConfigType.MovementDisabled);
                break; 

            case "Pause":
                break; 

            default:
                Debug.LogError("ObjectManager::OnGameStateEntered invalid State.");
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
                break;

            case "LocationTest":
                objectCreator.RemoveObject(testObject);
                break;

            case "LocationEstimation":
                objectCreator.RemoveObjects(interactionObjects); 
                break;

            case "PriceTest":
                objectCreator.RemoveObject(testObject);
                break;

            case "PriceEstimation":
                objectCreator.RemoveObjects(interactionObjects);
                break;

            case "Pause":
                objectCreator.RemoveObjects(interactionObjects);
                break;

            default:
                Debug.LogError("ObjectManager::OnGameStateLeft invalid State."); 
                break;
        } 
    }

    #endregion gameStates

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    private void CheckDefaultParameters()
    {
        if (currentSettings == null)
        {
            currentSettings = DataManager.Instance.CurrentSettings;
        }
        if (interactionObjects == null)
        {
            interactionObjects = objectCreator.CreateInteractionObjects(currentSettings);
        }
        if (testObject == null)
        {
            testObject = objectCreator.CreateInteractionObject(currentSettings);
            testPosition = new Vector3(-0.0f, -0.177f, 1.14f);                                        // \TODO add to general settings
            testRotation = Quaternion.identity;
        }
    }
}

public enum ConfigType { MovementEnabled, MovementDisabled }



