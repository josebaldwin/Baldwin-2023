using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Adjust this to set the movement speed

    void Update()
    {
        // Get input from keyboard
        float horizontalInput = Input.GetAxis("Vertical");
        float verticalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction in local space
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

        // Apply local movement to the ship
        transform.Translate(movement, Space.Self);

        // Clamp player's position to the specified boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -30f, 30f),
            Mathf.Clamp(transform.position.y, -12f, 14f),
            transform.position.z
        );
    }
}
