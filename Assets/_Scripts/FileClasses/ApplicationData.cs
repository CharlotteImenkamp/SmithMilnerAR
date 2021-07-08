using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ApplicationData
{
    public static ApplicationData DefaultGeneralSettingsFile()
    {
        ApplicationData defaultData = new ApplicationData();

        defaultData.objectDataFolder = "settings";
        defaultData.userDataFolder = "data";

        defaultData.newObjectData = new List<string> { };

        defaultData.newUserData = new List<string> { }; 
        defaultData.incompleteUserData = new List<string> { };
        defaultData.completeUserData = new List<string> { }; 
        
        return defaultData;
    }

    // Folders 
    public string objectDataFolder;
    public string userDataFolder;

    // Sets
    public List<string> newObjectData;

    public List<string> newUserData; 
    public List<string> incompleteUserData;
    public List<string> completeUserData;
}

