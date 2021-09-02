using System;
using System.IO;
using UnityEngine;

class LogData : IState
{
    #region private parameters

    private string dataFolder;
    private string generalFolder; 
    private string directoryPath;
    private string persistentPath;
    private string userID; 

    // filenames
    private string name_contLog;
    private string name_end;
    private string name_headData;
    private string name_start;

    // unique filenames
    private string uqName_contLog;
    private string uqName_end;
    private string uqName_headData;
    private string uqName_start;

    // json strings
    private string data_contLog;
    private string data_endState;
    private string data_startState;
    private string data_headData; 

    // data parameters
    private float sampleRate; // in miliseconds

    // time parameters
    private float prevTime, currTime, startTime;  // in seconds
    private bool firstLog; 
    private float backupPeriod;  // in seconds

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

        sampleRate = DataManager.Instance.CurrentSet.UserData.updateRate;
        dataFolder = GameManager.Instance.GeneralSettings.userDataFolder;
        generalFolder = GameManager.Instance.MainFolder;
        userID = DataManager.Instance.CurrentSet.UserData.UserID.ToString();

        // default
        data_contLog = "";
        data_endState = "";
        data_headData = "";

        // time
        firstLog = true;
        backupPeriod = GameManager.Instance.backupPeriod; 

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
        startTime = Time.time; 
    }

    public void Execute()
    {
        currTime = Time.time; 

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

        // Backup Save
        if ((Time.time - startTime) % backupPeriod <= (0 + Time.deltaTime))
        {
            // Save Data
            if (gameType == GameType.Locations)
            {
                BackupObjectData(firstLog);
                BackupHeadData(firstLog);
            }
            else if (gameType == GameType.Prices)
                BackupHeadData(firstLog);
            else
                throw new ArgumentException("LogData::Execute no valid GameType.");

            if (firstLog)
                firstLog = false;
        }
    }

    public void Exit()
    {
        GameManager.Instance.debugText.text = "LogData Exit"; 

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


    #region backup
    private void BackupObjectData(bool firstSave)
    {
        // update endstate to current state
        var tmp_end = data_endState + DataFile.AddLine<ObjectData>(GetObjectsInScene());
        tmp_end = tmp_end.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        tmp_end += DataFile.EndFile(true);

        // save copy of data 
        var tmp_contLog = data_contLog.TrimEnd('\n').TrimEnd('\r').TrimEnd(','); 
        tmp_contLog += DataFile.EndFile(true);

        var tmp_start = data_startState.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        tmp_start = tmp_start + DataFile.EndFile(true);
        

        if (firstSave)
        {
            // save file and get unique file name to overwride file later
            uqName_contLog = DataFile.Save(tmp_contLog, directoryPath, name_contLog);
            uqName_end    = DataFile.Save(tmp_end,      directoryPath, name_end);
            uqName_start  = DataFile.Save(tmp_start,    directoryPath, name_start); 
        }
        else
        {
            // override file 
            DataFile.Overwrite(tmp_contLog, directoryPath, uqName_contLog);
            DataFile.Overwrite(tmp_end,     directoryPath, uqName_end);
            DataFile.Overwrite(tmp_start,   directoryPath, uqName_start);
        }
    }

    private void BackupHeadData(bool firstSave)
    {
        var tmp_headData = data_headData.TrimEnd('\n').TrimEnd('\r').TrimEnd(','); 
        tmp_headData += DataFile.EndFile(true);

        // save file and get unique file name to overwrite file later
        if (firstSave)
            uqName_headData = DataFile.Save(tmp_headData, directoryPath, name_headData); 
        else
            DataFile.Overwrite(tmp_headData, directoryPath, uqName_headData);
    }

    #endregion 

    #region handle dataTypes
    void PrepareObjectData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSet;
        name_contLog  = "MovingObject" + GameManager.Instance.gameType.ToString() + currentSet.UserData.UserID.ToString();
        name_end      = "EndObject"    + GameManager.Instance.gameType.ToString() + currentSet.UserData.UserID.ToString();
        name_start    = "StartObject"  + GameManager.Instance.gameType.ToString() + currentSet.UserData.UserID.ToString();

        // Start Writing
        data_contLog  += DataFile.StartFile();
        data_endState       += DataFile.StartFile();
        data_startState     += DataFile.StartFile();
        data_startState     += DataFile.AddLine<ObjectData>(GetObjectsInScene());
    }

    void ExecuteObjectData()
    {
        var data = GetMovingObject();
        if (data != null && data.gameObjects.Count != 0)
            data_contLog += DataFile.AddLine<ObjectData>(data); 
    }

    void EndObjectData()
    {
        // Last Object Positions
        data_endState += DataFile.AddLine<ObjectData>(GetObjectsInScene());

        // Last Line
        data_endState = data_endState.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        data_endState   += DataFile.EndFile(false);

        data_contLog = data_contLog.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        data_contLog += DataFile.EndFile(false);

        data_startState = data_startState.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        data_startState += DataFile.EndFile(false);

        // Overwrite Backup Files
        if (uqName_end == "" || uqName_end == null)
            DataFile.Overwrite(data_endState,   directoryPath, name_end);
        else
            DataFile.Overwrite(data_endState, directoryPath, uqName_end);

        if (uqName_contLog == "" || uqName_contLog == null)
            DataFile.Overwrite(data_contLog,    directoryPath, name_contLog);
        else
            DataFile.Overwrite(data_contLog, directoryPath, uqName_contLog);

        if (uqName_start == "" || uqName_start == null)
            DataFile.Overwrite(data_startState, directoryPath, name_start); 
        else
            DataFile.Overwrite(data_startState, directoryPath, uqName_start);
    }

    void PrepareHeadData()
    {
        // Filenames
        var currentSet = DataManager.Instance.CurrentSet;
        name_headData = "HeadData" + GameManager.Instance.gameType.ToString() + currentSet.UserData.UserID.ToString(); 

        directoryPath = DataFile.GenerateDirectory(directoryPath);
        name_headData = DataFile.GenerateUniqueFileName(directoryPath, name_headData);

        // Start Writing
        data_headData += DataFile.StartFile();
    }

    void ExecuteHeadData()
    {
        // Add Data to String
        var data = GetCurrentHeadData();
        if (data != null)
            data_headData += DataFile.AddLine<HeadData>(data);
    }

    void EndHeadData()
    {
        // End continuus logging
        data_headData = data_headData.TrimEnd('\n').TrimEnd('\r').TrimEnd(',');
        data_headData += DataFile.EndFile(false);

        if(uqName_headData == "" || uqName_headData == null )
            DataFile.Overwrite(data_headData, directoryPath, name_headData);   
        else
            DataFile.Overwrite(data_headData, directoryPath, uqName_headData);
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

