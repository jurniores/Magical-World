using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainThunder : MonoBehaviour
{
    public void SetPosRain(Vector3 pos){
        pos.y += 14.90f;
        transform.position = pos;
    }
}
