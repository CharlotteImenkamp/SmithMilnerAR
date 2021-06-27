using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////// DAta
    /// </summary>
    storageFile settings;

    //private list<string> configFiles;

    void Start()
    {
        //Path = System.IO.Path.Combine(Application.persistentDataPath, "myFile.txt");

    }

    public void WriteText()
    {
        //TextWriter writer = File.CreateText();
        //writer.WriteLine("Hello Hololens");
        //writer.Close();
        Debug.Log(Application.persistentDataPath);
    }
    /////////////////////////////////////////////////////////////////////////////////////////
}
