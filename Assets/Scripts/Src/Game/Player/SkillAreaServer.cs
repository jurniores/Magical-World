using System.Collections;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using UnityEngine;

public class SkillAreaServer : MonoBehaviour
{
    [SerializeField]
    protected float timeDano;
    protected List<CharacterServer> playerServer = new();
    protected float dano;
    protected int myIdentityId;

    public async void SetInfoSkillArea(int identityId, Vector3 pos, float TimeSkillAnim, float danoP)
    {
        dano = danoP;
        gameObject.SetActive(false);
        myIdentityId = identityId;
        await UniTask.WaitForSeconds(TimeSkillAnim);
        transform.position = pos;
        gameObject.SetActive(true);
        await UniTask.WaitForSeconds(timeDano);
        AfterCd();
        Destroy(gameObject);
    }

    protected virtual void AfterCd()
    {
        print("Dano area: " + dano);
        playerServer.ForEach(e =>
        {
            e.SetDemage(dano);
        });
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.name + " Entrou");
        if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;
        var cEnemy = mEnemy.Identity.Get<CharacterServer>();
        playerServer.Add(cEnemy);

    }

    void OnTriggerExit(Collider other)
    {
        print(other.name + " Saiu");
        if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;
        var cEnemy = mEnemy.Identity.Get<CharacterServer>();
        playerServer.RemoveAll(e => e.IdentityId == cEnemy.IdentityId);
    }
}
