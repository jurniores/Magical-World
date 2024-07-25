using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using TMPro;
public class CanvasPing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pingPro, pingSntp, timeSntp;
    void Start()
    {

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.IsClientActive) return;
        pingPro.text = "Ping: " + NetworkManager.LocalPeer.Ping.ToString();
        pingSntp.text = "Ping Sntp: " + NetworkManager.SNTP.Client.Ping;
        timeSntp.text = "Time Sntp: " + NetworkManager.SNTP.Client.Time;
    }
}
