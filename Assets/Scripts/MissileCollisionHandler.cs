using UnityEngine;

public class MissileCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if this missile collides with an enemy missile
        if (collision.gameObject.CompareTag("EnemyMissile") && gameObject.CompareTag("Missile") ||
            collision.gameObject.CompareTag("Missile") && gameObject.CompareTag("EnemyMissile"))
        {
            // Destroy both this missile and the collided missile
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
