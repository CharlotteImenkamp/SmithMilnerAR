using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.Input;

/// <summary>
/// Provide Functionality to Interaction Object. Add To Components when instantiating
/// </summary>
public class ObjectHelper : MonoBehaviour
{
    /// <summary>
    /// Helper Methods to add to OnRotationStarted Event
    /// </summary>
    public void AddMovingObject()
    {
        DataManager.Instance.MovingObjects.Add(this.gameObject); 
    }

    public void RemoveMovingObject()
    {
        DataManager.Instance.MovingObjects.Remove(this.gameObject); 
    }

    /// <summary>
    /// Helper Methods to listen to OnManipulationStarted Event
    /// </summary>
    /// <returns></returns>
    public void AddObject(ManipulationEventData eventData)
    {
        AddMovingObject();
    }

    public void RemoveObject(ManipulationEventData eventData)
    {
        RemoveMovingObject();
    }

}
