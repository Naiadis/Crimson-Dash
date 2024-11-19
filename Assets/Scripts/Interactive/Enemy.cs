using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 1;
    public float knockbackForce = 10f;
    private bool hasPassedPlayer = false;
    private Transform playerTransform;
    private Camera mainCamera;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if enemy has passed the player
        if (!hasPassedPlayer && transform.position.x < playerTransform.position.x)
        {
            hasPassedPlayer = true;
            ScoreManager.Instance.AddEnemyAvoided();
        }

        // Destroy only when enemy has passed player and is off-screen
        if (hasPassedPlayer)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
            if (viewportPoint.x < -0.1f)  // Slightly off screen to the left
            {
                Destroy(gameObject);
            }
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
                
            }
        }
    }
}