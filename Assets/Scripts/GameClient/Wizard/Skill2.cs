using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill2 : Skills
{
    [SerializeField]
    private GameObject magicBomb;

    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL2_PLAYER;
    }

    protected override void SkillBeforeCd()
    {
        var circleMain = character.circle.main;
        circleMain.startLifetime = timeTotalCowndown;
        character.circle.Play();
    }

    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(propSkills.animTime);
        base.SkillAfeterCd();
        MagicBomb mb = Instantiate(magicBomb).GetComponent<MagicBomb>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        mb.SetDirection(posInitialSkill.position, posEnemy);
    }

    protected override void CancelAnimations()
    {
        character.circle.Stop();
    }
}
