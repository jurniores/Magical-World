using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class CharacterBotServer : Character
{
    [SerializeField]
    private CharacterAttributes charAttribues;
    [SerializeField]
    private BotServer moveBot;
    private bool death = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override async void SetDemage(float dano)
    {
        if (charAttribues.hp <= 0) return;

        charAttribues.hp -= dano;
        if (charAttribues.hp <= 0)
        {
            //Neste caso como o hp é negativo deve-se somar,
            //para pegar o valor que diminui o hp para 0 e envia ao cliente
            dano += charAttribues.hp;
            charAttribues.hp = 0;

            enabled = false;
            moveBot.enabled = false;
            death = true;
        }

        half halfDano = (half)dano;

        var buffer = NetworkManager.Pool.Rent();
        buffer.Write(halfDano);

        Remote.Invoke(ConstantsRPC.RECIEVE_DEMAGE, buffer, groupId: moveBot.GroupID);
        buffer.Dispose();
        
        if (death)
        {
            Remote.Invoke(ConstantsRPC.DEATH, groupId: moveBot.GroupID);
            await UniTask.WaitForSeconds(10);
            Identity.Destroy();
        }
    }

    [Server(ConstantsRPC.RECIEVE_CONFIGS_INITIALS)]
    protected async void GetPropSkills()
    {
        //PODE DAR PROBLEMA CASO HOUVER MUITO LAG. POR CAUSA DO TEMPO DE ESPERA
        await UniTask.WaitForSeconds(1);
        using var buffer = NetworkManager.Pool.Rent();
        buffer.WriteAsBinary(charAttribues);
        Remote.Invoke(ConstantsRPC.RECIEVE_CONFIGS_INITIALS, buffer, groupId: moveBot.GroupID);
    }

    public override void SetShield(float dano)
    {
        print("SetShieldBot");
    }
}
