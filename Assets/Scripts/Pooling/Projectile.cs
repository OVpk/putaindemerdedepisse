using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    private PoolingSystem poolingSystem;
    private Rigidbody rb;

    public void Init(PoolingSystem pool, PlayerController.PlayerID playerID, Sprite sprite)
    {
        rb = GetComponent<Rigidbody>();
        poolingSystem = pool;
        tag = playerID == PlayerController.PlayerID.Player1 ? "P1" : "P2";
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag is "P1" or "P2") return;
        poolingSystem.AddToPool(this);
    }

    public void Launch(Vector3 force)
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
