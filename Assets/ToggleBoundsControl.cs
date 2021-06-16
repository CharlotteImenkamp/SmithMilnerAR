using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoundsControl : MonoBehaviour
{
    private int DefaultLayer;
    private int ResizePlayarea;
    private GameObject[] interactionObjects; 
   

    // Start is called before the first frame update
    void Start()
    {
        DefaultLayer = 0;
        ResizePlayarea = 6; 

        Physics.IgnoreLayerCollision(DefaultLayer, ResizePlayarea);
        interactionObjects = GameObject.FindGameObjectsWithTag("InteractionObject"); 
         
    }

    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            _ActivateResize();
        }
        if (Input.GetKeyDown("d"))
        {
            _DeactivateResize(); 
        }
    }

    public void _DeactivateResize()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoundsControl>().Active = false;

        foreach (GameObject obj in interactionObjects)
        {
            obj.AddComponent<Rigidbody>(); 
        }
    }

    public void _ActivateResize()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoundsControl>().Active = true;

        foreach (GameObject obj in interactionObjects)
        {
            Destroy(obj.GetComponent<Rigidbody>()); 
        }
       
    }
}
