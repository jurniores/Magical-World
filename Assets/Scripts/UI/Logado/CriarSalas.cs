using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Omni.Core.HttpLite;

public class CriarSalas : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameSalaField,
        passwordField;

    [SerializeField]
    private TextMeshProUGUI errorNameField,
        errorPassword;

    [SerializeField]
    private Button btnSalas;
    private int tgPass;
    private int tgSalasQtd = 10;
    private GameManager gManager;

    public void Toggles(int tgIntPass)
    {
        tgPass = tgIntPass;
        if (tgPass == 0)
        {
            passwordField.transform.parent.gameObject.SetActive(false);
            return;
        }
        passwordField.transform.parent.gameObject.SetActive(true);
    }

    void Awake()
    {
        gManager = NetworkService.Get<GameManager>();

    }

    void Start()
    {
        btnSalas.onClick.AddListener(SetSalas);
    }

    void OnEnable()
    {
        ActiveError();
    }

    public void TogglesQtd(int qtdInt)
    {
        tgSalasQtd = qtdInt;
    }

    void ActiveError()
    {
        errorNameField.gameObject.SetActive(false);
        errorPassword.gameObject.SetActive(false);
    }

    void SetSalas()
    {
        if (!Debounce.Bounce(1))
            return;

        //Validações de erros
        ActiveError();
        if (nameSalaField.text.Length < 4)
        {
            errorNameField.gameObject.SetActive(true);
            return;
        }
        if (nameSalaField.text.Length > 0 && nameSalaField.text.Length > 7)
        {
            errorNameField.gameObject.SetActive(true);
            return;
        }

        if (tgPass == 1 && (passwordField.text.Length < 4 || passwordField.text.Length > 10))
        {
            errorPassword.gameObject.SetActive(true);
            return;
        }
        
        Fetch.Post(
            "/room",
            req =>
            {
                req.WriteAsJson(
                    new Sala()
                    {
                        nameSala = nameSalaField.text,
                        qtd = tgSalasQtd,
                        password = passwordField.text,
                        IsPass = tgPass == 1
                    }
                );
            },
            res =>
            {
                ValidateRoom(res);
            }
        );
    }

    void ValidateRoom(DataBuffer res)
    {
        var response = res.ReadAsJson<NetworkResponse<Sala>>();

        ErrorManager.ValidateError(response, 2, () =>
        {
            gManager.AddSala(response.Data);
            gManager.salaGame = response.Data;
            
            gManager.salaInGame = res.ReadAsJson<SalaInGame>();
            NetworkManager.LoadScene(2);
        });
    }
}
