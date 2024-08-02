using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public class Skill2 : Skills
{
    [SerializeField]
    private ParticleSystem circle;
    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL2_PLAYER;
        circle.Stop();
    }

    protected override void SkillBeforeCd()
    {
        var circleMain = circle.main;
        circleMain.startLifetime = timeTotalCowndown;
        circle.Play();
    }
}
