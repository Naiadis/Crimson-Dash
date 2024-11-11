using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("Dash/Slide")]
    public float dashCooldown = 1f;
    public float normalColliderHeight = 2f;
    public float slideColliderHeight = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float horizontalInput;
    private bool isJumping = false;
    private bool isDashing = false;
    private float dashCooldownTimeLeft;
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Store original collider properties
        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed, isGrounded: " + isGrounded);
        }

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
        
        // Trigger the dash animation
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
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    CheckGroundContact(collision);
}

void OnCollisionStay2D(Collision2D collision)
{
    // Only check if we're not already grounded
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
        Debug.Log("Grounded set to false");
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
                Debug.Log("Ground contact detected");
                return; // Exit as soon as we find a valid ground contact
            }
        }
    }
}
}