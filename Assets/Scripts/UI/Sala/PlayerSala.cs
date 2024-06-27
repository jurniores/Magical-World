using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Omni.Core;
public class PlayerSala : MonoBehaviour
{
    [SerializeField]
    private Image imgPlayer, rune1, rune2, rune3;
    [SerializeField]
    private TextMeshProUGUI namePlayerPro;
    [SerializeField]
    private Image backGroundColorPlayer;
    [SerializeField]
    private float timeTotal, timeTotalBack;
    private GameManager gManager;
    private UsersModel user;
    private Animator anim;

    private bool click = false, animBack = false;
    private float time = 0, timeBack = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        gManager = NetworkService.Get<GameManager>("GameManager");
        if (gManager.User.hwid == user.hwid)
        {
            backGroundColorPlayer.color = new Color32(5, 101, 0, 164);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (click)
        {
            time += Time.deltaTime;
            if (time > timeTotal)
            {
                OnExit();
                AbreAnim();
            }
        }
        if (animBack)
        {
            timeBack += Time.deltaTime;

            if (timeBack > timeTotalBack)
            {
                OnExit();
                FechaAnim();
            }
        }
    }
    public void SetInfoPlayer(UsersModel user)
    {
        string namePlayer;
        this.user = user;
        if (user.name == null)
        {
            namePlayer = "Anonymous";
        }
        else
        {
            namePlayer = user.name;

        }
        namePlayerPro.text = namePlayer;

    }


    public void OnClick()
    {
        click = true;
    }

    public void OnExit()
    {
        click = false;
        time = 0;
    }

    void AbreAnim()
    {
        anim.Play("BtnKikarAnim");
        animBack = true;
    }
    void FechaAnim()
    {
        anim.Play("BtnKikarAnimBack");
        timeBack = 0;
        animBack = false;
    }

    public void Kikar(){
        print("Kikado");
    }
}
