using System;
using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using static Omni.Core.HttpLite;

public class Debounce : MonoBehaviour
{
    public static Debounce instance;
    Action Onfn;
    private bool bounce = true;

    void Start()
    {
        instance = this;
    }

    public bool Bounce(float time)
    {
        if (bounce)
        {
            bounce = false;
            Invoke(nameof(ActivatedBounce), time);
            return true;
        }
        else
        {
            return false;
        }
    }

    void ActivatedBounce()
    {
        bounce = true;
    }
}
