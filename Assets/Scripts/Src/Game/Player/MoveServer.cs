using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Omni.Core;
using Omni.Shared;
using UnityEngine;
public class MoveServer : BaseMoveBots
{
    [SerializeField]
    private float correctLagDistance = 3;
    private Vector3 position;
    private Vector3 corretLag;
    float distPlayerAndServer;
    private bool lagClientReposition = false;
    // Update is called once per frame
    void Update()
    {
        distPlayerAndServer = Vector3.Distance(transform.position, corretLag);

        Move(position);
    }

    protected override void OnStart()
    {

    }

    [Server(ConstantsRPC.MOVIMENT_PLAYER)]
    void MovimentPlayerRpcServer(DataBuffer buffer)
    {
        //Recebe a posição atual do personagem na quantidade de ticks
        Vector3 hMove = buffer.Read<HalfVector3>();
        lagClientReposition = false;
        corretLag = hMove;

        //Pega a direção que pela posição enviada do cliente
        hMove -= transform.position;
        position = hMove;
        IsMoving = true;
    }

    [Server(ConstantsRPC.MOVIMENT_PLAYER_STOP)]
    void MovimentPlayerRpcServerStop(DataBuffer buffer)
    {
        Remote.Invoke(ConstantsRPC.MOVIMENT_PLAYER_STOP, buffer, Target.GroupMembersExceptSelf);
        //Recebe a posição onde o personagem parou
        HalfVector3 hMove = buffer.Read<HalfVector3>();

        position = hMove;

        //Não deixa o bot voltar para posição que o player parou antes de corrigir
        IsMoving = false;
    }

    //RPC para recebimento de volta da correção do lag
    //RPC específico para não influenciar no IsMove
    [Server(ConstantsRPC.MOVIMENT_PLAYER_CORRECT_POSITION)]
    void CorrectConfirmePositionLag(DataBuffer buffer)
    {
        Vector3 hMove = buffer.Read<HalfVector3>();
        lagClientReposition = false;
        corretLag = hMove;
    }

    public override void OnTick(ITickInfo data)
    {
        if (IsMoving)
        {
            //Enviando a movimentação para todos os clientes
            using DataBuffer buffer = NetworkManager.Pool.Rent();
            HalfVector3 hMove = transform.position;
            buffer.Write(hMove);

            Remote.Invoke(ConstantsRPC.MOVIMENT_PLAYER, buffer, Target.GroupMembersExceptSelf, DeliveryMode.Sequenced, sequenceChannel: 1);
        }

        if (distPlayerAndServer > correctLagDistance && !lagClientReposition)
        {
            //Envia um RPC para reposição de posição e espera o player reenviar a confirmação para continuar andar
            lagClientReposition = true;

            position = transform.position;
            IsMoving = false;

            //Corrigindo a posição lagado
            using DataBuffer buffer = NetworkManager.Pool.Rent();
            buffer.Write((HalfVector3)transform.position);
            Remote.Invoke(ConstantsRPC.MOVIMENT_PLAYER_CORRECT_POSITION, buffer, Target.Self);
        }

    }
}
