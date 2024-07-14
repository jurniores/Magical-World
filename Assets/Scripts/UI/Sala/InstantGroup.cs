using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;


public partial class InstantGroup : ClientEventBehaviour
{
    [SerializeField]
    private NetworkIdentity clientGroup;

    [Client(ConstantsRPC.INIT_GAME)]
     void InitiGame(DataBuffer res)
    {
        res.InstantiateOnClient(clientGroup);
    }
}
