using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public class BotServer : BaseMoveBots
{

    [SerializeField]
    private float TimeWalk, TimeWalk2;
    private Transform posInitial;
    [SerializeField]
    private Transform move1, move2;
    private Vector3 dir = Vector3.zero, objDistance;
    int groupID = 0;

    public int GroupID { get => groupID; private set => groupID = value; }

    void Start()
    {
        posInitial = NetworkService.GetAsComponent<Transform>("initPos");
        move1 = NetworkService.GetAsComponent<Transform>("move1");
        move2 = NetworkService.GetAsComponent<Transform>("move2");
        transform.position = posInitial.position;
        WalkEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(dir == Vector3.zero) return;
        dir = objDistance - transform.position;

        dir.Normalize();
        float distance = Vector3.Distance(objDistance, transform.position);

        if (distance > 0.5f)
        {
            Move(dir);
        }
        else if (distance > 9)
        {
            transform.position = objDistance;
        }
        else if (distance < 0.5f && IsMoving)
        {
            IsMoving = false;
            dir = objDistance;
            using var buffer = NetworkManager.Pool.Rent();
            HalfVector3 hMove = transform.position;
            buffer.Write(hMove);
        }

    }

    async void WalkEnemy()
    {
        //
        float randWalk = Random.Range(TimeWalk, TimeWalk2);
        await UniTask.WaitForSeconds(randWalk);
        if (this == null) return;
        speed = 1;
        dir.x = Random.Range(move1.position.x, move2.position.x);
        dir.z = Random.Range(move1.position.z, move2.position.z);

        objDistance = dir;
        objDistance.y = transform.position.y;
        IsMoving = true;

        var buffer = NetworkManager.Pool.Rent();
        HalfVector3 hMove = objDistance;
        buffer.Write(hMove);
        Remote.Invoke(ConstantsRPC.BOT_WALK, buffer, groupId: GroupID);
        buffer.Dispose();

        WalkEnemy();
    }

    public void Group(int groupId)
    {
        GroupID = groupId;
    }

}
