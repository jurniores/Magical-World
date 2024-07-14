using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Omni.Core;
using static Omni.Core.HttpLite;
public class PlayerSala : MonoBehaviour
{
    [SerializeField]
    private Image imgPlayer;
    [SerializeField]
    private List<Image> runesImg;

    [SerializeField]
    private TextMeshProUGUI namePlayerPro;
    [SerializeField]
    private Image backGroundColorPlayer, masterImg;
    [SerializeField]
    private float timeTotal, timeTotalBack;
    private GameManager gManager;
    private UsersModel user;
    private Animator anim;

    private bool click = false, animBack = false;
    private float time = 0, timeBack = 0;
    [SerializeField]
    private bool ImMaster = false;
    private InstantiateRunes instRunes;
    private readonly int indexRune = 1000;
    void Start()
    {
        anim = GetComponent<Animator>();

        gManager = NetworkService.Get<GameManager>();
        instRunes = NetworkService.Get<InstantiateRunes>();

        if (gManager.User.peerId == user.peerId)
        {
            backGroundColorPlayer.color = new Color32(5, 101, 0, 164);
        }
        IsMaster();
        SetRunes(user.pConfig);
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
        this.user = user;

        namePlayerPro.text = user.name;
    }
    public void SetRunes(PlayerConfigs pConfigs)
    {
        int count = 0;
        pConfigs.listRunes.ForEach(rune =>
        {
            if (rune > 0)
            {
                int index = rune - indexRune;
                runesImg[count].color = Color.white;
                runesImg[count].sprite = instRunes.runes[index].sprite;
                count++;
            }
        });
    }
    public void IsMaster()
    {
        if (gManager.salaGame.master == user.peerId)
        {
            masterImg.gameObject.SetActive(true);
            ImMaster = true;
        }
        else
        {
            ImMaster = false;
            masterImg.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        if (gManager.salaGame.master != gManager.User.peerId || ImMaster) return;
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
    public void MeDestroy(UsersModel user)
    {
        if (user.peerId == this.user.peerId)
        {
            Destroy(gameObject);
        }
    }
    public void Kikar()
    {
        if (!Debounce.Bounce(2)) return;
        Fetch.Post("/room/kick",
        req =>
        {
            req.ToJson(user);
            req.Send();
        },
        res =>
        {
            var response = res.FromJson<NetworkResponse>();
            ErrorManager.ValidateError(response, 1);
        });
    }
}
