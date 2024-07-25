using Omni.Core;
using UnityEngine;
using Cinemachine;
public class MovePlayer : MovimentClient
{
    [SerializeField]
    private Transform character;
    [SerializeField]
    private float speedRot;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cam;
    private Joystick joystick;
    private Vector3 move = Vector3.zero;
    private bool corretLag = false;
    HalfVector3 hMove;
    bool movePlayer = true;
    private bool rpc = false;

    protected override void OnStart()
    {

        joystick = FindAnyObjectByType<Joystick>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;

        using DataBuffer dataBuffer = NetworkManager.Pool.Rent();
        Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER, dataBuffer);

    }


    // Update is called once per frame
    void Update()
    {
        //Existe essa condição, pois o transfrom.position não pode ser alterado direto caso o move for diferente de vector3.zero
        //isso existe para não bugar a correção de posição vinda do servidor
        move = corretLag ? Vector3.zero : transform.forward * joystick.Vertical + transform.right * joystick.Horizontal;

        if (corretLag) corretLag = false;
        if (joystick.Vertical != 0)
        {
            character.rotation = Quaternion.LookRotation(move, Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rpc = !rpc;
        }

        Move(move);

        if (Input.GetKey(KeyCode.A))
        {
            Rotate(-1 * speedRot);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(1 * speedRot);
        }
    }


    public override void OnTick(ITickInfo data)
    {
        MovePlayerRPC();
    }
    //RPC MOVIMENTO
    void MovePlayerRPC()
    {
        if(rpc) return;
        //Só envia quando altera o estado de movimento
        using DataBuffer dataBuffer = NetworkManager.Pool.Rent();
        if (IsMoving)
        {
            movePlayer = true;
            hMove = transform.position;
            dataBuffer.FastWrite(hMove);
            Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER, dataBuffer, DeliveryMode.Sequenced, sequenceChannel: 1);
        }
        else if (!IsMoving && movePlayer)
        {
            movePlayer = false;
            hMove = transform.position;
            dataBuffer.FastWrite(hMove);
            Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER_STOP, dataBuffer);
        }
    }

    [Client(ConstantsRPC.MOVIMENT_PLAYER_CORRECT_POSITION)]
    void CorrectPositionRPC(DataBuffer buffer)
    {
        Local.Invoke(ConstantsRPC.MOVIMENT_PLAYER_STOP, buffer);
        HalfVector3 correctPosition = buffer.Read<HalfVector3>();
        corretLag = true;
        transform.position = correctPosition;

    }

}
