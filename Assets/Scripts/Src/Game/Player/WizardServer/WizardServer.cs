using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
//Lembrar que o cowntdown do servidor é o tempo de cowntdown do cliente + o tempo de animação
public class WizardServer : CharacterServer
{
    private bool skilletServer = false;

    [Server(ConstantsRPC.SKILLBASE_PLAYER)]
    protected override void SkillBase(DataBuffer buffer)
    {
        var identity = buffer.Read<int>();
        PropSkills myProp = propSkill[0];
        bool verifyDistance = VerifyDistance(myProp.distance, identity);

        if (!verifyDistance) return;

        SkillSendServer(myProp.cd, 0);
    }

    [Server(ConstantsRPC.SKILL1_PLAYER)]
    protected override void Skill1(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[1];
        var identity = buffer.Read<int>();

        bool verifyDistance = VerifyDistance(myProp.distance, identity);

        if (!verifyDistance) return;
        SkillSendServer(myProp.cd, 1);
    }

    [Server(ConstantsRPC.SKILL2_PLAYER)]
    protected override void Skill2(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[2];
        var identity = buffer.Read<int>();
        bool verifyDistance = VerifyDistance(myProp.distance, identity);

        if (!verifyDistance) return;
        SkillSendServer(myProp.cd, 2);
    }

    [Server(ConstantsRPC.SKILL3_PLAYER)]
    protected override void Skill3(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[3];
        var identity = buffer.Read<int>();
        bool verifyDistance = VerifyDistance(myProp.distance, identity);

        if (!verifyDistance) return;
        SkillSendServer(myProp.cd, 3);
    }

    [Server(ConstantsRPC.SKILL4_PLAYER)]
    protected override void Skill4(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[4];

        SkillSendServer(myProp.cd, 4);
    }

    [Server(ConstantsRPC.SKILL5_PLAYER)]
    protected override void Skill5(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[5];
        var identity = buffer.Read<int>();
        bool verifyDistance = VerifyDistance(myProp.distance, identity);

        if (!verifyDistance) return;
        SkillSendServer(myProp.cd, 5);
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

    bool VerifyDistance(float distance, int identity)
    {
        var IdentityEnemy = NetworkManager.Server.GetIdentity(identity);

        Transform enemy = IdentityEnemy.GetComponent<Transform>();

        //Menos um é porque o servidor está sempre um pouco mais para trás
        float distCompare = Vector3.Distance(enemy.position, transform.position) - 1;

        if (distCompare <= distance)
        {
            return true;
        }
        return false;
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
