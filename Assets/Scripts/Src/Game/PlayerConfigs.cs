using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs
{
    public List<int> listRunes = new();


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
