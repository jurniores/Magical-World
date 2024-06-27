using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : NetworkService
{
    [SerializeField]
    private List<ErrorImage> errorImage;

    public static ErrorManager instance;



    void Start()
    {
        instance = this;


    }

    public void ValidateError(NetworkResponse response, int time, Action fnSuccess = default, Action fnError = default)
    {
        if (response.status == ConstantsDB.SUCCESS)
        {
            ForEach(true);
            fnSuccess?.Invoke();
        }
        else
        {
            ForEach(false);
            fnError?.Invoke();
        }

        void ForEach(bool valida)
        {
            foreach (var e in errorImage)
            {
                if (!e.gameObject.activeSelf)
                {
                    e.SetMsg(valida, response.message, time);
                    break;
                }
            }
        }

    }
}
