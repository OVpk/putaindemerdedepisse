using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    public enum CouleurEnum
    {
        Violet,
        Vert,
        Rouge,
        Bleu
    }

    public List<GameObject> player1Repere = new List<GameObject>();
    public List<GameObject> player2Repere = new List<GameObject>();
    
    private CouleurEnum choixJoueur1 = CouleurEnum.Violet;
    private CouleurEnum choixJoueur2 = CouleurEnum.Vert;

    private bool joueur1Ready = false;
    private bool joueur2Ready = false;

    public Image player1Img;
    public Image player2Img;

    public List<Sprite> aliens;
    public List<Sprite> aliensVERSUS;
    public List<Sprite> aliensSILOUHETE;
    public List<Sprite> aliensNamesSpritesDROITE;
    public List<Sprite> aliensNamesSpritesGAUCHE;
    
    public Image name1Img;
    public Image name2Img;
    
    public TMP_Text chronoText;
    public int chronoStartValue = 5;
    private int currentChrono;
    private Coroutine chronoRoutine;

    public List<AudioClip> selectedSongs;

    public void StartChrono()
    {
        if (chronoRoutine != null) StopCoroutine(chronoRoutine);
        chronoRoutine = StartCoroutine(ChronoRoutine());
    }

    private IEnumerator ChronoRoutine()
    {
        currentChrono = chronoStartValue;

        while (currentChrono > 0)
        {
            chronoText.text = currentChrono.ToString();
            yield return new WaitForSeconds(1f);
            currentChrono--;
        }

        chronoText.text = "0";
        StartCoroutine(BeginGame());
    }
    
    public Image player1VERSUSImg;
    public Image player2VERSUSImg;

    public Image silouhetePlayer1;
    public Image silouhetePlayer2;

    public void ShowRepere(CouleurEnum couleur, PlayerController.PlayerID playerId)
    {
        if (playerId == PlayerController.PlayerID.Player1)
        {
            player1Repere[0].SetActive(false);
            player1Repere[1].SetActive(false);
            player1Repere[2].SetActive(false);
            player1Repere[3].SetActive(false);
            player1Repere[(int)couleur].SetActive(true);

            player1Img.sprite = aliens[(int)couleur];
            player1VERSUSImg.sprite = aliensVERSUS[(int)couleur];
            silouhetePlayer1.sprite = aliensSILOUHETE[(int)couleur];
            name1Img.sprite = aliensNamesSpritesGAUCHE[(int)couleur];
        }
        
        if (playerId == PlayerController.PlayerID.Player2)
        {
            player2Repere[0].SetActive(false);
            player2Repere[1].SetActive(false);
            player2Repere[2].SetActive(false);
            player2Repere[3].SetActive(false);
            player2Repere[(int)couleur].SetActive(true);
            
            player2Img.sprite = aliens[(int)couleur];
            player2VERSUSImg.sprite = aliensVERSUS[(int)couleur];
            silouhetePlayer2.sprite = aliensSILOUHETE[(int)couleur];
            name2Img.sprite = aliensNamesSpritesDROITE[(int)couleur];
        }
    }
    
    public void AssignerCouleur(CouleurEnum couleur, PlayerController.PlayerID playerId)
    {
        if (choixJoueur1 == couleur || choixJoueur2 == couleur) return;
        
        if (playerId == PlayerController.PlayerID.Player1 && joueur1Ready) return;
        if (playerId == PlayerController.PlayerID.Player2 && joueur2Ready) return;
        

        if (playerId == PlayerController.PlayerID.Player1)
        {
            choixJoueur1 = couleur;
        }
        
        if (playerId == PlayerController.PlayerID.Player2)
        {
            choixJoueur2 = couleur;
        }
        
        ShowRepere(couleur, playerId);
    }

    public void SelectionerCouleur(PlayerController.PlayerID playerId)
    {
        switch (playerId)
        {
            case PlayerController.PlayerID.Player1 :
                joueur1Ready = true;
                SoundManager.Instance.PlaySFX(selectedSongs[(int)choixJoueur1]);
                break;
            case PlayerController.PlayerID.Player2 :
                joueur2Ready = true;
                SoundManager.Instance.PlaySFX(selectedSongs[(int)choixJoueur2]);
                break;
        }
        
        if (GameMustBegin()) StartCoroutine(BeginGame());
    }

    public bool GameMustBegin() => joueur1Ready && joueur2Ready;

    public Animator animDeTransition;

    public IEnumerator BeginGame()
    {
        animDeTransition.SetTrigger("StartAnim");
        yield return new WaitForSeconds(10f);
        
        GameManager.Instance.InitSpecificAlienThings(choixJoueur1, PlayerController.PlayerID.Player1);
        GameManager.Instance.InitSpecificAlienThings(choixJoueur2, PlayerController.PlayerID.Player2);
        GameManager.Instance.GoGame();
        GameManager.Instance.currentGameMode = GameManager.GameMode.InGame;
        Destroy(this.gameObject);
    }
}
