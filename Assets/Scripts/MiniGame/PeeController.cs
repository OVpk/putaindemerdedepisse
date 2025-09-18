using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeeController : MonoBehaviour
{
    [SerializeField] public PeePoolingSystem peeGenerator;
    
    [SerializeField] private float minUpwardForce = -1f;
    [SerializeField] private float maxUpwardForce = 0f;
    [SerializeField] private float minRotationY = 0f;
    [SerializeField] private float maxRotationY = 180f;
    [SerializeField] private float verticalSpeed = 0.01f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 45f;
    
    [SerializeField] private float rotationTarget1 = 55f;
    [SerializeField] private float rotationTarget2 = 125f;
    [SerializeField] private float targetUpwardForce = 1.9f;
    [SerializeField] private float destabilizationSpeed = 0.5f;
    [SerializeField] private float rotationChangeInterval = 5f;
    
    private float currentUpwardForce;
    private float currentRotationY;
    private float targetRotationY;
    private float rotationChangeTimer;

    public PlayerController.PlayerID playerID;

    private void Start()
    {
        Init();
    }

    public void RestartPee()
    {
        peeGenerator.StartGenerator();
    }

    public void StopPee()
    {
        peeGenerator.StopGenerator();
    }


    public void Init()
    {
        peeGenerator.playerID = this.playerID;
        currentUpwardForce = peeGenerator.upwardForce;
        currentRotationY = peeGenerator.transform.eulerAngles.y;
        
        targetRotationY = playerID switch
        {
            PlayerController.PlayerID.Player1 => rotationTarget1,
            PlayerController.PlayerID.Player2 => rotationTarget2,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        rotationChangeTimer = rotationChangeInterval;
    }

    public void MoveVertically(float direction)
    {
        currentUpwardForce = Mathf.Clamp(
            currentUpwardForce + direction * verticalSpeed * Time.deltaTime,
            minUpwardForce,
            maxUpwardForce
        );
    }
    
    public void RotateHorizontally(float direction)
    {
        currentRotationY = Mathf.Clamp(
            currentRotationY + direction * horizontalSpeed * Time.deltaTime,
            minRotationY,
            maxRotationY
        );
    }
    
    private void UpdateTargetRotation()
    {
        rotationChangeTimer -= Time.deltaTime;

        if (rotationChangeTimer <= 0f)
        {
            targetRotationY = Mathf.Approximately(targetRotationY, rotationTarget1) ? rotationTarget2 : rotationTarget1;

            rotationChangeTimer = rotationChangeInterval;
        }
    }


    [SerializeField] private bool isDestabilizationEnable;

    private void Update()
    {

        if (isDestabilizationEnable)
        {
            ApplyDestabilization();
            UpdateTargetRotation();
        }

        peeGenerator.upwardForce = Mathf.Lerp(
            peeGenerator.upwardForce,
            currentUpwardForce,
            smoothSpeed * Time.deltaTime
        );
        peeGenerator.transform.rotation = Quaternion.Lerp(
            peeGenerator.transform.rotation,
            Quaternion.Euler(0f, currentRotationY, 0f),
            smoothSpeed * Time.deltaTime
        );
        
    }
    
    private void ApplyDestabilization()
    {
        currentUpwardForce = Mathf.Lerp(
            currentUpwardForce,
            targetUpwardForce,
            destabilizationSpeed * Time.deltaTime
        );

        currentRotationY = Mathf.Lerp(
            currentRotationY,
            targetRotationY,
            destabilizationSpeed * Time.deltaTime
        );
    }
}
