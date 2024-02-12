using UnityEngine;

public class MissileCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy missile collides with the player missile
        if ((collision.gameObject.CompareTag("Missile") && gameObject.CompareTag("EnemyMissile")) ||
            (collision.gameObject.CompareTag("EnemyMissile") && gameObject.CompareTag("Missile")))
        {
            // Destroy both the enemy missile and the player missile
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        // Check if the enemy missile collides with the player
        else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyMissile"))
        {
            // Destroy only the enemy missile
            Destroy(gameObject);
        }
        // Check if the enemy missile collides with the shield
        else if (collision.gameObject.CompareTag("Shield") && gameObject.CompareTag("EnemyMissile"))
        {
            // Get the shield's ShieldBehavior component
            ShieldBehavior shield = collision.gameObject.GetComponent<ShieldBehavior>();
            if (shield != null)
            {
                // Call TakeDamage to initiate the glow effect
                shield.TakeDamage(1.0f); // Adjust the damage amount as needed
            }

            // Destroy the enemy missile
            Destroy(gameObject);
        }
    }
}
