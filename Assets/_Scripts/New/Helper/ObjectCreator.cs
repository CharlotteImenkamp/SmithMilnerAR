using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

/// <summary>
///  Helper class to use Functions from MonoBehaviour
/// </summary>
public class ObjectCreator : ScriptableObject
{

    private string prefabFolderName;
    public string PrefabFolderName { get => prefabFolderName; set => prefabFolderName = value; }
    public string BoundingBoxFolderName { get => boundingBoxFolderName; set => boundingBoxFolderName = value; }

    private string boundingBoxFolderName; 

    public ObjectCreator()
    {
        prefabFolderName = "Objects";
        boundingBoxFolderName = "BoundingBox"; 
    }

    public void SpawnObject(GameObject obj, Vector3 position, Quaternion rotation, ConfigType config)
    {
        ApplyConfiguration(obj, config); 
        var generatedObject = Instantiate(obj, position, rotation);
    }

    private void ApplyConfiguration(GameObject obj, ConfigType config)
    {
        if(config == ConfigType.MovementDisabled)
        {
            try
            {
                obj.GetComponent<BoundsControl>().enabled = false;
                obj.GetComponent<ObjectManipulator>().enabled = false;
                obj.GetComponent<NearInteractionGrabbable>().enabled = false; 
            }
            catch (InvalidCastException e)
            {
                throw new System.MemberAccessException("ObjectCreator:: ApplyConfiguration, not all Components found.", e); 
            }
        }
        else if(config == ConfigType.MovementEnabled)
        {
            try
            {

                var comp = (BoundsControl)obj.GetComponent(typeof(BoundsControl));
                comp.enabled = true; 

                var oM = (ObjectManipulator)obj.GetComponent(typeof(ObjectManipulator));
                oM.enabled = true; 

                var iG = (NearInteractionGrabbable)obj.GetComponent(typeof(NearInteractionGrabbable));
                iG.enabled = true; 
            }
            catch (InvalidCastException e)
            {
                throw new System.MemberAccessException("ObjectCreator:: ApplyConfiguration, not all Components found.", e);
            }

        }
        else
        {
            Debug.LogError("ObjectCreator::ApplyConfiguration wrong configType format."); 
        }
    }

    public void SpawnObjects(UnityEngine.Object[] obj, Vector3[] positions, Quaternion[] rotations, ConfigType config)
    {
        //Instantiate(obj);

        Debug.LogError("ObjectCreator::SpawnObjects not implemented."); 
    }



    public GameObject CreateInteractionObject(userSettingsData currentDataSet)
    {
        // set first object in list to test Object
        GameObject loadedObj = Resources.Load<GameObject>(PrefabFolderName + "/" + currentDataSet.gameObjects[0].Objectname.ToString());

        if (loadedObj == null)
        {
            throw new FileNotFoundException("... ObjectManager::CreateInteractionObject no file found");
        }

        ApplyRelevantComponents(loadedObj);

        return loadedObj;
    }



    public GameObject[] CreateInteractionObjects(userSettingsData currentDataSet)
    {
        int length = currentDataSet.gameObjects.Count;
        GameObject[] objs = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            var loadedObj = Resources.Load<GameObject>(PrefabFolderName + "/" + currentDataSet.gameObjects[i].Objectname.ToString());
            if (loadedObj == null)
            {
                throw new FileNotFoundException("... ObjectManager::CreateInteractionObjects no file found");
            }
            else
            {
                ApplyRelevantComponents(loadedObj); 
                objs[i] = loadedObj;
            }
        }

        return objs;
    }

    private void ApplyRelevantComponents(GameObject loadedObj)
    {
        // Rigidbody
        var rb = loadedObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = loadedObj.AddComponent<Rigidbody>();
            rb.mass = 1;
            rb.drag = 0;
            rb.angularDrag = 0;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.freezeRotation = true;
        }

        // BoxCollider
        var col = loadedObj.GetComponent<BoxCollider>();
        if (col == null)
        {
            col = loadedObj.AddComponent<BoxCollider>();
        }

        // Audio Source
        var audio = loadedObj.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = loadedObj.AddComponent<AudioSource>();
        }


        // Tethered Placement
        var placementComp = loadedObj.GetComponent<TetheredPlacement>();
        if (placementComp == null)
        {
            placementComp = loadedObj.AddComponent<TetheredPlacement>();
            placementComp.DistanceThreshold = 20.0f;
        }

        // Near Interaction Grabbable
        var grabComp = loadedObj.GetComponent<NearInteractionGrabbable>();
        if (grabComp == null)
        {
            grabComp = loadedObj.AddComponent<NearInteractionGrabbable>();
            grabComp.ShowTetherWhenManipulating = false;
            grabComp.IsBoundsHandles = true;
        }

        // ConstraintManager
        var constMan = loadedObj.GetComponent<ConstraintManager>();
        if (constMan == null)
        {
            constMan = loadedObj.AddComponent<ConstraintManager>();
        }

        // RotationAxisConstraint
        var rotConst = loadedObj.GetComponent<RotationAxisConstraint>();
        if (rotConst == null)
        {
            rotConst = loadedObj.AddComponent<RotationAxisConstraint>();
            rotConst.HandType = ManipulationHandFlags.OneHanded;
            rotConst.ConstraintOnRotation = AxisFlags.XAxis;
            rotConst.ConstraintOnRotation = AxisFlags.ZAxis;
            rotConst.UseLocalSpaceForConstraint = true;

            constMan.AddConstraintToManualSelection(rotConst);
        }


        // Min Max Scale Constraint
        var scaleConst = loadedObj.GetComponent<MinMaxScaleConstraint>();
        if (scaleConst == null)
        {
            scaleConst = loadedObj.AddComponent<MinMaxScaleConstraint>();
            scaleConst.HandType = ManipulationHandFlags.TwoHanded;
            scaleConst.ProximityType = ManipulationProximityFlags.Far;
            scaleConst.ProximityType = ManipulationProximityFlags.Near;
            scaleConst.ScaleMaximum = 1;
            scaleConst.ScaleMinimum = 1;
            scaleConst.RelativeToInitialState = true;

            constMan.AddConstraintToManualSelection(scaleConst);
        }

        // Custom Movement Constraint
        var moveConst = loadedObj.GetComponent<CustomMovementConstraint>();
        if (moveConst == null)
        {
            moveConst = loadedObj.AddComponent<CustomMovementConstraint>();
            moveConst.HandType = ManipulationHandFlags.TwoHanded;
            moveConst.UseConstraint = true;
            moveConst.ConstraintOnMovement = AxisFlags.YAxis;

            constMan.AddConstraintToManualSelection(moveConst);
        }

        // Object Manipulator
        var objMan = loadedObj.GetComponent<ObjectManipulator>();
        if (objMan == null)
        {
            objMan = loadedObj.AddComponent<ObjectManipulator>();
            objMan.AllowFarManipulation = false;
            objMan.EnableConstraints = true;
            objMan.ConstraintsManager = constMan;
        }

        // BoundsControl

        var boundsControl = loadedObj.GetComponent<BoundsControl>();
        if (boundsControl == null)
        {
            boundsControl = loadedObj.AddComponent<BoundsControl>();
            boundsControl.Target = loadedObj;
            boundsControl.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateOnStart;
            boundsControl.BoundsOverride = col;
            boundsControl.CalculationMethod = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsCalculationMethod.RendererOverCollider;
            boundsControl.ScaleHandlesConfig = null;
            boundsControl.TranslationHandlesConfig = null;

            // Rotation Handle
            var rotationHandle = new RotationHandlesConfiguration();

            var mat = Resources.Load<Material>( boundingBoxFolderName + "/BoundingBoxHandleWhite");
            if (mat == null)
                throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file found");
            rotationHandle.HandleMaterial = mat;

            var grMat = Resources.Load<Material>(boundingBoxFolderName + "/BoundingBoxHandleBlueGrabbed");
            if (grMat == null)
                throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file found");
            rotationHandle.HandleGrabbedMaterial = grMat;

            var go = Resources.Load<GameObject>(boundingBoxFolderName + "/MRTK_BoundingBox_RotateHandle");
            if (go == null)
                throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file found");
            rotationHandle.HandlePrefab = go;

            boundsControl.RotationHandlesConfig = rotationHandle;
        }
    }



    #region object removal
    public void RemoveObject(UnityEngine.Object obj)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveObjects(UnityEngine.Object[] obj)
    {
        throw new System.NotImplementedException();
    }
    #endregion object removal

    // benötigt??
    public GameObject[] GetInteractionObjectsInScene()
    {
        GameObject[] ObjInScene;
        ObjInScene = GameObject.FindGameObjectsWithTag("InteractionObject");
        return ObjInScene;
    }
}
