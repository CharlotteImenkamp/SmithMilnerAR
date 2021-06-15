using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoundsControl : MonoBehaviour
{
    private int DefaultLayer;
    private int ResizePlayarea; 

    // Start is called before the first frame update
    void Start()
    {
        DefaultLayer = 0;
        ResizePlayarea = 6; 

        Physics.IgnoreLayerCollision(DefaultLayer, ResizePlayarea);  
    }

    public void _DeactivateResize()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoundsControl>().Active = false; 
    }

    public void _ActivateResize()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoundsControl>().Active = true;
    }
}
