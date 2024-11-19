using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float autoRunSpeed = 5f;
    public float enemyBounceForce = 8f;
    
    [Header("Dash/Slide")]
    public float dashCooldown = 0.5f;
    public float normalColliderHeight = 2f;
    public float slideColliderHeight = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isJumping = false;
    private bool isDashing = false;
    private float dashCooldownTimeLeft;
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private bool isKnockedBack = false;
    private float knockbackRecoveryTime = 0.5f;
    private float knockbackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;
        transform.localScale = new Vector3(1, 1, 1); // Face right by default
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
            return;
        }

        // jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // dashing
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isDashing && dashCooldownTimeLeft <= 0)
        {
            StartDash();
        }

        // dash cooldown
        if (dashCooldownTimeLeft > 0)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
        }

        UpdateAnimationStates();
    }

    void UpdateAnimationStates()
    {
        animator.SetFloat("magnitude", 1);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashCooldownTimeLeft = dashCooldown;

        // Shrink the collider
        Vector2 newSize = boxCollider.size;
        newSize.y = slideColliderHeight;
        boxCollider.size = newSize;

        // Adjust collider offset to keep character grounded
        Vector2 newOffset = boxCollider.offset;
        newOffset.y = (slideColliderHeight - originalColliderSize.y) / 2;
        boxCollider.offset = newOffset;
        
        // dash animation
        animator.SetTrigger("Dash");
        
        StartCoroutine(EndDashAfterAnimation());
    }

    System.Collections.IEnumerator EndDashAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        // Restore original collider properties
        boxCollider.size = originalColliderSize;
        boxCollider.offset = originalColliderOffset;
        
        isDashing = false;
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
        if (!isKnockedBack)
        {
            rb.velocity = new Vector2(autoRunSpeed, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    CheckGroundContact(collision);
    if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
}
private void HandleEnemyCollision(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        
        // If hitting enemy from above
        if (contact.normal.y > 0.7f)
        {
            rb.velocity = new Vector2(rb.velocity.x, enemyBounceForce);
            isGrounded = false;
            isJumping = true;
        }
        else // Side collision
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackDirection.y = 0.5f; // Add slight upward force
            rb.velocity = knockbackDirection * collision.gameObject.GetComponent<Enemy>().knockbackForce;
            
            isKnockedBack = true;
            knockbackTimer = knockbackRecoveryTime;
        }
    }
void OnCollisionStay2D(Collision2D collision)
{
    if (!isGrounded)
    {
        CheckGroundContact(collision);
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = false;
    }
}

private void CheckGroundContact(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y >= 0.7f)
            {
                isGrounded = true;
                isJumping = false;
                return; // Exit as soon as it finds a valid ground contact
            }
        }
    }
}
}