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
    protected bool IsMoving { get; set; }
    protected bool distanceFinal;

    protected override void OnStart()
    {
        IsMoving = false;
        transform.position = new Vector3(0, 2, 0);
    }
    protected void Move(Vector3 lastPosition)
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        //Verifica se a distancia entre o bot e o player é mínima, para parar
        if (distance < 0.5f)
        {
            distanceFinal = true;
            return;
        };

        distanceFinal = false;

        if (IsMoving)
        {
            //Move o personagem na direção sem parar
            lastPosition.Normalize();
            lastPosition.y = gravity;
            characterController.Move(speed * Time.deltaTime * lastPosition);
        }
        else if (!IsMoving)
        {
            //Move o personagem até o ponto onde o cliente parou
            //Direção obtida do ponto que está, até o ponto que foi enviado na parada do cliente
            lastPosition -= transform.position;

            lastPosition.Normalize();
            lastPosition.y = gravity;
            characterController.Move(speed * Time.deltaTime * lastPosition);
        }
    }

}
