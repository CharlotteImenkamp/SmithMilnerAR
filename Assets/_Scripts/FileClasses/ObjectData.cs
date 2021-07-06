﻿using System.Collections.Generic;
using UnityEngine;

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
            var intObj = new CustomObject(obj.name, obj.transform.localPosition, obj.transform.localRotation);
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
public class CustomObject
{
    public string Objectname;
    public Vector3 localPosition;
    public Quaternion localRotation;

    public CustomObject(string name, Vector3 position, Quaternion rotation)
    {
        this.Objectname = name;
        this.localPosition = position;
        this.localRotation = rotation;
    }
}