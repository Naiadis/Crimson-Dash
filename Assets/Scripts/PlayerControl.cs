using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float horizontalInput;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Update animator parameters
        UpdateAnimationStates();

        // Handle character flipping
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void UpdateAnimationStates()
    {
        // Update movement magnitude
        animator.SetFloat("magnitude", Mathf.Abs(horizontalInput));
        
        // Update vertical velocity
        animator.SetFloat("yVelocity", rb.velocity.y);

        // Handle jump state transitions
        if (isJumping && rb.velocity.y < 0)
        {
            // Transition from jump to falling
            isJumping = false;
            animator.SetBool("Jump", false);
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
        isJumping = true;
        animator.SetBool("Jump", true);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y >= 0.7f)
                {
                    // Landing
                    isGrounded = true;
                    isJumping = false;
                    animator.SetBool("Jump", false);
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}