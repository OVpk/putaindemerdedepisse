using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour
{
    [Header("Durée de vie du FX")]
    public float lifetime = 0.5f;      
    public float moveDistance = 40f;

    [HideInInspector]
    public FloatingScorePool pool;

    private float elapsed;
    private RectTransform rect;
    private TextMeshProUGUI tmp;
    private Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        tmp  = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / lifetime);

        rect.anchoredPosition = startPos + new Vector2(0, moveDistance * t);

        if (tmp != null)
        {
            Color c = tmp.color;
            c.a = 1f - t;
            tmp.color = c;
        }

        if (elapsed >= lifetime)
        {
            pool.ReturnToPool(this);
        }
    }

    /// <summary>
    /// Initialise le FX avec sa position et valeur
    /// </summary>
    public void Initialize(Vector2 pos, int score)
    {
        startPos = pos;
        rect.anchoredPosition = startPos;
        elapsed = 0f;

        if (tmp != null)
        {
            if (score > 0)
            {
                tmp.text = "+" + score;
            }
            else
            {
                tmp.text = score.ToString();
            }
        }
            
    }
}