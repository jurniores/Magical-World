
using System;
using System.Collections.Generic;
using Omni.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using static Omni.Core.HttpLite;
using static Omni.Core.NetworkManager;

public class RoomRoute : ServerBehaviour
{
    [SerializeField]
    private NetworkIdentity ServerGroup;
    private readonly Dictionary<int, Sala> listRooms = new();
    private readonly Dictionary<int, Sala> userRoomDiconnect = new();

    public override void Start()
    {
        base.Start();
        Http.Post("/room", CriaSala);
        Http.Post("/room/join", Join);
        Http.Post("/room/kick", PlayerKick);
        Http.Get("/room/leave", SairRoom);
        Http.Get("/room/start", StartGame);
        Http.Get("/room/get", GetRooms);
        Http.Get("/room/get/players", GetPlayers);
        Http.Get("/room/get/player", GetPlayer);
        Http.Post("/room/set/rune", SetRune);
        Http.Get("/room/get/pconfig", GetPlayerConfigs);
    }

    private void GetPlayerConfigs(DataBuffer res, NetworkPeer peer)
    {
        Sala sala = peer.Data.Get<Sala>("sala");

        SalaInGame salaIngame = new();

        salaIngame.SetRooms(sala);

        res.ToJson(new NetworkResponse()
        {
            status = ConstantsDB.SUCCESS,
            message = "Configurações obtidas!"
        });
        res.ToJson(salaIngame);

        res.Send();
    }

    private void SetRune(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        int runeId = req.FastRead<int>();
        UsersModel user = peer.Data.Get<UsersModel>("user");
        bool setValida;
          
        setValida = user.pConfig.SetRune(runeId);

        if (setValida)
        {
            res.ToJson(new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Runa já está setada!",
            });
        }
        else
        {
            res.ToJson(new NetworkResponse<UsersModel>()
            {
                status = ConstantsDB.SUCCESS,
                message = "Runa adicionada com sucesso!",
                Data = user
            });
        }

        res.Send();

        Remote.Invoke(ConstantsRPC.SET_RUNE, peer, res, Target.GroupMembers);
    }

    private void StartGame(DataBuffer res, NetworkPeer peer)
    {
        var sala = peer.Data.Get<Sala>("sala");
        var user = peer.Data.Get<UsersModel>("user");

        if (user.inGame)
        {
            res.ToJson(new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Você já iniciou a partida!",
            });
            res.Send(HttpTarget.GroupMembers);
            return;
        }

        // if (sala.qtd != sala.playersNaSala.Count)
        // {
        //     res.ToJson(new NetworkResponse()
        //     {
        //         status = ConstantsDB.ERROR,
        //         message = "A sala ainda não está cheia!",
        //     });
        //     res.Send(HttpTarget.GroupMembers);
        //     return;
        // }


        var group = peer.Data.Get<NetworkGroup>("group");

        foreach (var (key, peerPlayer) in group.Peers)
        {
            var userPlayer = peerPlayer.Data.Get<UsersModel>("user");
            userPlayer.inGame = true;
            userPlayer.group = group.Name;
        }

        res.ToJson(new NetworkResponse()
        {
            status = ConstantsDB.SUCCESS,
            message = "A partida vai iniciar!",
        });
        res.Send(HttpTarget.GroupMembers);

        InitGame(peer, group);

    }

    protected override void OnServerPeerDisconnected(NetworkPeer peer, Status status)
    {
        if (status == Status.Begin)
        {
            Leave(peer);
        }
    }


    private void PlayerKick(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        var userKick = req.FromJson<UsersModel>();
        var sala = peer.Data.Get<Sala>("sala");
        NetworkGroup group = peer.Data.Get<NetworkGroup>("group");

        if (sala.master != peer.Id)
        {
            res.ToJson(new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Você não é o master da sala"
            });
            res.Send();
            return;
        }

        if (userKick.peerId == peer.Id)
        {
            res.ToJson(new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Você não pode se expulsar!"
            });
            res.Send();
            return;
        }

        foreach (var (key, peerKick) in group.Peers)
        {
            var userKickCompare = peerKick.Data.Get<UsersModel>("user");

            if (userKickCompare.peerId == userKick.peerId)
            {
                Matchmaking.Server.LeaveGroup(group, peerKick);
                res.ToJson(new NetworkResponse()
                {
                    status = ConstantsDB.NEUTRO,
                });
                res.Send();
                return;
            }
        }

        res.ToJson(new NetworkResponse()
        {
            status = ConstantsDB.ERROR,
            message = "Aconteceu algum erro!"
        });
        res.Send();

    }

    private void CriaSala(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        var salaClient = req.FromJson<Sala>();
        string name = salaClient.nameSala;
        string password = salaClient.password;
        bool IsPass = salaClient.IsPass;
        bool dontName = salaClient.dontName;

        if (!dontName)
        {
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

        }

        if (dontName)
        {
            salaClient.nameSala = Utils.RandName(5);
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

        var groupValida = Matchmaking.Server.TryAddGroup(name, out var group);

        if (!groupValida)
        {

            res.ToJson(
                new NetworkResponse()
                {
                    status = ConstantsDB.ERROR,
                    message = "Nome do grupo ja existe!"
                }
            );
            res.Send();
            return;

        }

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
        int rand = UnityEngine.Random.Range(0, 2147483647);

        salaClient.master = peer.Id;
        salaClient.qtdPlayer = 1;
        group.Data.TryAdd("pass", password);


        //inserindo a sala no grupo e o player
        salaClient.password = "";
        var user = peer.Data.Get<UsersModel>("user");
        salaClient.lado1.Add(user);
        salaClient.playersNaSala.Add(user);

        group.Data.TryAdd("sala", salaClient);

        //Colocar o jogador dentro de grupo, depois de criado e configurado

        peer.Data.TryAdd("group", group);
        SalaInGame salaIngame = new();
        salaIngame.SetRooms(salaClient);
        bool listOk = listRooms.TryAdd(rand, salaClient);

        while (!listOk)
        {
            rand = UnityEngine.Random.Range(0, 2147483647);
            listOk = listRooms.TryAdd(rand, salaClient);
        }

        salaClient.id = rand;

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
        res.Send(HttpTarget.NonGroupMembers);
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
        res.Send(HttpTarget.NonGroupMembers);
    }

    private void GetPlayers(DataBuffer res, NetworkPeer peer)
    {
        NetworkGroup Group = peer.Data.Get<NetworkGroup>("group");
        List<UsersModel> users = new();

        foreach (var (key, value) in Group.Peers)
        {
            var user = value.Data.Get<UsersModel>("user");
            users.Add(user);
            print(user.pConfig.listRunes.Count);
        }


        res.ToJson(new NetworkResponse<List<UsersModel>>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Players obtido com sucesso!",
            Data = users
        });
        res.Send(HttpTarget.GroupMembers);
    }

    private void Join(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        string name = req.ReadString();
        string password = req.ReadString();
        var existsGp = Matchmaking.Server.TryGetGroup(name, out var group);

        NetworkResponse response;
        if (!existsGp)
        {
            response = new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Grupo não existe mais!"
            };
            res.ToJson(response);
            res.Send();
            return;
        }

        group.Data.TryGet("sala", out Sala sala);
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
        bool exists = peer.Data.ContainsKey("sala");

        if (exists)
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

        bool player = group.TryGetPeer(peer.Id, out _);
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

        else if (player != false)
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

        peer.Data.Add("sala", sala);
        var user = peer.Data.Get<UsersModel>("user");
        peer.Data.TryAdd("group", group);

        group.Data.TryAdd("sala", sala);

        if (sala.lado1.Count > sala.lado2.Count)
        {
            sala.lado2.Add(user);
        }
        else
        {
            sala.lado1.Add(user);
        }
        sala.playersNaSala.Add(user);

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
        res.Send(HttpTarget.NonGroupMembers);
        return;
    }

    private void SairRoom(DataBuffer res, NetworkPeer peer)
    {
        Leave(peer);
        res.Send();
    }
    private void GetRooms(DataBuffer res, NetworkPeer peer)
    {
        res.ToJson(listRooms);
        res.Send();
    }

    protected override void OnPlayerJoinedGroup(DataBuffer buffer, NetworkGroup group, NetworkPeer peer)
    {
        using DataBuffer res = Pool.Rent();
        res.ToJson(new NetworkResponse<UsersModel>()
        {
            status = ConstantsDB.SUCCESS,
            message = "Novo player entrou!",
            Data = peer.Data.Get<UsersModel>("user")
        });
        Remote.Invoke(ConstantsRPC.INSTANT_PLAYER, peer, res, Target.GroupMembersExceptSelf);
    }

    protected override void OnPlayerLeftGroup(NetworkGroup group, NetworkPeer peer, Status status, string reason)
    {
        Sala sala = group.Data.Get<Sala>("sala");

        if (status == Status.Begin)
        {
            UsersModel user = peer.Data.Get<UsersModel>("user");
            user.pConfig.listRunes = new();
            user.inGame = false;
            user.group = null;

            sala.qtdPlayer--;
            peer.Data.Remove("sala");
            peer.Data.Remove("group");

            sala.lado1.RemoveAll(e => e.peerId == peer.Id);
            sala.lado2.RemoveAll(e => e.peerId == peer.Id);
            sala.playersNaSala.RemoveAll(e => e.peerId == peer.Id);

            using DataBuffer res = Pool.Rent();

            res.ToJson(new NetworkResponse<UsersModel>()
            {
                status = ConstantsDB.SUCCESS,
                message = "Um player foi removido da sala!",
                Data = peer.Data.Get<UsersModel>("user")
            });
            Remote.Invoke(ConstantsRPC.DESTROY_PLAYER_ROOM, peer, res, Target.GroupMembersExceptSelf);

            //

            if (sala.qtdPlayer == 0)
            {
                if (listRooms.ContainsKey(sala.id))
                {
                    listRooms.Remove(sala.id);
                }
            }

            using DataBuffer resKick = Pool.Rent();

            resKick.ToJson(new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Você foi removido da sala!",
            });

            Remote.Invoke(ConstantsRPC.ME_DESTROY_ROOM, peer, resKick, Target.Self);
        }

    }
    //Este método só é chamado quando o player escolhe sair da sala ou é desconectado
    void Leave(NetworkPeer peer)
    {

        peer.Data.TryGet("sala", out Sala sala);
        if (sala == null) return;


        var groupValida = peer.Data.TryGet<NetworkGroup>("group", out var group);

        if (!groupValida) return;

        using DataBuffer res = Pool.Rent();

        //Se for lider da sala troca-o
        if (sala.master == peer.Id && sala.playersNaSala.Count > 1)
        {
            if (sala.playersNaSala[0].peerId == peer.Id)
            {
                sala.master = sala.playersNaSala[1].peerId;
            }
            else
            {
                sala.master = sala.playersNaSala[0].peerId;
            }


            res.ToJson(new NetworkResponse<Sala>()
            {
                Data = sala
            });
            res.CompressRaw();
            Remote.Invoke(ConstantsRPC.CHANGE_MASTER, peer, res, Target.GroupMembersExceptSelf);
        }

        Matchmaking.Server.LeaveGroup(group, peer);
    }
    //Só serve para iniciar uma partida
    private void InitGame(NetworkPeer peer, NetworkGroup group)
    {
        using DataBuffer buffer = Pool.Rent();
        NetworkIdentity identityGroup = ServerGroup.InstantiateOnServer(peer);
        var groupInstServer = identityGroup.GetComponent<ServerGroup>();
        groupInstServer.SetInfoGroupServer(group);

        buffer.WriteIdentity(identityGroup);

        Remote.GlobalInvoke(ConstantsRPC.INIT_GAME, peer, buffer, Target.GroupMembers);
        
    }

}

