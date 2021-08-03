using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public float time;

    public List<CustomObject> gameObjects;

    public ObjectData(GameObject[] movingObj, float time)
    {
        gameObjects = new List<CustomObject>();

        this.time = time;
        foreach (GameObject obj in movingObj)
        {
            var intObj = new CustomObject(obj.name, obj.transform.position, obj.transform.rotation);
            gameObjects.Add(intObj);
        }
    }
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
            rotations[i] = gameObjects[i].globalRotations;
        }
        return rotations;
    }

    public ObjectData() { }

    public ObjectData(List<GameObject> movingObj, float time)
    {
        gameObjects = new List<CustomObject>();

        this.time = time;
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

    public ObjectData(List<CustomObject> objList, float time)
    {
        this.gameObjects = objList;
        this.time = time;
    }

}



[System.Serializable]
public class CustomObject
{
    public string Objectname;
    public Vector3 globalPosition;
    public Quaternion globalRotations;

    public CustomObject(string name, Vector3 position, Quaternion rotation)
    {
        this.Objectname = name;
        this.globalPosition = position;
        this.globalRotations = rotation;
    }
}