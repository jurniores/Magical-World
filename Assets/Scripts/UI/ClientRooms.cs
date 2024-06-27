using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Shared.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Omni.Core.HttpLite;

public class ClientRooms : MonoBehaviour
{
    private GameManager gManager;

    void Start()
    {
        gManager = NetworkService.Get<GameManager>("GameManager");

        Fetch.Get(
            "/room/get",
            res =>
            {
                gManager.Salas = res.FromJson<Dictionary<int, Sala>>();
            }
        );

        Fetch.AddPostHandler(
            "/room",
            res =>
            {
                var response = res.FromJson<NetworkResponse<Sala>>();
                gManager.AddSala(response.Data);
            }
        );
        Fetch.AddPostHandler("/room/join", res =>
        {
            var response = res.FromJson<NetworkResponse<Sala>>();
            gManager.UpdateSala(response.Data);
        });

    }

    public void InstanciaSalas(){
        if(!Debounce.instance.Bounce(1)) return;
        gManager.OnRooms?.Invoke(gManager.GetSalas());
    }
}
