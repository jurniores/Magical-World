using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Shared.Collections;
using UnityEngine;

public class InstanciaSalas : MonoBehaviour
{
    [SerializeField]
    private GameObject objSala;
    private GameManager gManager;
    Dictionary<int, BtnSalas> listSalasInstanciadas = new();
    void Start()
    {
        gManager = NetworkService.Get<GameManager>("GameManager");
        gManager.OnRooms += InstantiateSalas;
        gManager.OnUpdateRoom += UpdateRoom;
        InstantiateSalas(gManager.GetSalas());
    }
    private void InstantiateSalas(Dictionary<int, Sala> salas)
    {
        foreach (var (key, value) in listSalasInstanciadas)
        {
            if(value.gameObject != null)
            Destroy(value.gameObject);
            
        }
        listSalasInstanciadas = new();
        foreach (var (key, value) in salas)
        {
            InstantiateSala(value);
        }
    }

    private void InstantiateSala(Sala sala)
    {
        Transform btn = Instantiate(objSala).transform;
        btn.SetParent(transform, false);
        BtnSalas btnSala = btn.GetComponent<BtnSalas>();
        btnSala.SetInfoSala(sala);
        listSalasInstanciadas.TryAdd(sala.master, btnSala);
    }


    private void UpdateRoom(Sala sala)
    {
        if (listSalasInstanciadas.ContainsKey(sala.master))
        {
            listSalasInstanciadas[sala.master].UpdateSala(sala);
        }else{
            InstantiateSala(sala);
        }
    }

    // Update is called once per frame
    void Update() { }
}
