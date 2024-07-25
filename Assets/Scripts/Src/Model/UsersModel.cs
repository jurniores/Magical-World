using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MemoryPack;
using Omni.Core;
using UnityEngine;

[MemoryPackable]
public partial class UsersModel : Models
{
    public int peerId;
    public string name;
    public string email;
    [JsonIgnore]
    public string password;
    public string newPass, passRepeat;
    public int master;
    public string hwid;
    public int cash;
    public int coin;
    public int ruby;
    public bool inGame;
    public string group;
    public PlayerConfigs pConfig = new();
    public int identity;

    public override object ToModel()
    {
        return new { id, name, email, password, master, hwid, cash, coin, ruby };
    }

}

