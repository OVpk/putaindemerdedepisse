using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinematicController introCinematic;
    [SerializeField] private float gameDuration;
    [SerializeField] public PlayerController player1;
    [SerializeField] public PlayerController player2;
    
    public TMP_Text scoreTextP1;
    public TMP_Text scoreTextP2;
    public int scoreP1 = 0;
    public int scoreP2 = 0;
    
    private int winningRoundP1 = 0;
    private int winningRoundP2 = 0;
    
    public FloatingScorePool scorePoolP1;
    public FloatingScorePool scorePoolP2;

    public GameEvent[] activeEvents;

    public static GameManager Instance;

    public SelectCharacter selectCharacter;
    
    public enum GameMode
    {
        Title,
        SelectCharacter,
        InGame
    }

    public GameMode currentGameMode = GameMode.Title;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public float roundDuration = 45f;
    public float minWait = 2f;
    public float maxWait = 5f;

    public List<Sprite> alienIcons;
    public List<string> listNoms;
    public List<Sprite> peeColorsSprite;

    public Image logoJoueur1;
    public Image logoJoueur2;
    public TMP_Text nomJoueur1;
    public TMP_Text nomJoueur2;

    public Sprite spriteJ1;
    public Sprite spriteJ2;

    public SelectCharacter.CouleurEnum couleurP1;
    public SelectCharacter.CouleurEnum couleurP2;
    
    public void InitSpecificAlienThings(SelectCharacter.CouleurEnum couleur, PlayerController.PlayerID joueur)
    {
        if (joueur == PlayerController.PlayerID.Player1)
        {
            logoJoueur1.sprite = alienIcons[(int)couleur];
            nomJoueur1.text = listNoms[(int)couleur];
            player1.peeController.peeGenerator.spriteOfProjectile = peeColorsSprite[(int)couleur];
            spriteJ1 = aliens[(int)couleur];
            couleurP1 = couleur;
        }

        if (joueur == PlayerController.PlayerID.Player2)
        {
            logoJoueur2.sprite = alienIcons[(int)couleur];
            nomJoueur2.text = listNoms[(int)couleur];
            player2.peeController.peeGenerator.spriteOfProjectile = peeColorsSprite[(int)couleur];
            spriteJ2 = aliens[(int)couleur];
            couleurP2 = couleur;
        }
    }

    public GameObject winScreen;
    
    public AudioClip globalMusic;

    public void GoGame()
    {
        StartCoroutine(GameRoutine());
        SoundManager.Instance.PlayBackgroundMusic(globalMusic);
    }

    public GameObject redyPiss;

    public AnimationClip animRedyPiss;

    public IEnumerator GameRoutine()
    {
        //yield return introCinematic.PlayCinematic();
        Debug.Log("cinematic intro");
        
        for (int i = 1; i <= 3; i++)
        {
            
            redyPiss.SetActive(true);

            yield return new WaitForSeconds(animRedyPiss.length);
            
            redyPiss.SetActive(false);
            
            Debug.Log($"=== Début Round {i} ===");
            yield return RoundRoutine();
            Debug.Log($"=== Fin Round {i} ===");
            
            transitionEntreRoundScript.gameObject.SetActive(true);
            transitionEntreRoundScript.LoadDatas(scoreP1, scoreP2, winningRoundP1, winningRoundP2, spriteJ1, spriteJ2, i);
            transitionEntreRoundScript.animator.SetTrigger("Start");

            yield return new WaitForSeconds(5f);
            
            transitionEntreRoundScript.gameObject.SetActive(false);
            
            ClearPoints();
            
            if (winningRoundP1 == 2 || winningRoundP2 == 2)
                break;
        }
        
        Debug.Log("Partie terminée !");
        StartCoroutine(StopGame());
    }
    
    public List<Sprite> aliens;

    public TransitionEntreRoundScript transitionEntreRoundScript;
    
    

    public GameEvent currentEvent;

    IEnumerator RoundRoutine()
    {
        PauseGame(false);
        
        List<GameEvent> availableEvents = new List<GameEvent>(activeEvents);
        
        Coroutine thisRound = null;
        IEnumerator RoundBody()
        {
            while (true)
            {
                float waitTime = Random.Range(minWait, maxWait);
                Debug.Log("wait");
                yield return new WaitForSeconds(waitTime);

                if (availableEvents.Count > 0)
                {
                    currentEvent = ChoiceEvent(availableEvents);
                    Debug.Log("event");
                    yield return StartCoroutine(currentEvent.EventRoutine());
                    currentEvent = null;
                }
                else
                {
                    Debug.Log("no more event available");
                }
            }
        }

        thisRound = StartCoroutine(RoundBody());

        yield return StartCoroutine(TimerRoutine(roundDuration, thisRound));
        
        PauseGame(true);
        
        GivePointRound();
    }

    public void GivePointRound()
    {
        if (scoreP1 > scoreP2)
        {
            winningRoundP1++;
        }
        else
        {
            winningRoundP2++;
        }
    }

    public void ClearPoints()
    {
        scoreP1 = 0;
        scoreP2 = 0;
        scoreTextP1.text = scoreP1.ToString();
        scoreTextP2.text = scoreP2.ToString();
    }

    public GameEvent ChoiceEvent(List<GameEvent> list)
    {
        int index = Random.Range(0, list.Count);
        GameEvent chosen = list[index];
        list.RemoveAt(index);
        return chosen;
    }

    IEnumerator TimerRoutine(float duration, Coroutine targetRound)
    {
        float t = duration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        
        StopCoroutine(targetRound);
        if (currentEvent != null)
        {
            currentEvent.StopAllCoroutines();
            currentEvent.StopEvent();
            currentEvent = null;
        }
        Debug.Log(">>> Timer: round interrompu !");
    }

    public void PauseGame(bool state)
    {
        ChangePlayerControls(!state);
        switch (state)
        {
            case true :
                player1.peeController.peeGenerator.StopGenerator();
                player2.peeController.peeGenerator.StopGenerator();
                break;
            case false :
                player1.peeController.peeGenerator.StartGenerator();
                player2.peeController.peeGenerator.StartGenerator();
                break;
        }
    }
    

    public void ChangePlayerControls(bool state)
    {
        player1.canUseControls = state;
        player2.canUseControls = state;
    }

    public Image winnerImage;
    public Image looserImage;
    public Image backgroundWin;

    public List<Sprite> alienWinners;
    public List<Sprite> alienLoosers;
    public List<Sprite> winnerBackgrounds;
    
    SelectCharacter.CouleurEnum winnerCouleur;
    SelectCharacter.CouleurEnum looserCouleur;
    
    public List<AudioClip> winnerSongs;

    public GameObject fresque;
    
    private IEnumerator StopGame()
    {
        winScreen.SetActive(true);
        
        
        if (winningRoundP1 == 2)
        {
            winnerCouleur = couleurP1;
            looserCouleur = couleurP2;
        }
        if (winningRoundP2 == 2)
        {
            winnerCouleur = couleurP2;
            looserCouleur = couleurP1;
        }

        winnerImage.sprite = alienWinners[(int)winnerCouleur];
        looserImage.sprite = alienLoosers[(int)looserCouleur];
        backgroundWin.sprite = winnerBackgrounds[(int)winnerCouleur];
        SoundManager.Instance.PlaySFX(winnerSongs[(int)winnerCouleur], 1.5f);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
