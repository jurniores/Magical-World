using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public class BotServer : BaseMoveBots
{

    [SerializeField]
    private float TimeWalk;
    [SerializeField]
    private Transform move1, move2;
    private Vector3 dir, objDistance;
    int groupID = 0;
    float distance;

    public int GroupID { get => groupID; private set => groupID = value; }

    void Start()
    {
        move1 = NetworkService.GetAsComponent<Transform>("move1");
        move2 = NetworkService.GetAsComponent<Transform>("move2");
        transform.position = move1.position;
        WalkEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        Move(dir);

        distance = Vector3.Distance(objDistance, transform.position);
        if (distance < 0.5f && IsMoving)
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
        speed = 1;
        dir.x = Random.Range(move1.position.x, move2.position.x);
        dir.z = Random.Range(move1.position.z, move2.position.z);

        objDistance = dir;
        objDistance.y = transform.position.y;
        dir -= transform.position;

        IsMoving = true;

        var buffer = NetworkManager.Pool.Rent();
        HalfVector3 hMove = objDistance;
        buffer.Write(hMove);
        Remote.Invoke(ConstantsRPC.BOT_WALK, buffer, groupId: GroupID);
        buffer.Dispose();
        //
        await UniTask.WaitForSeconds(TimeWalk);

        WalkEnemy();
    }

    public void Group(int groupId)
    {
        GroupID = groupId;
    }

}
