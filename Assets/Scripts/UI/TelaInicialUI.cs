using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Omni.Core;
public class TelaInicialUI : MonoBehaviour
{
    [SerializeField]
    private GameObject telaPrincipal, TelainvLoja;
    [SerializeField]
    private TextMeshProUGUI coinProInicial, rubyProInicial;
    GameManager gManager;
    void Start()
    {
        gManager = NetworkService.Get<GameManager>("GameManager");
        SetUser();
        gManager.OnUser += SetUser;
    }

    void SetUser()
    {
        coinProInicial.text = gManager.User.coin.ToString();
        rubyProInicial.text = gManager.User.ruby.ToString();
    }
    public void AbreTelaInvLoja()
    {
        if (telaPrincipal.activeSelf)
        {
            telaPrincipal.SetActive(false);
            TelainvLoja.SetActive(true);
            return;
        }

        telaPrincipal.SetActive(true);
        TelainvLoja.SetActive(false);
    }

}
