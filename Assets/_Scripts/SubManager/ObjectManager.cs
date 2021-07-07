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

    private userSettingsData currentUserSettings;
    private ObjectData currentObjectData; 

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
                    currentObjectData.GetObjectPositions(),
                    currentObjectData.GetObjectRotations(), 
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
                    currentObjectData.GetObjectPositions(),
                    currentObjectData.GetObjectRotations(), 
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
        GameManager.Instance.debugText.text = "ObjectManager::Reset"; 
        Debug.Log("ObjectManager::Reset"); 

        interactionObjects = null; 
        testObject = null;
        testPosition= Vector3.zero;
        testRotation = Quaternion.identity;
        currentUserSettings = null;
        objectCreator = null;
    }


    private void CheckDefaultParameters()
    {
        if (currentUserSettings == null)
        {
            currentUserSettings = DataManager.Instance.CurrentSettings;
        }
        if(currentObjectData == null)
        {
            currentObjectData = DataManager.Instance.CurrentObjectData; 
        }
        if (interactionObjects == null)
        {
            interactionObjects = objectCreator.CreateInteractionObjects(currentObjectData);

        }
        if (testObject == null)
        {
            testObject = objectCreator.CreateInteractionObject(currentObjectData);
            testPosition = new Vector3(-0.0f, -0f, 0f);                                        // \TODO add to general settings
            testRotation = Quaternion.identity;
        }
    }
}





