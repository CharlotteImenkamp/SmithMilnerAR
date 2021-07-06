using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class userSettingsData
{
    #region methods
    public userSettingsData(List<CustomObject> obj, float updateRate, int userId, userSet set, gameState state)
    {
        gameObjects = new List<CustomObject>();
        gameObjects.AddRange(obj);

        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set;
        this.state = state;
    }

    public userSettingsData(List<GameObject> Objects, float updateRate, int userId, userSet set, gameState state)
    {
        gameObjects = new List<CustomObject>();

        foreach (GameObject obj in Objects)
        {
            gameObjects.Add(new CustomObject(obj.name, obj.transform.localPosition, obj.transform.localRotation));
        }

        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set;
        this.state = state;
    }

    public userSettingsData() { }

    /// <summary>
    /// Load user settings into class instance.         /\ TODO: zusammen mit GameManager load general settings als helfer methode
    /// </summary>
    /// <param name="filepath">complete path without fileending</param>
    /// <returns></returns>
    public static userSettingsData LoadUserSettings(string filepath, bool loadFromResources)
    {
        userSettingsData newGeneralSettings = new userSettingsData();
        if (loadFromResources)
        {
            var textFile = Resources.Load<TextAsset>(filepath);
            newGeneralSettings = JsonUtility.FromJson<userSettingsData>(textFile.text);
        }
        else
        {
            string jsonString = File.ReadAllText(filepath + ".json");
            newGeneralSettings = JsonUtility.FromJson<userSettingsData>(jsonString);
        }


        if (newGeneralSettings == null)
        {
            throw new FileLoadException("LoadSettings::LoadUserSettings no settings loaded.");
        }

        return newGeneralSettings;
    }

    public Vector3[] GetObjectPositions()
    {
        Vector3[] positions = new Vector3[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            positions[i] = gameObjects[i].localPosition;
        }
        return positions;
    }

    public Quaternion[] GetObjectRotations()
    {
        Quaternion[] rotations = new Quaternion[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            rotations[i] = gameObjects[i].localRotation;
        }
        return rotations;
    }

    public static void SaveNewFile(userSettingsData data, string folderAfterPersistentPath, string name)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, folderAfterPersistentPath);

        string filePath = DataFile.GenerateFilePath(directoryPath, name);

        // start Writing
        DataFile.StartFile(filePath);
        DataFile.AddLine<userSettingsData>(data, filePath);     // \TODO This hier richtig??
        DataFile.EndFile(filePath);
    }
    #endregion

    #region parameters

    // User
    public int UserID;
    public userSet set;
    public gameState state;
    public enum userSet { JG, AG, AK };
    public enum gameState { locationsCompleted, pricesCompleted, None }

    // Game State
    public List<CustomObject> gameObjects;

    // Saving
    public float updateRate;
    #endregion 

}



