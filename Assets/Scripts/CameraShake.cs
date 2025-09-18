using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
    private Transform camTransform;
    private Vector3 originalPos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Paramètres du shake")]
    public float shakeMagnitude = 0.2f; // intensité
    public bool isShaking = false;      // on/off

    void Start()
    {
        camTransform = transform;
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
        }
        else
        {
            camTransform.localPosition = originalPos;
        }
    }

    // Active le shake
    public void StartShake()
    {
        isShaking = true;
    }

    // Désactive le shake
    public void StopShake()
    {
        isShaking = false;
    }
}