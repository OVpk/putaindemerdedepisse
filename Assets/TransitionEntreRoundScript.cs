using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEntreRoundScript : MonoBehaviour
{
    public TMP_Text scoreP1;
    public TMP_Text scoreP2;
    public TMP_Text RoundP1;
    public TMP_Text RoundP2;
    public Image bonhommeP1;
    public Image bonhommeP2;
    public TMP_Text numeroRound;

    public void LoadDatas(int score1, int score2, int nbRoundP1, int nbRoundP2, Sprite bonhomme1, Sprite bonhomme2, int leRound)
    {
        scoreP1.text = score1.ToString();
        scoreP2.text = score2.ToString();
        RoundP1.text = nbRoundP1.ToString();
        RoundP2.text = nbRoundP2.ToString();
        bonhommeP1.sprite = bonhomme1;
        bonhommeP2.sprite = bonhomme2;
        numeroRound.text = leRound.ToString();
    }

    public Animator animator;

}
