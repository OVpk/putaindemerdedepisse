using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunette : GameEvent
{
    public Animator animator;
    
    public override void StartEvent()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("down");
    }

    public override void StopEvent()
    {
        StartCoroutine(OuvreLunette());
    }

    public AnimationClip lunetteUp;

    public IEnumerator OuvreLunette()
    {
        animator.SetTrigger("up");
        yield return new WaitForSeconds(lunetteUp.length);
        gameObject.SetActive(false);
    }
}
