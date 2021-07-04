using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Examples.Demos; 

public class ToggleBoundsControl : MonoBehaviour
{
    private bool isResizeEnabled; 

    // Start is called before the first frame update
    void Start()
    {
        isResizeEnabled = true; 
    }


    // Debug
    // \TODO löschen, wenn nicht mehr benötigt
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            ToggleResize();
        }
    }

    /// <summary>
    /// Toggle Bounds Control of Interaction Area and change Spawn point of interaction Objects
    /// </summary>
    public void ToggleResize()
    {
        if (isResizeEnabled)
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<BoundsControl>().Active = false;
            isResizeEnabled = false; 
        }
        else
        {
            var comp = (BoxCollider)GetComponent(typeof(BoxCollider));
            comp.enabled = true;

            var bC = (BoundsControl)GetComponent(typeof(BoundsControl));
            bC.Active = true; 

            isResizeEnabled = true; 
        }

        TetheredPlacement[] gameObjComp = FindObjectsOfType<TetheredPlacement>();
        foreach (TetheredPlacement tp in gameObjComp)
            tp.LockSpawnPoint(); 
    }
}
