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
    private ParticleSystem spell, circle, raio;
    [SerializeField]
    private GameObject powerRock;

    protected override void Start()
    {
        base.Start();
        spell.Stop();
        circle.Stop();
        raio.Stop();
        //pSystemWizard.time
        ConstantsRPCForServer = ConstantsRPC.SKILL1_PLAYER;
    }
    protected override void SkillBeforeCd()
    {

        var spellMain = spell.main;
        var circleMain = circle.main;
        var raioMain = raio.main;

        spellMain.startLifetime = timeTotalCowndown;
        circleMain.startLifetime = timeTotalCowndown;
        raioMain.duration = timeTotalCowndown;

        spell.Play();
        circle.Play();
        raio.Play();
    }
    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(animTime);

        PowerRock rt = Instantiate(powerRock).GetComponent<PowerRock>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        rt.SetInfoPowerRock(posEnemy.position);
    }
}
