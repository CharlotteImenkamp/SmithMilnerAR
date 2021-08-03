using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;


public class ObjectManager : SubManager
{
    private GameObject[] interactionObjects;

    // Game Objects
    private GameObject parentPlayTable;
    private GameObject parentSideTable;

    // Grid Object Collections
    private GridObjectCollection playTableObjectCollection;
    private GridObjectCollection sideTableObjectCollection;

    private GameObject testObject; 
    private Vector3 testPositionPrices;
    private Vector3 testPositionLocations;

    private DataManager.Data currentData; 
    private ObjectCreator objectCreator; 

    public void Initialize()
    {
        objectCreator = ScriptableObject.CreateInstance<ObjectCreator>();

        // Game Objects
        parentPlayTable = GameManager.Instance.parentPlayTable;
        parentSideTable = GameManager.Instance.parentSideTable;

        // Object Collections
        playTableObjectCollection = parentPlayTable.GetComponent<GridObjectCollection>();
        if(playTableObjectCollection == null)
        {
            playTableObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane; 
            playTableObjectCollection.CellHeight = 0.25f;
            playTableObjectCollection.CellWidth = 0.25f; 
        }

        sideTableObjectCollection = parentSideTable.GetComponent<GridObjectCollection>();
        if (sideTableObjectCollection == null)
        {
            sideTableObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            sideTableObjectCollection.CellHeight = 0.19f;
            sideTableObjectCollection.CellWidth = 0.19f;
        }

        objectCreator.PrefabFolderName = "Objects";
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
                objectCreator.SpawnObject(testObject,parentSideTable,testPositionLocations,testObject.transform.rotation, ConfigType.MovementEnabled);

                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects; 
                break;

            case "LocationEstimation":
                objectCreator.SpawnObjects(interactionObjects,
                    parentSideTable,
                    currentData.ObjData.GetObjectPositions(),
                    currentData.ObjData.GetObjectRotations(), 
                    ConfigType.MovementEnabled);
                sideTableObjectCollection.UpdateCollection(); 

                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects;
                break;

            case "PriceTest":
                CheckDefaultParameters();
                objectCreator.SpawnObject(testObject, parentPlayTable, testPositionPrices, testObject.transform.rotation, ConfigType.MovementDisabled);

                DataManager.Instance.ObjectsInScene = objectCreator.InstantiatedObjects;
                break;

            case "PriceEstimation":
                objectCreator.SpawnObjects(interactionObjects, parentPlayTable,
                    currentData.ObjData.GetObjectPositions(),
                    currentData.ObjData.GetObjectRotations(), 
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
        currentData.ObjData = null;
        currentData.UserData = null;
        objectCreator.Reset();
    }


    private void CheckDefaultParameters()
    {
        if (currentData.UserData == null || currentData.ObjData == null)
        {
            currentData = DataManager.Instance.CurrentSettings;
        }
        if (interactionObjects == null)
        {
            if(currentData.ObjData != null)
            {
                interactionObjects = objectCreator.CreateInteractionObjects(currentData.ObjData);
            }
            else
            {
                Debug.LogWarning("Choose Object Data!");
            }
        }
        if (testObject == null)
        {
            testObject = objectCreator.CreateInteractionObject(currentData.ObjData);
            testPositionPrices = GameManager.Instance.spawnPointGame.position;                                    
            testPositionLocations = GameManager.Instance.spawnPointSide.position; 
        }
    }
}





