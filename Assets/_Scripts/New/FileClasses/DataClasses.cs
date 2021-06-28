using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class userSettingsData
{
    #region methods
    public userSettingsData(List<Object> obj, float updateRate, int userId, userSet set)
    {
        gameObjects = new List<Object>();
        gameObjects.AddRange(obj);

        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set; 
    }

    public userSettingsData()
    {

    }
    #endregion

    #region parameters

    public int UserID;
    public userSet set;
    // saving
    public float updateRate;
    public List<Object> gameObjects;
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
    private List<Object> movingObjects;
}


[System.Serializable]
public class Object
{
    public string Objectname;
    public Vector3 position;
    public Vector3 rotation;

    public Object(string name, Vector3 position, Vector3 rotation)
    {
        this.Objectname = name;
        this.position = position;
        this.rotation = rotation; 
    }
}