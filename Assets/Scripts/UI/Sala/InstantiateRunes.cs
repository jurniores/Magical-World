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
        runes.ForEach(e=>InstRunes());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstRunes(){
        Transform runeInst = Instantiate(rune).transform;
        runeInst.SetParent(content,false);
    }
}
