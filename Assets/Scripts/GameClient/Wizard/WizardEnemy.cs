using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WizardEnemy : CharacterEnemy
{
    [SerializeField]
    private ParticleSystem spell, circle, raio;
    private bool casting = true;
    [SerializeField]
    private GameObject inverseField, magicBomb, thunderTime, powerRock;
    [SerializeField]
    private ThunderBird thunderBird;
    
    private NetworkIdentity IdentityClicked;

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

    protected override async void Skill1()
    {
        if (casting)
        {
            spell.Play();
            circle.Play();
            raio.Play();
            return;
        }


        RecieveSkillOnServer(nameof(Skill1));
        await UniTask.WaitForSeconds(propSkillsEnemy[1].animTime);

        PowerRock rt = Instantiate(powerRock).GetComponent<PowerRock>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        rt.SetInfoPowerRock(posEnemy.position);
    }

    protected override async void Skill2()
    {
        if (casting)
        {
            circle.Play();
            return;
        }

        RecieveSkillOnServer(nameof(Skill2));
        await UniTask.WaitForSeconds(propSkillsEnemy[2].animTime);

        MagicBomb mb = Instantiate(magicBomb).GetComponent<MagicBomb>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        mb.SetDirection(posInitialSkill.position, posEnemy);

    }

    protected async override void Skill3()
    {
        if (casting)
        {
            circle.Play();
            return;
        }

        RecieveSkillOnServer(nameof(Skill3));
        await UniTask.WaitForSeconds(propSkillsEnemy[3].animTime);
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        await thunderBird.SetPosThunder(posEnemy);
    }

    protected override void Skill4()
    {
        if (casting)
        {
            circle.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill4));
        InverseField iF = Instantiate(inverseField).GetComponent<InverseField>();
        iF.SetInfoField(posInitialSkill, propSkillsEnemy[4].animTime);
    }

    protected async override void Skill5()
    {
        if (casting)
        {
            raio.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill5));
        await UniTask.WaitForSeconds(propSkillsEnemy[5].animTime);
        ThunderTime tt = Instantiate(thunderTime).GetComponent<ThunderTime>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        tt.SetInfoThunderTime(posEnemy.position);
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

        int Identity = buffer.Read<int>();
        var identClicked = NetworkManager.Client.GetIdentity(Identity);

        if (identClicked == null) return;
        IdentityClicked = identClicked;
        moveEnemy.RotateToClicked(IdentityClicked);
    }

    [Client(ConstantsRPC.CONFIRMED_SKILL)]
    private void ConfirmedSkill(DataBuffer buffer)
    {
        casting = false;
        byte confirmSkill = buffer.Read<byte>();
        dicSkills[confirmSkill]?.Invoke();
    }

    [Client(ConstantsRPC.CANCELED_SKILL)]
    private void CanceledSkill(DataBuffer buffer)
    {
        print("Cheguei e cancelei spells");
        spell.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        circle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        raio.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        animEnemy.Play("Idle");
    }


}
