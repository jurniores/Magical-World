using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

//Lembrar que o cowntdown do servidor é o tempo de cowntdown do cliente + o tempo de animação
public class WizardServer : CharacterServer
{
    [SerializeField]
    private GameObject powerRockServer, thunderTimeServer;
    [SerializeField]
    private SkillSingleServer LineThunder, MagicBomb, BasicSkill, InverseField;

    [Server(ConstantsRPC.SKILLBASE_PLAYER)]
    protected override async void SkillBase(DataBuffer buffer)
    {
        if (!skillLiberate[0]) return;
        skillLiberate[0] = false;
        //Escrevendo o id da skill
        var identity = buffer.Read<int>();
        PropSkills myProp = propSkill[0];
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        // if (identityEnemy == null) return;

        await SkillSendServer(myProp.cd, 0, identity);
        if (!moveAction[0])
        {
            BasicSkill.SetInfoSkill(myProp.dano, identityEnemy, myProp.animDano, false);
        }

        //Serve para esperar o tempo de espera da skill, eviitar speedhack no cliente
        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[0] = true;
    }

    [Server(ConstantsRPC.SKILL1_PLAYER)]
    protected override void Skill1(DataBuffer buffer)
    {
        InvokeSkill(buffer, 1, Skill);

        void Skill(NetworkIdentity identityEnemy, PropSkills myProp)
        {
            var pr = Instantiate(powerRockServer).GetComponent<PowerRockServer>();
            pr.SetInfoSkillArea(IdentityId, identityEnemy.transform.position, myProp.animDano, myProp.dano);
        };
    }

    [Server(ConstantsRPC.SKILL2_PLAYER)]
    protected override async void Skill2(DataBuffer buffer)
    {
        if (!skillLiberate[2]) return;
        skillLiberate[2] = false;
        PropSkills myProp = propSkill[2];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        // if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 2, identity);

        if (!moveAction[2])
        {
            MagicBomb.SetInfoSkill(myProp.dano, identityEnemy, myProp.animDano, false);
        }

        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[2] = true;

    }

    [Server(ConstantsRPC.SKILL3_PLAYER)]
    protected override async void Skill3(DataBuffer buffer)
    {
        if (!skillLiberate[3]) return;
        skillLiberate[3] = false;
        PropSkills myProp = propSkill[3];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        // if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 3, identity);
        if (!moveAction[3])
        {
            LineThunder.SetInfoSkill(myProp.dano, identityEnemy, myProp.animDano, false);
        }

        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[3] = true;
    }

    [Server(ConstantsRPC.SKILL4_PLAYER)]
    protected override async void Skill4(DataBuffer buffer)
    {
        if (!skillLiberate[4]) return;
        skillLiberate[4] = false;
        PropSkills myProp = propSkill[4];

        await SkillSendServer(myProp.cd, 4, 0);

        if (!moveAction[4])
        {
            InverseField.SetInfoSkill(myProp.dano, Identity, myProp.animDano, true);
        }

        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[4] = true;

    }

    [Server(ConstantsRPC.SKILL5_PLAYER)]
    protected override async void Skill5(DataBuffer buffer)
    {
        if (!skillLiberate[5]) return;
        skillLiberate[5] = false;
        PropSkills myProp = propSkill[5];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        // if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 5, identity);

        if (!moveAction[5])
        {
            var tt = Instantiate(thunderTimeServer).GetComponent<ThunderTimeServer>();
            tt.SetInfoSkillArea(IdentityId, identityEnemy.transform.position, myProp.animDano, myProp.dano);
        }


        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[5] = true;
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
