using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Omni.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerGroup : NetworkBehaviour
{
    [SerializeField]
    private NetworkIdentity serverPlayer;
    private readonly Dictionary<int, int> dicPlayers = new();
    private Scene sceneGame;
    private Sala sala;
    private NetworkGroup group;
    public static int identityId = 0;

    void Start()
    {
        sala = group.Data.Get<Sala>("sala");
        #if UNITY_EDITOR
        DontDestroyOnLoad(gameObject);
        #endif
    }

    public void SetInfoGroupServer(NetworkGroup pGroup)
    {
        Invoke(nameof(StartedGame), 1);
        group = pGroup;
    }

    void Update()
    {
    }
    public void SetInfoGame(NetworkGroup pGroup)
    {
        group = pGroup;
    }
    void StartedGame()
    {
        using var res = NetworkManager.Pool.Rent();
        Remote.Invoke(ConstantsRPC.INIT_GAME_GO, res, Target.GroupMembers);
    }
    [Server(ConstantsRPC.INSTANT_PLAYER_GAME)]
    void InstantePlayerRPC(DataBuffer buffer, NetworkPeer peer)
    {

        //Adicionando o buffer de instanciação
        NetworkIdentity identity = serverPlayer.SpawnOnServer(peer);
       

        buffer.WriteIdentity(identity);

        this.InvokeByPeer(ConstantsRPC.INSTANT_ENEMY_GAME, peer, buffer, Target.GroupMembersExceptSelf);
        this.InvokeByPeer(ConstantsRPC.INSTANT_PLAYER_GAME, peer, buffer, Target.Self);

        using DataBuffer forAllBuffer = NetworkManager.Pool.Rent();
        print(dicPlayers.Count);
        forAllBuffer.WriteAsJson(dicPlayers);
        this.InvokeByPeer(ConstantsRPC.INSTANT_PLAYERS_GAME, peer, forAllBuffer, Target.Self);

        dicPlayers.Add(peer.Id, identity.IdentityId);
    }




}
