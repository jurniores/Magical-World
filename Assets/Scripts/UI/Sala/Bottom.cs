using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;

public class Bottom : MonoBehaviour
{
    [SerializeField]
    private Button btnPlay;
    private GameManager gManager;
    void Start()
    {
        gManager = NetworkService.Get<GameManager>();
        gManager.OnNewMaster = ()=> SouMaster(gManager.User, gManager.salaGame);
        SouMaster(gManager.User, gManager.salaGame);
    }


    void Update()
    {

    }

    void SouMaster(UsersModel me, Sala minhaSala)
    {
        if (me.peerId == minhaSala.master)
        {
            btnPlay.gameObject.SetActive(true);
        }
    }
}
