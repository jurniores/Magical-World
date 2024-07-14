using UnityEngine;
using TMPro;
using Omni.Core;
using static Omni.Core.HttpLite;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
public class ConfigSala : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI qtdPlayerTotalPro, qtdPlayerLado1Pro, qtdPlayerLado2Pro, nameSalaPro, timeSalaPro, goPro;
    private GameManager gManager;
    [SerializeField]
    private int time = 60;

    void Start()
    {
        gManager = NetworkService.Get<GameManager>("GameManager");
        gManager.OnPlayerRoom += InfoSalaAction;
        gManager.OnStartGame = IniciaGame;
        SetInfoSala(gManager.salaGame);

        // Configs

        int divMetade = gManager.salaGame.qtd / 2;
        string qtdPlayer = divMetade + "X" + divMetade;
        qtdPlayerTotalPro.text = qtdPlayer;
        timeSalaPro.text = "60";

        //Evento Fetch

        Fetch.AddGetHandler("/room/start", IniciaGame);
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
    //Setado na fetch do InstancePlayerSala
    void IniciaGame(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse>();

        ErrorManager.ValidateError(response, 2, async () =>
        {
            goPro.gameObject.SetActive(true);
            await RelogioGame();
        });
    }
    async UniTask RelogioGame()
    {
        await UniTask.WaitForSeconds(1);
        if (time <= 0) return;
        time--;
        timeSalaPro.text = time.ToString();

        await RelogioGame();
    }
}
