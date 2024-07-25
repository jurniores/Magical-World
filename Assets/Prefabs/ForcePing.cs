using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Omni.Core;
using Omni.Shared;
using Omni.Threading.Tasks;
using UnityEngine;

public class ForcePing : MonoBehaviour
{
    [SerializeField]
    private int qtd = 100;
    [SerializeField]
    private bool validateClient = false;

    
    void Start()
    {
        var layer1 = LayerMask.NameToLayer("LPlayer");
        Physics.IgnoreLayerCollision()
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
}

