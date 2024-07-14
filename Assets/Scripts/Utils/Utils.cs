using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Omni.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class Utils
{
    static char[] consonants = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'y', 'x', 'z' };
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

    public static string RandName(int count)
    {
        int rand = UnityEngine.Random.Range(0, consonants.Length - 1);
        string nameReturn = "";
        for (int i = 0; i < count; i++)
        {
            nameReturn += consonants[rand];
            rand = UnityEngine.Random.Range(0, consonants.Length - 1);

        }
        return nameReturn;
    }

    public async static UniTask FakeLoad(Action<float> fn)
    {
        float count = 1;
        while (count != 50)
        {
            float div = count / 100;
            fn(div);
            count++;
            await UniTask.WaitForSeconds(0.001f);
        }
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

public class Sala
{
    public int id;
    public int master;
    public string nameSala;
    public int qtd, qtdPlayer;
    public bool IsPass, dontName = false;
    public string password;
    [JsonIgnore]
    public List<UsersModel> lado1 = new(), lado2 = new();
    [JsonIgnore]
    public List<UsersModel> playersNaSala = new();


}

public class SalaInGame
{

    public List<UsersModel> lado1 = new(), lado2 = new();

    public void SetRooms(Sala sala)
    {
        lado1 = sala.lado1;
        lado2 = sala.lado2;
    }

}
