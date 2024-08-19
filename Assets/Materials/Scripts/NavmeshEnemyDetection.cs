using UnityEngine;

public class NavmeshEnemyDetection : MonoBehaviour
{
    public bool playerInArea = false;  
    public GameObject healthPanel;  

    void Start()
    {
        if (healthPanel != null)
        {
            healthPanel.SetActive(false); 
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
            playerInArea = true; 
            if (healthPanel != null)
            {
                healthPanel.SetActive(true); 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false; 
            if (healthPanel != null)
            {
                healthPanel.SetActive(false); 
            }
        }
    }
}