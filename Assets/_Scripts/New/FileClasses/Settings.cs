using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    // Object
    private Object[] gameObjects; 
    private int ObjectNumber;

    // saving
    private float updateRate;

    //login
    int UserID;
    userSet set; 

    enum userSet { JG, AG, AK}; 
}

public class Data
{
    private Time time;
    private List<Object> movingObjects; 



}

public struct Object
{
    string Objectname;
    Vector3 position;
    Vector3 rotation;
}