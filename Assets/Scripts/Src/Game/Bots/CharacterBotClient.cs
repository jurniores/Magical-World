using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Unity.Mathematics;
using UnityEngine;

public class CharacterBotClient : NetworkBehaviour
{
    [SerializeField]
    private CanvasPlayer cBot;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private BotClient moveBot;
    [SerializeField]
    private CharacterAttributes charAttribues;
    private bool death = false;

    //IMCOMPLETO
    void Start()
    {
        Local.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Client(ConstantsRPC.RECIEVE_DEMAGE)]
    private void Demage(DataBuffer buffer)
    {
        float dano = buffer.Read<half>();
        charAttribues.hp -= dano;
        print("Dano no cliente de " + dano);
        cBot.imgHp.SetHp(charAttribues.hp);
    }

    [Client(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected void RecieveConfigsInitiais(DataBuffer buffer)
    {
        var attributes = buffer.ReadAsBinary<CharacterAttributes>();
        charAttribues = attributes;
        cBot.imgHp.SetConfig(charAttribues.hp);
    }


    [Client(ConstantsRPC.DEATH)]
    protected virtual void Death(DataBuffer buffer)
    {
        anim.Play("Death");
        moveBot.enabled = false;
        cBot.gameObject.SetActive(false);
        enabled = false;
        death = true;
    }
}
