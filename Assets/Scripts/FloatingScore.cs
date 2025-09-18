using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FloatingScore : MonoBehaviour
{
    [Header("Durée de vie du FX")]
    public float lifetime = 0.5f;      
    public float moveDistance = 40f;

    [Header("UI Elements")]
    public Image symbolImage;
    public Image tensImage;
    public Image unitsImage;

    [Header("Sprites symboles")]
    public Sprite plusSprite;
    public Sprite minusSprite;

    [Header("Sprites positifs")]
    public Sprite spritePos0;
    public Sprite spritePos1;
    public Sprite spritePos2;
    public Sprite spritePos5;

    [Header("Sprites négatifs")]
    public Sprite spriteNeg0;
    public Sprite spriteNeg1;
    public Sprite spriteNeg2;
    public Sprite spriteNeg5;

    [HideInInspector]
    public FloatingScorePool pool;

    private float elapsed;
    private RectTransform rect;
    private Vector2 startPos;

    // Dictionnaires pour retrouver les bons sprites
    private Dictionary<int, Sprite> positiveMap;
    private Dictionary<int, Sprite> negativeMap;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        positiveMap = new Dictionary<int, Sprite>()
        {
            {0, spritePos0},
            {1, spritePos1},
            {2, spritePos2},
            {5, spritePos5}
        };

        negativeMap = new Dictionary<int, Sprite>()
        {
            {0, spriteNeg0},
            {1, spriteNeg1},
            {2, spriteNeg2},
            {5, spriteNeg5}
        };
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / lifetime);

        rect.anchoredPosition = startPos + new Vector2(0, moveDistance * t);

        // Fade-out (alpha)
        float alpha = 1f - t;
        SetAlpha(alpha);

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
        if (score > 100) return;
        
        startPos = pos;
        rect.anchoredPosition = startPos;
        elapsed = 0f;

        bool isPositive = score >= 0;
        int absValue = Mathf.Abs(score);

        // --- Symbole ---
        symbolImage.sprite = isPositive ? plusSprite : minusSprite;

        // --- Chiffres ---
        int tens = absValue / 10;
        int units = absValue % 10;

        // Dizaine
        if (tens > 0)
        {
            tensImage.enabled = true;
            tensImage.sprite = isPositive ? positiveMap[tens] : negativeMap[tens];
        }
        else
        {
            tensImage.enabled = false;
        }

        // Unité
        unitsImage.enabled = true;
        if ((isPositive && positiveMap.ContainsKey(units)) ||
            (!isPositive && negativeMap.ContainsKey(units)))
        {
            unitsImage.sprite = isPositive ? positiveMap[units] : negativeMap[units];
        }
        else
        {
            Debug.LogWarning("Pas de sprite défini pour le chiffre " + units);
            unitsImage.enabled = false;
        }

        // Reset alpha
        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        SetImageAlpha(symbolImage, a);
        SetImageAlpha(tensImage, a);
        SetImageAlpha(unitsImage, a);
    }

    private void SetImageAlpha(Image img, float a)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = a;
            img.color = c;
        }
    }
}
