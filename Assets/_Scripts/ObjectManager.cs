using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Define Prefabs to Spawn
    /// </summary>

    [SerializeField]
    [Tooltip("Prefabs to Spawn")]
    private GameObject[] spawnObjects;

    public GameObject[] SpawnObjects
    {
        get => spawnObjects;
        set => spawnObjects = value;
    }
    

    /// <summary>
    /// Define Width and Heigth of Cell Array
    /// </summary>

    [SerializeField]
    [Tooltip("Apply Width (X)")]
    private float cellWidthX;

    public float CellWidthX
    {
        get => cellWidthX;
        set => cellWidthX = value;
    }

    [SerializeField]
    [Tooltip("Apply Heigth (Y)")]
    private float cellHeigthY;

    public float CellHeigthY
    {
        get => cellHeigthY;
        set => cellHeigthY = value;
    }

    Vector3[][] positionArray;
    
    #endregion Properties


    #region Public Methods
    // Start is called before the first frame update
    void Start()
    {

        CalculateRaster(); 
    }

    public void SpawnRandom()
    {
        //Vector3 _t = OriginalCube.transform.position + new Vector3(0.06f,0,0);
        // SpawnedCube = Instantiate(OriginalCube, _t, Quaternion.identity); 
        //Instantiate(myPrefabObj, _t, Quaternion.identity); 
    }

    #endregion Public Methods


    #region Private Methods
    private void CalculateRaster()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

            }
        }

    }
    #endregion PrivateMethods
}
