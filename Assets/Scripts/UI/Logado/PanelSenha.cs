using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Humanizer;
public class PanelSenha : ServiceBehaviour
{
    [SerializeField]
    private Button btnPass;
    [SerializeField]
    private TMP_InputField fieldPass;

    private Action<string> fnExtern;
    protected override void OnStart()
    {
        btnPass.onClick.AddListener(SetPassBtn);
    }


    void SetPassBtn()
    {
        fnExtern?.Invoke(fieldPass.text);
        gameObject.SetActive(false);
    }

    public void SetInfoPass(Action<string> fn)
    {
        gameObject.SetActive(true);
        fnExtern = fn;
    }

}
