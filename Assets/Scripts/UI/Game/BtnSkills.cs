using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using Omni.Threading.Tasks;
public class BtnSkills : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timePro;
    [SerializeField]
    private Image imgFill, skillImage, imgColor1, imgColor2;
    private Button myButton;
    private float time, timeText = 10, timeTotal = 10;
    private bool skillActivated = false, btnSelected = false;
    void Start()
    {
        myButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (time < timeTotal && skillActivated)
        {
            time += Time.deltaTime;
            imgFill.fillAmount = time / timeTotal;
            timeText -= Time.deltaTime;
            timePro.text = Mathf.RoundToInt(timeText).ToString();
            if (time > timeTotal)
            {
                skillActivated = false;
                timePro.gameObject.SetActive(false);
                myButton.interactable = true;
            }
        }
    }

    public void SetInfo(Sprite sprite)
    {
        skillImage.sprite = sprite;
    }
    public void SetColor(Color color1, Color color2)
    {
        imgColor1.color = color1;
        imgColor2.color = color2;
    }
    public void SetSkill(UnityAction characterSkill)
    {
        myButton.onClick.AddListener(characterSkill);
    }
    public void ActiveSkill(float timeParam)
    {
        
        timeText = timeTotal = timeParam;
        
        myButton.interactable = false;
        imgFill.fillAmount = 0;
        time = 0;
        timeText = timeTotal;
        skillActivated = true;
        timePro.gameObject.SetActive(true);
    }
}
