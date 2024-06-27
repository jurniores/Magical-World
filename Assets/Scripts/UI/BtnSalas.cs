using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Omni.Core.HttpLite;

public class BtnSalas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nomeDaSalaPro,
        qtdPlayerPro;

    [SerializeField]
    private Image online;

    [SerializeField]
    private GameObject cadeado;
    private Button myBtn;
    private Sala minhaSala;
    private PanelSenha passPanel;
    private GameManager gManager;

    void Start()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(EntraSala);
        passPanel = NetworkService.Get<PanelSenha>();
        gManager = NetworkService.Get<GameManager>();

    }

    public void SetInfoSala(Sala sala)
    {
        minhaSala = sala;
        if (sala.IsPass)
            cadeado.SetActive(true);

        nomeDaSalaPro.text = sala.nameSala;
        qtdPlayerPro.text = sala.qtdPlayer + "/" + sala.qtd;
        float divide = (float)sala.qtdPlayer / sala.qtd * 100;
        if (divide >= 60)
        {
            online.color = new Color32(255, 93, 0, 255);
        }
        else
        {
            online.color = Color.green;
        }
    }
    public void UpdateSala(Sala sala)
    {
        SetInfoSala(sala);
    }
    void FetchRoom(string password)
    {
        Fetch.Post(
                    "/room/join",
                    req =>
                    {
                        req.FastWrite(minhaSala.nameSala);
                        req.FastWrite(password);
                        req.Send();
                    },
                    res =>
                    {
                        var response = res.FromJson<NetworkResponse<Sala>>();
                        ErrorManager.instance.ValidateError(response, 3, () =>
                        {
                            gManager.salaGame = response.Data;
                            gManager.salaInGame = res.FromJson<SalaInGame>();
                            SceneManager.LoadScene(1);
                        });
                    }
                );


    }
  
    void EntraSala()
    {
        if (!Debounce.instance.Bounce(2)) return;
        if (minhaSala.IsPass)
        {
            passPanel.SetInfoPass(FetchRoom);
            return;
        }
        FetchRoom("");

    }
}
