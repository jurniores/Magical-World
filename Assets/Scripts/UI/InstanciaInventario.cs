using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;
using UnityEngine;

public class InstanciaInventario : MonoBehaviour
{
    [SerializeField]
    private GameObject btnInst;

    [SerializeField]
    public SerializableDictionaryBase<int, ScriptableItens> itens;
    private GameManager gManager;
    List<GameObject> ListBtnInstanciados = new();

    void OnEnable()
    {
        gManager = NetworkService.Get<GameManager>();
        InstanceInv();
    }

    void InstanceInv()
    {
        ListBtnInstanciados.ForEach(e => Destroy(e));
        foreach (var (key, value) in gManager.Inventory.itensDic)
        {
            Transform btn = Instantiate(btnInst).transform;
            btn.SetParent(transform, false);
            BtnInstanciados btnInstanciado = btn.GetComponent<BtnInstanciados>();
            btnInstanciado.InsertInv(itens[key]);
            ListBtnInstanciados.Add(btn.gameObject);
        }
    }
}
