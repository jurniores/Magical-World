using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Shared.Collections;
using UnityEngine;
public class InstanciaSalas : ClientBehaviour
{
    [SerializeField]
    private GameObject objSala;
    private GameManager gManager;
    Dictionary<int, BtnSalas> listSalasInstanciadas = new();
    public override void Start()
    {
        base.Start();
        listSalasInstanciadas = new();
        gManager = NetworkService.Get<GameManager>("GameManager");
        gManager.OnRooms += InstantiateSalas;
        gManager.OnUpdateRoom += UpdateRoom;
        InstantiateSalas(gManager.GetSalas());
    }
    private void InstantiateSalas(Dictionary<int, Sala> salas)
    {
        foreach (var (key, value) in listSalasInstanciadas)
        {
            if (value != null)
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
        if (this == null) return;
        Transform btn = Instantiate(objSala).transform;
        btn.SetParent(transform, false);
        BtnSalas btnSala = btn.GetComponent<BtnSalas>();
        btnSala.SetInfoSala(sala);
        listSalasInstanciadas.TryAdd(sala.id, btnSala);
    }


    private void UpdateRoom(Sala sala)
    {
        if (listSalasInstanciadas.ContainsKey(sala.id))
        {
            listSalasInstanciadas[sala.id].UpdateSala(sala);
        }
        else
        {
            InstantiateSala(sala);
        }
    }

    [Client(ConstantsRPC.CHANGE_MASTER)]
    void ChangeMasteR(DataBuffer res)
    {

    }


    [Client(ConstantsRPC.UPDATE_ROOM)]
    void UpdateRoomRPC(DataBuffer res)
    {
        var sala = res.FromJson<Sala>();
        gManager.UpdateSala(sala);
    }
}
