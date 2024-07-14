using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Omni.Core;
using Omni.Shared.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : ServiceBehaviour
{
    private UsersModel user;
    private InventoryModel inventory;
    private Dictionary<int, Sala> salas = new();
    public Action OnUser;
    public Action OnInventory;
    public Action<Dictionary<int, Sala>> OnRooms;
    public Action<Sala> OnUpdateRoom;
    public Action<SalaInGame> OnPlayerRoom;
    public Action<DataBuffer> OnStartGame;
    public Action OnNewMaster;
    public Sala salaGame;
    public SalaInGame salaInGame;

    public override void Start()
    {
        base.Start();
        BufferWriterExtensions.DefaultEncoding = Encoding.UTF8;
    }

    public void AddSala(Sala sala)
    {
        salas.TryAdd(sala.master, sala);
        OnUpdateRoom?.Invoke(sala);
    }

    public void UpdateSala(Sala sala)
    {
        if (salas.ContainsKey(sala.master))
        {
            salas[sala.master] = sala;
        }
        OnUpdateRoom?.Invoke(sala);
    }
    public Dictionary<int, Sala> GetSalas()
    {
        return salas;
    }


    public UsersModel User
    {
        get => user;
        set
        {
            user = value;
            OnUser?.Invoke();
        }
    }
    public InventoryModel Inventory
    {
        get => inventory;
        set
        {
            inventory = value;
            inventory.InitDic();
            OnInventory?.Invoke();
        }
    }

    public Dictionary<int, Sala> Salas
    {
        set
        {
            salas = value;
            OnRooms?.Invoke(value);
        }
    }

}

