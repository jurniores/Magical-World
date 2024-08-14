using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class BaseMoveBots : NetworkBehaviour
{

    [SerializeField]
    protected float speed, gravity;
    [SerializeField]
    protected CharacterController characterController;
    public bool IsMoving { get; set; }
    protected bool distanceFinal;

    protected override void OnStart()
    {
        IsMoving = false;
        transform.position = new Vector3(0, 2, 0);
    }
    protected void Move(Vector3 lastPosition)
    {
        float distance = Vector3.Distance(transform.position, lastPosition);

        if (IsMoving && distance > 0.4f)
        {
            //Move o personagem na direção sem parar
            lastPosition.Normalize();
            lastPosition.y = gravity;
            characterController.Move(speed * Time.deltaTime * lastPosition);
            distanceFinal = false;
        }
        else if (!IsMoving && !distanceFinal)
        {
            //Valida a posição que o bot parou do player
            if (distance < 0.5f)
            {
                distanceFinal = true;
            }

            //Move o personagem até o ponto onde o cliente parou
            //Direção obtida do ponto que está, até o ponto que foi enviado na parada do cliente
            lastPosition -= transform.position;

            lastPosition.Normalize();
            lastPosition.y = gravity;
            characterController.Move(speed * Time.deltaTime * lastPosition);
        }
    }

}
