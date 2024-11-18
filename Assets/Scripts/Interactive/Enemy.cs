using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 1;
    public float knockbackForce = 10f;
    private bool hasPassedPlayer = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if enemy has passed the player
        if (!hasPassedPlayer && transform.position.x < playerTransform.position.x)
        {
            hasPassedPlayer = true;
            ScoreManager.Instance.AddEnemyAvoided();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}