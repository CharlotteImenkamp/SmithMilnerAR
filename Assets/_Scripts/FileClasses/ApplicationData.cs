using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ApplicationData
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filepath">Path with filename without ending</param>
    /// <param name="loadFromResources">first try from resources</param>
    /// <returns></returns>
    public static ApplicationData Load(string filepath, bool loadFromResources)
    {
        ApplicationData newGeneralSettings = new ApplicationData();
        if (loadFromResources)
        {
            var textFile = Resources.Load<TextAsset>(filepath);
            newGeneralSettings = JsonUtility.FromJson<ApplicationData>(textFile.text);
        }
        else
        {
            string jsonString = File.ReadAllText(filepath + ".json");
            newGeneralSettings = JsonUtility.FromJson<ApplicationData>(jsonString);
        }

        if (newGeneralSettings != null)
        {
            GameManager.Instance.debugText.text = "GameManager::LoadGeneralSettings successful.";
            Debug.Log("GameManager::LoadGeneralSettings successful.");
        }
        else
        {
            GameManager.Instance.debugText.text = "GameManager::LoadGeneralSettings no file found.";
            Debug.LogError("GameManager::LoadGeneralSettings no file found.");
            newGeneralSettings = DefaultGeneralSettingsFile();
            if (newGeneralSettings != null)
            {
                GameManager.Instance.debugText.text = "GameManager::LoadGeneralSettings generated default data.";
                Debug.LogWarning("GameManager::LoadGeneralSettings generated default data.");
            }
        }

        return newGeneralSettings;
    }

    public static void SaveToPersistentPath(ApplicationData data)
    {
        string jsonString = JsonUtility.ToJson(data, true);
        jsonString += Environment.NewLine;

        // override existing text
        UnityEngine.Windows.File.WriteAllBytes(Application.persistentDataPath + "/generalSettings.json", Encoding.ASCII.GetBytes(jsonString));
    }

    public static ApplicationData DefaultGeneralSettingsFile()
    {
        ApplicationData defaultData = new ApplicationData();
        defaultData.dataFiles = new List<string> { "default1.json" };
        defaultData.dataFolder = "data";
        defaultData.settingFiles = new List<string> { };
        defaultData.settingsFolder = "settings";

        return defaultData;
    }

    // Folders 
    public string settingsFolder;
    public string dataFolder;

    // Files
    public List<string> settingFiles;
    public List<string> dataFiles;
    public List<string> incompleteSets;
}

