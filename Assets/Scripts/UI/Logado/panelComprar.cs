using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static Omni.Core.HttpLite;

public class panelComprar : ClientBehaviour
{
    [SerializeField]
    private Image img;

    [SerializeField]
    private TextMeshProUGUI tmProName,
        tmProText;

    [SerializeField]
    private GameObject coinSelect,
        rubySelect,
        moneySelect,
        loadSelect,
        errorMessageSelect;

    [SerializeField]
    private TextMeshProUGUI coinProVal,
        rubyProVal,
        moneyProVal;
    private int idItem;
    private byte tipoDoItem;
    private Action Fechar;

    string erroOriginal;
    private TextMeshProUGUI erroText;
    private GameManager gManager;
    private bool bounce = true;

    public override void Start()
    {
        base.Start();
        Fechar = () => Camera.main.GetComponent<ComprarOuCancelar>().CancelPanel();
        erroText = errorMessageSelect.GetComponent<TextMeshProUGUI>();
        erroOriginal = erroText.text;
        gManager = NetworkService.Get<GameManager>("GameManager");
    }

    void OnEnable()
    {
        errorMessageSelect.SetActive(false);
    }

    public void InsertInfo(
        int id,
        string nameInfo,
        string text,
        int valCoin,
        int valRuby,
        Sprite sprite
    )
    {
        coinSelect.SetActive(true);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        coinProVal.text = valCoin.ToString();
        rubyProVal.text = valRuby.ToString();
        img.sprite = sprite;
        idItem = id;
        tipoDoItem = ConstantsDB.IS_ITEM;
    }

    public void InsertInfo(int id, string nameInfo, int qtd, int valMoney, Sprite sprite)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(false);
        moneySelect.SetActive(true);

        tmProName.text = nameInfo;
        tmProText.text = qtd.ToString() + " Rubys";
        moneyProVal.text = valMoney.ToString() + "R$";
        img.sprite = sprite;
        idItem = id;
        tipoDoItem = ConstantsDB.IS_RUBY;
    }

    public void InsertInfo(int id, string nameInfo, string text, int valRuby, Sprite sprite)
    {
        coinSelect.SetActive(false);
        rubySelect.SetActive(true);
        moneySelect.SetActive(false);

        tmProName.text = nameInfo;
        tmProText.text = text;
        rubyProVal.text = valRuby.ToString();
        img.sprite = sprite;
        idItem = id;
        tipoDoItem = ConstantsDB.IS_RUNE;
    }

    public void Comprar(int tipoMoeda)
    {
        if (!Debounce.Bounce(1))
            return;

        Fetch.Post(
            "/cloja",
            (req) =>
            {
                req.Write(idItem);
                req.Write(tipoDoItem);
                req.Write(tipoMoeda);
                req.Send();
                loadSelect.SetActive(true);
            },
            (res) =>
            {
                MsgDeErro(res);
                loadSelect.SetActive(false);
            }
        );
    }

    void MsgDeErro(DataBuffer response)
    {
        var res = response.FromJson<NetworkResponse>();
        if (res.status == ConstantsDB.SUCCESS)
        {
             var user = response.FromJson<UsersModel>();
             var inv = response.FromJson<InventoryModel>();
            gManager.User = user;
            gManager.Inventory =  inv;

            Fechar();
        }
        else if (res.status == ConstantsDB.ERROR)
        {
            erroText.text = res.message;
            errorMessageSelect.SetActive(true);
        }
    }
}
