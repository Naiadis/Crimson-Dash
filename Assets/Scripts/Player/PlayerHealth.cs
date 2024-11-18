using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
    public HealthBar healthBar;

    private int currentHealth;
    private GameManager deathScreen;
    private Animator animator;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        deathScreen = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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
    animator.SetTrigger("Die");
    GetComponent<PlayerController>().enabled = false;
    GameManager.Instance.GameOver();
}

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void RestoreHealth(int newHealth)
{
    currentHealth = newHealth;
    healthBar.SetHealth(currentHealth);
}
}