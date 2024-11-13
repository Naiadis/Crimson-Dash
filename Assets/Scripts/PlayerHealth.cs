using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        onDamaged?.Invoke();

        // Flash effect when hit
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRoutine());
        }

        // Start invincibility
        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        float flashDuration = 0.1f;
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    void Die()
    {
        onDeath?.Invoke();
        Debug.Log("Player died!");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}