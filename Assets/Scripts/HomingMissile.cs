using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float missileSpeed = 15f;
    public float turnSpeed = 50f;
    private Transform targetEnemy; // Reference to the current target enemy
    private bool isHomingEnabled = false; // Flag to control homing missile behavior

    void Start()
    {
        targetEnemy = null; // Initialize targetEnemy to null
    }

    public void SetTarget(Transform newTarget)
    {
        targetEnemy = newTarget;
    }

    void Update()
    {
        if (isHomingEnabled)
        {
            // Check if we have a target enemy and it's still active
            if (targetEnemy != null && targetEnemy.gameObject.activeSelf)
            {
                // Calculate the direction to the target enemy
                Vector3 direction = (targetEnemy.position - transform.position).normalized;

                // Rotate the missile to face the target
                transform.rotation = Quaternion.LookRotation(direction);

                // Move the missile forward
                transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
            }
            else
            {
                // Target enemy is destroyed or depleted, stop homing
                isHomingEnabled = false;
            }
        }
    }

    // Method to enable homing behavior for this missile
    public void EnableHoming()
    {
        isHomingEnabled = true;
    }

    // Method to disable homing behavior for this missile
    public void DisableHoming()
    {
        isHomingEnabled = false;
    }
}
