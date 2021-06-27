using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{

    storageFile settings; 

    //private list<string> configFiles;






    // Start is called before the first frame update
    void Start()
    {
        //Path = System.IO.Path.Combine(Application.persistentDataPath, "myFile.txt");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteText()
    {
        //TextWriter writer = File.CreateText();


        //writer.WriteLine("Hello Hololens");
        //writer.Close();
        Debug.Log(Application.persistentDataPath);

    }
}
