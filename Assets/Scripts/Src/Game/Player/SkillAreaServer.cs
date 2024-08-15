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
    protected List<Character> charServer = new();

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
        charServer.ForEach(e =>
        {
            e.SetDemage(dano);
        });
    }

    void OnTriggerEnter(Collider other)
    {
        Character cEnemy;
        if (other.CompareTag("bot"))
        {
            cEnemy = other.GetComponent<Character>();
            charServer.Add(cEnemy);
        }
        else
        {
            if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;
            cEnemy = mEnemy.Identity.Get<Character>();
            charServer.Add(cEnemy);
        }


    }

    void OnTriggerExit(Collider other)
    {
        print(other.name + " Saiu");

         Character cEnemy;
        if (other.CompareTag("bot"))
        {
            cEnemy = other.GetComponent<Character>();
            charServer.RemoveAll(e => e.IdentityId == cEnemy.IdentityId);
        }
        else
        {
            if (!other.TryGetComponent<MoveServer>(out var mEnemy) || myIdentityId == mEnemy.IdentityId) return;
            cEnemy = mEnemy.Identity.Get<Character>();
            charServer.RemoveAll(e => e.IdentityId == cEnemy.IdentityId);
        }
        
    }
}
