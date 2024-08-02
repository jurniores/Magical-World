using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public class Skill5 : Skills
{
    [SerializeField]
    private ParticleSystem raio;
    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL5_PLAYER;
        raio.Stop();
    }

    protected override void SkillBeforeCd()
    {
        var raioMain = raio.main;
        raioMain.duration = timeTotalCowndown;
        raio.Play();
    }


}
