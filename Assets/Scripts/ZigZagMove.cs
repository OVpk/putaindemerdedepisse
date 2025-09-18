using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZigZagMove : MonoBehaviour
{
    public RectTransform target;       // L'image à déplacer
    public RectTransform worldCanvas;  // Canvas en World Space
    public float speed = 2f;           // Vitesse du zigzag
    public float directionChangeTime = 1.5f;
    public float entrySpeed = 3f;      // Vitesse d'entrée

    Vector2 direction;
    float timer;
    bool hasEntered = false;           // True une fois qu'on est dans le canvas
    Vector2 entryTargetPos;

    private bool isActive = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (target == null) target = GetComponent<RectTransform>();

        float offset = target.rect.height * 1.2f;
        Vector2 canvasMax = worldCanvas.rect.max;
        Vector2 canvasMin = worldCanvas.rect.min;

        // Position de départ : au-dessus du canvas, x choisi aléatoirement dans la largeur
        float startX = Random.Range(canvasMin.x, canvasMax.x);
        float startY = canvasMax.y + offset;
        Vector2 startPos = new Vector2(startX, startY);
        target.anchoredPosition = startPos;

        hasEntered = false;

        // Position cible d'entrée (par exemple au même x mais un peu plus bas dans le canvas)
        entryTargetPos = new Vector2(startX, canvasMax.y * 0.7f); 
    }


    public void StartMove()
    {
        gameObject.SetActive(true);
        Init();
        isActive = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag is "P1" or "P2")
        {
            StopMove();
        }
    }

    public void StopMove()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    void Update()
    {
        if (!isActive) return;
        
        if (!hasEntered)
        {
            // Phase d'entrée : on se déplace vers entryTargetPos
            target.anchoredPosition =
                Vector2.MoveTowards(target.anchoredPosition, entryTargetPos, entrySpeed * Time.deltaTime);

            // Vérifie si on est arrivé
            if (Vector2.Distance(target.anchoredPosition, entryTargetPos) < 0.1f)
            {
                hasEntered = true;
                PickNewDirection(); // Commence le zigzag
            }
        }
        else
        {
            // Mouvement zigzag existant
            target.anchoredPosition += direction * speed * Time.deltaTime;

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PickNewDirection();
            }

            KeepInsideCanvas();
        }
    }

    void PickNewDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector2(x, y).normalized;
        timer = directionChangeTime;
    }

    void KeepInsideCanvas()
    {
        Vector2 min = worldCanvas.rect.min;
        Vector2 max = worldCanvas.rect.max;

        Vector2 pos = target.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        target.anchoredPosition = pos;
    }
}
