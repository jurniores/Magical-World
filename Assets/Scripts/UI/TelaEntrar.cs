using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using static Omni.Core.HttpLite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using TMPro;

public class TelaInicial : ClientEventBehaviour
{
    [SerializeField]
    private Button btnEntrar;

    [SerializeField]
    private Image imageload1, imageload2;
    [SerializeField]
    private TMP_InputField fieldNumber;
    protected override void Start()
    {
        base.Start();
        //PlayerPrefs.DeleteAll();
        btnEntrar.onClick.AddListener(Entrar);
    }

    async void Entrar()
    {

        if(!Debounce.instance.Bounce(1)) return;
        var req = await Fetch.PostAsync("/login", (res) =>
        {   
            string identify = "asdasdasd"+fieldNumber.text;//SystemInfo.deviceUniqueIdentifier;
            if(Application.isMobilePlatform) identify = SystemInfo.deviceUniqueIdentifier;
            UsersModel model = new() { hwid = identify };

            res.ToJson(model);
            res.Send();
        });

        var recieve = req.Read<byte>();
        
        if (recieve == ConstantsDB.SUCCESS)
        {
            UsersModel user = req.FromJson<UsersModel>();
            InventoryModel inventory = req.FromJson<InventoryModel>();
            GameManager gManager = NetworkService.Get<GameManager>("GameManager");

            gManager.User = user;
            gManager.Inventory = inventory;
            StartCoroutine(LoadSceneLogado());
        }


    }

   

    IEnumerator LoadSceneLogado()
    {
        imageload1.gameObject.SetActive(true);
        var asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            imageload2.fillAmount = asyncLoad.progress;
            yield return null;
        }

    }


}
