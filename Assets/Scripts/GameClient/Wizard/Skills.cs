using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skills : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    protected float timeSkillwait;
    [SerializeField]
    protected float animTime = 2;
    [SerializeField]
    protected Transform posInitialSkill;
    public string nameSkill;
    protected byte ConstantsRPCForServer;
    protected NetworkIdentity identityCliked;
    protected UnityAction fnSkillReceived;
    protected UnityAction<string> fnAnimCharacter;
    private BtnSkills btnSkill;
    protected Character character;

    public float distance, timeTotalCowndown;

    private PropSkills propSkills;

    public PropSkills PropSkills
    {
        get => propSkills;
        set
        {
            
            propSkills = value;
            timeTotalCowndown = propSkills.cd + animTime;
        }
    }

    protected virtual void Start()
    {
        btnSkill.SetInfo(sprite);
        character = GetComponent<Character>();
    }

    //Recebe o click de envio de skills
    public void PlaySkillArea(NetworkBehaviour Character, UnityAction<string> fnAnim)
    {
        fnAnimCharacter = fnAnim;
        SendSkillAreaServer(Character);
    }
    public void PlaySkill(NetworkBehaviour Character, NetworkIdentity identity, UnityAction<string> fnAnim)
    {
        fnAnimCharacter = fnAnim;
        identityCliked = identity;
        SendSkillServer(Character, identityCliked.IdentityId);
    }

    private void SendSkillAreaServer(NetworkBehaviour Character)
    {
        //Validação para enviar somente a skill primeira que foi clicada
        if (character.skillet) return;
        //Pegou o RPC da identidade para enviar de qualquer local
        Character.Local.Invoke(ConstantsRPCForServer);
        character.skillet = true;
    }
    //Envia para o servidor qual skill está sendo acionada para enviar para todos e confirmar para mim
    private void SendSkillServer(NetworkBehaviour Character, int IdentityId)
    {
        //Validação para enviar somente a skill primeira que foi clicada
        if (character.skillet) return;
        using var buffer = NetworkManager.Pool.Rent();
        buffer.Write(IdentityId);
        //Pegou o RPC da identidade para enviar de qualquer local
        Character.Local.Invoke(ConstantsRPCForServer, buffer);

    }
    //Ativa skill chamada pelo servidor pós cowntdown
    public void AtackSkillAsync()
    {
        SkillAfeterCd();
        fnAnimCharacter(nameSkill);
        character.skillet = false;
    }
    //Servidor chama o Cowndown para depois chamar a skill
    public void CowntDownSkill()
    {
        character.cowntdownFilled.SetCowntDown(PropSkills.cd);
        character.animCharacter.Play("Casting");
        SkillBeforeCd();
        float cowntdownReal = PropSkills.cd + animTime;
        btnSkill.ActiveSkill(timeSkillwait, cowntdownReal);
        character.skillet = true;
    }
    protected virtual void SkillBeforeCd()
    {
        //
    }
    protected virtual void SkillAfeterCd()
    {
        //
    }

    //Método para ser o botão da skill
    public void SetBtn(BtnSkills btnSkillP)
    {
        btnSkill = btnSkillP;
    }
}
