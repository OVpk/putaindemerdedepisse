using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremblementTerre : GameEvent
{
    public override void StartEvent()
    {
        CameraShake.Instance.StartShake();
        GameManager.Instance.player1.peeController.peeGenerator.forwardForce = 2f;
        GameManager.Instance.player2.peeController.peeGenerator.forwardForce = 2f;
    }

    public override void StopEvent()
    {
        CameraShake.Instance.StopShake();
        GameManager.Instance.player1.peeController.peeGenerator.forwardForce = 3f;
        GameManager.Instance.player2.peeController.peeGenerator.forwardForce = 3f;
    }
}
