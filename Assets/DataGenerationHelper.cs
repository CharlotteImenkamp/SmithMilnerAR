using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataGenerationHelper : MonoBehaviour
{
    [SerializeField]
    private bool saveNewSettings; 
    // Start is called before the first frame update
    void Start()
    {
        if (saveNewSettings)
        {
            SaveNewUserSettings(SaveCurrentLayout(), "/settings", "/settings_1", ".json");
            //SaveNewUserSettings(GenerateTestSet(), "", "/settings_user_4.json");
        }
    }

    userSettingsData SaveCurrentLayout()
    {
        GameObject[] intObj = GameObject.FindGameObjectsWithTag("InteractionObject");
        List<Object> obj = new List<Object>(); 


        foreach (GameObject o in intObj)
        {
            obj.Add(new Object(o.name, o.transform.position, o.transform.rotation)); 
        }

        userSettingsData res = new userSettingsData(
                obj,
                0.1f,
                1, 
                userSettingsData.userSet.AG);

        return res; 
    }

    userSettingsData GenerateTestSet()
    {
        userSettingsData test = new userSettingsData(
    new List<Object> {
                new Object("eins", Vector3.zero, Quaternion.identity),
                new Object("zwei", Vector3.zero, Quaternion.identity)},
    0f, 1, userSettingsData.userSet.AG);

        test.gameObjects = new List<Object>();
        test.gameObjects.Add(new Object("eins", Vector3.zero, Quaternion.identity));
        test.gameObjects.Add(new Object("zwei", Vector3.zero, Quaternion.identity));
        test.updateRate = 0f;
        test.UserID = 1;
        test.set = userSettingsData.userSet.AG;

        return test; 
    } 
    private void SaveNewUserSettings(userSettingsData data, string foldername, string filename, string fileending)
    { 
        string persistentPath = Application.persistentDataPath; 
        string filepath = persistentPath + foldername + filename + fileending;
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
        System.IO.File.AppendAllText(filepath, jsonString);

        File.WriteAllText(filepath, jsonString);
            Debug.Log(filepath); 
        }
        else
        {
            Debug.LogError("LoadSettings::SaveNewUserSettings no data to save");
        }
        AddNewSettingsFileToGeneralSettings(filename, fileending); 
    }

    void AddNewSettingsFileToGeneralSettings(string filename, string fileending)
    {
        applicationData settings = GameManager.Instance.LoadGeneralSettings(Application.persistentDataPath + "/generalSettings.json");

        settings.settingFiles.Add(filename+fileending);

        GameManager.Instance.SaveGeneralSettings(settings); 
    }

}
