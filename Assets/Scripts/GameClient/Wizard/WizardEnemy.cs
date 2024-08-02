using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WizardEnemy : CharacterEnemy
{
    [SerializeField]
    private ParticleSystem spell, circle, raio;
    private bool casting = true;

    void Start()
    {
        spell.Stop();
        circle.Stop();
        raio.Stop();
    }
    protected override void SkillBase()
    {
        if (casting) return;


        RecieveSkillOnServer(nameof(SkillBase));
    }

    protected override void Skill1()
    {
        if (casting)
        {
            spell.Play();
            circle.Play();
            raio.Play();
            return;
        }


        RecieveSkillOnServer(nameof(Skill1));
    }

    protected override void Skill2()
    {
        if (casting)
        {
            circle.Play();
            return;
        }

        RecieveSkillOnServer(nameof(Skill2));

    }

    protected override void Skill3()
    {
        if (casting)
        {
            circle.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill3));
    }

    protected override void Skill4()
    {
        if (casting)
        {
            circle.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill4));
    }

    protected override void Skill5()
    {
        if (casting)
        {
            raio.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill5));
    }
    void RecieveSkillOnServer(string nameSkill)
    {
        animEnemy.Play(nameSkill);
    }


    [Client(ConstantsRPC.COWNTDOWN_SKILL)]
    private void CowntDownSkill(DataBuffer buffer)
    {

        casting = true;
        animEnemy.Play("Casting");

        byte confirmSkill = buffer.Read<byte>();
        dicSkills[confirmSkill]?.Invoke();
    }

    [Client(ConstantsRPC.CONFIRMED_SKILL)]
    private void ConfirmedSkill(DataBuffer buffer)
    {
        casting = false;
        byte confirmSkill = buffer.Read<byte>();
        print(confirmSkill);
        dicSkills[confirmSkill]?.Invoke();
    }

    [Client(ConstantsRPC.DEMAGE_PLAYER)]
    protected override void Demage(DataBuffer buffer)
    {

    }
}
