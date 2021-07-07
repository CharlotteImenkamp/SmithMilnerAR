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

        defaultData.settingsFolder = "settings";
        defaultData.dataFolder = "data";

        defaultData.dataFiles = new List<string> { "default1.json" };

        defaultData.newSets = new List<string> { };
        defaultData.incompleteUserData = new List<string> { };
        defaultData.completeUserData = new List<string> { }; 
        
        return defaultData;
    }

    // Folders 
    public string settingsFolder;
    public string dataFolder;

    // Files
    public List<string> dataFiles;      //\TODO einbinden? 

    // Sets
    public List<string> newSets;
    public List<string> incompleteUserData;
    public List<string> completeUserData;
}

