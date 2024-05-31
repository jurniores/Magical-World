using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class panelComprar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmProName, tmProText;
    [SerializeField]
    private GameObject coinSelect, rubySelect, moneySelect, loadSelect, succesSelect, errorSelect;
    [SerializeField]
    private TextMeshProUGUI coinProVal, rubyProVal, moneyProVal;
    [SerializeField]
    private Toggle tgCoin, tgRuby, tgMoney;
    public void InsertInfo(string nameInfo, string text, int valCoin, int valRuby)
    {
        coinSelect.SetActive(true);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        coinProVal.text = valCoin.ToString();
        rubyProVal.text = valRuby.ToString();
        TogglesOn(true,false,false);
    }
     public void InsertInfo(string nameInfo, int qtd, int valMoney)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(false);
        moneySelect.SetActive(true);

        tmProName.text = nameInfo;
        tmProText.text = qtd.ToString() +" Rubys";
        moneyProVal.text = valMoney.ToString()+"R$";
        TogglesOn(false,false,true);
        
    }
    public void InsertInfo(string nameInfo, string text, int valRuby)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        rubyProVal.text = valRuby.ToString();
        TogglesOn(false,true,false);
        
    }

    void TogglesOn(bool coin, bool ruby, bool money){
        tgCoin.isOn = coin;
        tgRuby.isOn = ruby;
        tgMoney.isOn = money;
    }

    public void Comprar()
    {
        if (tgCoin.gameObject.activeSelf && tgCoin.isOn)
        {
            print("Comprei com coin, codigo servidor");
        }
        else if (tgRuby.gameObject.activeSelf && tgRuby.isOn)
        {
            print("Comprei com ruby, codigo servidor");
        }
        else if (tgMoney.gameObject.activeSelf && tgMoney.isOn)
        {
            print("Comprei com money, codigo servidor");
        }
        
        loadSelect.SetActive(true);
        StartCoroutine(SimulaturServer());

    }

    //Código fictício, virá esse load do servidor
    IEnumerator SimulaturServer(){
        yield return new WaitForSeconds(2);
        loadSelect.SetActive(false);
        StartCoroutine(SimulaturServer2());
        
    }
    IEnumerator SimulaturServer2(){
        succesSelect.SetActive(true);
        yield return new WaitForSeconds(1f);
        succesSelect.SetActive(false);
        Camera.main.GetComponent<ComprarOuCancelar>().CancelPanel();
    }


}
