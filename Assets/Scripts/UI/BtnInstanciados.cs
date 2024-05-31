using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BtnInstanciados : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmProName, tmProCoin, tmProRuby;
    [SerializeField]
    private Image image;

    public void InsertScriptable(ScriptableItens item)
    {
        tmProName.text = item.nameObject;
        image.sprite = item.sprite;
        if (tmProCoin)
        {
            tmProCoin.text = item.valCoin.ToString();
        }
        if (tmProRuby)
        {
            tmProRuby.text = item.valRuby.ToString();
        }
        PegaFuncPanel(item);
    }

    public void InsertRuby(ScriptableItens ruby)
    {
        tmProName.text = ruby.qtd.ToString();
        tmProRuby.text = ruby.preco.ToString() + "R$";
        PegaFuncPanel(ruby);
    }

    void PegaFuncPanel(ScriptableItens item)
    {   
        ComprarOuCancelar compraCancel = Camera.main.GetComponent<ComprarOuCancelar>();
        GetComponent<Button>().onClick.AddListener(()=>compraCancel.abrirPanel(item));
       
    }
}
