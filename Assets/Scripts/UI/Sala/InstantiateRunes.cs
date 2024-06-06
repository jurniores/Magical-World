using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRunes : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject rune;
    [SerializeField]
    private List<ScriptableItens> runes;
    void Start()
    {
        runes.ForEach(e=>InstRunes(e));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstRunes(ScriptableItens item){
        Transform runeInst = Instantiate(rune).transform;
        RunaDaSala rS = runeInst.GetComponent<RunaDaSala>();
        rS.SetRune(item);
        runeInst.SetParent(content,false);
    }
}
