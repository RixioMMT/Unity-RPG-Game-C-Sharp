using UnityEngine;

public class PlayerWardrobeInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject interactPanel; 
    public InventoryManager inventoryManager; 

    private bool playerInRange;
    private bool isInteracting;

    void Start()
    {
        interactPanel.SetActive(false); 
    }

    void Update()
    {
        if (playerInRange && !isInteracting && Input.GetKeyDown(KeyCode.Space))
        {
            StartInteraction();
        }
        else if (isInteracting && !inventoryManager.IsInventoryOpen)
        {
            EndInteraction();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
            interactPanel.SetActive(true); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
            interactPanel.SetActive(false);
            if (isInteracting)
            {
                EndInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        interactPanel.SetActive(false); 
        inventoryManager.OpenInventory(); 
    }

    void EndInteraction()
    {
        isInteracting = false;
        if (playerInRange)
        {
            interactPanel.SetActive(true); 
        }
    }
}