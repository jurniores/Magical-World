using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CowntdownFilled : MonoBehaviour
{
    [SerializeField]
    private Image filledImg;
    private float time = 0, timeTotal = 0;

    // Update is called once per frame
    void Update()
    {
        if (time <= timeTotal)
        {
            time += Time.deltaTime;
            filledImg.fillAmount = time / timeTotal;

            if (time >= timeTotal) ActiveSkill();

        }
    }
    public void SetCowntDown(float timeP)
    {
        gameObject.SetActive(true);
        timeTotal = timeP;
        time = 0;
        filledImg.fillAmount = 0;
    }

    private void ActiveSkill()
    {
        gameObject.SetActive(false);
    }


}
