using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Vector3 initialVelocity;
    private Quaternion initialRotation;
    public float correctionSpeed = 5f; // Speed at which the enemy corrects its course
    public float speed = 5f; // Speed set in Unity Inspector
    private float fixedZPosition = 0f; // The Z position you want to fix the enemies at
    private Rigidbody enemyRigidbody;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        // Set initial velocity based on the speed
        initialVelocity = new Vector3(-speed, 0f, 0f);
        initialRotation = transform.rotation;

        // Apply the initial velocity
        enemyRigidbody.velocity = initialVelocity;
    }

    void Update()
    {
        // Maintain the initial velocity and fixed Z position
        MaintainVelocityAndPosition();

        // Correct the enemy's rotation continuously
        CorrectRotation();
    }

    private void MaintainVelocityAndPosition()
    {
        // Maintain fixed Z position
        Vector3 currentPosition = transform.position;
        if (currentPosition.z != fixedZPosition)
        {
            transform.position = new Vector3(currentPosition.x, currentPosition.y, fixedZPosition);
        }

        // Maintain initial velocity
        enemyRigidbody.velocity = initialVelocity;
    }

    private void CorrectRotation()
    {
        // Only correct the rotation
        if (transform.rotation != initialRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, correctionSpeed * Time.deltaTime);
        }
    }
}
