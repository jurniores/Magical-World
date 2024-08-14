using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill3 : Skills
{
    [SerializeField]
    private ThunderBird thunderBird;

    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL3_PLAYER;
    }

     protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(propSkills.animTime);
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        await thunderBird.SetPosThunder(posEnemy);
        base.SkillAfeterCd();
    }

    protected override void CancelAnimations()
    {
        character.circle.Stop();
    }
}
