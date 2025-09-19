using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasseDEau : GameEvent
{
    public GameObject chasseDEau;
    
    public override void StartEvent()
    {
        chasseDEau.SetActive(true);
    }

    public override void StopEvent()
    {
        chasseDEau.SetActive(false);
    }
}
