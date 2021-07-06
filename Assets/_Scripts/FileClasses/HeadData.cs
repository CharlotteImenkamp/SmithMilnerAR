using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class HeadData
{
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public Vector3 GazeOrigin;
    public Vector3 GazeDirection;
    public string timeAfterStart;

    public void SetCameraParameters(Transform t)
    {
        CameraPosition = t.position;
        CameraRotation = t.rotation;
    }

    public void SetGazeParameters(Vector3 gazeOrigin, Vector3 gazeDirection)
    {
        GazeOrigin = gazeOrigin;
        GazeDirection = gazeDirection;
    }


    public HeadData()
    {
        timeAfterStart = Time.time.ToString(); ;
    }

    public HeadData(Transform CameraTransform, Vector3 gazeOrigin, Vector3 gazeDirection)
    {
        timeAfterStart = Time.time.ToString();
        CameraPosition = CameraTransform.position;
        CameraRotation = CameraTransform.rotation;
        GazeOrigin = gazeOrigin;
        GazeDirection = gazeDirection;
    }
}
