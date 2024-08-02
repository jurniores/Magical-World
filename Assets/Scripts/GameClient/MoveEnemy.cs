using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class MoveEnemy : BaseMoveBots
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private float speedRotation;
    [SerializeField]
    private Animator animEnemy;
    private Vector3 position;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move(position);

        //Se o player parou seta anim de parado
        if (distanceFinal)
        {
            animEnemy.SetBool("Run", false);

        }
        else
        {
            animEnemy.SetBool("Run", true);
        }



        //Suavização da rotação
        Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        character.rotation = Quaternion.Slerp(character.rotation, newRotation, speedRotation * Time.deltaTime);
    }

    [Client(ConstantsRPC.MOVIMENT_PLAYER)]
    void MovimentPlayerRpcClient(DataBuffer buffer)
    {
        //Recebe a posição atual do personagem na quantidade de ticks
        Vector3 hMove = buffer.Read<HalfVector3>();
        //Pega a direção que pela posição enviada do cliente
        hMove -= transform.position;
        moveDirection = hMove;
        position = hMove;
        IsMoving = true;
    }

    [Client(ConstantsRPC.MOVIMENT_PLAYER_STOP)]
    void MovimentPlayerRpcClientStop(DataBuffer buffer)
    {
        //Recebe a posição onde o personagem parou
        position = buffer.Read<HalfVector3>();
        moveDirection = position - transform.position;
        IsMoving = false;
    }
}
