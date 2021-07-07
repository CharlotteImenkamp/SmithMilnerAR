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
        DataManager.Instance.IncompleteUserData = LoadIncompleteSets();
        DataManager.Instance.CompleteUserData = LoadCompleteSets(); 
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
        int NumNew = GameManager.Instance.generalSettings.newSets.Count;

        string mainFolder = GameManager.Instance.mainFolder; 
        string objectDataFolder = GameManager.Instance.generalSettings.objectDataFolder; 

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < NumNew; i++)
        {
            var filePath = Path.Combine(mainFolder, objectDataFolder, GameManager.Instance.generalSettings.newSets[i]);
            var set = DataFile.Load<ObjectData>(filePath);

            newData.Add(set); 
        }

        return newData; 
    }

    private List<ObjectData> LoadIncompleteSets()
    {
        List<ObjectData> newData = new List<ObjectData>();

        // get parameters from GameManager
        int N = GameManager.Instance.generalSettings.incompleteUserData.Count;

        // filepath
        string mainFolder = GameManager.Instance.mainFolder;
        string userDataFolder = GameManager.Instance.generalSettings.userDataFolder; 

        // load each file into own parameter and save in DataManager
        for (int i = 0; i < N; i++)
        {
            var filePath = Path.Combine(mainFolder, userDataFolder, GameManager.Instance.generalSettings.incompleteUserData[i]);
            var set = DataFile.Load<ObjectData>(filePath);

            newData.Add(set);
        }

        return newData;
    }

    private List<ObjectData> LoadCompleteSets()
    {
        List<ObjectData> newData = new List<ObjectData>();

        // get parameters from GameManager
        int NumNew = GameManager.Instance.generalSettings.completeUserData.Count;

        string mainFolder = GameManager.Instance.mainFolder;
        string userDataFolder = GameManager.Instance.generalSettings.userDataFolder;


        // load each file into own parameter and save in DataManager
        for (int i = 0; i < NumNew; i++)
        {
            var filePath = Path.Combine(mainFolder, userDataFolder, GameManager.Instance.generalSettings.completeUserData[i]);
            var set = DataFile.Load<ObjectData>(filePath);

            newData.Add(set);
        }

        return newData;
    }

    #endregion 
}
