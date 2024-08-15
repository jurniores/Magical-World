using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Omni.Core;
using Omni.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : NetworkBehaviour
{
   public abstract void SetDemage(float dano);
   public abstract void SetShield(float dano);

}
