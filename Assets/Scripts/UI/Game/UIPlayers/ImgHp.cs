using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;

public class ImgHp : MonoBehaviour
{
    [SerializeField]
    private Image imgFill1, imgFill2;
    [SerializeField]
    private float velHp = 1;
    private Camera cam;
    [SerializeField]
    private float totalHp, atualHp, insertDownTime = 0.3f;
    private float downTimeFilled = 0;
    void Start()
    {
        cam = NetworkService.GetAsComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);

        float nowHp = atualHp / totalHp;

        if (downTimeFilled <= 0)
        {
            if (imgFill1.fillAmount <= nowHp)
            {
                imgFill2.fillAmount = Mathf.MoveTowards(imgFill2.fillAmount, nowHp, velHp * Time.deltaTime);
            }
        }
        else
        {
            downTimeFilled -= Time.deltaTime;
        }



    }
    public void SetConfig(float hpInicial)
    {
        totalHp = atualHp = hpInicial;
    }
    public void SetHp(float hpNow)
    {
        downTimeFilled = insertDownTime;
        print("Hp agora: " + hpNow);
        atualHp = hpNow;
        print(atualHp / totalHp);
        imgFill1.fillAmount = atualHp / totalHp;
    }
}
