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
    private Vector3 dir, objDistance = Vector3.zero;

    void Start()
    {
        posInitial = NetworkService.GetAsComponent<Transform>("initPos");
        transform.position = posInitial.position;
        print(posInitial.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (objDistance == Vector3.zero) return;
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
        Vector3 rot = Quaternion.LookRotation(moveDirection, Vector3.up).eulerAngles;
        rot.x = 0;

        Quaternion newRotation = Quaternion.Euler(rot);
        character.rotation = Quaternion.Slerp(character.rotation, newRotation, speedRotation * Time.deltaTime);
    }
    public async void Death()
    {
        characterController.enabled = false;
        await UniTask.WaitForSeconds(4);
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.drag = 20f;
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
