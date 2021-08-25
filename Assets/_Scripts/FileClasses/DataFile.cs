using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataFile
{
    static string fileending = ".json";

    #region load

    /// <summary>
    /// Try from persistent datapath first, then try from resources or generate a default set
    /// </summary>
    /// <param name="filepath">local path with filename without ending</param>
    /// <returns></returns>
    public static T SecureLoad<T>(string filepath) where T: new()
    {
        string jsonString;

        T newData = new T(); 

        // Load from persistent Datapath
        string path = Path.Combine(Application.persistentDataPath, filepath);

        if (File.Exists(path + fileending))
        {
            jsonString = File.ReadAllText(path + fileending);
            JsonFile<T> file = JsonUtility.FromJson<JsonFile<T>>(jsonString);
            newData = file.entries;

            GameManager.Instance.debugText.text = "data loaded from persistent Path.";
        }
        else
        {
            // else load from Resources
            var textFile = Resources.Load<TextAsset>(filepath);
            if (textFile != null)
            {
                JsonFile<T> file = JsonUtility.FromJson<JsonFile<T>>(textFile.text);
                newData = file.entries;
                GameManager.Instance.debugText.text = " data loaded from Resources.";
                Debug.Log(" data from Resources.");

                // Save in persistent Datapath for next time
                Save(newData, Path.GetDirectoryName(filepath), Path.GetFileName(filepath));
            }
            else
            {
                GameManager.Instance.debugText.text = "data not found at path " + filepath;
                Debug.LogWarning("data not found at path " + filepath);

            }
        }
        return newData;
    }

    /// <summary>
    /// Loads file from persistent Datapath
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="completePath">local path without ending</param>
    /// <returns></returns>
    public static T Load<T>(string completePath) where T : new()
    {
        string jsonString;
        
        // Load from persistent Datapath
        string path = Path.Combine(Application.persistentDataPath, completePath);

        if (File.Exists(path + fileending))
        {
            jsonString = File.ReadAllText(path + fileending);
            JsonFile<T> file = JsonUtility.FromJson<JsonFile<T>>(jsonString);
            T newData = file.entries; 

            // debug
            GameManager.Instance.debugText.text = "data loaded from persistent Path.";
            Debug.Log("data loaded from persistent Path.");

            return newData;
        }
        else
        {
            throw new Exception("... path " + path + " not found"); 
        }
    }

    /// <summary>
    /// Helper Function to load User sets into game.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<DataManager.Data> LoadUserSets(List<string> path)
    {
        List<DataManager.Data> newData = new List<DataManager.Data>();

        // get parameters from GameManager
        int N = path.Count;

        // filepath
        string mainFolder = GameManager.Instance.mainFolder;
        string userDataFolder = GameManager.Instance.GeneralSettings.userDataFolder;

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < N; i++)
        {
            var filePath = Path.Combine(mainFolder, userDataFolder, path[i]);
            var userData = DataFile.SecureLoad<UserSettingsData>(filePath);

            var objPath = Path.Combine(mainFolder, userDataFolder, "User" + userData.UserID.ToString(), "settings" + userData.UserID.ToString());
            var objData = DataFile.SecureLoad<ObjectData>(objPath);

            newData.Add(new DataManager.Data(objData, userData));
        }
        return newData;
    }

    #endregion load 

    #region save

    /// <summary>
    /// Save into persistent data path. generates new name if it exists
    /// Begin and End of the string are added automatically
    /// </summary>
    /// <param name="data"></param>
    /// <param name="folderAfterPersistentPath"></param>
    /// <param name="name"></param>
    public static string Save<T>(T data, string folderAfterPersistentPath, string name)
    {
        string directory = GenerateDirectory(Path.Combine(Application.persistentDataPath, folderAfterPersistentPath));
        string fileName = GenerateUniqueFileName(directory, name);

        string filePath = Path.Combine(directory, fileName);

        string jsonString = StartFile();
        jsonString += AddLine<T>(data);
        jsonString += EndFile(false);

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(filePath + fileending, Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data saved into persistent Path.";
        Debug.Log("Data saved into persistent Path: " + filePath);

        return fileName; 
    }

    /// <summary>
    /// Save into persistent data path. generates new name if it exists
    /// </summary>
    /// <param name="data"></param>
    /// <param name="folderAfterPersistentPath"></param>
    /// <param name="name"></param>
    public static void Overwrite<T>(T data, string folderAfterPersistentPath, string name)
    {
        // prepare file path 
        string directory = Path.Combine(Application.persistentDataPath, folderAfterPersistentPath);
        string filePath = Path.Combine(directory, name);

        // prepare file content
        string jsonString = StartFile();
        jsonString += AddLine<T>(data);
        jsonString += EndFile(false);

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(filePath + fileending, Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data overritten in " + filePath;
        Debug.Log("Data overritten in " + filePath);
    }

    /// <summary>
    /// Save complete json string. Begin and End of the string must be added beforehand
    /// generates new filename if exists and saves file
    /// </summary>
    /// <param name="jsonString"></param>
    /// <param name="folderName"></param>
    /// <param name="name">without fileending</param>
    public static string Save(string jsonString, string folderName, string name)
    {
        string directory = GenerateDirectory(Path.Combine(Application.persistentDataPath, folderName));
        string fileName = GenerateUniqueFileName(directory, name);

        string filePath = Path.Combine(directory, fileName);

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(filePath + fileending, Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data saved into persistent Path.";
        Debug.Log("Data saved into " + filePath);

        return fileName; 
    }

    public static void Overwrite(string jsonString, string folderName, string name)
    {
        // prepare file path
        string directory = GenerateDirectory(Path.Combine(Application.persistentDataPath, folderName));
        string filePath = Path.Combine(directory, name);

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(filePath + fileending, Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data saved into persistent Path.";
        Debug.Log("Data overwritten in " + filePath);
    }

    #endregion save


    #region file helper
    /// <summary>
    /// Adds equal beginning to every dataFile
    /// </summary>
    /// <returns></returns>
    public static string StartFile()
    {
        return "{\n \"start\": \"" + DateTime.Now.ToString("F") + "\"," 
            + Environment.NewLine
            + "\"entries\": ";
    }

    /// <summary>
    /// Converts data class into json string and adds new line
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string AddLine<T>(T data)
    {
        string jsonString = JsonUtility.ToJson(data, true);
        jsonString += "," + System.Environment.NewLine;

        return jsonString; 
    }

    /// <summary>
    /// Add equal end to every dataFile
    /// </summary>
    /// <param name="backup"> if the end is of a backupfile, it is marked in the line</param>
    /// <returns></returns>
    public static string EndFile(bool backup)
    {
        if (backup)
            return " \n \"ende\" : \"BACKUP\"\n }";
        else
            return " \n \"ende\" : \"END\"\n }";
    }

    /// <summary>
    /// returns directorypath
    /// </summary>
    /// <param name="directroyPath"></param>
    /// <returns></returns>
    public static string GenerateDirectory(string directroyPath)
    {
        // Generate Directory
        if (!Directory.Exists(directroyPath))
            Directory.CreateDirectory(directroyPath);

        return directroyPath; 
    }

    /// <summary>
    /// returns new filename
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string GenerateUniqueFileName(string directoryPath, string filename)
    {
        string filePath = Path.Combine(directoryPath, filename + fileending);

        // File
        if (File.Exists(filePath))
        {
            // unique name
            filename = filename + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath = Path.Combine(directoryPath, filename);
        }

        return filename; 
    }

    #endregion file helper 

}

/// <summary>
/// Helper Class used in DataFile, to maintain a file structure 
/// </summary>
/// <typeparam name="T"></typeparam>
public class JsonFile<T>
{
    public string start;
    public T entries;
    public string ende; 
}