using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterEnemy : NetworkBehaviour
{

    [SerializeField]
    protected Animator animEnemy;
    [SerializeField]
    protected Transform posInitialSkill;
    [SerializeField]
    protected List<PropSkills> propSkillsEnemy;
    protected MoveEnemy moveEnemy;
    [SerializeField]
    protected CharacterAttributes charAttribues;
    protected Dictionary<byte, UnityAction> dicSkills;
    public CanvasPlayer cPlayer;
    protected override void OnAwake()
    {
        moveEnemy = Identity.Get<MoveEnemy>();
        dicSkills = new()
        {
            {0, SkillBase},
            {1, Skill1},
            {2, Skill2},
            {3, Skill3},
            {4, Skill4},
            {5, Skill5},
        };

        moveEnemy = Identity.Get<MoveEnemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected abstract void SkillBase();
    protected abstract void Skill1();
    protected abstract void Skill2();
    protected abstract void Skill3();
    protected abstract void Skill4();
    protected abstract void Skill5();

    [Client(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected void RecieveSKillsRPC(DataBuffer buffer)
    {
        buffer.DecompressRaw();
        var prop = buffer.ReadAsBinary<List<PropSkills>>();
        propSkillsEnemy = prop;

        var cAttributes = buffer.ReadAsBinary<CharacterAttributes>();
        //
        charAttribues = cAttributes;
        cPlayer.imgHp.SetConfig(charAttribues.hp);
    }

    [Client(ConstantsRPC.RECIEVE_DEMAGE)]
    protected void Demage(DataBuffer buffer)
    {
        float dano = buffer.Read<Half>();
        charAttribues.hp -= dano;
        cPlayer.imgHp.SetHp(charAttribues.hp);
    }
    
    [Client(ConstantsRPC.DEATH)]
    protected virtual void Death(DataBuffer buffer)
    {
        animEnemy.Play("Death");
        moveEnemy.enabled = false;
        cPlayer.gameObject.SetActive(false);
        enabled = false;
    }
}
