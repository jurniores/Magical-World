using Omni.Core;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Events;
public class MovePlayer : BaseMoveClient
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private float speedRot, distanceCharClicked;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cam;
    private Joystick joystick;
    private Vector3 move = Vector3.zero;
    private bool corretLag = false;
    private bool movePlayer = true;
    private bool rpc = false;
    public bool validateMove = true;
    private bool moveClicked = false, rotateCharClicked = false;
    private HalfVector3 hMove;
    NetworkIdentity identityCharClicked;


    private UnityAction fnSkill;
    protected override void OnStart()
    {

        joystick = FindAnyObjectByType<Joystick>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;

        using DataBuffer dataBuffer = NetworkManager.Pool.Rent();
        hMove = transform.position;
        dataBuffer.Write(hMove);
        Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER, dataBuffer);

    }


    // Update is called once per frame
    void Update()
    {
        //Valida para parar ou movimentar o char em outros scripts
        if (!validateMove) return;


        FollowClickedSkill();
        MoveChar();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rpc = !rpc;
        }




    }
    void MoveChar()
    {
        if (!moveClicked)
        {
            move = corretLag ? Vector3.zero : transform.forward * joystick.Vertical + transform.right * joystick.Horizontal;
            move.Normalize();
            //Existe essa condição, pois o transfrom.position não pode ser alterado direto caso o move for diferente de vector3.zero
            //isso existe para não bugar a correção de posição vinda do servidor
            
            if (corretLag) corretLag = false;

            if (Input.GetKey(KeyCode.A))
            {
                Rotate(-1 * speedRot);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Rotate(1 * speedRot);
            }

            Move(move);
        }

        //Se caso houver um alvo clicado, ele fica olhando para onde o player se move;
        if (joystick.Vertical != 0)
        {
            RotateMyChar(move);
            moveClicked = false;
            rotateCharClicked = false;
        }
        else if (joystick.Vertical == 0 && rotateCharClicked)
        {
            Vector3 dirCharClicked = identityCharClicked.transform.position - transform.position;
            RotateMyChar(dirCharClicked);
        }
    }
    void FollowClickedSkill()
    {
        //Algoritmo para fazer o player seguir após soltar uma skill que não tenha a distancia suficiente
        if (joystick.Vertical == 0 && joystick.Horizontal == 0 && moveClicked)
        {
            MoveToChar(identityCharClicked.transform.position, distanceCharClicked, fnSkill, ref moveClicked);
            Vector3 dirCharClicked = identityCharClicked.transform.position - transform.position;
            RotateMyChar(dirCharClicked);
            return;
        }
    }
    public override void OnTick(ITickInfo data)
    {
        MovePlayerRPC();
    }
    //RPC MOVIMENTO
    void MovePlayerRPC()
    {
        if (rpc) return;
        //Só envia quando altera o estado de movimento
        using DataBuffer dataBuffer = NetworkManager.Pool.Rent();
        if (IsMoving)
        {
            movePlayer = true;
            hMove = transform.position;
            dataBuffer.Write(hMove);
            Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER, dataBuffer, DeliveryMode.Sequenced, sequenceChannel: 1);
        }
        else if (!IsMoving && movePlayer)
        {
            movePlayer = false;
            hMove = transform.position;
            dataBuffer.Write(hMove);
            Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER_STOP, dataBuffer);
        }
    }

    public void MoveToClickedChar(NetworkIdentity idCharClicked, float distance, UnityAction funcSkill)
    {
        distanceCharClicked = distance;
        identityCharClicked = idCharClicked;
        fnSkill = funcSkill;
        rotateCharClicked = true;
        moveClicked = true;
    }

    public void RotateToClicked(NetworkIdentity identity)
    {
        identityCharClicked = identity;
        rotateCharClicked = true;
    }
    public void SetMoveValidate(bool validaMove)
    {
        validateMove = validaMove;
    }

    [Client(ConstantsRPC.MOVIMENT_PLAYER_CORRECT_POSITION)]
    void CorrectPositionRPC(DataBuffer buffer)
    {
        Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER_STOP, buffer);
        HalfVector3 correctPosition = buffer.Read<HalfVector3>();
        corretLag = true;
        transform.position = correctPosition;

    }
    void RotateMyChar(Vector3 dir)
    {
        Vector3 rot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        if(rot.y == 0 || rot == Vector3.zero) return;
        rot.x = 0;
        character.rotation = Quaternion.Euler(rot);
    }
}
