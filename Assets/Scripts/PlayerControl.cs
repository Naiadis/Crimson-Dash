using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("Dash")]
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float horizontalInput;
    private bool isJumping = false;
    private bool isDashing = false;
    private float dashCooldownTimeLeft;
    private Collider2D playerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
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

        // Handle dashing
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isDashing && dashCooldownTimeLeft <= 0)
        {
            StartDash();
        }

        // Update dash cooldown
        if (dashCooldownTimeLeft > 0)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
        }

        // Handle character flipping
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        UpdateAnimationStates();
    }

    void UpdateAnimationStates()
    {
        animator.SetFloat("magnitude", Mathf.Abs(horizontalInput));
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            animator.SetBool("Jump", false);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashCooldownTimeLeft = dashCooldown;
        playerCollider.enabled = false;
        
        // Trigger the dash animation
        animator.SetTrigger("Dash");
        
        // Start a coroutine to re-enable collider after animation
        StartCoroutine(EndDashAfterAnimation());
    }

    System.Collections.IEnumerator EndDashAfterAnimation()
    {
        // Wait for the current animation state to end
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        isDashing = false;
        playerCollider.enabled = true;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
        isJumping = true;
        animator.SetTrigger("Jump");
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
                    isGrounded = true;
                    isJumping = false;
                    // animator.SetBool("Jump", false);
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