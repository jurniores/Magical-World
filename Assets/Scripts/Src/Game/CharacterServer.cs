using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterServer : NetworkBehaviour
{
    protected CharacterAttributes charAttribues;
    public List<PropSkills> propSkill;
    protected abstract void Skill5(DataBuffer buffer);
    protected abstract void Skill4(DataBuffer buffer);
    protected abstract void Skill3(DataBuffer buffer);
    protected abstract void Skill2(DataBuffer buffer);
    protected abstract void Skill1(DataBuffer buffer);
    protected abstract void SkillBase(DataBuffer buffer);
    protected abstract void Demage(float dano);
    
    [Server(ConstantsRPC.RECIEVE_SKILL)]
    protected void GetPropSkills()
    {
        using var buffer = NetworkManager.Pool.Rent();
        buffer.WriteAsBinary(propSkill);
        buffer.CompressRaw();
        Remote.Invoke(ConstantsRPC.RECIEVE_SKILL, buffer, Target.GroupMembers);
    }

}
