using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroupClient : NetworkBehaviour
{
    [SerializeField]
    private Material skybox;
    private CanvasGo canvasGo;
    private int scene = 3;

    void Awake()
    {
        NetworkService.Register(this, nameof(GroupClient));
    }
    void Start()
    {
        canvasGo = NetworkService.Get<CanvasGo>();
    }


    //RPC que espera 60 segundos do servidor para iniciar o game de todos
    [Client(ConstantsRPC.INIT_GAME_GO)]
    async void InitGameLoad(){
        await canvasGo.Go(scene);
        Scene SceneGame = SceneManager.GetSceneByBuildIndex(scene);

        SceneManager.MoveGameObjectToScene(gameObject, SceneGame);

        if (Application.isEditor)
        {   
            Scene sceneUnload = SceneManager.GetSceneAt(0);
            SceneManager.UnloadSceneAsync(sceneUnload);
            RenderSettings.skybox = skybox;
        }
    }
}
