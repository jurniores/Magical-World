using System.Collections;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using UnityEngine;

public class ThunderBird : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;
    [SerializeField]
    private Transform livroPos;
    private Transform posEnemy;
    void Update()
    {
        if(!posEnemy) return;
        line.SetPosition(0,livroPos.position);
        line.SetPosition(1, posEnemy.position);
    }

    public async void SetPosThunder(Transform posEnemyP)
    {
        gameObject.SetActive(true);
        posEnemy = posEnemyP;
        await UniTask.WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
