using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Omni.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerGroup : NetworkBehaviour
{
    private Scene scene;
    private Sala sala;
    private SalaInGame salaInGame;
    private NetworkGroup group;
    public void SetInfoGroupServer(NetworkGroup pGroup)
    {
        Invoke(nameof(StartedGame), 11);
        group = pGroup;
        scene = SceneManager.CreateScene(group.Name, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        SceneManager.MoveGameObjectToScene(gameObject, scene);
    }

    void FixedUpdate()
    {
        scene.GetPhysicsScene().Simulate(Time.fixedDeltaTime);
    }
    public void SetInfoGame(NetworkGroup pGroup)
    {
        group = pGroup;
    }
    void StartedGame()
    {
        var res = NetworkManager.Pool.Rent();
        Remote.Invoke(ConstantsRPC.INIT_GAME_GO, res, Target.GroupMembers);
    }
    void ReturnPlayers()
    {

    }



}
