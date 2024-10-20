using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float groundLevel = 0f;

    private Vector2 velocity;
    private bool isGrounded;

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = transform.position.y <= groundLevel;

        // Handle jump input
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Handle horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        velocity.x = horizontalInput * moveSpeed;

        // Apply gravity
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            // Ensure the player doesn't sink below the ground
            velocity.y = Mathf.Max(0, velocity.y);
        }

        // Move the player
        transform.Translate(velocity * Time.deltaTime);

        // Keep the player at ground level
        if (transform.position.y < groundLevel)
        {
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
        }
    }

    void Jump()
    {
        velocity.y = jumpForce;
    }
}