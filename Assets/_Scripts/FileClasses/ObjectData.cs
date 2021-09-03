using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public float time;
    public Vector3 positionOffset; 

    public List<CustomObject> gameObjects;

    #region constructors
    public ObjectData(GameObject[] movingObj, float time, Vector3 positionOffset)
    {
        gameObjects = new List<CustomObject>();

        this.time = time;
        this.positionOffset = positionOffset; 
        foreach (GameObject obj in movingObj)
        {
            var intObj = new CustomObject(obj.name, obj.transform.position, obj.transform.rotation);
            gameObjects.Add(intObj);
        }
    }

    public ObjectData(List<GameObject> movingObj, float time, Vector3 positionOffset)
    {
        gameObjects = new List<CustomObject>();

        this.time = time;
        this.positionOffset = positionOffset;
        foreach (GameObject obj in movingObj)
        {
            // Remove naming Conventions from Instantiating
            if (obj.name.Contains("(Clone)"))
            {
                obj.name = obj.name.Replace("(Clone)", "");
            }
            var intObj = new CustomObject(obj.name, obj.transform.position, obj.transform.rotation);
            gameObjects.Add(intObj);
        }
    }

    public ObjectData(List<CustomObject> objList, float time, Vector3 positionOffset)
    {
        this.gameObjects = objList;
        this.time = time;
    }

    public ObjectData() { }
    #endregion constructors

    public Vector3[] GetObjectPositions()
    {
        Vector3[] positions = new Vector3[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            positions[i] = gameObjects[i].globalPosition;
        }
        return positions;
    }

    public Quaternion[] GetObjectRotations()
    {
        Quaternion[] rotations = new Quaternion[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            rotations[i] = gameObjects[i].globalRotation;
        }
        return rotations;
    }



}



[System.Serializable]
public class CustomObject
{
    public string Objectname;
    public Vector3 globalPosition;
    public Quaternion globalRotation;

    public CustomObject(string name, Vector3 position, Quaternion rotation)
    {
        this.Objectname = name;
        this.globalPosition = position;
        this.globalRotation = rotation;
    }
}