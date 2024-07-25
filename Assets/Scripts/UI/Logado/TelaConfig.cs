using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Omni.Core;
using static Omni.Core.HttpLite;
public class TelaConfig : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField namePlayerField, emailField, newPassField, passRepeatField, passwordField;
    private TextMeshProUGUI namePlayerErrorTxt, emailErrorTxt, newPassErrorTxt, passRepeatErrorTxt, passwordErrorTxt;
    [SerializeField]
    private Button btnConfig;
    private GameManager gManager;
    private int tg;

    void Start()
    {
        btnConfig.onClick.AddListener(SetConfig);
        namePlayerErrorTxt = namePlayerField.GetComponentsInChildren<TextMeshProUGUI>()[2];
        emailErrorTxt = emailField.GetComponentsInChildren<TextMeshProUGUI>()[2];
        newPassErrorTxt = newPassField.GetComponentsInChildren<TextMeshProUGUI>()[2];
        passRepeatErrorTxt = passRepeatField.GetComponentsInChildren<TextMeshProUGUI>()[2];
        passwordErrorTxt = passwordField.GetComponentsInChildren<TextMeshProUGUI>()[2];
        gManager = NetworkService.Get<GameManager>("GameManager");
        ActivePassWord();
        CloseFieldsError();
    }

    void OnEnable()
    {

        ActivePassWord();
        CloseFieldsError();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisconnectClient();
        }
    }
    void DisconnectClient()
    {
        NetworkManager.Disconnect();
    }
    void ActivePassWord()
    {
        if (gManager != null && (gManager.User.password == null || gManager.User.password.Length == 0))
        {
            passwordField.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            passwordField.transform.parent.gameObject.SetActive(true);
        }
    }

    void CloseFieldsError()
    {
        if (!namePlayerErrorTxt) return;

        namePlayerErrorTxt.gameObject.SetActive(false);
        emailErrorTxt.gameObject.SetActive(false);
        newPassErrorTxt.gameObject.SetActive(false);
        passRepeatErrorTxt.gameObject.SetActive(false);
        passwordErrorTxt.gameObject.SetActive(false);
    }

    void LimpaFields()
    {
        namePlayerField.text = "";
        emailField.text = "";

        passRepeatField.text = "";
        newPassField.text = "";
        passwordField.text = "";
    }

    public void Toggles(int tgInt)
    {
        tg = tgInt;
        if (tg == 1)
        {

            passRepeatField.transform.parent.gameObject.SetActive(true);
            newPassField.transform.parent.gameObject.SetActive(true);

            namePlayerField.transform.parent.gameObject.SetActive(false);
            emailField.transform.parent.gameObject.SetActive(false);
            return;
        }
        namePlayerField.transform.parent.gameObject.SetActive(true);
        emailField.transform.parent.gameObject.SetActive(true);

        passRepeatField.transform.parent.gameObject.SetActive(false);
        newPassField.transform.parent.gameObject.SetActive(false);

        LimpaFields();
    }

    void SetConfig()
    {
        if (!Debounce.Bounce(1)) return;
        CloseFieldsError();
        if (tg == 0) Info();
        if (tg == 1) Pass();
    }

    async void Info()
    {
        int error = 0;

        if (namePlayerField.text.Length == 0 && emailField.text.Length == 0) error = 1;

        if (namePlayerField.text.Length > 0 && namePlayerField.text.Length < 3)
        {
            namePlayerErrorTxt.gameObject.SetActive(true);
            error = 1;
        }
        if (emailField.text.Length > 0 && !Utils.IsEmail(emailField.text))
        {
            emailErrorTxt.gameObject.SetActive(true);
            error = 1;
        }
        if (error != 0) return;

        using var req = await Fetch.PostAsync("/info", res =>
        {
            UsersModel modelUser = new()
            {
                name = namePlayerField.text,
                email = emailField.text,
                password = passwordField.text,
            };

            res.ToJson(modelUser);

        });

        Response(req);
    }
    async void Pass()
    {
        int error = 0;

        if (newPassField.text.Length == 0 && passRepeatField.text.Length == 0) error = 1;


        if ((passwordField.text.Length > 0 && passwordField.text.Length < 6) || (gManager.User.password != null && passwordField.text.Length < 6))
        {
            passwordErrorTxt.gameObject.SetActive(true);
            error = 1;
        }
        if (newPassField.text.Length > 0 && newPassField.text.Length < 6)
        {
            newPassErrorTxt.gameObject.SetActive(true);
            error = 1;
        }

        if (newPassField.text.Length > 0 && newPassField.text != passRepeatField.text)
        {
            passRepeatErrorTxt.gameObject.SetActive(true);
            error = 1;
        }

        if (error != 0) return;
        using var req = await Fetch.PostAsync("/pass", res =>
        {
            UsersModel modelUser = new()
            {
                newPass = newPassField.text,
                passRepeat = passRepeatField.text,
                password = passwordField.text,
            };

            res.ToJson(modelUser);

        });
        Response(req);
    }

    void Response(DataBuffer res)
    {
        var response = res.FromJson<NetworkResponse<UsersModel>>();
        ErrorManager.ValidateError(response, 3, () =>
        {
            gManager.User = response.Data;
            ActivePassWord();
            LimpaFields();
        });
    
    }
}
