using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill3 : Skills
{
    [SerializeField]
    private ParticleSystem circle;
    [SerializeField]
    private ThunderBird thunderBird;

    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL3_PLAYER;
        circle.Stop();
    }

     protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(animTime);
        Transform posEnemy = identityCliked.GetComponent<Transform>();
        thunderBird.SetPosThunder(posEnemy);
    }
 
}
