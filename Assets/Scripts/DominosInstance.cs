using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DominosInstance : MonoBehaviour
{
    private int lado1, lado2;
    [SerializeField]
    private TextMeshProUGUI tmPro;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Val(int l1, int l2){
        lado1 = l1;
        lado2 = l2;

        tmPro.text = lado1+" "+lado2;
    }
}
