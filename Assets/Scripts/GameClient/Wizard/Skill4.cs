using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill4 : Skills
{
    [SerializeField]
    private ParticleSystem circle;
    [SerializeField]
    private GameObject inverseField;

    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL4_PLAYER;
        circle.Stop();
    }

    protected override void SkillBeforeCd()
    {
        var raioMain = circle.main;
        raioMain.startLifetime = timeTotalCowndown;
        circle.Play();
    }

    protected override void SkillAfeterCd()
    {
        InverseField iF = Instantiate(inverseField).GetComponent<InverseField>();
        iF.SetInfoField(posInitialSkill, animTime);
    }
}