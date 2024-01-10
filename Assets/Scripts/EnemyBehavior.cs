using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Vector3 initialVelocity;
    private Quaternion initialRotation;
    public float correctionSpeed = 5f; // Speed at which the enemy corrects its course
    public float speed = 5f; // Speed set in Unity Inspector

    private Rigidbody enemyRigidbody;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        initialVelocity = new Vector3(-speed, 0f, 0f); // Use speed for initial velocity
        initialRotation = transform.rotation;

        enemyRigidbody.velocity = initialVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Implement collision logic if needed
    }

    void Update()
    {
        // Continuously correct the enemy's course
        StartCorrection();
    }

    private void StartCorrection()
    {
        enemyRigidbody.velocity = Vector3.Lerp(enemyRigidbody.velocity, initialVelocity, correctionSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, correctionSpeed * Time.deltaTime);
    }
}
