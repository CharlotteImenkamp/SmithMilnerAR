﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataFile
{

    /// <summary>
    /// Try from persistent datapath first, then try from resources or generate a default set
    /// </summary>
    /// <param name="filepath">local path with filename without ending</param>
    /// <returns></returns>
    public static T SecureLoad<T>(string filepath, string filename) where T: new()
    {
        string jsonString;
        T newData = new T(); 

        // Load from persistent Datapath
        string path = Path.Combine(Application.persistentDataPath, filepath, filename);

        if (File.Exists(path + ".json"))
        {
            jsonString = File.ReadAllText(path + ".json");
            JsonFile<T> file = JsonUtility.FromJson<JsonFile<T>>(jsonString);
            newData = file.entries;

            GameManager.Instance.debugText.text = "data loaded from persistent Path.";
            Debug.Log("data loaded from persistent Path.");
        }
        else
        {
            throw new System.NotImplementedException(" apply new structiure"); 
            // else load from Resources
            path = Path.Combine(filepath, filename);
            var textFile = Resources.Load<TextAsset>(path);
            if (textFile != null)
            {
                newData = JsonUtility.FromJson<T>(textFile.text);
                GameManager.Instance.debugText.text = "UserSettings data loaded from Resources.";
                Debug.Log("UserSettings data from Resources.");

                // Save in persistent Datapath for next time
                Save(newData, filepath, filename);
            }

            // or generate default data
            else
            {
                GameManager.Instance.debugText.text = "UserSettings data not found.";
                Debug.LogWarning("UserSettings data not found.");

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

        if (File.Exists(path + ".json"))
        {
            jsonString = File.ReadAllText(path + ".json");
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
    /// Save into persistent data path. generates new name if it exists
    /// </summary>
    /// <param name="data"></param>
    /// <param name="folderAfterPersistentPath"></param>
    /// <param name="name"></param>
    public static void Save<T>(T data, string folderAfterPersistentPath, string name)
    {
        string directory = GenerateDirectory(Path.Combine(Application.persistentDataPath, folderAfterPersistentPath));
        string fileName = GenerateUniqueFileName(directory, name);

        string filePath = Path.Combine(directory, fileName);

        string jsonString = StartFile();
        jsonString += AddLine<T>(data);
        jsonString += EndFile(); 

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes( filePath + ".json", Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data saved into persistent Path.";
        Debug.LogWarning("Data saved into persistent Path.");
    }

    /// <summary>
    /// Save into persistent data path. generates new name if it exists
    /// </summary>
    /// <param name="data"></param>
    /// <param name="folderAfterPersistentPath"></param>
    /// <param name="name"></param>
    public static void Override<T>(T data, string folderAfterPersistentPath, string name)
    {

        string directory = Path.Combine(Application.persistentDataPath, folderAfterPersistentPath);
        string filePath = Path.Combine(directory, name);

        string jsonString = StartFile();
        jsonString += AddLine<T>(data);
        jsonString += EndFile();

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(filePath + ".json", Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data overritten in " + filePath;
        Debug.LogWarning("Data overritten in " + filePath);
    }

    /// <summary>
    /// Save complete json string. generates new filename if exists and saves
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
        UnityEngine.Windows.File.WriteAllBytes(filePath + ".json", Encoding.ASCII.GetBytes(jsonString));

        // debug
        GameManager.Instance.debugText.text = "Data saved into persistent Path.";
        Debug.LogWarning("Data saved into persistent Path.");

        return filePath; 
    }


    public static string StartFile()
    {
        return "{\n \"start\": \"" + DateTime.Now.ToString("F") + "\"," 
            + Environment.NewLine
            + "\"entries\":";
    }

    public static string AddLine<T>(T data)
    {

        string jsonString = JsonUtility.ToJson(data, true);
        jsonString += "," + System.Environment.NewLine;

        return jsonString; 
    }

    public static string EndFile()
    {
        return ", \n \"ende\" : \"END\"\n }";
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
        string filePath = Path.Combine(directoryPath, filename + ".json");

        // File
        if (File.Exists(filePath))
        {
            // unique name
            filename = filename + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath = Path.Combine(directoryPath, filename);
        }

        return filename; 
    }

}

public class JsonFile<T>
{
    public string start;
    public T entries;
    public string ende; 
}