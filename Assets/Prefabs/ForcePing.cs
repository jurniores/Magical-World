using UnityEngine;

public class ForcePing : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerPlayer, playerServer, layerEnemy;
    void Start()
    {
        int LPlayer = (int)Mathf.Log(layerPlayer.value, 2);
        int LServer = (int)Mathf.Log(playerServer.value, 2);

        Physics.IgnoreLayerCollision(LPlayer, LServer);
        Physics.IgnoreLayerCollision(LServer, LServer);

    }
    // Update is called once per frame
    void Update()
    {

    }


}

