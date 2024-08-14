using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill4 : Skills
{
    [SerializeField]
    private GameObject inverseField;

    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL4_PLAYER;
    }

    protected override void SkillBeforeCd()
    {
        var raioMain = character.circle.main;
        raioMain.startLifetime = timeTotalCowndown;
        character.circle.Play();
    }

    protected override async void SkillAfeterCd()
    {
        InverseField iF = Instantiate(inverseField).GetComponent<InverseField>();
        iF.SetInfoField(posInitialSkill, propSkills.animTime);
        await UniTask.WaitForSeconds(propSkills.animTime);
        base.SkillAfeterCd();
    }

    protected override void CancelAnimations()
    {
        character.circle.Stop();
    }
}