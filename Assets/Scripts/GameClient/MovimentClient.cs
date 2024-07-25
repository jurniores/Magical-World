using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using RockTools;
using UnityEngine;

public class MovimentClient : NetworkBehaviour
{
    [SerializeField]
    protected float speed, gravity;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected CharacterController characterController;
    public bool IsMoving { get; set; }
    protected override void OnStart()
    {
        IsMoving = false;
    }
    protected void Move(Vector3 move)
    {
        //Verificando se a movimentação está senod inserida, senao seta animação de IDLE
        if (move != Vector3.zero)
        {
            IsMoving = true;
            animator.SetBool("Run", true);
            move.y = gravity;
            move *= speed;
            characterController.Move(move * Time.deltaTime);
        }
        else
        {
            IsMoving = false;
            animator.SetBool("Run", false);
        }

    }

    public void Rotate(float rotate)
    {
        transform.Rotate(0, rotate * Time.deltaTime, 0);
    }
}
