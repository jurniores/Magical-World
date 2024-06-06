using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancePlayerSala : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSala;
    [SerializeField]
    private Transform sala1, sala2;
    void Start()
    {
        InstancePlayer(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InstancePlayer(bool type){
        if(type){
            Transform pSalaInst = Instantiate(playerSala).transform;
            pSalaInst.SetParent(sala1,false);
        }
    }
}
