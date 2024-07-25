using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public partial class InstantGroup : ClientBehaviour
{
    [SerializeField]
    private NetworkIdentity clientGroup;

    [Client(ConstantsRPC.INIT_GAME)]
     void InitiGame(DataBuffer res)
    {
        res.ReadIdentity(out int peerId, out int identityId);
        clientGroup.InstantiateOnClient(peerId, identityId);
    }
}
