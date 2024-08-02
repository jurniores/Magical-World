using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using RockTools;
using UnityEngine;
using UnityEngine.Events;

public class BaseMoveClient : NetworkBehaviour
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
            animator.Play("Run");
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
    protected void MoveToChar(Vector3 lastPosition, float distance, UnityAction fnSkill, ref bool moveClicked)
    {
        float distanceChar = Vector3.Distance(transform.position, lastPosition);
        

        if (distance > distanceChar)
        {
            fnSkill();
            IsMoving = false;
            animator.SetBool("Run", false);
            moveClicked = false;
            return;
        }
        IsMoving = true;
        animator.SetBool("Run", true);
        lastPosition -= transform.position;

        lastPosition.Normalize();
        lastPosition.y = gravity;
        characterController.Move(speed * Time.deltaTime * lastPosition);
    }
    public void Rotate(float rotate)
    {
        transform.Rotate(0, rotate * Time.deltaTime, 0);
    }
}
