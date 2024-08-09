using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public class SkillSingleServer : MonoBehaviour
{
    [SerializeField]
    private float timeDano;
    protected float dano;
    protected float shield;
    protected NetworkIdentity identity;

    public async void SetInfoSingle(float danoP, NetworkIdentity idEnemy, float timeAnim)
    {
        dano = danoP;
        identity = idEnemy;
        await UniTask.WaitForSeconds(timeAnim + timeDano);
        SkillSingle();
    }

    public async void SetInfoShield(float shieldP, NetworkIdentity myIdentity, float timeAnim)
    {
        shield = shieldP;
        identity = myIdentity;
        await UniTask.WaitForSeconds(timeAnim + timeDano);
        SkillSingle();
    }


    protected virtual void SkillSingle()
    {
        print("Dano single: " + dano);
        var charEnemy = identity.Get<CharacterServer>();
        charEnemy.SetDemage(dano);
    }

}
