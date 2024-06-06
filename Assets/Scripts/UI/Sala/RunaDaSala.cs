using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunaDaSala : MonoBehaviour
{
    [SerializeField]
    private Image imgRune;
    private Button myBtn;
    private Color originalColor;
    private bool activated = true;
    void Start()
    {
        originalColor = imgRune.color;
        myBtn = GetComponentInChildren<Button>();
        myBtn.onClick.AddListener(ClickMe);
    }

    public void SetRune(ScriptableItens item)
    {
        imgRune.sprite = item.sprite;
    }

    void ClickMe()
    {
        if (activated)
        {
            imgRune.color = new Color32(121,121,121,255);
            //imgRune.color = Color.gray;

        }
        else { imgRune.color = originalColor; }

        activated = !activated;


    }


}
