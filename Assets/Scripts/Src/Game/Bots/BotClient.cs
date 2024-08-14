using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BotClient : BaseMoveBots
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private Transform posInitial;
    [SerializeField]
    private float speedRotation;
    [SerializeField]
    private Animator animBot;
    private Vector3 dir, objDistance;

    void Start()
    {
        posInitial = NetworkService.GetAsComponent<Transform>("move1");
        transform.position = posInitial.position;
    }

    // Update is called once per frame
    void Update()
    {
        dir = objDistance - transform.position;

        dir.Normalize();
        float distance = Vector3.Distance(objDistance, transform.position);
        if (distance < 0.5f)
        {
            animBot.SetBool("Walk", false);
            animBot.SetBool("Run", false);
        }
        else
        {
            Move(dir);
            RotateBot(dir);
        }


    }


    void RotateBot(Vector3 moveDirection)
    {
        Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        character.rotation = Quaternion.Slerp(character.rotation, newRotation, speedRotation * Time.deltaTime);
    }

    [Client(ConstantsRPC.BOT_WALK)]
    void WalkRPC(DataBuffer buffer)
    {
        objDistance = buffer.Read<HalfVector3>();
        speed = 1;
        animBot.SetBool("Walk", true);
        IsMoving = true;
    }

    [Client(ConstantsRPC.BOT_RUN)]
    void RunRPC(DataBuffer buffer)
    {

    }

    [Client(ConstantsRPC.BOT_STOP)]
    void StopRPC(DataBuffer buffer)
    {
        Vector3 objDistance = buffer.Read<HalfVector3>();
        dir = objDistance;
        IsMoving = false;
        print("parei");
    }

    [Client(ConstantsRPC.BOT_ATACK)]
    void AtackRPC(DataBuffer buffer)
    {

    }
}
