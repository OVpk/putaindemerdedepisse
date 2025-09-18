using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerID
    {
        Player1,
        Player2
    }

    public PlayerID playerId;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode hold;

    private void Awake()
    {
        peeController.playerID = playerId;
    }

    private void Start()
    {
        switch (playerId)
        {
            case PlayerID.Player1 :
                up = KeyCode.W;
                down = KeyCode.S;
                left = KeyCode.A;
                right = KeyCode.D;
                hold = KeyCode.T;
                break;
            case PlayerID.Player2 :
                up = KeyCode.UpArrow;
                down = KeyCode.DownArrow;
                left = KeyCode.LeftArrow;
                right = KeyCode.RightArrow;
                hold = KeyCode.Y;
                break;
        }
    }

    public bool canUseControls = true;
    public PeeController peeController;


    private void Update()
    {
        if (!canUseControls) return;
        
        if (GameManager.Instance.currentGameMode == GameManager.GameMode.InGame)
        {
            if (Input.GetKey(up))
            {
                peeController.MoveVertically(1);
            }
            else if (Input.GetKey(down))
            {
                peeController.MoveVertically(-1);
            }
        
            if (Input.GetKey(right))
            {
                peeController.RotateHorizontally(1);
            }
            else if (Input.GetKey(left))
            {
                peeController.RotateHorizontally(-1);
            }

            if (Input.GetKeyDown(hold))
            {
                peeController.StopPee();
            }
            if (Input.GetKeyUp(hold))
            {
                peeController.RestartPee();
            }
        }

        if (GameManager.Instance.currentGameMode == GameManager.GameMode.SelectCharacter)
        {
            if (Input.GetKey(up) && Input.GetKey(left))
            {
                GameManager.Instance.selectCharacter.AssignerCouleur(SelectCharacter.CouleurEnum.Violet, playerId);
            }
            if (Input.GetKey(up) && Input.GetKey(right))
            {
                GameManager.Instance.selectCharacter.AssignerCouleur(SelectCharacter.CouleurEnum.Vert, playerId);
            }
            if (Input.GetKey(down) && Input.GetKey(left))
            {
                GameManager.Instance.selectCharacter.AssignerCouleur(SelectCharacter.CouleurEnum.Bleu, playerId);
            }
            if (Input.GetKey(down) && Input.GetKey(right))
            {
                GameManager.Instance.selectCharacter.AssignerCouleur(SelectCharacter.CouleurEnum.Rouge, playerId);
            }

            if (Input.GetKeyDown(hold))
            {
                GameManager.Instance.selectCharacter.SelectionerCouleur(playerId);
            }
        }
        
    }
}
