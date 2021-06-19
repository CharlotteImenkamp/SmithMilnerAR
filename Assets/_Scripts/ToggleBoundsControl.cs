using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Examples.Demos; 

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


    // Debug
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
        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            Debug.Log("ToggleBondsControl::_DeactivateResize() No Box Collider found"); 
        }

        if (GetComponent<BoundsControl>())
        {
            GetComponent<BoundsControl>().Active = false;
        }
        else
        {
            Debug.Log("ToggleBondsControl::_DeactivateResize() No BoundsControl found"); 
        }
        

        foreach (GameObject obj in interactionObjects)
        {
           
            if (obj.GetComponent<BoundsControl>() )
            {
                obj.GetComponent<BoundsControl>().Active = true;
                //obj.AddComponent<Rigidbody>(); 
            }
            else
            {
                Debug.Log("ToggleBondsControl::_DeactivateResize() No BoundsControl found in InteractionObj");
            }
    
        }


        // Lock new Spawn Point 
        foreach (GameObject obj in interactionObjects)
        {
            obj.GetComponent<TetheredPlacement>().LockSpawnPoint(); 
        }

    }


    public void _ActivateResize()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoundsControl>().Active = true;

        foreach (GameObject obj in interactionObjects)
        {
            // Deactivate BoundsControl and destroy Rigidbody in Children
            if (obj.GetComponent<BoundsControl>() && obj.GetComponent<Rigidbody>())     // besser doch nicht beides zusammen, falls eins ausgeschaltte wird
            {
                obj.GetComponent<BoundsControl>().Active = false;
                //Destroy(obj.GetComponent<Rigidbody>());
            }
            // Error Handling
            else if (!obj.GetComponent<BoundsControl>())
            {
                Debug.Log("ToggleBondsControl::_ActivateResize() No BoundsControl found in InteractionObj");
            }
            else if (!obj.GetComponent<Rigidbody>())
            {
                Debug.Log("ToggleBondsControl::_ActivateResize() No Rigidbody found in InteractionObj");
            }
            else
            {
                Debug.Log("ToggleBondsControl::_ActivateResize() undefined error");
            }

        }

    }



}
