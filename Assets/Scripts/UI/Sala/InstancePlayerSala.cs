using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Omni.Core;
using UnityEngine;
using static Omni.Core.HttpLite;
public class InstancePlayerSala : ClientEventBehaviour
{
    [SerializeField]
    private GameObject playerSala;
    [SerializeField]
    private Transform sala1, sala2;
    private GameManager gManager;

    protected override void Start()
    {
        base.Start();
        gManager = NetworkService.Get<GameManager>("GameManager");
        InstancePlayers(gManager.salaInGame);
    }

    [Client(ConstantsRPC.INSTANT_PLAYER)]
    private void InstantPlayerRPC(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();

        ErrorManager.instance.ValidateError(response, 2, () => InstancePlayer(response.Data));
    }

    public void InstancePlayer(UsersModel user){

        SalaInGame salaIngame = gManager.salaInGame;
        if(salaIngame.lado1.Count > salaIngame.lado2.Count){
            salaIngame.lado2.Add(user);
            InsP(user, sala2);
        }else{
            salaIngame.lado1.Add(user);
            InsP(user, sala1);
        }

        gManager.OnPlayerRoom?.Invoke(salaIngame);
    }
    public void InstancePlayers(SalaInGame salaInGame)
    {
        salaInGame.lado1.ToList().ForEach(e=>InsP(e,sala1));
        salaInGame.lado2.ToList().ForEach(e=>InsP(e,sala2));
    }
    void InsP(UsersModel user, Transform lado)
        {
            PlayerSala pSalaInst = Instantiate(playerSala).GetComponent<PlayerSala>();
            pSalaInst.SetInfoPlayer(user);
            pSalaInst.transform.SetParent(lado, false);
        }
}
