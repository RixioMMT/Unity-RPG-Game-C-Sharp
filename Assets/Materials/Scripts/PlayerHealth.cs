using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int healAmount = 10;
    public float healInterval = 5f;
    public Animator animator; // Reference to the Animator component
    public Text healthText; // Reference to the UI Text component for health display

    private Coroutine healCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player!");
        }

        UpdateHealthDisplay(); // Update the health display at the start
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
            PlayKnockbackAnimation(); // Trigger knockback animation
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(HealOverTime());
            }
        }

        UpdateHealthDisplay(); // Update the health display after taking damage
    }

    void PlayKnockbackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Knockback"); // Play the knockback animation
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
        }

        // Optionally, you could reset or hide the health display here
        UpdateHealthDisplay(); // Update the health display when the player dies
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
            UpdateHealthDisplay(); // Update the health display after healing
        }

        healCoroutine = null;  // Reset coroutine reference when healing is done
    }

    // Method to update the health display on the UI
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