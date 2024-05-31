using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciaScriptables : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableItens> itens;
    [SerializeField]
    private GameObject btnInst;

    [SerializeField]
    void Start()
    {
    

        itens.ForEach(item =>
        {
            
            Transform btn = Instantiate(btnInst).transform;
            btn.SetParent(transform, false);
            BtnInstanciados btnInstanciado = btn.GetComponent<BtnInstanciados>();
            if(item.isRuby){
                btnInstanciado.InsertRuby(item);
                return;
            }
            btnInstanciado.InsertScriptable(item);

        });

        print("Chamei");
    }

    public void InstanciaInventario(){
        print("Instanciando objetos do inventario");
    }
}
