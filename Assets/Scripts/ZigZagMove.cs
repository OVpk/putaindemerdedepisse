using System.Collections;
using UnityEngine;

public class ZigZagMove : MonoBehaviour
{
    public RectTransform target;       // L'image à déplacer
    public RectTransform worldCanvas;  // Canvas en World Space
    public float entrySpeed = 3f;      // Vitesse de descente
    public float speed = 2f;           // Vitesse du zigzag
    public float directionChangeTime = 1.5f;

    private Vector2 direction;
    private float timer;
    private bool hasEntered = false;   // True quand arrivé dans le canvas
    private bool isActive = false;
    private Vector2 entryTargetPos;

    private void Start()
    {
        StopMove();
    }

    public void Init()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        float offset = target.rect.height * 1.2f;

        Vector2 canvasMax = worldCanvas.rect.max;
        Vector2 canvasMin = worldCanvas.rect.min;

        // Spawn : au-dessus du canvas, centré horizontalement
        Vector2 startPos = new Vector2(0f, canvasMax.y + offset);
        target.anchoredPosition = startPos;

        hasEntered = false;

        // Position d’arrivée de la descente (milieu du canvas)
        entryTargetPos = new Vector2(0f, 0f);
    }

    public void StartMove()
    {
        potiteMouche.SetActive(true);
        Init();
        isActive = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("P1") || other.gameObject.CompareTag("P2"))
        {
            StartCoroutine(StopMoveForKill());
        }
    }

    public GameObject potiteMouche;

    public AnimationClip animDeMort;

    public Animation animatorScoreMouche;

    public IEnumerator StopMoveForKill()
    {
        animatorScoreMouche.gameObject.transform.position = gameObject.transform.position;
        animatorScoreMouche.Play();
        yield return new WaitForSeconds(animDeMort.length);
        StopMove();
    }
    
    
    public void StopMove()
    {
        isActive = false;
        potiteMouche.SetActive(false);
    }


    private void Update()
    {
        if (!isActive) return;

        if (!hasEntered)
        {
            // Phase d’entrée : descente verticale jusqu’au centre
            target.anchoredPosition = Vector2.MoveTowards(
                target.anchoredPosition,
                entryTargetPos,
                entrySpeed * Time.deltaTime
            );

            // Vérifie si arrivé
            if (Vector2.Distance(target.anchoredPosition, entryTargetPos) < 0.1f)
            {
                hasEntered = true;
                PickNewDirection(); // Commence le zigzag
            }
        }
        else
        {
            // Phase zigzag
            target.anchoredPosition += direction * speed * Time.deltaTime;
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PickNewDirection();
            }

            KeepInsideCanvas();
        }
    }

    private void PickNewDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector2(x, y).normalized;
        timer = directionChangeTime;

        // Flip le scale en X selon la direction
        if (direction.x != 0)
        {
            Vector3 scale = target.localScale;
            scale.x = -(Mathf.Abs(scale.x) * Mathf.Sign(direction.x));
            target.localScale = scale;
        }
    }


    private void KeepInsideCanvas()
    {
        Vector2 min = worldCanvas.rect.min;
        Vector2 max = worldCanvas.rect.max;

        Vector2 pos = target.anchoredPosition;

        // Empêche de sortir du canvas
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        target.anchoredPosition = pos;
    }
}
