using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasseDEau : GameEvent
{
    public GameObject water;
    
    public override void StartEvent()
    {
        water.transform.localScale *= 2;
    }

    public override void StopEvent()
    {
        water.transform.localScale /= 2;
    }
}
