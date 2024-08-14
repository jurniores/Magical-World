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
    protected Transform posInitialSkill;
    public string nameSkill;
    protected byte ConstantsRPCForServer;
    protected NetworkIdentity IdentityClicked;
    protected UnityAction fnSkillReceived;
    protected UnityAction<string> fnAnimCharacter;
    private BtnSkills btnSkill;
    protected CharacterClient character;

    public float timeTotalCowndown;

    public PropSkills propSkills;

    public PropSkills PropSkills
    {
        get => propSkills;
        set
        {

            propSkills = value;
            timeTotalCowndown = propSkills.cd + propSkills.animTime;
        }
    }
    protected virtual void Start()
    {
        btnSkill.SetInfo(sprite);
        character = GetComponent<CharacterClient>();
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
        IdentityClicked = identity;
        SendSkillServer(Character, IdentityClicked.IdentityId);
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
        character.movePlayer.enabled = false;
        fnAnimCharacter(nameSkill);
        SkillAfeterCd();
    }
    //Servidor chama o Cowndown para depois chamar a skill
    public void CowntDownSkill()
    {
        //Trava o boneco para não andar nem poder soltar nova skill
        character.cowntdownFilled.SetCowntDown(PropSkills.cd);
        character.animCharacter.Play("Casting");
        SkillBeforeCd();
        float cowntdownReal = PropSkills.cd + propSkills.animTime;

        character.skillet = true;
    }
    public void CancelSkill()
    {
        btnSkill.ActiveSkill(propSkills.skillWait);
    }
    protected virtual void SkillBeforeCd()
    {
        //
    }
    //Obrigatório chamar em todas as classes filhas devido ativar a movimentação do personagem
    protected virtual void SkillAfeterCd()
    {
        //Depois do cowndown e da animação da skill é liberado para soltar uma nova skill e andar
        character.movePlayer.enabled = true;
        character.skillet = false;
        btnSkill.ActiveSkill(propSkills.skillWait);
    }

    protected virtual void CancelAnimations()
    {
        print("Cancelei no metodo");
    }


    //Método para ser o botão da skill
    public void SetBtn(BtnSkills btnSkillP)
    {
        btnSkill = btnSkillP;
    }
}
