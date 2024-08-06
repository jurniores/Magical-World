using UnityEngine;

public class MagicBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject particleDano;
    private Vector3 dir;
    private Transform followPos;

    [SerializeField]
    private float vel = 1;
    void Update()
    {
        dir = followPos.position - transform.position;
        dir.Normalize();
        if (Vector3.Distance(followPos.position, transform.position) < 1)
        {
            Transform tP = Instantiate(particleDano).transform;
            tP.position = followPos.position;
            Destroy(tP.gameObject, 2);
            Destroy(gameObject);
        }
        transform.Translate(dir * Time.deltaTime * vel);
    }

    public void SetDirection(Vector3 charPos, Transform enemyPos)
    {
        followPos = enemyPos;
        transform.position = charPos;
    }
}
