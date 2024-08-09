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
    [SerializeField]
    private float animTimeBase, animTime1, animTime2, animTime3, animTime4, animTime5;
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
        await UniTask.WaitForSeconds(animTime1);

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
        await UniTask.WaitForSeconds(animTime2);

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
        await UniTask.WaitForSeconds(animTime3);
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        thunderBird.SetPosThunder(posEnemy);
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
        iF.SetInfoField(posInitialSkill, animTime4);
    }

    protected async override void Skill5()
    {
        if (casting)
        {
            raio.Play();
            return;
        }
        RecieveSkillOnServer(nameof(Skill5));
        await UniTask.WaitForSeconds(animTime5);
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

        if(identClicked == null) return;
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

    
}
