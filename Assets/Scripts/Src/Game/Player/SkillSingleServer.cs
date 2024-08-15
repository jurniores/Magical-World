using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SkillSingleServer : MonoBehaviour
{
    protected float dano;
    protected float shield;

    private bool move = false;
    protected Character charEnemy;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    public async void SetInfoSkill(float danoP, NetworkIdentity identity, float timeDano, bool buff)
    {
        dano = danoP;

        charEnemy = identity.Get<Character>();
        await UniTask.WaitForSeconds(timeDano);

        if (!buff)
        {
            SkillSingleDemage();
        }
        else
        {
            SkillSingleBuff();
        }
    }

    protected virtual void SkillSingleDemage()
    {
        //Se o personagem se mover, cancela o dano da  skill
        charEnemy.SetDemage(dano);
    }
    protected virtual void SkillSingleBuff()
    {
        //Se o personagem se mover, cancela o dano da  skill
        charEnemy.SetShield(dano);
    }

}
