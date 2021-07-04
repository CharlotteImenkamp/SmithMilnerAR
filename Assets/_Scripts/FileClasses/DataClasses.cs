using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class userSettingsData
{
    #region methods
    public userSettingsData(List<CustomObject> obj, float updateRate, int userId, userSet set)
    {
        gameObjects = new List<CustomObject>();
        gameObjects.AddRange(obj);

        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set; 
    }

    public userSettingsData() {  }

    public Vector3[] GetObjectPositions()
    {
        Vector3[] positions = new Vector3[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            positions[i] = gameObjects[i].position; 
        }
        return positions; 
    }

    public Quaternion[] GetObjectRotations()
    {
        Quaternion[] rotations = new Quaternion[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            rotations[i] = gameObjects[i].rotation;
        }
        return rotations;
    }

    #endregion

    #region parameters

    public int UserID;
    public userSet set;
    // saving
    public float updateRate;
    public List<CustomObject> gameObjects;
    public enum userSet { JG, AG, AK };

    #endregion 

}


[System.Serializable]
public class ApplicationData
{
    public string settingsFolder; 
    public List<string> settingFiles;
    public string dataFolder;
    public List<string> dataFiles;
}


[System.Serializable]
public class ObjectData
{
    public float time;
    public List<CustomObject> movingObjects;

    public ObjectData(GameObject[] movingObj, float time)
    {
        movingObjects = new List<CustomObject>();  

        this.time = time;
        foreach (GameObject obj in movingObj)
        {
            var intObj = new CustomObject(obj.name, obj.transform.position, obj.transform.rotation);
            movingObjects.Add(intObj); 
        }
    }

    public ObjectData(List<GameObject> movingObj, float time)
    {
        movingObjects = new List<CustomObject>();

        this.time = time;
        foreach (GameObject obj in movingObj)
        {
            // Remove naming Conventions from Instantiating
            if (obj.name.Contains("(Clone)"))
            {
                obj.name = obj.name.Replace("(Clone)", ""); 
            }
            var intObj = new CustomObject(obj.name, obj.transform.position, obj.transform.rotation);
            movingObjects.Add(intObj);
        }
    }

    public ObjectData(List<CustomObject> objList, float time)
    {
        this.movingObjects = objList;
        this.time = time; 
    }
}

[System.Serializable]
public class HeadData
{
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public Vector3 GazeOrigin;
    public Vector3 GazeDirection;
    public string timeAfterStart; 

    public void SetCameraParameters(Transform t)
    {
        CameraPosition = t.position;
        CameraRotation = t.rotation;
    }

    public void SetGazeParameters(Vector3 gazeOrigin, Vector3 gazeDirection)
    {
        GazeOrigin = gazeOrigin;
        GazeDirection = gazeDirection;
    }

    public HeadData()
    {
        timeAfterStart = Time.time.ToString(); ;
    }

    public HeadData(Transform CameraTransform, Vector3 gazeOrigin, Vector3 gazeDirection)
    {
        timeAfterStart = Time.time.ToString();
        CameraPosition = CameraTransform.position;
        CameraRotation = CameraTransform.rotation;
        GazeOrigin = gazeOrigin;
        GazeDirection = gazeDirection; 
    }
}

[System.Serializable]
public class CustomObject
{
    public string Objectname;
    public Vector3 position;
    public Quaternion rotation;

    public CustomObject(string name, Vector3 position, Quaternion rotation)
    {
        this.Objectname = name;
        this.position = position;
        this.rotation = rotation; 
    }
}