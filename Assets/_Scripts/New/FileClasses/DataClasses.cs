using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSettings
{
    // Object
    private Object[] gameObjects; 
    private int ObjectNumber;

    // saving
    private float updateRate;

    //login
    int UserID;
    userSet set; 

    enum userSet { JG, AG, AK}; 
}

[System.Serializable]
public class applicationData
{

    public string[] settingFiles;
    public string dataFolder;
    public string[] dataFiles;
}

[System.Serializable]
public class Data
{
    private Time time;
    private List<Object> movingObjects; 

}

[System.Serializable]
public struct Object
{
    string Objectname;
    Vector3 position;
    Vector3 rotation;
}