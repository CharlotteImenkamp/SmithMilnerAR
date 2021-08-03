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

        if(GameManager.Instance.GeneralSettings.newObjectData.Count != 0)
            DataManager.Instance.NewSets = LoadNewSets();
        if(GameManager.Instance.GeneralSettings.newUserData.Count != 0)
            DataManager.Instance.NewUserData = DataFile.LoadUserSets(GameManager.Instance.GeneralSettings.newUserData);
        if (GameManager.Instance.GeneralSettings.incompleteUserData.Count != 0)
            DataManager.Instance.IncompleteUserData = DataFile.LoadUserSets(GameManager.Instance.GeneralSettings.incompleteUserData);
        if (GameManager.Instance.GeneralSettings.completeUserData.Count != 0)
            DataManager.Instance.CompleteUserData = DataFile.LoadUserSets(GameManager.Instance.GeneralSettings.completeUserData); 
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
        int NumNew = GameManager.Instance.GeneralSettings.newObjectData.Count;

        string mainFolder = GameManager.Instance.mainFolder; 
        string objectDataFolder = GameManager.Instance.GeneralSettings.objectDataFolder; 

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < NumNew; i++)
        {
            var filePath = Path.Combine(mainFolder, objectDataFolder, GameManager.Instance.GeneralSettings.newObjectData[i]);
            var set = DataFile.SecureLoad<ObjectData>(filePath);

            newData.Add(set); 
        }

        return newData; 
    }

    #endregion 
}
