using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couvercle : GameEvent
{
    public Animator animator;
    
    public override void StartEvent()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("down");
    }

    public override void StopEvent()
    {
        gameObject.SetActive(false);
        animator.SetTrigger("up");
    }
}
