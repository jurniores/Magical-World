using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Omni.Core;
using UnityEngine;

public class InventoryModel : Models
{
    public string itens = "{\"-1\": \"init\"}";
    public int idUser;
    [JsonIgnore]
    public Dictionary<int, string> itensDic = new();
    public override object ToModel()
    {
        itens = NetworkManager.ToJson(itensDic);
        return new { id, itens, idUser };
    }

    public void InitDic()
    {

        itensDic = NetworkManager.FromJson<Dictionary<int, string>>(itens);
    }

    public InventoryModel ToCopy()
    {
        return new InventoryModel()
        {
            id = -1,
            itens = NetworkManager.ToJson(itensDic),
            idUser = -1
        };
    }
}
