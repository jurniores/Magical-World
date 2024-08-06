using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill2 : Skills
{
    [SerializeField]
    private ParticleSystem circle;
    [SerializeField]
    private GameObject magicBomb;

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

    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(animTime);

        MagicBomb mb = Instantiate(magicBomb).GetComponent<MagicBomb>();
        Transform posEnemy = identityCliked.GetComponent<Transform>();
        mb.SetDirection(posInitialSkill.position, posEnemy);
    }
}
