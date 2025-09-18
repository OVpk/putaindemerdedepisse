using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunette : GameEvent
{
    public override void StartEvent()
    {
        gameObject.SetActive(true);
    }

    public override void StopEvent()
    {
        gameObject.SetActive(false);
    }
}
