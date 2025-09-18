using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public float eventDuration;
    
    public IEnumerator EventRoutine()
    {
        float t = 0f;
        Debug.Log("   >>> Événement en cours...");
        StartEvent();
        while (t < eventDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        StopEvent();
        Debug.Log("   >>> Événement terminé.");
    }

    public abstract void StartEvent();
    
    public abstract void StopEvent();
}
