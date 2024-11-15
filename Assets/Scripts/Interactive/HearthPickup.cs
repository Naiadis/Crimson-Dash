using UnityEngine;

public class HearthPickup : MonoBehaviour
{
    [SerializeField] private GameObject healEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null && playerHealth.GetCurrentHealth() < playerHealth.maxHealth)
            {
                // Create heal effect at torch position
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