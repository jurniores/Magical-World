using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill5 : Skills
{
    [SerializeField]
    private GameObject thunderTime;
    protected override void Start()
    {
        base.Start();
        ConstantsRPCForServer = ConstantsRPC.SKILL5_PLAYER;
    }

    protected override void SkillBeforeCd()
    {
        var raioMain = character.raio.main;
        raioMain.duration = timeTotalCowndown;
        character.raio.Play();
    }

    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(propSkills.animTime);
        base.SkillAfeterCd();
        ThunderTime tt = Instantiate(thunderTime).GetComponent<ThunderTime>();
        Transform posEnemy = IdentityClicked.GetComponent<Transform>();
        tt.SetInfoThunderTime(posEnemy.position);
    }

    protected override void CancelAnimations()
    {
        character.raio.Stop();
    }
}
