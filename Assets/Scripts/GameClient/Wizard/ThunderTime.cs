using UnityEngine;

public class ThunderTime : MonoBehaviour
{
    Vector3 posEnemy;

    void Start()
    {

        transform.position = posEnemy;
    }
    public void SetInfoThunderTime(Vector3 posEnemyP)
    {
        posEnemyP.y += 2f;
        posEnemy = posEnemyP;
    }
}
