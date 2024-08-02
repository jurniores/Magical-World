using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public class SkillBase : Skills
{
    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILLBASE_PLAYER;
    }
    

    protected override void SkillAfeterCd()
    {
        print("Ataquei");
    }
}
