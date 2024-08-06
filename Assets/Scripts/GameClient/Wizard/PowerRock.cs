using System.Collections;
using System.Collections.Generic;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class PowerRock : MonoBehaviour
{
    [SerializeField]
    private GameObject particleDestroy;
    [SerializeField]
    private Transform objCristal;
    [SerializeField]
    private float vel = 10;
    private Vector3 dir;
    private bool run = false;
    private Vector3 rotateExplos = new Vector3(-90, 0, 0);
    async void Start()
    {
        dir = transform.position - objCristal.transform.position;
        await UniTask.WaitForSeconds(2);
        run = true;
    }
    void Update()
    {
        if (!run) return;

        if (Vector3.Distance(transform.position, objCristal.transform.position) < 0.5f)
        {
            Destroy(Instantiate(particleDestroy, transform.position, Quaternion.Euler(rotateExplos)), 5);
            run = !run;
            Destroy(gameObject, 4);
            Destroy(objCristal.gameObject);

        }
        else
        {
            objCristal.Translate(Time.deltaTime * vel * dir);
        }
    }

    public void SetInfoPowerRock(Vector3 posEnemyP)
    {
        posEnemyP.y -= 1f;
        transform.position = posEnemyP;
    }
}
