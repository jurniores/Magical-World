using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComprarOuCancelar : MonoBehaviour
{
    [SerializeField]
    private Color colordef, colordefm, colorataq, colorataqm, colorvel, colorvelataq, colorcd;
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
            pComprar.InsertInfo(item.nameObject, TratDesc(item), item.valRuby, item.sprite);

        }
        else
        {
            pComprar.InsertInfo(item.nameObject, TratDesc(item), item.valCoin, item.valRuby, item.sprite);
        }

    }

    string TratDesc(ScriptableItens item)
    {
        List<string> stringMaster = new List<string>();

        if (item.def > 0)
        {
            string cor = tratColor(colordef);
            stringMaster.Add($" Defesa de <color=#{cor}>{item.def}</color>");
        }
        if (item.defM > 0)
        {
             string cor = tratColor(colordefm);
            stringMaster.Add($" Defesa Mágica de <color=#{cor}>{item.defM}</color>");
        }
        if (item.ataq > 0)
        {
             string cor = tratColor(colorataq);
            stringMaster.Add($" Ataque de <color=#{cor}>{item.ataq}</color>");
        }
        if (item.ataqM > 0)
        {
             string cor = tratColor(colorataqm);
            stringMaster.Add($" Ataque Mágico de <color=#{cor}>{item.ataqM}</color>");
        }
        if (item.vel > 0)
        {
             string cor = tratColor(colorvel);
            stringMaster.Add($" Velocidade de <color=#{cor}>{item.vel}</color>");
        }
        if (item.velAtaq > 0)
        {
             string cor = tratColor(colorvelataq);
            stringMaster.Add($" Velocidade de Ataque de <color=#{cor}>{item.velAtaq}</color>");
        }
        if (item.cd > 0)
        {
             string cor = tratColor(colorcd);
            stringMaster.Add($" CowntDown de <color=#{cor}>{item.cd}</color>");
        }

        string tratColor(Color cor){
            string color = ColorUtility.ToHtmlStringRGBA(cor);
            return color.EndsWith("00") ? color.Substring(0, color.Length - 2) : color;
        }


        return string.Join(',', stringMaster);
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
