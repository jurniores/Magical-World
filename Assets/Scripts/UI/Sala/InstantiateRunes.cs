using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class InstantiateRunes : ServiceBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject rune;
    private readonly List<RunaDaSala> listRunaBtn = new();
    public List<ScriptableItens> runes;
    private GameManager gManager;
    private readonly int indexRune = 1000;
    private int countRunasClicadas = 0;
    protected override void OnStart()
    {
        gManager = NetworkService.Get<GameManager>();
        InventoryModel inv = gManager.Inventory;
        inv.InitDic();
        foreach (var (key, item) in inv.itensDic)
        {
            if (key >= indexRune)
            {
                InstRunes(runes[key - indexRune]);
            }
        }
    }

    public void ValidateRune(RunaDaSala runaSala)
    {
        listRunaBtn.Add(runaSala);
        if (listRunaBtn.Count > 3)
        {
            listRunaBtn[0].BlockMe();
            listRunaBtn.RemoveAt(0);
        }
    }
    void InstRunes(ScriptableItens item)
    {
        Transform runeInst = Instantiate(rune).transform;
        RunaDaSala rS = runeInst.GetComponent<RunaDaSala>();
        rS.SetRune(item);
        runeInst.SetParent(content, false);
    }
}
