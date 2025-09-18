using System;
using UnityEngine;

public class LaMouche : GameEvent
{
    public ZigZagMove movementController;
    
    public override void StartEvent()
    {
        movementController.StartMove();
    }

    public override void StopEvent()
    {
        movementController.StopMove();
    }
}
