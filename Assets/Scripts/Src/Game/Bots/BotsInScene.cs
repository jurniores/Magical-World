using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using MemoryPack;
using Omni.Core;
using UnityEngine;

public class BotsInScene : MonoBehaviour
{
    public SerializedDictionary<int, BotsQtd> bots;

    [Serializable]
    public struct BotsQtd
    {
        public NetworkIdentity botServer, botClient;
        public int qtd;
    }
}


