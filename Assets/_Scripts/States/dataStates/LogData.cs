using UnityEngine;
using System.IO;
using System;

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
        Debug.Log("LogData::Enter");
        sampleRate = DataManager.Instance.CurrentSettings.updateRate;

        // Directory
        dataFolder = GameManager.Instance.generalSettings.dataFolder;
        persistentPath = Application.persistentDataPath;
        directoryPath = Path.Combine(persistentPath, dataFolder, "User_" + DataManager.Instance.CurrentSettings.UserID.ToString());

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

        // generate continuous Logging File
        filePath_continuousLogging = Path.Combine(directoryPath, fileName_continuousLogging + ".json");
        if (File.Exists(filePath_continuousLogging))
        {
            // unique name
            fileName_continuousLogging = fileName_continuousLogging + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath_continuousLogging = Path.Combine(directoryPath, fileName_continuousLogging + ".json");
        }

        // Generate end Log File
        filePath_endState = Path.Combine(directoryPath, fileName_endState + ".json");
        if (File.Exists(filePath_continuousLogging))
        {
            // unique name
            fileName_endState = fileName_endState + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath_endState = Path.Combine(directoryPath, fileName_endState + ".json");
        }

        // start Writing
        StartFile(filePath_continuousLogging);
        AddFileToGeneralSettings(fileName_continuousLogging); 

        StartFile(filePath_endState);
        AddFileToGeneralSettings(fileName_endState); 
    }

    void ExecuteObjectData()
    {
        var data = GetMovingObject();
        if (data != null && data.movingObjects.Count != 0)
            AddLine<ObjectData>(data, filePath_continuousLogging);

        if(data.movingObjects.Count != 0)
            Debug.Log(data.movingObjects.Count); 
    }

    void EndObjectData()
    {
        // End continuus logging
        EndFile(filePath_continuousLogging);

        // Save last Object Positions in new File
        AddLine(GetObjectsInScene(), filePath_endState);
        EndFile(filePath_endState);
    }

    void PrepareHeadData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSettings;
        fileName_headData = "User" + currentSet.UserID.ToString() + "_" + currentSet.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_headData";

        // Generate end Log File
        filePath_headData = Path.Combine(directoryPath, fileName_headData + ".json");
        if (File.Exists(filePath_headData))
        {
            // unique name
            fileName_headData = fileName_headData + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath_headData = Path.Combine(directoryPath, fileName_headData + ".json");
        }

        // start Writing
        StartFile(filePath_headData);
        AddFileToGeneralSettings(fileName_headData); 
    }

    void ExecuteHeadData()
    {
        var data = GetCurrentHeadData();
        if (data != null)
            AddLine<HeadData>(data, filePath_headData);
    }

    void EndHeadData()
    {
        // End continuus logging
        EndFile(filePath_headData);
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
    private void StartFile(string filepath)
    {
        string start = "{ \n \"entries " + DateTime.Now.ToString("D") + DateTime.Now.ToString("F") + "\":[" + Environment.NewLine;
        File.AppendAllText(filepath, start);
    }

    private void AddFileToGeneralSettings(string fileName)
    {
        GameManager.Instance.generalSettings.dataFiles.Add(fileName); 
    }

    private void AddLine<T>(T data, string filepath)
    {
        string jsonString = JsonUtility.ToJson(data, true);

        jsonString += "," + System.Environment.NewLine;
        File.AppendAllText(filepath, jsonString);
    }

    private void EndFile(string filepath)
    {
        string ende = " \"END\"\n ]}\n";
        File.AppendAllText(filepath, ende);
    }
    #endregion 

}

