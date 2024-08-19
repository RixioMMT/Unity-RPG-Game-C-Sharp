using UnityEngine;

public class PlayerWardrobeInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject interactPanel; // Reference to the Interact Panel GameObject
    public InventoryManager inventoryManager; // Reference to the Inventory Manager

    private bool playerInRange;
    private bool isInteracting;

    void Start()
    {
        interactPanel.SetActive(false); // Hide the interact panel initially
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
            interactPanel.SetActive(true); // Show the interact panel
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
            interactPanel.SetActive(false); // Hide the interact panel
            if (isInteracting)
            {
                EndInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        interactPanel.SetActive(false); // Hide the interact panel
        inventoryManager.OpenInventory(); // Open the inventory
    }

    void EndInteraction()
    {
        isInteracting = false;
        if (playerInRange)
        {
            interactPanel.SetActive(true); // Show the interact panel if player is still in range
        }
    }
}