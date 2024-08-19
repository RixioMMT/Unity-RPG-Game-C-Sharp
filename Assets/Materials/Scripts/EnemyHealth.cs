using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int healAmount = 10;
    public float healInterval = 5f;
    public Text healthText; // Reference to the UI Text component for health display

    public ChestInteraction chestInteraction; // Reference to the ChestInteraction script

    private Coroutine healCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay(); // Initialize health display at the start
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthDisplay(); // Update health display when taking damage

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(HealOverTime());
            }
        }
    }

    void Die()
    {
        Debug.Log("Enemy has died.");
        if (healthText != null)
        {
            healthText.color = Color.black;
            healthText.text = "Vida: 0/100";
        }

        // Set the boolean in the ChestInteraction script
        if (chestInteraction != null)
        {
            chestInteraction.enemyDefeated = true;
        }

        Destroy(gameObject); // Destroy the enemy GameObject
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

            Debug.Log("Enemy healed. Current health: " + currentHealth);
            UpdateHealthDisplay(); // Update health display during healing
        }

        healCoroutine = null;  // Reset coroutine reference when healing is done
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