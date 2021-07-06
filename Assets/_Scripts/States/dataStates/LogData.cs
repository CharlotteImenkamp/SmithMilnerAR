using UnityEngine;
using System;
using System.IO;
using System.Text;

class LogData : IState
{
    #region private parameters

    private string dataFolder;
    private string directoryPath;
    private string persistentPath; 
    private string fileName_continuousLogging;
    private string fileName_endState;
    private string fileName_headData; 
    private float sampleRate;
    private long time1, time2;

    private string filePath_continuousLogging;
    private string filePath_endState;
    private string filePath_headData;

    #endregion private parameters

    private GameType gameType; 

    public LogData(GameType gameType)
    {
        this.gameType = gameType;
    }

    public void Enter()
    {
        GameManager.Instance.debugText.text = "LogData Enter"; 

        Debug.Log("LogData::Enter");
        sampleRate = DataManager.Instance.CurrentSettings.updateRate;

        // Directory
        dataFolder = GameManager.Instance.generalSettings.dataFolder;   //TODO nutzen
        directoryPath = Path.Combine(Application.persistentDataPath, "data", "User_" + DataManager.Instance.CurrentSettings.UserID.ToString());

        // Generate Directory
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);       
        
        // Prepare Files
        if(gameType == GameType.Locations)
        {
            PrepareHeadData(); 
            PrepareObjectData(); 
        }
        else if(gameType == GameType.Prices)
            PrepareHeadData(); 
        else
            throw new ArgumentException("LogData::Enter no valid GameType."); 
        

        // Set Time for updateRate
        time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    public void Execute()
    {
        time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        if (time2 - time1 >= sampleRate)
        {
            if (gameType == GameType.Locations)
            {
                ExecuteHeadData();
                ExecuteObjectData();
            }
            else if (gameType == GameType.Prices)
                ExecuteHeadData();
            else
                throw new ArgumentException("LogData::Execute no valid GameType.");
        }
    }

    public void Exit()
    {
        GameManager.Instance.debugText.text = "LogData Exit"; 

        Debug.Log("LogData::Exit");

        if (gameType == GameType.Locations)
        {
            EndHeadData();
            EndObjectData();
        }
        else if (gameType == GameType.Prices)
            EndHeadData();
        else
            throw new ArgumentException("LogData::Exit no valid GameType.");
    }


    #region handle dataTypes
    void PrepareObjectData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSettings;
        fileName_continuousLogging = "User" + currentSet.UserID.ToString() + "_" + currentSet.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_MovingObject";
        fileName_endState = "User" + currentSet.UserID.ToString() + "_" + currentSet.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_EndObject";

        filePath_continuousLogging = DataFile.GenerateFilePath(directoryPath, fileName_continuousLogging);
        filePath_endState =          DataFile.GenerateFilePath(directoryPath, fileName_endState); 

        // start Writing
        DataFile.StartFile(filePath_continuousLogging);
        AddFileToGeneralSettings(fileName_continuousLogging); 

        DataFile.StartFile(filePath_endState);
        AddFileToGeneralSettings(fileName_endState); 
    }

    void ExecuteObjectData()
    {
        var data = GetMovingObject();
        if (data != null && data.movingObjects.Count != 0)
            DataFile.AddLine<ObjectData>(data, filePath_continuousLogging); 

        if(data.movingObjects.Count != 0)
            Debug.Log(data.movingObjects.Count); 
    }

    void EndObjectData()
    {
        // End continuus logging
        DataFile.EndFile(filePath_continuousLogging);

        // Save last Object Positions in new File
        DataFile.AddLine(GetObjectsInScene(), filePath_endState);
        DataFile.EndFile(filePath_endState);
    }

    void PrepareHeadData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSettings;
        fileName_headData = "User" + currentSet.UserID.ToString() + "_" + currentSet.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_headData";

        filePath_headData = DataFile.GenerateFilePath(directoryPath, fileName_headData); 

        // start Writing
        DataFile.StartFile(filePath_headData);
        AddFileToGeneralSettings(fileName_headData); 
    }

    void ExecuteHeadData()
    {
        var data = GetCurrentHeadData();
        if (data != null)
            DataFile.AddLine<HeadData>(data, filePath_headData);
    }

    void EndHeadData()
    {
        // End continuus logging
        DataFile.EndFile(filePath_headData);
    }

    #endregion 


    #region get data
    private ObjectData GetMovingObject()
    {
        return new ObjectData(DataManager.Instance.MovingObjects, DateTime.Now.Hour); 
    }

    private ObjectData GetObjectsInScene()
    {
        return new ObjectData(DataManager.Instance.ObjectsInScene, DateTime.Now.Hour);
    }

    private HeadData GetCurrentHeadData()
    {
        return DataManager.Instance.CurrentHeadData; 
    }

    #endregion get data


    #region handle data

    private void AddFileToGeneralSettings(string fileName)
    {
        GameManager.Instance.generalSettings.dataFiles.Add(fileName);
    }
    #endregion 

}

