using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScorePool : MonoBehaviour
{
    [Header("Prefab et pool")]
    public GameObject floatingScorePrefab;
    public RectTransform parentCanvas;
    public int poolSize = 20;

    private Queue<FloatingScore> pool = new Queue<FloatingScore>();

    void Awake()
    {
        // Crée la pool initiale
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fxObj = Instantiate(floatingScorePrefab, parentCanvas);
            fxObj.SetActive(false);
            FloatingScore fx = fxObj.GetComponent<FloatingScore>();
            fx.pool = this; // référence pour retour à la pool
            pool.Enqueue(fx);
        }
    }
    
    private List<FloatingScore> activeFX = new List<FloatingScore>();

    public void Spawn(Vector3 screenPos, int score)
    {
        if (score == 0) return;

        FloatingScore fx;

        if (pool.Count > 0)
        {
            // On prend un FX disponible dans la pool
            fx = pool.Dequeue();
            fx.gameObject.SetActive(true);
        }
        else
        {
            return;
        }

        fx.Initialize(screenPos, score);
        activeFX.Add(fx); // ajoute aux actifs
    }


    public void ReturnToPool(FloatingScore fx)
    {
        fx.gameObject.SetActive(false);
        pool.Enqueue(fx);
    }

}