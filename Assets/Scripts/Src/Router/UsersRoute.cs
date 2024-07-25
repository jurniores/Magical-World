using System;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using static Omni.Core.HttpLite;

public class UsersRoute : ServerBehaviour
{
    private Controllers User = new Controllers("users");
    private Controllers Inventory = new Controllers("inventory");

    public override void Start()
    {
        base.Start();
        Http.PostAsync("/login", Login);
        Http.PostAsync("/info", UpdateInfo);
        Http.PostAsync("/pass", UpdatePass);
    }

    protected override async void OnServerPeerDisconnected(NetworkPeer peer, Status status)
    {
        if (status == Status.Begin)
        {
            peer.Data.TryGet<UsersModel>("user", out var peerUser);

            if (peerUser == null) return;

            InventoryModel peerInv = peer.Data.Get<InventoryModel>("inv");
            await User.UpdateAll(peerUser);
            await Inventory.UpdateAll(peerInv);
        }
    }

    async UniTask Login(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        try
        {
            UsersModel user = req.FromJson<UsersModel>();
            InventoryModel inventory = new();
            user.peerId = peer.Id;
            var userPlayer = await User.Get<UsersModel>(new { user.hwid });
            //Cria um novo player e seu intentário
            if (userPlayer == null)
            {
                int userInsert = await User.Insert(user);
                inventory.idUser = userInsert;
                await Inventory.Insert(inventory);
                user.name = Utils.RandName(6);
                peer.Data.TryAdd("user", user);
                peer.Data.TryAdd("inv", inventory);

                res.Write(ConstantsDB.SUCCESS);
                res.ToJson(user);
                res.ToJson(inventory);

                res.Send();
                return;
            }

            //Envia para o player, sua classe e seu inventário e os salva no peer

            var invPlayer = await Inventory.Get<InventoryModel>(new { idUser = userPlayer.id });
            invPlayer.InitDic();
            userPlayer.peerId = peer.Id;
            userPlayer.name ??= Utils.RandName(6);

            peer.Data.TryAdd("user", userPlayer);
            peer.Data.TryAdd("inv", invPlayer);

            UsersModel userClient = userPlayer;

            if (userPlayer.password == null)
                userClient.password = null;
            InventoryModel invClient = invPlayer;
            
            res.Write(ConstantsDB.SUCCESS);
            res.ToJson(userClient);
            res.ToJson(invClient);
            res.Send();
        }
        catch
        {
            res.Write(ConstantsDB.ERROR);
            res.ToJson(new UsersModel());
            res.ToJson(new InventoryModel());
            res.Send();
        }
    }

    async UniTask UpdateInfo(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        var userPeer = peer.Data.Get<UsersModel>("user");
        var userClient = req.FromJson<UsersModel>();

        NetworkResponse response;
        if (userPeer.password == null || userClient.password == userPeer.password)
        {
            if (userClient.email.Length > 0 && Utils.IsEmail(userClient.email))
            {
                var userPlayer = await User.Get<UsersModel>(new { userClient.email });
                if (userPlayer == null)
                {
                    userPeer.email = userClient.email;
                    if (userClient.name.Length > 3)
                    {
                        userPeer.name = userClient.name;
                        response = new NetworkResponse<UsersModel>()
                        {
                            status = ConstantsDB.SUCCESS,
                            message = "Seu email e nome foram alterados",
                            Data = userPeer
                        };
                    }
                    else
                    {
                        response = new NetworkResponse<UsersModel>()
                        {
                            status = ConstantsDB.SUCCESS,
                            message = "Seu email foi alterado",
                            Data = userPeer
                        };
                    }
                }
                else
                {
                    response = new NetworkResponse<UsersModel>()
                    {
                        status = ConstantsDB.ERROR,
                        message = "Email já existe, tente outro",
                        Data = null
                    };
                }
            }
            else if (userClient.name.Length > 3 && userClient.email.Length == 0)
            {
                userPeer.name = userClient.name;
                response = new NetworkResponse<UsersModel>()
                {
                    status = ConstantsDB.SUCCESS,
                    message = "Seu nome foi alterado",
                    Data = userPeer
                };
            }
            else
            {
                response = new NetworkResponse<UsersModel>()
                {
                    status = ConstantsDB.ERROR,
                    message = "Não é um email e o nome é muito pequeno",
                    Data = null
                };
            }
        }
        else
        {
            response = new NetworkResponse<UsersModel>()
            {
                status = ConstantsDB.ERROR,
                message = "Password errado",
                Data = null
            };
        }
        res.ToJson(response);
        res.Send();
    }

    void UpdatePass(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        var userPeer = peer.Data.Get<UsersModel>("user");
        var userClient = req.FromJson<UsersModel>();
        var response = new NetworkResponse();
        if (userPeer.password == null || userClient.password == userPeer.password)
        {
            if (userClient.newPass.Length >= 6 && userClient.newPass == userClient.passRepeat)
            {
                userPeer.name = userClient.name;
                userPeer.password = userClient.newPass;
                response = new NetworkResponse<UsersModel>()
                {
                    status = ConstantsDB.SUCCESS,
                    message = "Password alterado com sucesso!",
                    Data = userPeer                };
            }
            else if (userClient.newPass.Length >= 6 && userClient.newPass != userClient.passRepeat)
            {
                response = new NetworkResponse<UsersModel>()
                {
                    status = ConstantsDB.ERROR,
                    message = "Passwords diferentes",
                    Data = null
                };
            }
        }
        else
        {
            response = new NetworkResponse<UsersModel>()
            {
                status = ConstantsDB.ERROR,
                message = "Password inválido",
                Data = null
            };
        }
        res.ToJson(response);
        res.Send();
    }
}
