using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private string path;
   //  StorageFile newFile; 

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "myFile.txt");

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteText()
    {
        TextWriter writer = File.CreateText(path);


        writer.WriteLine("Hello Hololens");
        writer.Close();
        Debug.Log(Application.persistentDataPath);

        // newFile = await DownloadsFolder.CreateFileAsync("fileNew.txt"); 
    }
}
