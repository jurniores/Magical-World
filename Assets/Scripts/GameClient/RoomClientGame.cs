using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using static Omni.Core.HttpLite;

public class RoomClientGame : ClientBehaviour
{
    private GroupClient clientGroup;
    private GameManager gManager;
    public override void Start()
    {
        base.Start();
        clientGroup = NetworkService.Get<GroupClient>();
        gManager = NetworkService.Get<GameManager>();
        //await UniTask.WaitForSeconds(3);

        Fetch.Get("/room/get/pconfig", RequestConfigPlayers);
    }

    [Client(ConstantsRPC.DESTROY_PLAYER_ROOM)]
    void PlayerDesconectado(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();
        ErrorManager.ValidateError(response, 1);
    }
    void RequestConfigPlayers(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse>();
        ErrorManager.ValidateError(response, 1, () =>
        {
            var sInGame = res.FromJson<SalaInGame>();
            gManager.salaInGame = sInGame;
        });


    }
}
