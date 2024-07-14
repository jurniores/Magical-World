using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using static Omni.Core.HttpLite;

public class InstancePlayerSala : ClientEventBehaviour
{

    [SerializeField]
    private GameObject playerSala;
    [SerializeField]
    private Transform sala1, sala2;
    [SerializeField]
    private Button btnSair;
    private GameManager gManager;
    private readonly Dictionary<int, PlayerSala> dicUsers = new();

    public override void Start()
    {
        base.Start();
        gManager = NetworkService.Get<GameManager>();

        InstancePlayers(gManager.salaInGame);

        //

        btnSair.onClick.AddListener(() =>
        {
            if (!Debounce.Bounce(0.5f)) return;
            Fetch.Get("/room/leave", res => { });
        });
    }

    [Client(ConstantsRPC.SET_RUNE)]
    private void SetRune(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();
        if (response.status == ConstantsDB.SUCCESS)
        {
            dicUsers[response.Data.peerId].SetRunes(response.Data.pConfig);
        }
    }

    [Client(ConstantsRPC.INSTANT_PLAYER)]
    private void InstantPlayerRPC(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();
        ErrorManager.ValidateError(response, 1, () => InstancePlayer(response.Data));
    }

    [Client(ConstantsRPC.DESTROY_PLAYER_ROOM)]
    void KikaDaSala(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();
        ErrorManager.ValidateError(response, 1, () =>
        {
            var user = response.Data;
            if (dicUsers.ContainsKey(user.peerId))
            {
                Destroy(dicUsers[user.peerId].gameObject);
                gManager.salaInGame.lado2.RemoveAll(e => e.peerId == user.peerId);
                gManager.salaInGame.lado1.RemoveAll(e => e.peerId == user.peerId);
                dicUsers.Remove(user.peerId);
            }

            gManager.OnPlayerRoom?.Invoke(gManager.salaInGame);

        });
    }

    [Client(ConstantsRPC.ME_DESTROY_ROOM)]
    void MeKikou(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse>();
        ErrorManager.ValidateError(response, 2, default, () => NetworkManager.LoadScene(1));
    }

    [Client(ConstantsRPC.CHANGE_MASTER)]
    void ChangeMasteR(DataBuffer res)
    {
        res.DecompressRaw();
        var response = res.FromJson<NetworkResponse<Sala>>();
        gManager.salaGame = response.Data;

        gManager.OnNewMaster?.Invoke();

        foreach (var (key, value) in dicUsers)
        {
            value.IsMaster();
        }

    }
    private void InsP(UsersModel user, Transform lado)
    {
        PlayerSala pSalaInst = Instantiate(playerSala).GetComponent<PlayerSala>();
        pSalaInst.SetInfoPlayer(user);
        pSalaInst.transform.SetParent(lado, false);
        dicUsers.Add(user.peerId, pSalaInst);
    }

    public void InstancePlayer(UsersModel user)
    {
        print("Senha: " + user.password);
        SalaInGame salaIngame = gManager.salaInGame;
        if (salaIngame.lado1.Count > salaIngame.lado2.Count)
        {
            salaIngame.lado2.Add(user);
            InsP(user, sala2);
        }
        else
        {
            salaIngame.lado1.Add(user);
            InsP(user, sala1);
        }

        gManager.OnPlayerRoom?.Invoke(salaIngame);
    }

    public void InstancePlayers(SalaInGame salaInGame)
    {
        salaInGame.lado1.ToList().ForEach(e => InsP(e, sala1));
        salaInGame.lado2.ToList().ForEach(e => InsP(e, sala2));
    }

    //Setado no botão de play
    public void StartGame(Button objBtn)
    {
        // if (gManager.salaGame.qtd == dicUsers.Count && objBtn.interactable)
        // {
            if (!Debounce.Bounce(1)) return;
            Fetch.Get("/room/start", res =>
            {
                objBtn.interactable = false;
                gManager.OnStartGame?.Invoke(res);
            });
        //}
    }
}
