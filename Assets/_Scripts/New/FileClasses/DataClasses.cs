using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class userSettingsData
{
    #region methods
    public userSettingsData(List<InteractionObject> obj, float updateRate, int userId, userSet set)
    {
        gameObjects = new List<InteractionObject>();
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
    public List<InteractionObject> gameObjects;
    public enum userSet { JG, AG, AK };

    #endregion 

}


[System.Serializable]
public class applicationData
{
    public string settingsFolder; 
    public List<string> settingFiles;
    public string dataFolder;
    public List<string> dataFiles;
}


[System.Serializable]
public class loggingData
{
    private Time time;
    private List<InteractionObject> movingObjects;
}


[System.Serializable]
public class InteractionObject
{
    public string Objectname;
    public Vector3 position;
    public Quaternion rotation;

    public InteractionObject(string name, Vector3 position, Quaternion rotation)
    {
        this.Objectname = name;
        this.position = position;
        this.rotation = rotation; 
    }
}