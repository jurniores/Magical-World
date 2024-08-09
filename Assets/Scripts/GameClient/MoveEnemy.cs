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
    private NetworkIdentity identityCharClicked;
    private bool rotateCharClicked = false;

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
        if (rotateCharClicked)
        {
            moveDirection = identityCharClicked.transform.position - transform.position;
        }
        
        Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        character.rotation = Quaternion.Slerp(character.rotation, newRotation, speedRotation * Time.deltaTime);
    }

    [Client(ConstantsRPC.MOVIMENT_PLAYER)]
    void MovimentPlayerRpcClient(DataBuffer buffer)
    {
        rotateCharClicked = false;
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

    void RotateMyChar(Vector3 dir)
    {
        Vector3 rot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        if (rot.y == 0 || rot == Vector3.zero) return;
        rot.x = 0;
        character.rotation = Quaternion.Euler(rot);
    }

    public void RotateToClicked(NetworkIdentity identity)
    {
        identityCharClicked = identity;
        rotateCharClicked = true;
    }
}
