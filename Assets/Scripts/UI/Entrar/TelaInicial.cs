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
using Omni.Threading.Tasks;
public class TelaInicial : ClientBehaviour
{
    [SerializeField]
    private Button btnEntrar;

    [SerializeField]
    private Image imageload1, imageload2;
    [SerializeField]
    private TMP_InputField fieldNumber;
    public override void Start()
    {
        base.Start();
        //PlayerPrefs.DeleteAll();
        btnEntrar.onClick.AddListener(Entrar);
    }

    async void Entrar()
    {
        if (!Debounce.Bounce(1)) return;
        using var req = await Fetch.PostAsync("/login", (res) =>
        {

            string identify = "asdasdasd" + fieldNumber.text;//SystemInfo.deviceUniqueIdentifier;
            if (Application.isMobilePlatform) identify = SystemInfo.deviceUniqueIdentifier;
            UsersModel model = new() { hwid = identify };

            res.WriteAsJson(model);
            res.Send();
        });
        req.SuppressTracking();
        var recieve = req.Read<byte>();

        if (recieve == ConstantsDB.SUCCESS)
        {
            UsersModel user = req.ReadAsJson<UsersModel>();
            InventoryModel inventory = req.ReadAsJson<InventoryModel>();
            GameManager gManager = NetworkService.Get<GameManager>();

            gManager.User = user;
            gManager.Inventory = inventory;

            imageload1.gameObject.SetActive(true);
            await Utils.FakeLoad(LoadSceneLogado);
            await NetworkManager.LoadSceneAsync(1).ToUniTask(Progress.Create<float>(LoadSceneLogado));
        }
    }

    void LoadSceneLogado(float progress)
    {
        float newP = Mathf.Clamp01(progress / 0.9f);
        if (newP < imageload2.fillAmount) return;

        imageload2.fillAmount = newP;


    }

}
