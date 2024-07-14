using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UnityEngine;

public class UsersModel : Models
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

    public override object ToModel()
    {
        return new { id, name, email, password, master, hwid, cash, coin, ruby };
    }

}
