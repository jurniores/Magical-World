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
    [SerializeField]
    private GameObject powerRockServer, thunderTimeServer;
    [SerializeField]
    private SkillSingleServer LineThunder, MagicBomb, BasicSkill, InverseField;

    [Server(ConstantsRPC.SKILLBASE_PLAYER)]
    protected override async void SkillBase(DataBuffer buffer)
    {
        //Escrevendo o id da skill

        var identity = buffer.Read<int>();
        PropSkills myProp = propSkill[0];
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        if (identityEnemy == null) return;

        await SkillSendServer(myProp.cd, 0, identity);
        BasicSkill.SetInfoSingle(myProp.dano, identityEnemy, myProp.animDano);
    }

    [Server(ConstantsRPC.SKILL1_PLAYER)]
    protected override async void Skill1(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[1];
        var identity = buffer.Read<int>();

        var identityEnemy = VerifyDistance(myProp.distance, identity);

        if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 1, identity);

        var pr = Instantiate(powerRockServer).GetComponent<PowerRockServer>();
        pr.SetInfoSkillArea(IdentityId, identityEnemy.transform.position, myProp.animDano, myProp.dano);
    }

    [Server(ConstantsRPC.SKILL2_PLAYER)]
    protected override async void Skill2(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[2];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 2, identity);
        MagicBomb.SetInfoSingle(myProp.dano, identityEnemy, myProp.animDano);

    }

    [Server(ConstantsRPC.SKILL3_PLAYER)]
    protected override async void Skill3(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[3];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 3, identity);
        LineThunder.SetInfoSingle(myProp.dano, identityEnemy, myProp.animDano);
    }

    [Server(ConstantsRPC.SKILL4_PLAYER)]
    protected override async void Skill4(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[4];

        await SkillSendServer(myProp.cd, 4, 0);
        InverseField.SetInfoSingle(myProp.dano, Identity, myProp.animDano);
        
    }

    [Server(ConstantsRPC.SKILL5_PLAYER)]
    protected override async void Skill5(DataBuffer buffer)
    {
        PropSkills myProp = propSkill[5];
        var identity = buffer.Read<int>();
        var identityEnemy = VerifyDistance(myProp.distance, identity);

        if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, 5, identity);

        var tt = Instantiate(thunderTimeServer).GetComponent<ThunderTimeServer>();
        tt.SetInfoSkillArea(IdentityId, identityEnemy.transform.position, myProp.animDano, myProp.dano);
    }


    async UniTask SkillSendServer(float cdSkill, byte skill, int idenity)
    {
        if (skilletServer) return;
        skilletServer = true;

        var buffer = NetworkManager.Pool.Rent();
        buffer.Write(skill);
        buffer.Write(idenity);

        Remote.Invoke(ConstantsRPC.COWNTDOWN_SKILL, buffer, Target.GroupMembers);
        buffer.Dispose();

        await UniTask.WaitForSeconds(cdSkill);
        skilletServer = false;

        Remote.Invoke(ConstantsRPC.CONFIRMED_SKILL, skill, Target.GroupMembers);
    }

    NetworkIdentity VerifyDistance(float distance, int identity)
    {
        var IdentityEnemy = NetworkManager.Server.GetIdentity(identity);
        if (IdentityEnemy == null) return null;
        Transform enemy = IdentityEnemy.GetComponent<Transform>();

        //Menos um é porque o servidor está sempre um pouco mais para trás
        float distCompare = Vector3.Distance(enemy.position, transform.position) - 1;

        if (distCompare <= distance)
        {
            return IdentityEnemy;
        }
        return null;
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
