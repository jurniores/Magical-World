using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Omni.Threading.Tasks;
using UnityEngine;

public class InverseField : MonoBehaviour
{
    [SerializeField]
    Vector3 pos;
    [SerializeField]
    private float duration, destroyTime = 6;
    void Update()
    {
        transform.DOScale(pos, duration);
    }

    public async void SetInfoField(Transform pos, float timeSkill)
    {
        transform.position = pos.position;
        transform.SetParent(pos);
        duration = 20;
        await UniTask.WaitForSeconds(timeSkill);
        duration = 0.5f;
        Destroy(gameObject, destroyTime);
        
    }



    void OnTriggerEnter(Collider other)
    {
        print("Dano em: " + other.name);
    }
}
