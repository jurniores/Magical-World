using AYellowpaper.SerializedCollections;
using Omni.Core;
using Unity.Mathematics;
using UnityEngine;


public class Wizard : Character
{

    [SerializeField]
    private ControllsMaterials cMaterialsChar, cMaterialLivro;

    protected override void SkillBase()
    {
        FuncSkills(skills[0]);
    }
    protected override void Skill1()
    {
        FuncSkills(skills[1]);
    }
    protected override void Skill2()
    {
        FuncSkills(skills[2]);
    }
    protected override void Skill3()
    {
        FuncSkills(skills[3]);
    }
    protected override void Skill4()
    {
        FuncSKillArea(skills[4]);
    }
    protected override void Skill5()
    {
        FuncSkills(skills[5]);
    }

   
}
