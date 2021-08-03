using UnityEngine;
using System;
using System.IO;
using System.Text;

class LogData : IState
{
    #region private parameters

    private string dataFolder;
    private string generalFolder; 
    private string directoryPath;
    private string persistentPath;
    private string userID; 

    // filenames
    private string fileName_continuousLogging;
    private string fileName_endState;
    private string fileName_headData;

    // file path
    private string filePath_continuousLogging;
    private string filePath_endState;
    private string filePath_headData;

    // json strings
    private string json_continuousLogging;
    private string json_endState;
    private string json_headData; 

    // data parameters
    private float sampleRate; // in miliseconds
    private float prevTime, currTime, nextTime; 
    private GameType gameType;


    #endregion private parameters

    public LogData(GameType gameType)
    {
        this.gameType = gameType;
    }

    public void Enter()
    {
        GameManager.Instance.debugText.text = "LogData Enter"; 
        Debug.Log("LogData::Enter");

        sampleRate = DataManager.Instance.CurrentSettings.UserData.updateRate;
        Debug.Log("************************************" + sampleRate); 
        dataFolder = GameManager.Instance.GeneralSettings.userDataFolder;
        generalFolder = GameManager.Instance.mainFolder;
        userID = DataManager.Instance.CurrentSettings.UserData.UserID.ToString();

        // default
        json_continuousLogging = "";
        json_endState = "";
        json_headData = ""; 

        // Directory
        directoryPath = Path.Combine(Application.persistentDataPath, generalFolder, dataFolder , "User" + userID);

        // Generate Directory
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);       
        
        // Prepare Files
        if(gameType == GameType.Locations)
        {
            PrepareHeadData(); 
            PrepareObjectData();
            GameManager.Instance.UpdateGeneralSettings(userID, GameType.Locations);
        }
        else if(gameType == GameType.Prices)
        {
            PrepareHeadData();
            GameManager.Instance.UpdateGeneralSettings(userID, GameType.Prices);
        }
        else
            throw new ArgumentException("LogData::Enter no valid GameType.");


        // Set Time for updateRate
        // time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        prevTime = Time.time;  
    }

    public void Execute()
    {
        // time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        currTime = Time.time; 
        // if (time2 - time1 >= sampleRate)
        if(currTime-prevTime >= sampleRate)
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

            prevTime = currTime; 
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
        {
            EndHeadData();
        }  
        else
            throw new ArgumentException("LogData::Exit no valid GameType.");
    }


    #region handle dataTypes
    void PrepareObjectData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSettings;
        fileName_continuousLogging = "User" + currentSet.UserData.UserID.ToString() + "_" + currentSet.UserData.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_MovingObject";
        fileName_endState = "User" + currentSet.UserData.UserID.ToString() + "_" + currentSet.UserData.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_EndObject";

        // start Writing
        json_continuousLogging += DataFile.StartFile();
        json_endState += DataFile.StartFile();
    }

    void ExecuteObjectData()
    {
        var data = GetMovingObject();
        if (data != null && data.gameObjects.Count != 0)
            json_continuousLogging += DataFile.AddLine<ObjectData>(data); 
    }

    void EndObjectData()
    {
        // End continuus logging
        json_continuousLogging += DataFile.EndFile();

        // Save last Object Positions in new File
        json_endState += DataFile.AddLine<ObjectData>(GetObjectsInScene());
        json_endState += DataFile.EndFile();

        DataFile.Save(json_endState, directoryPath, fileName_endState);
        DataFile.Save(json_continuousLogging, directoryPath, fileName_continuousLogging); 
    }

    void PrepareHeadData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSettings;
        fileName_headData = "User" + currentSet.UserData.UserID.ToString() + "_" + currentSet.UserData.set.ToString() + "_" + GameManager.Instance.gameType.ToString() + "_headData";

        directoryPath = DataFile.GenerateDirectory(directoryPath);
        fileName_headData = DataFile.GenerateUniqueFileName(directoryPath, fileName_headData);

        filePath_headData = Path.Combine(directoryPath, fileName_headData + ".json");

        // start Writing
        json_headData += DataFile.StartFile();
    }

    void ExecuteHeadData()
    {
        var data = GetCurrentHeadData();
        if (data != null)
            json_headData += DataFile.AddLine<HeadData>(data);
    }

    void EndHeadData()
    {
        // End continuus logging
        json_headData += DataFile.EndFile();

        DataFile.Save(json_headData, directoryPath, fileName_headData);
        
    }

    #endregion 


    #region get data
    private ObjectData GetMovingObject()
    {
        return new ObjectData(DataManager.Instance.MovingObjects, Time.time); 
    }

    private ObjectData GetObjectsInScene()
    {
        return new ObjectData(DataManager.Instance.ObjectsInScene, Time.time);
    }

    private HeadData GetCurrentHeadData()
    {
        return DataManager.Instance.CurrentHeadData; 
    }

    #endregion get data

}

