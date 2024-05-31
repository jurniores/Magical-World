using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComprarOuCancelar : MonoBehaviour
{
    [SerializeField]
    private GameObject panelComprar;
    public panelComprar pComprar;
    private Animator panelAnim;
    void Start()
    {
        panelAnim = panelComprar.GetComponent<Animator>();
    }
    public void abrirPanel(ScriptableItens item)
    {
        panelComprar.SetActive(true);
        if (item.isRuby)
        {
            pComprar.InsertInfo(item.nameObject, item.qtd, item.preco, item.sprite);

        }
        else if (item.isRuna)
        {
            pComprar.InsertInfo(item.nameObject, item.descObject, item.valRuby, item.sprite);

        }
        else
        {
            pComprar.InsertInfo(item.nameObject, item.descObject, item.valCoin, item.valRuby, item.sprite);
        }
        
    }

    public void CancelPanel()
    {
        StartCoroutine(disablePanel());
    }

    IEnumerator disablePanel()
    {
        panelAnim.Play("PanelInfoBack");
        yield return new WaitForSeconds(0.25f);
        panelComprar.SetActive(false);
    }

}
