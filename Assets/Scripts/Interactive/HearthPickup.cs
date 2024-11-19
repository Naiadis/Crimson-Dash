using UnityEngine;

public class HearthPickup : MonoBehaviour
{
    [SerializeField] private GameObject healEffectPrefab;
    private Camera mainCamera;
    private Transform playerTransform;
    private bool hasPassedPlayer = false;

    private void Start()
    {
        mainCamera = Camera.main;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!hasPassedPlayer && transform.position.x < playerTransform.position.x)
        {
            hasPassedPlayer = true;
        }

        if (hasPassedPlayer)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
            if (viewportPoint.x < -0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null && playerHealth.GetCurrentHealth() < playerHealth.maxHealth)
            {
                if (healEffectPrefab != null)
                {
                    Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
                }

                int healthToRestore = Mathf.CeilToInt(playerHealth.maxHealth / 3f);
                int newHealth = Mathf.Min(playerHealth.GetCurrentHealth() + healthToRestore, playerHealth.maxHealth);
                playerHealth.RestoreHealth(newHealth);
                
                Destroy(gameObject);
            }
        }
    }
}