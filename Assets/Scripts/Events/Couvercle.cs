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
        StartCoroutine(OuvreCouvercle());
    }

    public AnimationClip couvercleUp;

    public IEnumerator OuvreCouvercle()
    {
        animator.SetTrigger("up");
        yield return new WaitForSeconds(couvercleUp.length);
        gameObject.SetActive(false);
    }
}
