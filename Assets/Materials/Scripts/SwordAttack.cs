using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager
    private GameObject equippedSword;
    private Animator swordAnimator;
    private string swingTriggerName = "Swing";
    private bool isInteracting = false; // Flag to check if interacting with an object
    private string swordName; // Name of the equipped sword
    private bool isSwinging = false; // Flag to check if the sword is currently swinging
    private AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip hitSound; // Sound to play when hitting the enemy

    void Start()
    {
        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the sword or player.");
        }
    }

    void Update()
    {
        // Only swing the sword if the inventory is closed and not interacting with tagged objects
        if (Input.GetKeyDown(KeyCode.Space) && equippedSword != null &&
            !inventoryManager.IsInventoryOpen && !isInteracting)
        {
            SwingSword();
        }
    }

    public void SetEquippedSword(GameObject sword, string itemName)
    {
        equippedSword = sword;
        swordName = itemName; // Store the name of the equipped sword
        if (equippedSword != null)
        {
            swordAnimator = equippedSword.GetComponent<Animator>();
            if (swordAnimator == null)
            {
                Debug.LogWarning("Animator component is missing on the equipped sword.");
            }
        }
        else
        {
            swordAnimator = null; // No sword equipped
        }
    }

    private void SwingSword()
    {
        if (swordAnimator != null)
        {
            isSwinging = true; // Start swinging
            Debug.Log("Triggering swing animation.");
            swordAnimator.SetTrigger(swingTriggerName);
        }
        else
        {
            Debug.LogWarning("Sword Animator is not assigned.");
        }
    }

    public void SetInteracting(bool interacting)
    {
        isInteracting = interacting;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSwinging && equippedSword != null && other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                                // Play the hit sound
                if (audioSource != null && hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
                int damage = swordName == "Espada azul" ? 50 : 10; // Apply special damage if sword is "Espada azul"
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Hit enemy with {swordName}. Damage dealt: {damage}");
                // Prevent further damage in the same swing
                isSwinging = false;
            }
        }
    }

    // Call this method when the swing animation ends to reset the swing state
    public void OnSwingAnimationEnd()
    {
        isSwinging = false;
    }
}