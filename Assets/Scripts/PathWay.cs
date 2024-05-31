using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


[ExecuteInEditMode]
public class PathWay : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            InstantiateMode();
        }
    }

    void InstantiateMode(){
        Instantiate(obj,Vector3.zero, Quaternion.identity);
    }
}
