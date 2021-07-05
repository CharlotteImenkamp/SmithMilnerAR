using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

class LoadSettings : IState
{

    private string settingsFolder; 

    /// <summary>
    /// load saved setting files. The filepath is listed in the GameManager
    /// </summary>
    public void Enter()
    {
        GameManager.Instance.debugText.text = "LoadSettings Enter"; 
        Debug.Log("LoadSettings Enter");

        // get parameters from GameManager
        int n = GameManager.Instance.generalSettings.settingFiles.Count;
        string persistentPath = GameManager.Instance.persistentPath;
        settingsFolder = GameManager.Instance.generalSettings.settingsFolder;

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < n; i++)
        {
            var file = GameManager.Instance.generalSettings.settingFiles[i];
            var set = LoadUserSettings("DataFiles/settings/" + file);
            DataManager.Instance.UserSettings.Add(set);
        }
    }

    public void Execute()
    {
    }

    /// <summary>
    /// Saves new user settings, if the user applied new 
    /// </summary>
    public void Exit()
    {
        GameManager.Instance.debugText.text = "LoadSettings Exit"; 

        Debug.Log("LoadSettings Exit");

    }

    #region dataManagement

    /// <summary>
    /// Load user settings into class instance.         /\ TODO: zusammen mit GameManager load general settings als helfer methode
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private userSettingsData LoadUserSettings(string filepath)
    {
        userSettingsData newGeneralSettings = new userSettingsData();

        var textFile = Resources.Load<TextAsset>(filepath); 
        // string jsonString = File.ReadAllText(filepath);
        newGeneralSettings = JsonUtility.FromJson<userSettingsData>(textFile.text);
        if (newGeneralSettings == null)
        {
            throw new FileLoadException("LoadSettings::LoadUserSettings no settings loaded.");
        }

        return newGeneralSettings;
    }

    /// <summary>
    /// \TODO connect with buttons
    /// </summary>
    /// <param name="data"></param>
    /// <param name="foldername"></param>
    /// <param name="filename"></param>
    private void SaveNewUserSettings(userSettingsData data, string foldername, string filename)
    {
            string persistentPath = GameManager.Instance.persistentPath;
            string filepath = persistentPath + foldername + filename;
            // save data in json 
            if (data != null)
            {
                string jsonString = JsonUtility.ToJson(data, true);
                jsonString += System.Environment.NewLine;
                File.AppendAllText(filepath, jsonString);
                File.WriteAllText(filepath, jsonString);
            }
            else
            {
                Debug.LogError("LoadSettings::SaveNewUserSettings no data to save");
            }
    }

    #endregion 
}
