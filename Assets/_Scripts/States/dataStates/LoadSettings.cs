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
            var set = userSettingsData.LoadUserSettings(Path.Combine(Application.persistentDataPath, "DataFiles", "settings", file), false);

            // use backup in resources
            if (set == null)
            {
                set = userSettingsData.LoadUserSettings("DataFiles/settings/" + file, true);
            }
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



    #endregion 
}
