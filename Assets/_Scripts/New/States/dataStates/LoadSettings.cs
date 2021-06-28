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
        Debug.Log("LoadSettings Enter");

        // get parameters from GameManager
        int n = GameManager.Instance.generalSettings.settingFiles.Count;
        string persistentPath = GameManager.Instance.persistentPath;
        settingsFolder = GameManager.Instance.generalSettings.settingsFolder;
         
        // load each file into own parameter and save in DataManager
        for (int i = 0; i < n; i++)
        {
            var file = GameManager.Instance.generalSettings.settingFiles[i];
            var set = LoadUserSettings(persistentPath + settingsFolder + file);
            DataManager.Instance.UserSettings.Add(set);
        }
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Saves new user settings, if the user applied new 
    /// </summary>
    public void Exit()
    {
        Debug.Log("LoadSettings Exit");
        if (false)      // \TODO save new settings, if user applied new settings
        {
            userSettingsData test = new userSettingsData(
            new List<Object> {
                new Object("eins", Vector3.zero, Vector3.one),
                new Object("zwei", Vector3.zero, Vector3.one)},
            0f, 1, userSettingsData.userSet.AG);

            test.gameObjects = new List<Object>();
            test.gameObjects.Add(new Object("eins", Vector3.zero, Vector3.one));
            test.gameObjects.Add(new Object("zwei", Vector3.zero, Vector3.one));
            test.updateRate = 0f;
            test.UserID = 1;
            test.set = userSettingsData.userSet.AG;

            SaveNewUserSettings(test, settingsFolder, "/settings_user_4.json");
        }
        Debug.LogWarning("LoadSettings::SaveNewUserSettings not implemented.");
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

        if (File.Exists(filepath))
        {
            string jsonString = File.ReadAllText(filepath);
            newGeneralSettings = JsonUtility.FromJson<userSettingsData>(jsonString);
            if (newGeneralSettings != null)
            {
                Debug.Log("LoadSettings::LoadUserSettings successful.");
            }
        }
        else
        {
            Debug.LogError("LoadSettings::LoadUserSettings no file found.");
        }
        Debug.Log(filepath);

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
                System.IO.File.AppendAllText(filepath, jsonString);
                File.WriteAllText(filepath, jsonString);
            }
            else
            {
                Debug.LogError("LoadSettings::SaveNewUserSettings no data to save");
            }
    }

    #endregion 
}
