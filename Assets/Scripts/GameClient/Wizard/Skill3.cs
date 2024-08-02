using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public class Skill3 : Skills
{
    [SerializeField]
    private ParticleSystem circle;
    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL3_PLAYER;
        circle.Stop();
    }

    protected override void SkillBeforeCd()
    {
        var raioMain = circle.main;
        raioMain.startLifetime = timeTotalCowndown;
        circle.Play();
    }
}
