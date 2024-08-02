using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Schema;
using Omni.Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : NetworkBehaviour
{
    protected List<Skills> skills;

    [SerializeField]
    private Color[] colors = new Color[2];
    public Animator animCharacter;
    public CowntdownFilled cowntdownFilled;

    [SerializeField]
    private LayerMask layerCollider;
    public MovePlayer movePlayer;
    private Camera mainCamera;
    protected NetworkIdentity IdentityClicked;
    protected CharacterAttributes charAttribues;
    protected ComponentsSkillsBtns componentsSkillsBtns;
    private GameObject canvasActive;

    public bool skillet = false;

    void Start()
    {
        mainCamera = NetworkService.GetAsComponent<Camera>();
        componentsSkillsBtns = NetworkService.GetAsComponent<ComponentsSkillsBtns>();
        cowntdownFilled = componentsSkillsBtns.cdFilled;
        skills = GetComponents<Skills>().ToList();
        var listFunc = new List<UnityAction>()
        {
            SkillBase,
            Skill1,
            Skill2,
            Skill3,
            Skill4,
            Skill5,
        };

        byte count = 0;

        //Define os botões que está setado para skill
        //Define também a posição do array que ele está, para enviar ao servidor e depois retornar
        foreach (var skill in skills)
        {
            BtnSkills btnSkill = componentsSkillsBtns.listBtnSkills[count];
            btnSkill.SetSkill(listFunc[count]);
            skill.SetBtn(btnSkill);
            count++;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerCollider))
            {
                IdentityClicked = hit.collider.GetComponent<NetworkIdentity>();

                if (canvasActive != null) canvasActive.SetActive(false);

                canvasActive = IdentityClicked.GetComponentInChildren<Canvas>(true).gameObject;
                canvasActive.SetActive(true);
            }
        }
    }


    public float AvaliableDistance(float distance)
    {
        if (IdentityClicked == null) return 1;
        Vector3 posCharClicked = IdentityClicked.GetComponent<Transform>().position;

        float distanceCharClicked = Vector3.Distance(movePlayer.transform.position, posCharClicked);

        if (distanceCharClicked > distance) return distance;
        return 0;
    }
    public void FuncSkills(Skills skill)
    {
        float avaliableDistance = AvaliableDistance(skill.distance);

        if (avaliableDistance != 0)
        {
            movePlayer.MoveToClickedChar(IdentityClicked, avaliableDistance, () => skill.PlaySkill(this, IdentityClicked, SkillActived));
            return;
        };

        skill.PlaySkill(this, IdentityClicked, SkillActived);

    }

    void SkillActived(string nameSkillAnim)
    {
        //Script de torno para ativar somente quando a skill for acionada
        //animação e rotação
        movePlayer.RotateToClicked(IdentityClicked);
        animCharacter.Play(nameSkillAnim);
    }

    [Client(ConstantsRPC.CONFIRMED_SKILL)]
    protected void RecieveSkill(DataBuffer buffer)
    {
        byte skCount = buffer.Read<byte>();
        print("Skill; " +skCount);
        Skills skill = skills[skCount];
        skill.AtackSkillAsync();
        print("Chamei skill");
    }
    [Client(ConstantsRPC.COWNTDOWN_SKILL)]
    protected void CowntDownSkill(DataBuffer buffer)
    {
        byte skCount = buffer.Read<byte>();
        Skills skill = skills[skCount];
        skill.CowntDownSkill();
        print("Chamei cowntdown");
    }

    protected abstract void Skill5();
    protected abstract void Skill4();
    protected abstract void Skill3();
    protected abstract void Skill2();
    protected abstract void Skill1();
    protected abstract void SkillBase();
    protected abstract void Demage(DataBuffer buffer);
}
