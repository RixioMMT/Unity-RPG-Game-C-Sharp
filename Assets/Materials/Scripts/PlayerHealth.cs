using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int healAmount = 10;
    public float healInterval = 5f;
    public Animator animator; 
    public Text healthText; 

    private Coroutine healCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player!");
        }

        UpdateHealthDisplay();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            PlayKnockbackAnimation(); 
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(HealOverTime());
            }
        }

        UpdateHealthDisplay();
    }

    void PlayKnockbackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Knockback"); 
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
        }

        UpdateHealthDisplay();
    }

    IEnumerator HealOverTime()
    {
        while (currentHealth < maxHealth)
        {
            yield return new WaitForSeconds(healInterval);

            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Debug.Log("Player healed. Current health: " + currentHealth);
            UpdateHealthDisplay(); 
        }

        healCoroutine = null; 
    }

    void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth.ToString() + "/100";
        }
        else
        {
            Debug.LogWarning("Health Text component is not assigned.");
        }
    }
}