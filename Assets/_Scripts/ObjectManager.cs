using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    private GameObject SpawnedCube;
    public GameObject OriginalCube;
    public GameObject myPrefabObj; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void SpawnObject()
    {
        Vector3 _t = OriginalCube.transform.position + new Vector3(0.06f,0,0);
        // SpawnedCube = Instantiate(OriginalCube, _t, Quaternion.identity); 
        Instantiate(myPrefabObj, _t, Quaternion.identity); 
    }
}
