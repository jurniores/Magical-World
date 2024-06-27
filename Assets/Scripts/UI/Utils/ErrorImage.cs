using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorImage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI errorSuccessPro;

    [SerializeField]
    private Image fil, colorImage, fade;

    [SerializeField]
    private Animator animES;
    private float timeTotal;
    public float timeError = 0;
    private bool back = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeError >= 0)
        {
            timeError -= Time.deltaTime;
            fil.fillAmount = timeError / timeTotal;
        }
        else if (back && timeError <= 0)
        {
            back = false;
            animES.Play("animAvisosBack");
            Invoke(nameof(DisableAnim), 0.416f);
        }
    }

    void DisableAnim()
    {
        gameObject.SetActive(false);
    }

    public void SetMsg(bool success, string msg, float time = 1)
    {
        if (success)
        {
            colorImage.color = new Color32(6, 118, 38, 210);
            fade.color = new Color32(83, 224, 88, 255);
        }
        else
        {
            colorImage.color = new Color32(150, 10, 1, 210);
            fade.color = new Color32(224, 96, 83, 255);
        }
        back = true;
        timeError = timeTotal = time;
        errorSuccessPro.text = msg;
        fil.fillAmount = 1;
        gameObject.SetActive(true);
    }
}
