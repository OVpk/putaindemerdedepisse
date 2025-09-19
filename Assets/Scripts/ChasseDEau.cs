using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasseDEau : GameEvent
{
    public GameObject chasseDEau;
    public Animator animator;
    
    public override void StartEvent()
    {
        chasseDEau.SetActive(true);
        animator.SetTrigger("ON");
    }

    public override void StopEvent()
    {
        StartCoroutine(Gone());
    }

    public AnimationClip gonee;

    public IEnumerator Gone()
    {
        animator.SetTrigger("OFF");
        yield return new WaitForSeconds(gonee.length);
        chasseDEau.SetActive(false);
    }
}
