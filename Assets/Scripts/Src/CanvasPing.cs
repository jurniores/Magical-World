using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using TMPro;
using Omni.Threading.Tasks;
using Omni.Shared;
public class CanvasPing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pingPro, pingSntp, timeSntp;
    public static BandwidthMonitor bandwidthMonitor = new();
    void Start()
    {

        DontDestroyOnLoad(gameObject);
        bandwidthMonitor.Start();
        bandwidthMonitor.OnAverageChanged += (avg) => timeSntp.text = "Band: " + avg.ToString();




    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.IsClientActive) return;
        pingPro.text = "Ping: " + NetworkManager.LocalPeer.Ping.ToString();
        //pingSntp.text = "Ping Sntp: " + NetworkManager.SNTP.Client.Ping;


    }


}
