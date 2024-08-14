using System.Collections;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SkillAreaServer : MonoBehaviour
{
    [SerializeField]
    protected float timeDano;

    [SerializeField]
    private BoxCollider boxC;
    protected List<CharacterServer> playerServer = new();
    protected List<CharacterBotServer> botServer = new();

    protected float dano;
    protected int myIdentityId;

    private bool bot = false;

    void Start()
    {
        boxC.enabled = false;
    }

    void Update() { }

    public async void SetInfoSkillArea(
        int identityId,
        Vector3 pos,
        float TimeSkillAnim,
        float danoP
    )
    {
        dano = danoP;
        myIdentityId = identityId;
        await UniTask.WaitForSeconds(TimeSkillAnim);
        boxC.enabled = true;
        transform.position = pos;
        await UniTask.WaitForSeconds(timeDano);
        AfterCd();
        Destroy(gameObject);
    }

    protected virtual void AfterCd()
    {
        print("Dano area: " + dano);
        if (bot)
        {
            botServer.ForEach(e =>
            {
                e.SetDemage(dano);
            });
            return;
        }
        playerServer.ForEach(e =>
        {
            e.SetDemage(dano);
        });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bot"))
        {
            
            var cEnemy = other.GetComponent<CharacterBotServer>();
            botServer.Add(cEnemy);
            bot = true;
        }
        else
        {
            if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;
            var cEnemy = mEnemy.Identity.Get<CharacterServer>();
            playerServer.Add(cEnemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        print(other.name + " Saiu");

        if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;


        var cEnemy = mEnemy.Identity.Get<CharacterServer>();
        playerServer.RemoveAll(e => e.IdentityId == cEnemy.IdentityId);
    }
}
