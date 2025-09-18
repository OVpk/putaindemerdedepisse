using TMPro;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    [Header("Score")]
    public int scoreToAdd;

    [Header("FX UI")]
    public GameObject floatingScorePrefab;   // Prefab avec TextMeshProUGUI
    public RectTransform parentCanvas;       // RectTransform du Canvas UI

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie qu’il y a bien un point de contact
        if (collision.contactCount == 0) return;

        // Récupère le premier point de contact précis
        Vector3 hitPoint = collision.gameObject.transform.position;

        switch (collision.gameObject.tag)
        {
            case "P1":
                AddScore(ref GameManager.Instance.scoreP1, GameManager.Instance.scoreTextP1);
                SpawnFX(hitPoint, PlayerController.PlayerID.Player1);
                break;

            case "P2":
                AddScore(ref GameManager.Instance.scoreP2, GameManager.Instance.scoreTextP2);
                SpawnFX(hitPoint, PlayerController.PlayerID.Player2);
                break;
        }
    }

    public Camera cam;

    /// <summary>
    /// Ajoute le score et met à jour le texte d’UI correspondant.
    /// </summary>
    private void AddScore(ref int playerScore, TMP_Text scoreText)
    {
        playerScore += scoreToAdd;
        if (playerScore < 0) playerScore = 0;
        scoreText.text = playerScore.ToString();
    }



    private void SpawnFX(Vector3 worldPos, PlayerController.PlayerID playerId)
    {
        // Convertir position monde -> position canvas
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas,
            screenPos,
            null,
            out localPos
        );

        switch (playerId)
        {
            case PlayerController.PlayerID.Player1 :
                GameManager.Instance.scorePoolP1.Spawn(localPos, scoreToAdd);
                break;
            case PlayerController.PlayerID.Player2 :
                GameManager.Instance.scorePoolP2.Spawn(localPos, scoreToAdd);
                break;
        }
    }

}
