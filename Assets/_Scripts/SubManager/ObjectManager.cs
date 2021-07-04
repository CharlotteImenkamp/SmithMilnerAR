using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SubManager
{
    private GameObject[] interactionObjects;
    private GameObject parent; 
    private GameObject testObject; 
    private Vector3 testPosition;
    private Quaternion testRotation;

    private userSettingsData currentSettings; 

    private ObjectCreator objectCreator; 

    public void Initialize()
    {
        objectCreator = ScriptableObject.CreateInstance<ObjectCreator>();
        parent = GameManager.Instance.parentInteractionObject; 
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
                objectCreator.SpawnObject(testObject,parent,testPosition,testRotation, ConfigType.MovementEnabled);
                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects; 
                break;

            case "LocationEstimation":
                objectCreator.SpawnObjects(interactionObjects,
                    parent,
                    currentSettings.GetObjectPositions(),
                    currentSettings.GetObjectRotations(), 
                    ConfigType.MovementEnabled);
                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects;
                break;

            case "PriceTest":
                CheckDefaultParameters();
                objectCreator.SpawnObject(testObject, parent, testPosition, testRotation, ConfigType.MovementDisabled);
                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects;
                break;

            case "PriceEstimation":
                objectCreator.SpawnObjects(interactionObjects, parent,
                    currentSettings.GetObjectPositions(),
                    currentSettings.GetObjectRotations(), 
                    ConfigType.MovementDisabled);
                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects;
                break; 

            case "Pause":
                break;

            case "End":
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
                objectCreator.RemoveAllObjects();
                break;

            case "LocationEstimation":
                objectCreator.RemoveAllObjects();
                break;

            case "PriceTest":
                objectCreator.RemoveAllObjects();
                break;

            case "PriceEstimation":
                objectCreator.RemoveAllObjects();
                break;

            case "Pause":
                objectCreator.RemoveAllObjects();
                break;
            case "End":
                objectCreator.RemoveAllObjects();
                break; 

            default:
                Debug.LogError("ObjectManager::OnGameStateLeft invalid State."); 
                break;
        } 
    }

    #endregion gameStates

    public override void Reset()
    {
        Debug.Log("ObjectManager::Reset"); 

        interactionObjects = null; 
        testObject = null;
        testPosition= Vector3.zero;
        testRotation = Quaternion.identity;
        currentSettings = null;
        objectCreator = null;
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





