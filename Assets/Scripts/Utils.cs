using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Omni.Core;
using UnityEngine;

public static class Utils
{
    public static bool IsEmail(string email)
    {
        // Expressão regular para verificar se o email é válido
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Verifica se o email corresponde ao padrão
        if (Regex.IsMatch(email, pattern))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static T DeepCopy<T>(this T obj)
        where T : class
    {
        var json = NetworkManager.ToJson(obj);
        return NetworkManager.FromJson<T>(json);
    }
}

public class NetworkResponse
{
    public byte status;
    public string message;
}

public class NetworkResponse<T> : NetworkResponse
{
    public T Data;
}

public class Sala{
    public int master;
    public string nameSala;
    public int qtd, qtdPlayer;
    public bool IsPass;
    public string password;
    [JsonIgnore]
    public List<UsersModel> lado1 = new(), lado2 = new();
    
    
}

public class SalaInGame{

    public List<UsersModel> lado1 = new (), lado2 = new ();

    public void SetRooms(Sala sala){
        lado1 = sala.lado1;
        lado2 = sala.lado2;
    }
    
}
