using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class Network : NetworkEventBehaviour
{
    [SerializeField]
    private NetworkIdentity obj;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Local.Invoke(1);
        }
    }
    [Client(1)]
    void Instantiates(DataBuffer buffer){
        
        buffer.InstantiateOnClient(obj);
    }

    [Server(1)]
    void ServerReceave(DataBuffer buffer, NetworkPeer peer){
        
        var inst = buffer.InstantiateOnServer(obj,peer);
        buffer.FastWrite("Local Player");
        Remote.Invoke(1,peer.Id,buffer);
    }
}
