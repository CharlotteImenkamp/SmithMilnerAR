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
using Microsoft.MixedReality.Toolkit;

// EnsureComponnet< besser? fügt hinzu, wenn nicht da ist


/// <summary>
///  Helper class to use Functions from MonoBehaviour in ObjectManager
/// </summary>
public class ObjectCreator : ScriptableObject
{
    public string PrefabFolderName { get => prefabFolderName; set => prefabFolderName = value; }
    public string BoundingBoxFolderName { get => boundingBoxFolderName; set => boundingBoxFolderName = value; }
    public List<GameObject> InstantiatedObjects { get => instantiatedObjects; set => instantiatedObjects = value; }
    
    private string boundingBoxFolderName;
    private string prefabFolderName;
    private List<GameObject> instantiatedObjects;

    #region public methods
    public ObjectCreator()
    {
        instantiatedObjects = new List<GameObject>();

        prefabFolderName = "Objects";
        boundingBoxFolderName = "BoundingBox";

    }
    public void SpawnObject(GameObject obj, GameObject parent, Vector3 position, Quaternion rotation, ConfigType config)
    {
        ApplyRelevantComponents(obj);
        ApplyConfiguration(obj, config);

        var generatedObject = Instantiate(obj, position, rotation);

        generatedObject.name = generatedObject.name.Replace("(Clone)", ""); 
        generatedObject.transform.parent = parent.transform;
        generatedObject.transform.localPosition = position;
        generatedObject.transform.localRotation = rotation; 

        generatedObject.SetActive(true);
        instantiatedObjects.Add(generatedObject);
    }

    public void SpawnObjects(GameObject[] gameObjects, GameObject parent, Vector3[] positions, Quaternion[] rotations, ConfigType config)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            SpawnObject(gameObjects[i], parent, positions[i], rotations[i], config); 
        }
    }
    public void SpawnObjects(GameObject[] gameObjects, GameObject parent, Vector3 position, Quaternion rotation, ConfigType config)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            SpawnObject(gameObjects[i], parent, position, rotation, config);
        }
    }

    public GameObject CreateInteractionObject(userSettingsData currentDataSet)
    {
        // set first object in list to test Object
        GameObject loadedObj = Resources.Load<GameObject>(prefabFolderName + "/" + currentDataSet.gameObjects[0].Objectname.ToString());

        if (loadedObj == null)
        {
            throw new FileNotFoundException("... ObjectManager::CreateInteractionObject no file found");
        }

        return loadedObj;
    }

    public GameObject[] CreateInteractionObjects(userSettingsData currentDataSet)
    {
        int length = currentDataSet.gameObjects.Count;
        GameObject[] objs = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            var loadedObj = Resources.Load<GameObject>(prefabFolderName + "/" + currentDataSet.gameObjects[i].Objectname.ToString());
            if (loadedObj == null)
            {
                throw new FileNotFoundException("... ObjectManager::CreateInteractionObjects no file found");
            }
            else
            {
                objs[i] = loadedObj;
            }
        }

        return objs;
    }

    public void RemoveAllObjects()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }

        instantiatedObjects.Clear();
    }

    #endregion public methods

    #region private methods

    private void ApplyConfiguration(GameObject obj, ConfigType config)
    {

        if (config == ConfigType.MovementDisabled)
        {
            try
            {
                if (obj.TryGetComponent(out BoundsControl bC))
                    bC.enabled = false;
                if (obj.TryGetComponent(out ObjectManipulator oM))
                    oM.enabled = false;
                if (obj.TryGetComponent(out NearInteractionGrabbable iG))
                    iG.enabled = false;
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.useGravity = false;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
                    

                if (obj.TryGetComponent(out NearInteractionTouchable nI))
                    nI.enabled = false;
            }
            catch (InvalidCastException e)
            {
                throw new System.MemberAccessException("ObjectCreator:: ApplyConfiguration, not all Components found.", e);
            }
        }
        else if (config == ConfigType.MovementEnabled)
        {
            try
            {

                var comp = (BoundsControl)obj.GetComponent(typeof(BoundsControl));
                comp.enabled = true;

                var oM = (ObjectManipulator)obj.GetComponent(typeof(ObjectManipulator));
                oM.enabled = true;

                var iG = (NearInteractionGrabbable)obj.GetComponent(typeof(NearInteractionGrabbable));
                iG.enabled = true;

                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotation; 
                }
                    

                if (obj.TryGetComponent(out NearInteractionTouchable nI))
                    nI.enabled = false;
            }
            catch (InvalidCastException e)
            {
                throw new System.MemberAccessException("ObjectCreator:: ApplyConfiguration, not all Components found.", e);
            }

        }
        else if(config == ConfigType.scrollBox)
        {
                if (obj.TryGetComponent(out BoundsControl bC))
                bC.enabled = false;
                if (obj.TryGetComponent(out ObjectManipulator oM))

                oM.enabled = false;
                if (obj.TryGetComponent(out NearInteractionGrabbable iG))

                iG.enabled = false;
                if (obj.TryGetComponent(out Rigidbody rb))
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll; 
                
                    
                if (obj.TryGetComponent(out BoxCollider col))
                    col.enabled = true;

                if (obj.TryGetComponent(out NearInteractionTouchable nI))
                    nI.enabled = true;
        }
        else
        {
            Debug.LogError("ObjectCreator::ApplyConfiguration wrong configType format.");
        }
    }


    private void ApplyRelevantComponents(GameObject loadedObj)
    {
        loadedObj.tag = "InteractionObject"; 

        // Custom Object Helper
        var helper = loadedObj.EnsureComponent<CustomObjectHelper>();

        // Rigidbody
        var rb = loadedObj.EnsureComponent<Rigidbody>();
        rb.mass = 1;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;

        // BoxCollider
        var col = loadedObj.EnsureComponent<BoxCollider>();

        // Near Interaction Touchable for Scrolling
        var nI = loadedObj.EnsureComponent<NearInteractionTouchable>();
        nI.enabled = false;
        nI.EventsToReceive = TouchableEventType.Touch; 
        nI.SetTouchableCollider(col);
        

        // Audio Source
        var audio = loadedObj.EnsureComponent<AudioSource>();

        // Tethered Placement
        var placementComp = loadedObj.EnsureComponent<TetheredPlacement>();
        placementComp.DistanceThreshold = 20.0f;

        // Near Interaction Grabbable
        var grabComp = loadedObj.EnsureComponent<NearInteractionGrabbable>();
        grabComp.ShowTetherWhenManipulating = false;
        grabComp.IsBoundsHandles = true;

        // ConstraintManager
        var constMan = loadedObj.EnsureComponent<ConstraintManager>();

        // RotationAxisConstraint
        var rotConst = loadedObj.EnsureComponent<RotationAxisConstraint>();
        rotConst.HandType = ManipulationHandFlags.OneHanded;
        rotConst.ConstraintOnRotation = AxisFlags.XAxis;
        rotConst.ConstraintOnRotation = AxisFlags.ZAxis;
        rotConst.UseLocalSpaceForConstraint = true;
        constMan.AddConstraintToManualSelection(rotConst);



        // Min Max Scale Constraint
        var scaleConst = loadedObj.EnsureComponent<MinMaxScaleConstraint>();
            scaleConst.HandType = ManipulationHandFlags.TwoHanded;
            scaleConst.ProximityType = ManipulationProximityFlags.Far;
            scaleConst.ProximityType = ManipulationProximityFlags.Near;
            scaleConst.ScaleMaximum = 1;
            scaleConst.ScaleMinimum = 1;
            scaleConst.RelativeToInitialState = true;

            constMan.AddConstraintToManualSelection(scaleConst);

        // Custom Movement Constraint
        var moveConst = loadedObj.EnsureComponent<CustomMovementConstraint>();
            moveConst.HandType = ManipulationHandFlags.TwoHanded;
            moveConst.UseConstraint = true;
            moveConst.ConstraintOnMovement = AxisFlags.YAxis;

            constMan.AddConstraintToManualSelection(moveConst);

        // Object Manipulator
        var objMan = loadedObj.EnsureComponent<ObjectManipulator>();
            objMan.AllowFarManipulation = false;
            objMan.EnableConstraints = true;
            objMan.ConstraintsManager = constMan;

        // Events
        objMan.OnManipulationStarted.AddListener(helper.AddObject);
        objMan.OnManipulationEnded.AddListener(helper.RemoveObject);

        // BoundsControl
        var boundsControl = loadedObj.EnsureComponent<BoundsControl>();
        boundsControl.Target = loadedObj;
            boundsControl.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateOnStart;
            boundsControl.BoundsOverride = col;
            boundsControl.CalculationMethod = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsCalculationMethod.RendererOverCollider;

            // Scale Handle
            ScaleHandlesConfiguration config = CreateInstance<ScaleHandlesConfiguration>();
            config.ShowScaleHandles = false;  
            boundsControl.ScaleHandlesConfig = config;

            // Translation Handle
            TranslationHandlesConfiguration tConfig = CreateInstance<TranslationHandlesConfiguration>();
            tConfig.ShowHandleForX = false;
            tConfig.ShowHandleForY = false;
            tConfig.ShowHandleForZ = false; 
            boundsControl.TranslationHandlesConfig = tConfig;

            // Rotation Handle
            var rotationHandle = CreateInstance<RotationHandlesConfiguration>();

            var mat = Resources.Load<Material>(boundingBoxFolderName + "/BoundingBoxHandleWhite");
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

        // Events
        boundsControl.RotateStarted.AddListener(helper.AddMovingObject);
        boundsControl.RotateStopped.AddListener(helper.RemoveMovingObject);
    }

    #endregion private methods

    //\TODO benötigt??
    public GameObject[] GetInteractionObjectsInScene()
    {
        GameObject[] ObjInScene;
        ObjInScene = GameObject.FindGameObjectsWithTag("InteractionObject");
        return ObjInScene;
    }
}
