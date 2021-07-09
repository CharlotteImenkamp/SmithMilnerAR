using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

class LoadSettings : IState
{

    /// <summary>
    /// load saved setting files. The filepath is listed in the GameManager
    /// </summary>
    public void Enter()
    {
        GameManager.Instance.debugText.text = "LoadSettings Enter"; 
        Debug.Log("LoadSettings Enter");

        DataManager.Instance.NewSets = LoadNewSets();
        DataManager.Instance.NewUserData = LoadSets(GameManager.Instance.generalSettings.newUserData); 
        DataManager.Instance.IncompleteUserData = LoadSets(GameManager.Instance.generalSettings.incompleteUserData);
        DataManager.Instance.CompleteUserData = LoadSets(GameManager.Instance.generalSettings.completeUserData); 
    }

    public void Execute()
    {
    }

    /// <summary>
    /// Saves new user settings, if the user applied new    //\TODO
    /// </summary>
    public void Exit()
    {
        GameManager.Instance.debugText.text = "LoadSettings Exit"; 

        Debug.Log("LoadSettings Exit");

    }

    #region Load Data

    private List<ObjectData> LoadNewSets()
    {
        List<ObjectData> newData = new List<ObjectData>(); 

        // get parameters from GameManager
        int NumNew = GameManager.Instance.generalSettings.newObjectData.Count;

        string mainFolder = GameManager.Instance.mainFolder; 
        string objectDataFolder = GameManager.Instance.generalSettings.objectDataFolder; 

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < NumNew; i++)
        {
            var filePath = Path.Combine(mainFolder, objectDataFolder, GameManager.Instance.generalSettings.newObjectData[i]);
            var set = DataFile.SecureLoad<ObjectData>(filePath);

            newData.Add(set); 
        }

        return newData; 
    }

    private List<DataManager.Data> LoadSets(List<string> path)
    {
        List<DataManager.Data> newData = new List<DataManager.Data>();

        // get parameters from GameManager
        int N = path.Count;

        // filepath
        string mainFolder = GameManager.Instance.mainFolder;
        string userDataFolder = GameManager.Instance.generalSettings.userDataFolder; 

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < N; i++)
        {
            var filePath = Path.Combine(mainFolder, userDataFolder, path[i]); 
            var userData = DataFile.SecureLoad<UserSettingsData>(filePath );       

            var objPath = Path.Combine(mainFolder, userDataFolder, "User" + userData.UserID.ToString(), "settings" + userData.UserID.ToString());
            var objData = DataFile.SecureLoad<ObjectData>(objPath);

            newData.Add(new DataManager.Data(objData, userData));
        }
        return newData;
    }
    #endregion 
}
