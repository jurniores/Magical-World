using System.Collections;
using System.Collections.Generic;
using MemoryPack;
using Omni.Core;
using UnityEngine;
[MemoryPackable]
public partial class PlayerConfigs
{
    private int peerId;
    public int identity = 0;
    public List<int> listRunes = new();

    private DataBuffer dBuffer = new DataBuffer();
    //Só seta no máximo 3 runas, se for enviado outra, esta passará tomar o primeiro lugar
    public bool SetRune(int rune)
    {
        if (listRunes.Contains(rune)) return true;

        listRunes.Add(rune);
        if (listRunes.Count > 3)
        {
            listRunes.RemoveAt(0);
        }
        return false;
    }

}
