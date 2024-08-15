using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterServer : Character
{
    public MoveServer moveServer;
    public CharacterAttributes charAttribues;
    public List<PropSkills> propSkill;
    private int SkLNow = 0;
    protected List<bool> skillLiberate = new() { true, true, true, true, true, true };
    protected List<bool> moveAction = new() { true, true, true, true, true, true };

    protected abstract void Skill5(DataBuffer buffer);
    protected abstract void Skill4(DataBuffer buffer);
    protected abstract void Skill3(DataBuffer buffer);
    protected abstract void Skill2(DataBuffer buffer);
    protected abstract void Skill1(DataBuffer buffer);
    protected abstract void SkillBase(DataBuffer buffer);
    protected abstract void Demage(float dano);
    protected bool move = false, moveStop = false, skilletServer = false, death = false;

    void Update()
    {

        /*Lógica de parar skill quando o boneco andar devido a lógica
        de movimentação ser seguir uma direção, e o ataque ser de acordo com a distancia, pois
        o servidor para depois do cliente visto que a comparação é feita com -1
        */
        if (!moveServer.IsMoving && skilletServer && !moveStop)
        {
            moveStop = true;
        }
        else if (moveServer.IsMoving && !skilletServer)
        {
            moveStop = false;
        }

        if (moveServer.IsMoving && !move && skilletServer && moveStop)
        {
            move = true;
            moveStop = false;
            skillLiberate[SkLNow] = true;
            moveAction[SkLNow] = true;
            skilletServer = false;
            print("Cancelei pois movimentou no servidor");
            Remote.Invoke(ConstantsRPC.CANCELED_SKILL, Target.GroupMembers);
        }
    }
    [Server(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected async void GetPropSkills()
    {
        //PODE DAR PROBLEMA CASO HOUVER MUITO LAG. POR CAUSA DO TEMPO DE ESPERA
        await UniTask.WaitForSeconds(1);
        using var buffer = NetworkManager.Pool.Rent();
        buffer.WriteAsBinary(propSkill);
        buffer.WriteAsBinary(charAttribues);
        buffer.CompressRaw();
        Remote.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS, buffer, Target.GroupMembers);

        // using var buffer2 = NetworkManager.Pool.Rent();
        // buffer2.WriteAsBinary(charAttribues);
        // buffer2.CompressRaw();
        // Remote.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS, buffer2, Target.GroupMembersExceptSelf);
    }

    public override void SetDemage(float dano)
    {
        if (charAttribues.hp <= 0) return;

        charAttribues.hp -= dano;
        if (charAttribues.hp <= 0)
        {
            //Neste caso como o hp é negativo deve-se somar,
            //para pegar o valor que diminui o hp para 0 e envia ao cliente
            dano += charAttribues.hp;
            charAttribues.hp = 0;

            enabled = false;
            moveServer.enabled = false;
            death = true;
            Remote.Invoke(ConstantsRPC.DEATH, Target.GroupMembers);
        }

        half halfDano = (half)dano;
        Remote.Invoke(ConstantsRPC.RECIEVE_DEMAGE, halfDano, Target.GroupMembers);

    }

    public override void SetShield(float shield)
    {
        charAttribues.hp += shield;
    }
    protected async UniTask SkillSendServer(float cdSkill, byte skill, int idenity)
    {
        if (skilletServer || death) return;
        skilletServer = true;
        SkLNow = skill;
        moveAction[skill] = false;
        move = false;
        var buffer = NetworkManager.Pool.Rent();
        buffer.Write(skill);
        buffer.Write(idenity);

        Remote.Invoke(ConstantsRPC.COWNTDOWN_SKILL, buffer, Target.GroupMembers);
        buffer.Dispose();

        await UniTask.WaitForSeconds(cdSkill);
        print(skill + " " + moveAction[skill]);
        skilletServer = false;
        if (moveAction[skill]) return;
        Remote.Invoke(ConstantsRPC.CONFIRMED_SKILL, skill, Target.GroupMembers);
    }

    protected NetworkIdentity VerifyDistance(float distance, int identity)
    {
        var IdentityEnemy = NetworkManager.Server.GetIdentity(identity);
        if (IdentityEnemy == null || death) return null;
        Transform enemy = IdentityEnemy.GetComponent<Transform>();

        //Menos um é porque o servidor está sempre um pouco mais para trás
        float distCompare = Vector3.Distance(enemy.position, transform.position) - 1;

        if (distCompare <= distance)
        {
            return IdentityEnemy;
        }
        return null;
    }
    protected async void InvokeSkill(DataBuffer buffer, byte skill, UnityAction<NetworkIdentity, PropSkills> fnSkill)
    {
        if (!skillLiberate[skill]) return;
        skillLiberate[skill] = false;
        PropSkills myProp = propSkill[skill];
        var identity = buffer.Read<int>();

        var identityEnemy = VerifyDistance(myProp.distance, identity);

        // if (identityEnemy == null) return;
        await SkillSendServer(myProp.cd, skill, identity);
        if (!moveAction[skill])
        {
            fnSkill(identityEnemy, myProp);
        }

        //Se o boneco moveu enquanto estava no cowndow não irá ativar a skill, só a espera dela, irá cancelar o ataque

        await UniTask.WaitForSeconds(myProp.skillWait);
        skillLiberate[skill] = true;
    }

}
