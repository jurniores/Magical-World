using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterServer : NetworkBehaviour
{
    public CharacterAttributes charAttribues;
    public List<PropSkills> propSkill;
    protected abstract void Skill5(DataBuffer buffer);
    protected abstract void Skill4(DataBuffer buffer);
    protected abstract void Skill3(DataBuffer buffer);
    protected abstract void Skill2(DataBuffer buffer);
    protected abstract void Skill1(DataBuffer buffer);
    protected abstract void SkillBase(DataBuffer buffer);
    protected abstract void Demage(float dano);

    [Server(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected async void GetPropSkills()
    {
        //PODE DAR PROBLEMA CASO HOUVER MUITO LAG. POR CAUSA DO TEMPO DE ESPERA
        await UniTask.WaitForSeconds(1);
        using var buffer = NetworkManager.Pool.Rent();
        buffer.WriteAsBinary(propSkill);
        buffer.WriteAsBinary(charAttribues);
        buffer.CompressRaw();
        Remote.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS, buffer, Target.Self);

        using var buffer2 = NetworkManager.Pool.Rent();
        buffer2.WriteAsBinary(charAttribues);
        buffer2.CompressRaw();
        Remote.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS, buffer2, Target.GroupMembersExceptSelf);
    }

    public void SetDemage(float dano)
    {

        charAttribues.hp -= dano;
        if (charAttribues.hp <= 0)
        {
            //Neste caso como o hp é negativo deve-se somar,
            //para pegar o valor que diminui o hp para 0 e envia ao cliente
            dano += charAttribues.hp;
            charAttribues.hp = 0;
        }
        half halfDano = (half)dano;
        Remote.Invoke(ConstantsRPC.RECIEVE_DEMAGE, halfDano, Target.GroupMembers);

    }
    public void SetShield(float shield)
    {
        charAttribues.hp += shield;
    }

}
