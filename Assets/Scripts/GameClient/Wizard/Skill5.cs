using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Skill5 : Skills
{
    [SerializeField]
    private ParticleSystem raio;
    [SerializeField]
    private GameObject thunderTime;
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

    protected override async void SkillAfeterCd()
    {
        await UniTask.WaitForSeconds(animTime);
        ThunderTime tt = Instantiate(thunderTime).GetComponent<ThunderTime>();
        Transform posEnemy = identityCliked.GetComponent<Transform>();
        tt.SetInfoThunderTime(posEnemy.position);
    }


}
