using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroupClient : NetworkBehaviour
{
    [SerializeField]
    private Material skybox;
    [SerializeField]
    private NetworkIdentity clientPlayer, clientEnemy;
    private Dictionary<int, NetworkIdentity> dicPlayerClient = new();
    private CanvasGo canvasGo;
    private int scene = 3;
    Scene sceneGame;
    private bool instante = false;

    void Awake()
    {
        NetworkService.Register(this, nameof(GroupClient));
    }
    void Start()
    {
        canvasGo = NetworkService.Get<CanvasGo>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I)) InvokeIntantePlayer();
    }
    //RPC que espera 60 segundos do servidor para iniciar o game de todos
    [Client(ConstantsRPC.INIT_GAME_GO)]
    async void InitGameLoad()
    {
        await canvasGo.Go(scene);

        sceneGame = SceneManager.GetSceneByBuildIndex(scene);

        SceneManager.MoveGameObjectToScene(gameObject, sceneGame);

        if (Application.isEditor)
        {
            Scene sceneUnload = SceneManager.GetSceneAt(0);
            SceneManager.UnloadSceneAsync(sceneUnload);
            RenderSettings.skybox = skybox;
        }

    }

    void InvokeIntantePlayer()
    {
        print("INICOU");
        Local.Invoke(ConstantsRPC.INSTANT_PLAYER_GAME);
        print("TERMINOU");
    }

    [Client(ConstantsRPC.INSTANT_PLAYER_GAME)]
    void InstantePlayerRPC(DataBuffer buffer)
    {
        buffer.ReadIdentity(out var peerId, out var identityId);
        clientPlayer.InstantiateOnClientInScene(peerId, identityId, sceneGame);
    }

    [Client(ConstantsRPC.INSTANT_ENEMY_GAME)]
    void InstanteEnemyRPC(DataBuffer buffer)
    {

        buffer.ReadIdentity(out var peerId, out var identityId);
        InstanteEnemy(peerId, identityId);
    }

    [Client(ConstantsRPC.INSTANT_PLAYERS_GAME)]
    void InstantePlayersRPC(DataBuffer buffer)
    {
        var dicP = buffer.ReadAsJson<Dictionary<int, int>>();

        foreach (var (peerId, identityId) in dicP)
        {
            InstanteEnemy(peerId, identityId);
        }
    }

    void InstanteEnemy(int peerId, int identityId)
    {
        //Verificando se a identitade já edxiste
        if (dicPlayerClient.ContainsKey(identityId)) return;
        NetworkIdentity identity = clientEnemy.InstantiateOnClientInScene(peerId, identityId, sceneGame);
        identity.transform.position = new Vector3(0,1,0);
        dicPlayerClient.Add(identityId, identity);
    }
}
