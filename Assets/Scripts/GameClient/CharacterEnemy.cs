using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterEnemy : NetworkBehaviour
{

    [SerializeField]
    protected Animator animEnemy;
    protected MoveEnemy moveEnemy;
    protected CharacterAttributes charAttribues;
    protected Dictionary<byte, UnityAction> dicSkills;
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

    protected abstract void Demage(DataBuffer buffer);
}
