using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Omni.Core;
using Omni.Core.Modules.Matchmaking;
using Omni.Shared.Collections;
using UnityEngine;
using static Omni.Core.HttpLite;
using static Omni.Core.NetworkManager;

public class RoomRoute : ServerEventBehaviour
{
    private readonly Dictionary<int, Sala> listRooms = new();

    protected override void Start()
    {
        base.Start();
        Http.Post("/room", CriaSala);
        Http.Post("/room/join", Join);
        Http.Get("/room/get", GetRooms);
        Http.Get("/room/get/players", GetPlayers);
        Http.Get("/room/get/player", GetPlayer);
    }



    private void CriaSala(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        var salaClient = req.FromJson<Sala>();
        string name = salaClient.nameSala;
        string password = salaClient.password;
        bool IsPass = salaClient.IsPass;

        if (name.Length < 4 || (name.Length > 0 && name.Length > 7))
        {
            res.ToJson(
                new NetworkResponse()
                {
                    status = ConstantsDB.ERROR,
                    message = "Nome da sala vazio ou muito grande!"
                }
            );
            res.Send();
            return;
        }
        if (IsPass)
        {
            if (password.Length < 4)
            {
                res.ToJson(
                    new NetworkResponse()
                    {
                        status = ConstantsDB.ERROR,
                        message = "Password muito pequeno!"
                    }
                );
                res.Send();
                return;
            }
        }

        bool exists = peer.Data.TryAdd("sala", salaClient);

        if (!exists)
        {
            res.ToJson(
                new NetworkResponse()
                {
                    status = ConstantsDB.ERROR,
                    message = "Você já tem uma sala!"
                }
            );
            res.Send();
            return;
        }

        var group = Matchmaking.Server.AddGroup(name);

        if (salaClient.qtd <= 2)
        {
            salaClient.qtd = 2;
        }
        else if (salaClient.qtd <= 6)
        {
            salaClient.qtd = 6;
        }
        else
        {
            salaClient.qtd = 10;
        }
        salaClient.master = peer.Id;
        salaClient.qtdPlayer = 1;
        group.Data.TryAdd("pass", password);


        //inserindo a sala no grupo e o player
        salaClient.password = "";
        var user = peer.Data.Get<UsersModel>("user").ToCopy();
        salaClient.lado1.Add(user);

        group.SerializedData.TryAdd("sala", salaClient);

        //Colocar o jogador dentro de grupo, depois de criado e configurado

        peer.Data.TryAdd("group", group);
        SalaInGame salaIngame = new();
        salaIngame.SetRooms(salaClient);
        listRooms.Add(salaClient.master, salaClient);

        Matchmaking.Server.JoinGroup(group, peer);


        res.ToJson(
            new NetworkResponse<Sala>()
            {
                status = ConstantsDB.SUCCESS,
                message = "Sala criada com sucesso!",
                Data = salaClient
            }
        );
        res.ToJson(salaIngame);
        res.Send(Target.NonGroupMembers, forceSendToSelf: true);
    }
    private void GetPlayer(DataBuffer res, NetworkPeer peer)
    {
        peer.Data.TryGet("user", out UsersModel user);
        res.ToJson(new NetworkResponse<UsersModel>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Player entrou na sala",
            Data = user
        });
        res.Send(Target.GroupMembers);
    }

    private void GetPlayers(DataBuffer res, NetworkPeer peer)
    {
        NetworkGroup Group = peer.Data.Get<NetworkGroup>("group");
        List<UsersModel> users = new();

        foreach (var (key, value) in Group.GetPeers())
        {

            users.Add(value.Data.Get<UsersModel>("user"));
        }


        res.ToJson(new NetworkResponse<List<UsersModel>>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Players obtido com sucesso!",
            Data = users
        });
        res.Send(Target.GroupMembers, groupId: Group.Id);
    }

    private void Join(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        string name = req.ReadString();
        string password = req.ReadString();
        var group = Matchmaking.Server.GetGroup(name);
        var response = new NetworkResponse();


        group.SerializedData.TryGet("sala", out Sala sala);
        group.Data.TryGet("pass", out string passServer);

        if (sala != null)
        {
            if (sala.IsPass)
            {
                if (password != passServer)
                {
                    response = new NetworkResponse()
                    {
                        status = ConstantsDB.ERROR,
                        message = "A senha está incorreta!"
                    };
                    res.ToJson(response);
                    res.Send();
                    return;
                }
            }
        }
        var player = group.GetPeerById(peer.Id);
        if (group == null)
        {
            response = new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Não foi possível encontrar o grupo!"
            };
            res.ToJson(response);
            res.Send();
            return;
        }

        else if (player != null)
        {
            response = new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Você já está neste grupo"
            };
            res.ToJson(response);
            res.Send();
            return;
        }
        else if (sala.qtdPlayer >= sala.qtd)
        {
            response = new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "A sala está cheia!"
            };
            res.ToJson(response);
            res.Send();
            return;
        }


        var user = peer.Data.Get<UsersModel>("user");
        peer.Data.TryAdd("group", group);
        if (sala.lado1.Count > sala.lado2.Count)
        {
            sala.lado2.Add(user);
        }
        else
        {
            sala.lado1.Add(user);
        }
        sala.qtdPlayer++;
        SalaInGame salaInGame = new();
        salaInGame.SetRooms(sala);


        Matchmaking.Server.JoinGroup(group, peer);

        response = new NetworkResponse<Sala>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Adicionado ao grupo com sucesso!",
            Data = sala
        };
        res.ToJson(response);
        res.ToJson(salaInGame);
        res.Send(Target.NonGroupMembers, forceSendToSelf: true);
        return;
    }

    private void GetRooms(DataBuffer res, NetworkPeer peer)
    {
        res.ToJson(listRooms);
        res.Send();
    }

    protected override void OnPlayerJoinedGroup(DataBuffer buffer, NetworkGroup group, NetworkPeer peer)
    {
        DataBuffer res = Pool.Rent();

        res.ToJson(new NetworkResponse<UsersModel>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Novo player entrou",
            Data = peer.Data.Get<UsersModel>("user").ToCopy()
        });
        Remote.Invoke(ConstantsRPC.INSTANT_PLAYER, peer.Id, res, Target.GroupMembersExceptSelf);
    }

    protected override void OnPlayerLeftGroup(
        NetworkGroup group,
        NetworkPeer peer,
        Status status,
        string reason
    )
    {
        base.OnPlayerLeftGroup(group, peer, status, reason);
    }
}
