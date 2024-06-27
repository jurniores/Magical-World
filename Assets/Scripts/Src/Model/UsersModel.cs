using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersModel : Models
{
    public string name;
    public string email;
    public string password, newPass, passRepeat;
    public int master;
    public string hwid;
    public int cash;
    public int coin;
    public int ruby;

    public override object ToModel()
    {
        return new { id, name, email, password, master, hwid, cash, coin, ruby };
    }

    public UsersModel ToCopy()
    {
        return new UsersModel()
        {
            id = -1,
            name = name,
            email = email,
            password = "password",
            newPass = newPass,
            passRepeat = passRepeat,
            master = master,
            hwid = hwid,
            cash = cash,
            coin = coin,
            ruby = ruby
        };
    }


}
