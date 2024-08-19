using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);

                Debug.Log("Player hit by hammer. Damage dealt: " + damageAmount);
            }
        }
    }
}