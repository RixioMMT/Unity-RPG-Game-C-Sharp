using UnityEngine;

public class NavmeshEnemyDetection : MonoBehaviour
{
    public bool playerInArea = false;  // Flag to indicate if the player is in the area
    public GameObject healthPanel;     // Reference to the UI panel displaying enemy health

    void Start()
    {
        if (healthPanel != null)
        {
            healthPanel.SetActive(false); // Initially hide the panel
        }
        else
        {
            Debug.LogWarning("Health Panel is not assigned.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;  // Player has entered the area
            if (healthPanel != null)
            {
                healthPanel.SetActive(true); // Show the panel when player is in the area
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;  // Player has left the area
            if (healthPanel != null)
            {
                healthPanel.SetActive(false); // Hide the panel when player is not in the area
            }
        }
    }
}