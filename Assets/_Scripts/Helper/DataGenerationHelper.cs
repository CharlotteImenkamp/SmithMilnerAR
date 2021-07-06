using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class DataGenerationHelper : MonoBehaviour
{
    [SerializeField]
    private bool saveNewSettings; 
    // Start is called before the first frame update
    void Start()
    {
        if (saveNewSettings)
        {
        //    // generate path
        //    string savePath = Path.Combine(Application.persistentDataPath, "test");
        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath); 
        //    }

        //    // generate new file
        //    FileStream file; 
        //    string fileName = "testData"; 
        //    string filePath = Path.Combine(savePath, fileName + ".json");
        //    if (!File.Exists(filePath))
        //    {
        //        file = File.Create(filePath);
        //    }
        //    else
        //    {
        //        // unique name
        //        fileName = fileName + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();      
        //        filePath = Path.Combine(savePath, fileName + ".json");
        //        //file = File.Create(filePath);
        //    }

        //    // start formatter
        //    BinaryFormatter bf = new BinaryFormatter();

        //    // start writer
        //    StartFile(bf, filePath); 
    

        //    // Write into file
        //    var test = new CustomObject("eins", Vector3.zero, Quaternion.identity);
        //    var list = new List<CustomObject>();
        //    list.Add(test); 
            
        //    AddLine(bf, new ObjectData(list, Time.deltaTime), filePath);
        //    AddLine(bf, new ObjectData(list, Time.deltaTime + 1), filePath);

        //    // end file
        //    EndFile(bf, filePath); 

            SaveNewUserSettings(SaveCurrentLayout(), "", "settings_1", ".json");
            //SaveNewUserSettings(GenerateTestSet(), "", "/settings_user_4.json");
        }
    }
    private void StartFile(BinaryFormatter bf, string filepath)
    {
        string start = "{ \n \"entries\":[" + Environment.NewLine;
        File.AppendAllText(filepath, start);
    }

    private void AddLine(BinaryFormatter bf, ObjectData data, string filepath)
    {
        string jsonString = JsonUtility.ToJson(data, true);

        jsonString += "," + System.Environment.NewLine;
        File.AppendAllText(filepath, jsonString);

    }

    private void EndFile(BinaryFormatter bf, string filepath)
    {
        string ende = " \"END\"\n ]}\n";
        File.AppendAllText(filepath, ende);
    }

    userSettingsData SaveCurrentLayout()
    {
        GameObject[] intObj = GameObject.FindGameObjectsWithTag("InteractionObject");
        List<CustomObject> obj = new List<CustomObject>(); 


        foreach (GameObject o in intObj)
        {
            obj.Add(new CustomObject(o.name, o.transform.position, o.transform.rotation)); 
        }

        userSettingsData res = new userSettingsData(
                obj,
                0.1f,
                1, 
                userSettingsData.userSet.AG,
                userSettingsData.gameState.pricesCompleted
                );

        return res; 
    }

    userSettingsData GenerateTestSet()
    {
        userSettingsData test = new userSettingsData(
    new List<CustomObject> {
                new CustomObject("eins", Vector3.zero, Quaternion.identity),
                new CustomObject("zwei", Vector3.zero, Quaternion.identity)},
    0f, 1, userSettingsData.userSet.AG, userSettingsData.gameState.None);

        test.gameObjects = new List<CustomObject>();
        test.gameObjects.Add(new CustomObject("eins", Vector3.zero, Quaternion.identity));
        test.gameObjects.Add(new CustomObject("zwei", Vector3.zero, Quaternion.identity));
        test.updateRate = 0f;
        test.UserID = 1;
        test.set = userSettingsData.userSet.AG;

        return test; 
    } 
    private void SaveNewUserSettings(userSettingsData data, string foldername, string filename, string fileending)
    { 
        string persistentPath = Application.persistentDataPath; 
        
        string filepath = Path.Combine(persistentPath, filename + fileending);

        Debug.Log(filepath); 
        // save data in json 
        if (data != null)
        {
            if (File.Exists(filepath))
            {
                filename = filename + System.DateTime.Now; 
                filepath = persistentPath + foldername + filename + fileending;
            }
            string jsonString = JsonUtility.ToJson(data, true);
        jsonString += System.Environment.NewLine;
        File.AppendAllText(filepath, jsonString);

        File.WriteAllText(filepath, jsonString);
            Debug.Log(filepath); 
        }
        else
        {
            Debug.LogError("LoadSettings::SaveNewUserSettings no data to save");
        }
        //AddNewSettingsFileToGeneralSettings(filename, fileending); 
    }

    void AddNewSettingsFileToGeneralSettings(string filename, string fileending)
    {
        GameManager.Instance.generalSettings.settingFiles.Add(filename + fileending);

        ApplicationData.SaveToPersistentPath(GameManager.Instance.generalSettings); 
    }

}
