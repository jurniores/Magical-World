using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;
using static Omni.Core.HttpLite;
public class RunaDaSala : MonoBehaviour
{
    [SerializeField]
    private Image imgRune;
    private InstantiateRunes instRunes;
    private Button myBtn;
    private Color originalColor;
    private ScriptableItens itemRune;
    void Start()
    {
        instRunes = NetworkService.Get<InstantiateRunes>();
        originalColor = imgRune.color;
        myBtn = GetComponentInChildren<Button>();
        myBtn.onClick.AddListener(ClickMe);
    }

    public void SetRune(ScriptableItens item)
    {
        itemRune = item;
        imgRune.sprite = item.sprite;
    }
    public void BlockMe()
    {
        imgRune.color = originalColor;
        myBtn.interactable = true;
    }
    public void ClickMe()
    {
        if(!Debounce.Bounce(0.5f)) return;
        //Escolher a runa que vai usar
        Fetch.Post("/room/set/rune", req =>
        {
            req.FastWrite(itemRune.id);
        }, 
        res =>
        {
            //Setar a runa em algum lugar para depois verificar a quantidade de runas e desativá-la
            instRunes.ValidateRune(this);
            myBtn.interactable = false;
        });
    }
}
