using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoupureCourant : GameEvent
{
    public Animator animator;
    
    public override void StartEvent()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("go");
    }

    public override void StopEvent()
    {
        gameObject.SetActive(false);
    }
}
