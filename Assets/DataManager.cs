﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 

public class DataManager : MonoBehaviour
{
    private string path;  

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "myFile.txt");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            WriteText(); 
        }
    }

    public void WriteText()
    {
        TextWriter writer = File.CreateText(path);
        writer.WriteLine("Hello Hololens");
        writer.Close();
        Debug.Log(Application.persistentDataPath);
    }
}