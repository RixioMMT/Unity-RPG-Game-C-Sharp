using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public InventoryManager inventoryManager; 
    private GameObject equippedSword;
    private Animator swordAnimator;
    private string swingTriggerName = "Swing";
    private bool isInteracting = false;
    private string swordName;
    private bool isSwinging = false; 
    private AudioSource audioSource;
    public AudioClip hitSound; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the sword or player.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && equippedSword != null &&
            !inventoryManager.IsInventoryOpen && !isInteracting)
        {
            SwingSword();
        }
    }

    public void SetEquippedSword(GameObject sword, string itemName)
    {
        equippedSword = sword;
        swordName = itemName; 
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
            swordAnimator = null; 
        }
    }

    private void SwingSword()
    {
        if (swordAnimator != null)
        {
            isSwinging = true; 
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
                if (audioSource != null && hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }
                int damage = swordName == "Espada azul" ? 50 : 10; 
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Hit enemy with {swordName}. Damage dealt: {damage}");
                isSwinging = false;
            }
        }
    }

    public void OnSwingAnimationEnd()
    {
        isSwinging = false;
    }
}