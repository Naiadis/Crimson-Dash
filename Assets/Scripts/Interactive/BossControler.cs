using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float maxDistanceFromPlayer = 10f;  // Distance when player has full health
    public float minDistanceFromPlayer = 5f;   // Distance when player has low health
    public float moveSpeed = 3f;
    public float smoothTime = 0.5f;

    [Header("References")]
    public PlayerHealth playerHealth;
    private Transform playerTransform;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
        playerTransform = playerHealth.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Calculate target distance based on player health
        float healthPercentage = (float)playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
        float targetDistance = Mathf.Lerp(minDistanceFromPlayer, maxDistanceFromPlayer, healthPercentage);

        // Calculate target position
        Vector3 directionToPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = playerTransform.position + directionToPlayer * targetDistance;
        targetPosition.y = transform.position.y; // Maintain current height

        // Smoothly move towards target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}