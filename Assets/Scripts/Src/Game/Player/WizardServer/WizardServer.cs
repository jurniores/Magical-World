using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public class WizardServer : CharacterServer
{
    [SerializeField]
    private float
        cdSkillBase,
        cdSkill1,
        cdSkill2,
        cdSkill3,
        cdSkill4,
        cdSkill5;


    private bool skilletServer = false;

    [Server(ConstantsRPC.SKILLBASE_PLAYER)]
    protected override void SkillBase(DataBuffer buffer)
    {
        SkillSendServer(cdSkillBase, 0);
    }

    [Server(ConstantsRPC.SKILL1_PLAYER)]
    protected override void Skill1(DataBuffer buffer)
    {
        SkillSendServer(cdSkill1, 1);
    }

    [Server(ConstantsRPC.SKILL2_PLAYER)]
    protected override void Skill2(DataBuffer buffer)
    {
        SkillSendServer(cdSkill2, 2);
    }

    [Server(ConstantsRPC.SKILL3_PLAYER)]
    protected override void Skill3(DataBuffer buffer)
    {
        SkillSendServer(cdSkill3, 3);
    }

    [Server(ConstantsRPC.SKILL4_PLAYER)]
    protected override  void Skill4(DataBuffer buffer)
    {
        SkillSendServer(cdSkill4, 4);
    }

    [Server(ConstantsRPC.SKILL5_PLAYER)]
    protected override void Skill5(DataBuffer buffer)
    {
        SkillSendServer(cdSkill5, 5);
    }


    async void SkillSendServer(float cdSkill, byte skill)
    {
        if (skilletServer) return;
        skilletServer = true;

        Remote.Invoke(ConstantsRPC.COWNTDOWN_SKILL, skill, Target.GroupMembers);

        await UniTask.WaitForSeconds(cdSkill);
        skilletServer = false;
        
        Remote.Invoke(ConstantsRPC.CONFIRMED_SKILL, skill, Target.GroupMembers);
    }
    protected override void Demage(float dano)
    {
        charAttribues.hp -= dano;
        if (charAttribues.hp <= 0)
        {
            charAttribues.hp = 0;
        }
    }
}
