using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Omni.Core;
using static Omni.Core.HttpLite;
public class ConfigSala : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI qtdPlayerTotalPro, qtdPlayerLado1Pro, qtdPlayerLado2Pro, nameSalaPro, timeSalaPro;

    private GameManager gManager;
    void Start()
    {
        gManager = NetworkService.Get<GameManager>("GameManager");
        gManager.OnPlayerRoom += InfoSalaAction;
        SetInfoSala(gManager.salaGame);


        //
        int divMetade = gManager.salaGame.qtd / 2;
        string qtdPlayer = divMetade + "X" + divMetade;
        qtdPlayerTotalPro.text = qtdPlayer;
        timeSalaPro.text = "60";

    }

    // Update is called once per frame
    void Update()
    {

    }
    void InfoSalaAction(SalaInGame sg)
    {
        qtdPlayerLado1Pro.text = sg.lado1.Count.ToString();
        qtdPlayerLado2Pro.text = sg.lado2.Count.ToString();
    }
    void SetInfoSala(Sala sala)
    {
        InfoSalaAction(gManager.salaInGame);
        nameSalaPro.text = sala.nameSala;

    }
}
