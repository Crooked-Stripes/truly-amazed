using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed of the player character
    public float speed = 5.0f;

    // Reference to the key that the player needs to collect
    public GameObject key;

    // Flag to keep track of whether the player has collected the key
    private bool hasKey = false;

    // Update is called once per frame
    void Update()
    {
        // Move the player character based on user input
        float horizontalInput = Input.GetKey(KeyCode.LeftArrow) ? -1.0f : (Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f);
        float verticalInput = Input.GetKey(KeyCode.DownArrow) ? -1.0f : (Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f);
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);
        transform.Translate(movement.normalized * speed * Time.deltaTime);

        // Check if the player has collided with the key
        if (!hasKey && Vector3.Distance(transform.position, key.transform.position) < 1.0f)
        {
            // The player has collided with the key, so collect it
            hasKey = true;
            key.SetActive(false);
        }
    }

    // Check if the player has collected the key
    public bool HasKey()
    {
        return hasKey;
    }
}
