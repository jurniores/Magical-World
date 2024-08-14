
using System.Collections.Generic;
using System.Linq;
using Omni.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterClient : NetworkBehaviour
{
    protected List<Skills> skills;
    
    [SerializeField]
    private Color[] colors = new Color[2];

    public ParticleSystem spell,
        circle,
        raio;
    public Animator animCharacter;
    public CowntdownFilled cowntdownFilled;
    public bool death = false;

    [SerializeField]
    private LayerMask layerCollider;
    private Skills skillNow;
    public MovePlayer movePlayer;
    private Camera mainCamera;
    protected NetworkIdentity IdentityClicked;

    [SerializeField]
    protected CharacterAttributes charAttribues;
    protected ComponentsSkillsBtns componentsSkillsBtns;
    private GameObject canvasActive;

    public CanvasPlayer cPlayer;
    public bool skillet = false;

    void StopParticles()
    {
        spell.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        circle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        raio.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

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
        StopParticles();
        Local.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerCollider))
            {
                IdentityClicked = hit.collider.GetComponent<NetworkIdentity>();

                if (canvasActive != null)
                    canvasActive.SetActive(false);

                canvasActive = IdentityClicked.GetComponentInChildren<Canvas>(true).gameObject;
                canvasActive.SetActive(true);
            }
        }
    }

    public float AvaliableDistance(float distance)
    {
        if (IdentityClicked == null)
            return 1;
        Vector3 posCharClicked = IdentityClicked.GetComponent<Transform>().position;

        float distanceCharClicked = Vector3.Distance(movePlayer.transform.position, posCharClicked);

        if (distanceCharClicked > distance)
            return distance;
        return 0;
    }

    public void FuncSkills(Skills skill)
    {
        if(death) return;
        float avaliableDistance = AvaliableDistance(skill.propSkills.distance);

        if (avaliableDistance != 0)
        {
            movePlayer.MoveToClickedChar(
                IdentityClicked,
                avaliableDistance,
                () => skill.PlaySkill(this, IdentityClicked, SkillActived)
            );
            return;
        }
        ;

        skill.PlaySkill(this, IdentityClicked, SkillActived);
    }

    public void FuncSKillArea(Skills skill)
    {
        skill.PlaySkillArea(this, SkillActivedArea);
    }

    public void SkillActivedArea(string nameSkillAnim)
    {
        //Script para ativar somente quando a skill for acionada pela skill em area

        animCharacter.Play(nameSkillAnim);
    }

    public void SkillActived(string nameSkillAnim)
    {
        //Script para ativar somente quando a skill for acionada pela skill
        //animação e rotação
        movePlayer.RotateToClicked(IdentityClicked);
        animCharacter.Play(nameSkillAnim);
    }

    protected abstract void Skill5();
    protected abstract void Skill4();
    protected abstract void Skill3();
    protected abstract void Skill2();
    protected abstract void Skill1();
    protected abstract void SkillBase();

    [Client(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected void RecieveSKillsRPC(DataBuffer buffer)
    {
        buffer.DecompressRaw();
        var prop = buffer.ReadAsBinary<List<PropSkills>>();

        var cAttributes = buffer.ReadAsBinary<CharacterAttributes>();
        charAttribues = cAttributes;

        cPlayer.imgHp.SetConfig(charAttribues.hp);
        int count = 0;

        skills.ForEach(e =>
        {
            e.PropSkills = prop[count];
            count++;
        });
    }

    [Client(ConstantsRPC.RECIEVE_DEMAGE)]
    protected virtual void Demage(DataBuffer buffer)
    {
        float dano = buffer.Read<Half>();
        charAttribues.hp -= dano;
        print("Dano no cliente de " + dano);
        cPlayer.imgHp.SetHp(charAttribues.hp);
    }

    [Client(ConstantsRPC.CONFIRMED_SKILL)]
    protected void RecieveSkill(DataBuffer buffer)
    {
        byte skCount = buffer.Read<byte>();
        skillNow = skills[skCount];
        skillNow.AtackSkillAsync();
    }

    [Client(ConstantsRPC.COWNTDOWN_SKILL)]
    protected void CowntDownSkill(DataBuffer buffer)
    {
        byte skCount = buffer.Read<byte>();
        skillNow = skills[skCount];
        movePlayer.RotateToClicked(IdentityClicked);
        skillNow.CowntDownSkill();
    }

    [Client(ConstantsRPC.CANCELED_SKILL)]
    protected void CanceledSkill(DataBuffer buffer)
    {
        skillNow.CancelSkill();
        skillet = false;
        movePlayer.enabled = true;
        StopParticles();
        cowntdownFilled.CancelCowntDown();
        print("Recebi do servidor o cancelamento");
    }

    [Client(ConstantsRPC.DEATH)]
    protected virtual void Death(DataBuffer buffer)
    {
        animCharacter.Play("Death");
        movePlayer.enabled = false;
        cPlayer.gameObject.SetActive(false);
        enabled = false;
        death = true;
    }

}
