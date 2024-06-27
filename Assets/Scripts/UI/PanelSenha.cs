using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class PanelSenha : NetworkService
{
    [SerializeField]
    private Button btnPass;
    [SerializeField]
    private TMP_InputField fieldPass;

    private Action<string> fnExtern;

    void Start()
    {
        btnPass.onClick.AddListener(SetPassBtn);
        gameObject.SetActive(false);
    }


    void SetPassBtn(){
        fnExtern?.Invoke(fieldPass.text);
        gameObject.SetActive(false);
    }

    public void SetInfoPass(Action<string> fn){
        gameObject.SetActive(true);
        fnExtern = fn;
    }
    
}
