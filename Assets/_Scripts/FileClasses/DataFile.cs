using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataFile
{

    public static void StartFile(string filepath)
    {
        string start = "{ \n \"entries " + DateTime.Now.ToString("D") + DateTime.Now.ToString("F") + "\":[" + Environment.NewLine;
        // File.AppendAllText(filepath, start);

        // TEST/ ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        UnityEngine.Windows.File.WriteAllBytes(filepath, Encoding.ASCII.GetBytes(start));
    }

    public static void AddLine<T>(T data, string filepath)
    {
        string jsonString = JsonUtility.ToJson(data, true);

        jsonString += "," + System.Environment.NewLine;
        File.AppendAllText(filepath, jsonString);
    }

    public static void EndFile(string filepath)
    {
        string ende = " \"END\"\n ]}\n";
        File.AppendAllText(filepath, ende);

    }

    public static string GenerateFilePath(string directoryPath, string name)
    {
        string filePath = Path.Combine(directoryPath, name + ".json");

        // Generate Directory
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // File
        if (File.Exists(filePath))
        {
            // unique name
            name = name + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            filePath = Path.Combine(directoryPath, name + ".json");
        }

        return filePath; 
    }
}