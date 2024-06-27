using System.Collections.Generic;
using System.Threading.Tasks;
using Omni.Core;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using static Omni.Core.HttpLite;

public class InventoryRoute : MonoBehaviour
{
    private Controllers Inventory = new Controllers("inventory");

    //
    public SerializableDictionaryBase<int, ScriptableItens> itens;
    public SerializableDictionaryBase<int, ScriptableItens> runes;

    void Awake()
    {
        Http.Post("/cloja", Insert);
    }

    void Insert(DataBuffer req, DataBuffer res, NetworkPeer peer)
    {
        int idItem = req.Read<int>();
        byte tipoItem = req.Read<byte>();
        int tipoMoeda = req.Read<int>();

        if (tipoItem == ConstantsDB.IS_ITEM)
            CompraTipoItem(itens);
        else if (tipoItem == ConstantsDB.IS_RUNE)
            CompraTipoItem(runes);
        else
        {
            res.ToJson(
                new NetworkResponse()
                {
                    status = ConstantsDB.ERROR,
                    message = "Algo de errado aconteceu, tente mais tarde!",
                }
            );
            res.Send();
            return;
        }

        void CompraTipoItem(SerializableDictionaryBase<int, ScriptableItens> tipoItens)
        {
            var response = new NetworkResponse();
            
            if (tipoItens.ContainsKey(idItem))
            {
                var item = tipoItens[idItem];
                UsersModel userPeer = peer.Data.Get<UsersModel>("user");
                InventoryModel invPeer = peer.Data.Get<InventoryModel>("inv");

                UsersModel userClient = new();
                InventoryModel invClient = new();

                if (tipoMoeda == ConstantsDB.COIN_VALIDATE)
                {
                    if (userPeer.coin >= item.valCoin)
                    {
                        if (!invPeer.itensDic.ContainsKey(item.id))
                        {
                            userPeer.coin -= item.valCoin;
                            invPeer.itensDic.Add(item.id, item.nameObject);

                            userClient = userPeer.ToCopy();
                            invClient = invPeer.ToCopy();

                            response = new NetworkResponse()
                            {
                                status = ConstantsDB.SUCCESS,
                                message = "Item comprado com sucesso!",
                            };
                        }
                        else
                        {
                            response = new NetworkResponse()
                            {
                                status = ConstantsDB.ERROR,
                                message = "Você já tem este item",
                            };
                        }
                    }
                    else
                    {
                        response = new NetworkResponse()
                        {
                            status = ConstantsDB.ERROR,
                            message = "Você não tem o valor suficiente para executar esta operação",
                        };
                    }
                }
                else if (tipoMoeda == ConstantsDB.RUBY_VALIDATE)
                {
                    if (userPeer.ruby >= item.valRuby)
                    {
                        if (!invPeer.itensDic.ContainsKey(item.id))
                        {
                            userPeer.ruby -= item.valRuby;
                            invPeer.itensDic.Add(item.id, item.nameObject);

                            userClient = userPeer.ToCopy();
                            invClient = invPeer.ToCopy();

                            response = new NetworkResponse()
                            {
                                status = ConstantsDB.SUCCESS,
                                message = "Item comprado com sucesso!",
                            };
                        }
                        else
                        {
                            response = new NetworkResponse()
                            {
                                status = ConstantsDB.ERROR,
                                message = "Você já tem este item",
                            };
                        }
                    }
                    else
                    {
                        response = new NetworkResponse()
                        {
                            status = ConstantsDB.ERROR,
                            message = "Você não tem o valor suficiente para executar esta operação",
                        };
                    }
                }

                res.ToJson(response);
                res.ToJson(userClient);
                res.ToJson(invClient);

                res.Send();
                return;
            }

            response = new NetworkResponse()
            {
                status = ConstantsDB.ERROR,
                message = "Este item não existe!",
            };
            res.ToJson(response);
            res.Send();
        }
    }

    [Server(ConstantsDB.INVENTORY_GET)]
    async void GetId(DataBuffer buffer)
    {
        int id = buffer.Read<int>();
        InventoryModel model = await Inventory.GetId<InventoryModel>(id);
    }

    [Server(ConstantsDB.INVENTORY_UPDATE)]
    async void UpdateAtt(DataBuffer buffer)
    {
        InventoryModel model = buffer.FromJson<InventoryModel>();
        int result = await Inventory.UpdateAll(model);
    }

    [Server(ConstantsDB.INVENTORY_DELETE)]
    async void Delete(DataBuffer buffer)
    {
        int id = buffer.Read<int>();
        int model = await Inventory.DeleteId(id);
    }
}
