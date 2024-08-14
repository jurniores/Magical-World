using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using Omni.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class Skill1 : Skills
{
    
    [SerializeField]
    private GameObject powerRock;

    protected override void Start()
    {
        base.Start();
        //pSystemWizard.time
        ConstantsRPCForServer = ConstantsRPC.SKILL1_PLAYER;
    }
    protected override void SkillBeforeCd()
    {

        var spellMain = character.spell.main;
        var circleMain = character.circle.main;
        var raioMain = character.raio.main;

        spellMain.startLifetime = timeTotalCowndown;
        circleMain.startLifetime = timeTotalCowndown;
        raioMain.duration = timeTotalCowndown;

        character.spell.Play();
        character.circle.Play();
        character.raio.Play();
    }
    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(propSkills.animTime);
        base.SkillAfeterCd();
        PowerRock rt = Instantiate(powerRock).GetComponent<PowerRock>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        rt.SetInfoPowerRock(posEnemy.position);
    }

    protected override void CancelAnimations()
    {
        base.CancelAnimations();
        print("Chamei no cliente cancelando");
        character.spell.Stop();
        character.circle.Stop();
        character.raio.Stop();
    }
}
