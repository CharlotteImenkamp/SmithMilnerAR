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


/// <summary>
///  Helper class to use Functions from MonoBehaviour in ObjectManager
/// </summary>
public class ObjectCreator : ScriptableObject
{
    public string PrefabFolderName { get => prefabFolderName; set => prefabFolderName = value; }
    public string BoundingBoxFolderName { get => boundingBoxFolderName; set => boundingBoxFolderName = value; }
    public List<GameObject> InstantiatedObjects { get => instantiatedObjects; set => instantiatedObjects = value; }
    
    private string boundingBoxFolderName;
    private string soundFolderName; 
    private string prefabFolderName;
    private List<GameObject> instantiatedObjects;
    private AudioClip rotateStart;
    private AudioClip rotateStop;
    private AudioClip manStart;
    private AudioClip manStop; 

    #region public methods

    public void OnEnable()
    {
        // parameters
        instantiatedObjects = new List<GameObject>();
        prefabFolderName = "Objects";
        boundingBoxFolderName = "BoundingBox";
        soundFolderName = "Sound";

        // Audio Rotation
        var rotfileName = "/MRTK_Rotate_Start";
        rotateStart = Resources.Load<AudioClip>(soundFolderName + rotfileName);
        if (rotateStart == null)
            throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file {0} found", rotfileName);

        var fileName = "/MRTK_Rotate_Stop";
        rotateStop = Resources.Load<AudioClip>(soundFolderName + fileName);
        if (rotateStop == null)
            throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file {0} found", fileName);

        // Audio Manipulation
        var fileNameManipulation = "/MRTK_Manipulation_End";
        manStop = Resources.Load<AudioClip>(soundFolderName + fileNameManipulation);
        if (manStop == null)
            throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file {0} found", fileNameManipulation);

        var fileNameManStart = "/MRTK_Manipulation_Start";
        manStart = Resources.Load<AudioClip>(soundFolderName + fileNameManStart);
        if (manStart == null)
            throw new FileNotFoundException("... ObjectManager::ApplyRelevantComponents no file {0} found", fileNameManStart);
    }


    public void SpawnObject(GameObject obj, GameObject parent, Vector3 position, Quaternion rotation, ConfigType config)
    {

        ApplyRelevantComponents(obj);
        ApplyConfiguration(obj, config);

        var generatedObject = Instantiate(obj, position, rotation);

        // Add Sounds to Movement
        generatedObject.GetComponent<BoundsControl>().RotateStarted.RemoveAllListeners(); 
        generatedObject.GetComponent<BoundsControl>().RotateStarted.AddListener(()=>HandleOnRotationStarted(generatedObject));

        generatedObject.GetComponent<BoundsControl>().RotateStopped.RemoveAllListeners();
        generatedObject.GetComponent<BoundsControl>().RotateStopped.AddListener(() => HandleOnRotationStopped(generatedObject));

        generatedObject.GetComponent<ObjectManipulator>().OnManipulationStarted.RemoveAllListeners();
        generatedObject.GetComponent<ObjectManipulator>().OnManipulationStarted.RemoveAllListeners();

        generatedObject.GetComponent<ObjectManipulator>().OnManipulationStarted.AddListener(HandleOnManipulationStarted); 
        generatedObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(HandleOnManipulationStopped);

        generatedObject.SetActive(true);

        generatedObject.name = generatedObject.name.Replace("(Clone)", ""); 
        generatedObject.transform.parent = parent.transform;
        generatedObject.transform.localPosition = position;
        generatedObject.transform.localRotation = rotation;

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

    public GameObject CreateInteractionObject(ObjectData currentData)
    {
        // set first object in list to test Object
        GameObject loadedObj = Resources.Load<GameObject>(prefabFolderName + "/" + currentData.gameObjects[0].Objectname.ToString());

        if (loadedObj == null)
        {
            throw new FileNotFoundException("... ObjectManager::CreateInteractionObject no file found");
        }

        return loadedObj;
    }

    public GameObject[] CreateInteractionObjects(ObjectData currentData)
    {
        int length = currentData.gameObjects.Count;
        GameObject[] objs = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            var loadedObj = Resources.Load<GameObject>(prefabFolderName + "/" + currentData.gameObjects[i].Objectname.ToString());
            if (loadedObj == null)
            {
                // throw new FileNotFoundException("... ObjectManager::CreateInteractionObjects Object " + currentData.gameObjects[i].Objectname.ToString() + " not found");
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

    public void Reset()
    {
        if (instantiatedObjects != null)
            RemoveAllObjects(); 

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
                comp.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateByProximity;

                var oM = (ObjectManipulator)obj.GetComponent(typeof(ObjectManipulator));
                oM.enabled = true;

                var iG = (NearInteractionGrabbable)obj.GetComponent(typeof(NearInteractionGrabbable));
                iG.enabled = true;

                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotation; 
                }

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

    private void ApplyRelevantComponents(GameObject loadedObj)
    {
        loadedObj.tag = "InteractionObject"; 

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
        //\ TODO Only working from editor. Fix
        var moveConst = loadedObj.EnsureComponent<CustomMovementConstraint>();
            moveConst.HandType = ManipulationHandFlags.TwoHanded;
            moveConst.ConstraintOnMovement = AxisFlags.YAxis;
        constMan.AddConstraintToManualSelection(moveConst);

        // Object Manipulator
        var objMan = loadedObj.EnsureComponent<ObjectManipulator>();
            objMan.AllowFarManipulation = false;
            objMan.EnableConstraints = true;
            objMan.ConstraintsManager = constMan;



        // BoundsControl
        var boundsControl = loadedObj.EnsureComponent<BoundsControl>();
            boundsControl.Target = loadedObj;
            boundsControl.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateByProximity;
            boundsControl.BoundsOverride = col;
            boundsControl.CalculationMethod = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsCalculationMethod.RendererOverCollider;

        // DisplayConfig
        BoxDisplayConfiguration dispConfig = CreateInstance<BoxDisplayConfiguration>();
        dispConfig.BoxMaterial = GameManager.Instance.BoundingBox;
        dispConfig.BoxGrabbedMaterial = GameManager.Instance.BoundingBoxGrabbed;
        boundsControl.BoxDisplayConfig = dispConfig;

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
        rotationHandle.HandleMaterial = GameManager.Instance.BoundingBoxHandleWhite;
        rotationHandle.HandleGrabbedMaterial = GameManager.Instance.BoundingBoxHandleBlueGrabbed;
        rotationHandle.HandlePrefab = GameManager.Instance.BoundingBox_RotateHandle;

        boundsControl.RotationHandlesConfig = rotationHandle;
        boundsControl.RotationHandlesConfig.ShowHandleForX = false;
        boundsControl.RotationHandlesConfig.ShowHandleForY = true;
        boundsControl.RotationHandlesConfig.ShowHandleForZ = false;

        // Links Config
        var linksConfig = CreateInstance<LinksConfiguration>();
        linksConfig.ShowWireFrame = false;
        boundsControl.LinksConfig = linksConfig; 

        boundsControl.ConstraintsManager = constMan;
    }

    private void HandleOnManipulationStarted(ManipulationEventData eventData)
    {
        Debug.Log("Manipulation Started"); 

        eventData.ManipulationSource.GetComponent<AudioSource>().PlayOneShot(manStart);
        Destroy(eventData.ManipulationSource.GetComponent<Rigidbody>()); 

        DataManager.Instance.MovingObjects.Add(eventData.ManipulationSource);
    }

    private void HandleOnManipulationStopped(ManipulationEventData eventData)
    {
        Debug.Log("Manipulation Stopped");

        Rigidbody rb = eventData.ManipulationSource.AddComponent<Rigidbody>();
        rb.mass = 1;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;

        eventData.ManipulationSource.GetComponent<AudioSource>().PlayOneShot(manStop);
        

        DataManager.Instance.MovingObjects.Remove(eventData.ManipulationSource);
    }

    private void HandleOnRotationStarted(GameObject generatedObject)
    {
        Debug.Log("Rotation Started");

        generatedObject.GetComponent<BoundsControl>().GetComponent<AudioSource>().PlayOneShot(rotateStart);
        Destroy(generatedObject.GetComponent<Rigidbody>()); 

        DataManager.Instance.MovingObjects.Add(generatedObject);
    }

    private void HandleOnRotationStopped(GameObject generatedObject)
    {
        Debug.Log("Rotation Stopped");

        generatedObject.GetComponent<BoundsControl>().GetComponent<AudioSource>().PlayOneShot(rotateStop);
        Rigidbody rb = generatedObject.AddComponent<Rigidbody>();
        rb.mass = 1;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;

        DataManager.Instance.MovingObjects.Remove(generatedObject);
    }





    #endregion private methods

}
