using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class panelComprar : MonoBehaviour
{
    [SerializeField]
    private Image img;
    [SerializeField]
    private TextMeshProUGUI tmProName, tmProText;
    [SerializeField]
    private GameObject coinSelect, rubySelect, moneySelect, loadSelect, errorMessageSelect;
    [SerializeField]
    private TextMeshProUGUI coinProVal, rubyProVal, moneyProVal;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        errorMessageSelect.SetActive(false);
    }
    public void InsertInfo(string nameInfo, string text, int valCoin, int valRuby, Sprite sprite)
    {
        coinSelect.SetActive(true);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        coinProVal.text = valCoin.ToString();
        rubyProVal.text = valRuby.ToString();
        img.sprite = sprite;
    }
     public void InsertInfo(string nameInfo, int qtd, int valMoney, Sprite sprite)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(false);
        moneySelect.SetActive(true);

        tmProName.text = nameInfo;
        tmProText.text = qtd.ToString() +" Rubys";
        moneyProVal.text = valMoney.ToString()+"R$"; 
        img.sprite = sprite;
    }
    public void InsertInfo(string nameInfo, string text, int valRuby, Sprite sprite)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        rubyProVal.text = valRuby.ToString();
        img.sprite = sprite;
    }



   
    public void Comprar()
    {
        
        loadSelect.SetActive(true);
        StartCoroutine(SimulaturServer());

    }

    //Código fictício, virá esse load do servidor
    IEnumerator SimulaturServer(){
        yield return new WaitForSeconds(2);
        loadSelect.SetActive(false);
        errorMessageSelect.SetActive(true);
        //StartCoroutine(SimulaturServer2());
        
    }
    IEnumerator SimulaturServer2(){
        yield return new WaitForSeconds(1f);
        Camera.main.GetComponent<ComprarOuCancelar>().CancelPanel();
        
    }


}
